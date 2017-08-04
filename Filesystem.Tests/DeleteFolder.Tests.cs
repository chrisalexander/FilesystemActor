using System;
using System.IO;
using System.Text;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Tests
{
    [TestClass]
    public class DeleteFolderTests : TestKit
    {
        private string emptyFolder;
        private string populatedFolder;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            try
            {
                Directory.Delete(this.emptyFolder, true);
            }
            catch { }

            try
            {
                Directory.Delete(this.populatedFolder, true);
            } catch { }
        }

        [TestInitialize]
        public void Initialise()
        {
            this.emptyFolder = Path.Combine(Path.GetTempPath(), "empty_" + Guid.NewGuid().ToString());
            this.populatedFolder = Path.Combine(Path.GetTempPath(), "populated_" + Guid.NewGuid().ToString());

            Directory.CreateDirectory(emptyFolder);
            Directory.CreateDirectory(populatedFolder);

            File.WriteAllText(Path.Combine(this.populatedFolder, "file"), "file");
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

        [TestMethod]
        public void Cant_delete_populated_folder_non_recursive()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new DeleteFolder(new DeletableFolder(this.populatedFolder)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
            Assert.IsTrue(Directory.Exists(this.populatedFolder));
        }

        [TestMethod]
        public void Can_delete_populated_folder_recursive()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new DeleteFolder(new DeletableFolder(this.populatedFolder)) { Recursive = true });
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsFalse(Directory.Exists(this.populatedFolder));
        }
    }
}
