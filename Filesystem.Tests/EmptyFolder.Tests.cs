using System;
using System.IO;
using System.Text;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Tests
{
    [TestClass]
    public class EmptyFolderTests : TestKit
    {
        private string emptyFolder;
        private string populatedFolder;
        private string lockedFolder;

        private FileStream openFile;
        private string openFilePath;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();

            this.openFile.Close();
            File.Delete(this.openFilePath);

            Shutdown();
            try
            {
                Directory.Delete(this.emptyFolder, true);
            }
            catch { }

            try
            {
                Directory.Delete(this.populatedFolder, true);
            }
            catch { }

            try
            {
                Directory.Delete(this.lockedFolder, true);
            }
            catch { }
        }

        [TestInitialize]
        public void Initialise()
        {
            this.emptyFolder = Path.Combine(Path.GetTempPath(), "empty_" + Guid.NewGuid().ToString());
            this.populatedFolder = Path.Combine(Path.GetTempPath(), "populated_" + Guid.NewGuid().ToString());
            this.lockedFolder = Path.Combine(Path.GetTempPath(), "locked_" + Guid.NewGuid().ToString());

            Directory.CreateDirectory(this.emptyFolder);
            Directory.CreateDirectory(this.populatedFolder);
            Directory.CreateDirectory(this.lockedFolder);

            File.WriteAllText(Path.Combine(this.populatedFolder, "file1_" + Guid.NewGuid().ToString()), "file1");
            File.WriteAllText(Path.Combine(this.populatedFolder, "file2_" + Guid.NewGuid().ToString()), "file2");
            File.WriteAllText(Path.Combine(this.populatedFolder, "file3_" + Guid.NewGuid().ToString()), "file3");

            this.openFilePath = Path.Combine(this.lockedFolder, "locked_" + Guid.NewGuid().ToString());
            this.openFile = File.OpenWrite(this.openFilePath);
        }

        [TestMethod]
        public void Can_empty_empty_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new EmptyFolder(new DeletableFolder(this.emptyFolder)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsTrue(Directory.Exists(this.emptyFolder));
            Assert.AreEqual(0, Directory.GetFiles(this.emptyFolder).Length);
        }

        [TestMethod]
        public void Can_empty_populated_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new EmptyFolder(new DeletableFolder(this.populatedFolder)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsTrue(Directory.Exists(this.populatedFolder));
            Assert.AreEqual(0, Directory.GetFiles(this.populatedFolder).Length);
        }

        [TestMethod]
        public void Cant_empty_locked_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new EmptyFolder(new DeletableFolder(this.lockedFolder)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
            Assert.IsTrue(Directory.Exists(this.lockedFolder));
            Assert.AreNotEqual(0, Directory.GetFiles(this.lockedFolder).Length);
        }
    }
}
