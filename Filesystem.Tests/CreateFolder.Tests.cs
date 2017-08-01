using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Tests
{
    [TestClass]
    public class CreateFolderTests : TestKit
    {
        private WritableFolder folder;
        private string folderName;
        private string expectedFullPath;

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(this.expectedFullPath);
            Shutdown();
        }

        [TestInitialize]
        public void Initialise()
        {
            this.folder = new WritableFolder(Path.GetTempPath());
            this.folderName = "TestFolder";
            this.expectedFullPath = Path.Combine(this.folder.Path, this.folderName);
        }

        [TestMethod]
        public void Create_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new CreateFolder(this.folder, this.folderName));
            var result = ExpectMsg<WritableFolder>();
            Assert.AreEqual(result.Path, this.expectedFullPath);
            Assert.IsTrue(Directory.Exists(this.expectedFullPath));
        }
    }
}
