using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class DeleteFileExistsTests : TestKit
    {
        private string existingFile;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.existingFile);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.existingFile = Path.Combine(Path.GetTempPath(), "existing_" + Guid.NewGuid().ToString());
            File.WriteAllText(this.existingFile, "Test");
        }

        [TestMethod]
        public void Can_delete_existing_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new DeleteFile(new DeletableFile(this.existingFile)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsFalse(File.Exists(this.existingFile));
        }
    }

    [TestClass]
    public class DeleteFileMissingTests : TestKit
    {
        private string missingFile;
        private string fileInMissingFolder;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
        }

        [TestInitialize]
        public void Initialise()
        {
            this.missingFile = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString());
            this.fileInMissingFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "missingfolder_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Can_delete_missing_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new DeleteFile(new DeletableFile(this.missingFile)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Cant_delete_file_in_missing_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new DeleteFile(new DeletableFile(this.fileInMissingFolder)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
