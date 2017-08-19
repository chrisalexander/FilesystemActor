using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class ListContentsExistingTests : TestKit
    {
        private string existingDirectory;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            Directory.Delete(this.existingDirectory, true);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.existingDirectory = Path.Combine(Path.GetTempPath(), "exists_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.existingDirectory);

            Directory.CreateDirectory(Path.Combine(this.existingDirectory, "A"));
            Directory.CreateDirectory(Path.Combine(this.existingDirectory, "B"));
            File.WriteAllText(Path.Combine(this.existingDirectory, "1"), "1");
            File.WriteAllText(Path.Combine(this.existingDirectory, "2"), "2");
            File.WriteAllText(Path.Combine(this.existingDirectory, "3"), "3");
        }

        [TestMethod]
        public void Readable_folder_contents()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new ListReadableContents(new ReadableFolder(this.existingDirectory)));
            var result = ExpectMsg<FolderReadableContents>();
            Assert.AreEqual(2, result.Folders.Count);
            Assert.AreEqual(3, result.Files.Count);
            Assert.IsTrue(result.Folders[0].Path.EndsWith("A"));
            Assert.IsTrue(result.Folders[1].Path.EndsWith("B"));
            Assert.IsTrue(result.Files[0].Path.EndsWith("1"));
            Assert.IsTrue(result.Files[1].Path.EndsWith("2"));
            Assert.IsTrue(result.Files[2].Path.EndsWith("3"));
        }
        
        [TestMethod]
        public void Writable_folder_contents()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new ListWritableContents(new WritableFolder(this.existingDirectory)));
            var result = ExpectMsg<FolderWritableContents>();
            Assert.AreEqual(2, result.Folders.Count);
            Assert.AreEqual(3, result.Files.Count);
            Assert.IsTrue(result.Folders[0].Path.EndsWith("A"));
            Assert.IsTrue(result.Folders[1].Path.EndsWith("B"));
            Assert.IsTrue(result.Files[0].Path.EndsWith("1"));
            Assert.IsTrue(result.Files[1].Path.EndsWith("2"));
            Assert.IsTrue(result.Files[2].Path.EndsWith("3"));
        }
        
        [TestMethod]
        public void Deletable_folder_contents()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new ListDeletableContents(new DeletableFolder(this.existingDirectory)));
            var result = ExpectMsg<FolderDeletableContents>();
            Assert.AreEqual(2, result.Folders.Count);
            Assert.AreEqual(3, result.Files.Count);
            Assert.IsTrue(result.Folders[0].Path.EndsWith("A"));
            Assert.IsTrue(result.Folders[1].Path.EndsWith("B"));
            Assert.IsTrue(result.Files[0].Path.EndsWith("1"));
            Assert.IsTrue(result.Files[1].Path.EndsWith("2"));
            Assert.IsTrue(result.Files[2].Path.EndsWith("3"));
        }
    }

    [TestClass]
    public class ListContentsMissingTests : TestKit
    {
        private string missingDirectory;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
        }

        [TestInitialize]
        public void Initialise()
        {
            this.missingDirectory = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString());
        }
        
        [TestMethod]
        public void Readable_missing_folder_contents()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new ListReadableContents(new ReadableFolder(this.missingDirectory)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
        
        [TestMethod]
        public void Writable_missing_folder_contents()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new ListWritableContents(new WritableFolder(this.missingDirectory)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
        
        [TestMethod]
        public void Deletable_missing_folder_contents()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new ListDeletableContents(new DeletableFolder(this.missingDirectory)));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
