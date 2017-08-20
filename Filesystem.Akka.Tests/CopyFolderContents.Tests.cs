﻿using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class CopyFolderContentsPopulatedTests : TestKit
    {
        private string sourcePath;
        private string targetPath;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            Directory.Delete(this.sourcePath, true);
            Directory.Delete(this.targetPath, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.sourcePath = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.sourcePath);

            Directory.CreateDirectory(Path.Combine(this.sourcePath, "A", "1"));
            Directory.CreateDirectory(Path.Combine(this.sourcePath, "A", "2"));
            Directory.CreateDirectory(Path.Combine(this.sourcePath, "A", "3"));
            Directory.CreateDirectory(Path.Combine(this.sourcePath, "B", "1"));
            Directory.CreateDirectory(Path.Combine(this.sourcePath, "B", "2"));
            File.WriteAllText(Path.Combine(this.sourcePath, "A", "1", "one.txt"), "One");
            File.WriteAllText(Path.Combine(this.sourcePath, "A", "two.txt"), "Two");
            File.WriteAllText(Path.Combine(this.sourcePath, "three.txt"), "Three");

            this.targetPath = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.targetPath);
        }

        [TestMethod]
        public void Copy_populated_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFolderContents(new ReadableFolder(this.sourcePath), new WritableFolder(this.targetPath)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);

            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "A", "1")));
            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "A", "2")));
            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "A", "3")));
            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "B", "1")));
            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "B", "2")));
            Assert.AreEqual("One", File.ReadAllText(Path.Combine(this.targetPath, "A", "1", "one.txt")));
            Assert.AreEqual("Two", File.ReadAllText(Path.Combine(this.targetPath, "A", "two.txt")));
            Assert.AreEqual("Three", File.ReadAllText(Path.Combine(this.targetPath, "three.txt")));
        }
    }

    [TestClass]
    public class CopyFolderContentsPopulatedConflictTests : TestKit
    {
        private string sourcePath;
        private string targetPath;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            Directory.Delete(this.sourcePath, true);
            Directory.Delete(this.targetPath, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.sourcePath = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.sourcePath);

            Directory.CreateDirectory(Path.Combine(this.sourcePath, "A", "1"));
            Directory.CreateDirectory(Path.Combine(this.sourcePath, "A", "2"));
            Directory.CreateDirectory(Path.Combine(this.sourcePath, "A", "3"));
            Directory.CreateDirectory(Path.Combine(this.sourcePath, "B", "1"));
            Directory.CreateDirectory(Path.Combine(this.sourcePath, "B", "2"));
            File.WriteAllText(Path.Combine(this.sourcePath, "A", "1", "one.txt"), "One");
            File.WriteAllText(Path.Combine(this.sourcePath, "A", "two.txt"), "Two");
            File.WriteAllText(Path.Combine(this.sourcePath, "three.txt"), "Three");

            this.targetPath = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.targetPath);

            // Create conflicting entries
            Directory.CreateDirectory(Path.Combine(this.targetPath, "A", "2"));
            File.WriteAllText(Path.Combine(this.targetPath, "A", "two.txt"), "Conflict");
        }

        [TestMethod]
        public void Copy_populated_conflicting_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFolderContents(new ReadableFolder(this.sourcePath), new WritableFolder(this.targetPath)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);

            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "A", "1")));
            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "A", "2")));
            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "A", "3")));
            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "B", "1")));
            Assert.IsTrue(Directory.Exists(Path.Combine(this.targetPath, "B", "2")));
            Assert.AreEqual("One", File.ReadAllText(Path.Combine(this.targetPath, "A", "1", "one.txt")));
            Assert.AreEqual("Two", File.ReadAllText(Path.Combine(this.targetPath, "A", "two.txt")));
            Assert.AreEqual("Three", File.ReadAllText(Path.Combine(this.targetPath, "three.txt")));
        }
    }

    [TestClass]
    public class CopyFolderContentsSameTests : TestKit
    {
        private string targetPath;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            Directory.Delete(this.targetPath, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.targetPath = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.targetPath);
        }

        [TestMethod]
        public void No_action_copy_same()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFolderContents(new ReadableFolder(this.targetPath), new WritableFolder(Path.GetDirectoryName(this.targetPath))));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
        }
    }

    [TestClass]
    public class CopyFolderContentsOneMissingTests : TestKit
    {
        private string existingDirectory;
        private string missingDirectory;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            Directory.Delete(this.existingDirectory, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.existingDirectory = Path.Combine(Path.GetTempPath(), "existing_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.existingDirectory);

            this.missingDirectory = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Source_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFolderContents(new ReadableFolder(this.missingDirectory), new WritableFolder(this.existingDirectory)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }

        [TestMethod]
        public void Target_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFolderContents(new ReadableFolder(this.existingDirectory), new WritableFolder(this.missingDirectory)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
        }
    }

    [TestClass]
    public class CopyFolderContentsBothMissingTests : TestKit
    {
        private string sourceMissingDirectory;
        private string targetMissingDirectory;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
        }

        [TestInitialize]
        public void Initialise()
        {
            this.sourceMissingDirectory = Path.Combine(Path.GetTempPath(), "missing_source_" + Guid.NewGuid().ToString());
            this.targetMissingDirectory = Path.Combine(Path.GetTempPath(), "missing_target_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Source_and_target_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFolderContents(new ReadableFolder(this.sourceMissingDirectory), new WritableFolder(this.targetMissingDirectory)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
