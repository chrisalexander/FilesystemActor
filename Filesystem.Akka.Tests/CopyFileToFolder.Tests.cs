using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class CopyFileToFolderTests : TestKit
    {
        private string sourceFile;
        private string targetPath;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.sourceFile);
            Directory.Delete(this.targetPath, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.sourceFile = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString() + ".txt");
            File.WriteAllText(this.sourceFile, "Test");

            this.targetPath = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.targetPath);
        }

        [TestMethod]
        public void Copy_file_to_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFolder(this.targetPath)));
            var result = ExpectMsg<WritableFile>();
            Assert.AreEqual(Path.Combine(this.targetPath, Path.GetFileName(this.sourceFile)), result.Path);
            Assert.AreEqual("Test", File.ReadAllText(result.Path));
        }
    }

    [TestClass]
    public class CopyFileToFolderConflictTests : TestKit
    {
        private string sourceFile;
        private string targetPath;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.sourceFile);
            Directory.Delete(this.targetPath, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.sourceFile = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString() + ".txt");
            File.WriteAllText(this.sourceFile, "Test");

            this.targetPath = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.targetPath);
            File.WriteAllText(Path.Combine(this.targetPath, Path.GetFileName(this.sourceFile)), "Conflicting");
        }

        [TestMethod]
        public void Copy_file_to_folder_conflict()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFolder(this.targetPath)));
            var result = ExpectMsg<WritableFile>();
            Assert.AreEqual(Path.Combine(this.targetPath, Path.GetFileName(this.sourceFile)), result.Path);
            Assert.AreEqual("Test", File.ReadAllText(result.Path));
        }
    }
    
    [TestClass]
    public class CopyFileToFolderFileMissingTests : TestKit
    {
        private string sourceFile;
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
            this.sourceFile = Path.Combine(Path.GetTempPath(), "source_" + Guid.NewGuid().ToString() + ".txt");

            this.targetPath = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.targetPath);
        }

        [TestMethod]
        public void Copy_file_to_folder_file_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFolder(this.targetPath)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }

    [TestClass]
    public class CopyFileToFolderFolderMissingTests : TestKit
    {
        private string sourceFile;
        private string targetPath;

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
            File.WriteAllText(this.sourceFile, "Test");

            this.targetPath = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Copy_file_to_folder_folder_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFolder(this.targetPath)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }

    [TestClass]
    public class CopyFileToFolderBothMissingTests : TestKit
    {
        private string sourceFile;
        private string targetPath;

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
            this.targetPath = Path.Combine(Path.GetTempPath(), "target_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Copy_file_to_folder_both_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CopyFile(new ReadableFile(this.sourceFile), new WritableFolder(this.targetPath)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
