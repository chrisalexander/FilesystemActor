using System;
using System.IO;
using System.Text;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class OverwriteExistingFileTests : TestKit
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
            this.existingFile = Path.Combine(Path.GetTempPath(), "exists_" + Guid.NewGuid().ToString());
            File.WriteAllText(this.existingFile, "Test");
        }

        [TestMethod]
        public void Can_overwrite_existing_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new OverwriteFile(new OverwritableFile(this.existingFile), "Test"));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(this.existingFile));
            Assert.AreEqual("Test", File.ReadAllText(this.existingFile));
        }
    }

    [TestClass]
    public class OverwriteMissingFileTests : TestKit
    {
        private string createFile;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.createFile);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.createFile = Path.Combine(Path.GetTempPath(), "create_" + Guid.NewGuid().ToString());
        }
        
        [TestMethod]
        public void Can_write_absent_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new OverwriteFile(new OverwritableFile(this.createFile), "Test"));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(this.createFile));
            Assert.AreEqual("Test", File.ReadAllText(this.createFile));
        }
    }

    [TestClass]
    public class OverwriteMissingFileStreamTests : TestKit
    {
        private string createFileStream;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.createFileStream);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.createFileStream = Path.Combine(Path.GetTempPath(), "createstream_" + Guid.NewGuid().ToString());
        }
        
        [TestMethod]
        public void Can_write_with_stream()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new OverwriteFile(new OverwritableFile(this.createFileStream), new MemoryStream(Encoding.ASCII.GetBytes("Test Weird ʣ Character"))));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(this.createFileStream));
            Assert.AreEqual("Test Weird ? Character", File.ReadAllText(this.createFileStream));
        }
    }

    [TestClass]
    public class OverwriteOpenFileTests : TestKit
    {
        private FileStream openFile;
        private string openFilePath;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            this.openFile.Close();
            File.Delete(this.openFilePath);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.openFilePath = Path.Combine(Path.GetTempPath(), "open_" + Guid.NewGuid().ToString());
            this.openFile = File.OpenWrite(this.openFilePath);
        }
        
        [TestMethod]
        public void Cant_overwrite_open_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new OverwriteFile(new OverwritableFile(this.openFilePath), "Test"));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
