using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class CopyFileToFileTests : TestKit
    {
        private string sourceFile;
        private string targetFile;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.sourceFile);
            File.Delete(this.targetFile);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.sourceFile = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString() + ".txt");
            File.WriteAllText(this.sourceFile, "Source");

            this.targetFile = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString() + ".txt");
        }

        [TestMethod]
        public void Copy_file_to_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFile(this.targetFile)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.AreEqual("Source", File.ReadAllText(this.targetFile));
        }
    }

    [TestClass]
    public class CopyFileToFileConflictTests : TestKit
    {
        private string sourceFile;
        private string targetFile;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.sourceFile);
            File.Delete(this.targetFile);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.sourceFile = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString() + ".txt");
            File.WriteAllText(this.sourceFile, "Source");

            this.targetFile = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString() + ".txt");
            File.WriteAllText(this.targetFile, "Target");
        }

        [TestMethod]
        public void Copy_file_to_file_conflict()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFile(this.targetFile)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.AreEqual("Source", File.ReadAllText(this.targetFile));
        }
    }

    [TestClass]
    public class CopyFileToFileSourceMissingTests : TestKit
    {
        private string sourceFile;
        private string targetFile;

        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestInitialize]
        public void Initialise()
        {
            this.sourceFile = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString() + ".txt");
            this.targetFile = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString() + ".txt");
        }

        [TestMethod]
        public void Copy_file_to_file_source_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFile(this.targetFile)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }

    [TestClass]
    public class CopyFileToFileFolderMissingTests : TestKit
    {
        private string sourceFile;
        private string targetFile;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.sourceFile);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.sourceFile = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString() + ".txt");
            File.WriteAllText(this.sourceFile, "Source");

            this.targetFile = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString(), "target_" + Guid.NewGuid().ToString() + ".txt");
        }

        [TestMethod]
        public void Copy_file_to_file_target_directory_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFile(this.targetFile)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }

    [TestClass]
    public class CopyFileToFileBothMissingTests : TestKit
    {
        private string sourceFile;
        private string targetFile;

        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestInitialize]
        public void Initialise()
        {
            this.sourceFile = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString() + ".txt");
            this.targetFile = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString(), "target_" + Guid.NewGuid().ToString() + ".txt");
        }

        [TestMethod]
        public void Copy_file_to_file_target_directory_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFile(this.targetFile)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
