using System;
using System.IO;
using System.Text;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class OverwriteFileTests : TestKit
    {
        private string existingFile;
        private string createFile;
        private string createFileStream;
        private FileStream openFile;
        private string openFilePath;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.existingFile);
            File.Delete(this.createFile);
            File.Delete(this.createFileStream);
            this.openFile.Close();
            File.Delete(this.openFilePath);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.existingFile = Path.Combine(Path.GetTempPath(), "exists_" + Guid.NewGuid().ToString());
            File.WriteAllText(this.existingFile, "Test");
            this.createFile = Path.Combine(Path.GetTempPath(), "create_" + Guid.NewGuid().ToString());
            this.createFileStream = Path.Combine(Path.GetTempPath(), "createstream_" + Guid.NewGuid().ToString());

            this.openFilePath = Path.Combine(Path.GetTempPath(), "open_" + Guid.NewGuid().ToString());
            this.openFile = File.OpenWrite(this.openFilePath);
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
