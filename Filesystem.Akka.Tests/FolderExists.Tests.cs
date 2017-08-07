using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Tests
{
    [TestClass]
    public class FolderExistsTests : TestKit
    {
        private string existingDirectory;
        private string missingDirectory;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            Directory.Delete(this.existingDirectory);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.existingDirectory = Path.Combine(Path.GetTempPath(), "exists_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.existingDirectory);
            this.missingDirectory = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Folder_exists()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new FolderExists(new ReadableFolder(this.existingDirectory)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Folder_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new FolderExists(new ReadableFolder(this.missingDirectory)));
            var result = ExpectMsg<bool>();
            Assert.IsFalse(result);
        }
    }
}
