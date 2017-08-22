using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.Tests
{
    [TestClass]
    public class EmptyFolderEmptyTests : TestKit
    {
        private string emptyFolder;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            Directory.Delete(this.emptyFolder, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.emptyFolder = Path.Combine(Path.GetTempPath(), "empty_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.emptyFolder);
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
    }

    [TestClass]
    public class EmptyFolderMissingTests : TestKit
    {
        private string missingFolder;

        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestInitialize]
        public void Initialise()
        {
            this.missingFolder = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Can_empty_missing_folder()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new EmptyFolder(new DeletableFolder(this.missingFolder)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsFalse(Directory.Exists(this.missingFolder));
        }
    }

    [TestClass]
    public class EmptyFolderPopulatedTests : TestKit
    {
        private string populatedFolder;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            Directory.Delete(this.populatedFolder, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.populatedFolder = Path.Combine(Path.GetTempPath(), "populated_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.populatedFolder);

            File.WriteAllText(Path.Combine(this.populatedFolder, "file1_" + Guid.NewGuid().ToString()), "file1");
            File.WriteAllText(Path.Combine(this.populatedFolder, "file2_" + Guid.NewGuid().ToString()), "file2");
            File.WriteAllText(Path.Combine(this.populatedFolder, "file3_" + Guid.NewGuid().ToString()), "file3");
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
    }

    [TestClass]
    public class EmptyFolderLockedTests : TestKit
    {
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
            Directory.Delete(this.lockedFolder, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.lockedFolder = Path.Combine(Path.GetTempPath(), "locked_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.lockedFolder);
            this.openFilePath = Path.Combine(this.lockedFolder, "locked_" + Guid.NewGuid().ToString());
            this.openFile = File.OpenWrite(this.openFilePath);
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
