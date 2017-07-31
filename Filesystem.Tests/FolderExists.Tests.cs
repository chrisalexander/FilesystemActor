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
            Directory.Delete(this.existingDirectory);
            Shutdown();
        }

        [TestInitialize]
        public void Initialise()
        {
            this.existingDirectory = Path.Combine(Path.GetTempPath(), "exists");
            Directory.CreateDirectory(this.existingDirectory);
            this.missingDirectory = Path.Combine(this.existingDirectory, "_missing");
        }

        [TestMethod]
        public void Folder_exists()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new FolderExists(new Folder(this.existingDirectory)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Folder_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new FolderExists(new Folder(this.missingDirectory)));
            var result = ExpectMsg<bool>();
            Assert.IsFalse(result);
        }
    }
}
