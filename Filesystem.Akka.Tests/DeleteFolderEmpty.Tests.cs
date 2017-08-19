using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class DeleteFolderEmptyTests : TestKit
    {
        private string emptyFolder;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            try
            {
                Directory.Delete(this.emptyFolder, true);
            }
            catch { }
        }

        [TestInitialize]
        public void Initialise()
        {
            this.emptyFolder = Path.Combine(Path.GetTempPath(), "empty_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.emptyFolder);
        }

        [TestMethod]
        public void Can_delete_empty_folder_non_recursive()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new DeleteFolder(new DeletableFolder(this.emptyFolder)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsFalse(Directory.Exists(this.emptyFolder));
        }

        [TestMethod]
        public void Can_delete_empty_folder_recursive()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new DeleteFolder(new DeletableFolder(this.emptyFolder)) { Recursive = true });
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsFalse(Directory.Exists(this.emptyFolder));
        }
    }
}
