using System;
using System.IO;
using System.Text;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class WriteFileTests : TestKit
    {
        private string existingFile;
        private string createFile;
        private string createFileStream;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.existingFile);
            File.Delete(this.createFile);
            File.Delete(this.createFileStream);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.existingFile = Path.Combine(Path.GetTempPath(), "exists_" + Guid.NewGuid().ToString());
            File.WriteAllText(this.existingFile, "Test");
            this.createFile = Path.Combine(Path.GetTempPath(), "create_" + Guid.NewGuid().ToString());
            this.createFileStream = Path.Combine(Path.GetTempPath(), "createstream_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Cant_overwrite_existing_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new WriteFile(new WritableFile(this.existingFile), "Test"));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }

        [TestMethod]
        public void Can_write_absent_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new WriteFile(new WritableFile(this.createFile), "Test"));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(this.createFile));
            Assert.AreEqual("Test", File.ReadAllText(this.createFile));
        }

        [TestMethod]
        public void Can_write_with_stream()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new WriteFile(new WritableFile(this.createFileStream), new MemoryStream(Encoding.ASCII.GetBytes("Test Weird ʣ Character"))));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(this.createFileStream));
            Assert.AreEqual("Test Weird ? Character", File.ReadAllText(this.createFileStream));
        }
    }
}
