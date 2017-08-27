using System;
using System.IO;
using System.Linq;
using System.Text;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.Tests
{
    [TestClass]
    public class ReadFileExistingTests : TestKit
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
        public void Read_existing_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new ReadFile(new ReadableFile(this.existingFile)));
            var result = ExpectMsg<FileContents>();
            Assert.IsTrue(Encoding.ASCII.GetBytes("Test").SequenceEqual(result.Bytes));
        }
    }

    [TestClass]
    public class ReadFileMissingTests : TestKit
    {
        private string missingFile;

        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestInitialize]
        public void Initialise()
        {
            this.missingFile = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void Read_missing_file()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new ReadFile(new ReadableFile(this.missingFile)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}