using System.IO;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class ListContentsTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        private string Setup(IActorRef actor)
        {
            var path = @"F:\Data\Folder";

            actor.Tell(new CreateTestFolder(path));
            actor.Tell(new CreateTestFolder(Path.Combine(path, "A")));
            actor.Tell(new CreateTestFolder(Path.Combine(path, "B")));
            actor.Tell(new CreateTestFile(Path.Combine(path, "1", "1")));
            actor.Tell(new CreateTestFile(Path.Combine(path, "1", "2")));
            actor.Tell(new CreateTestFile(Path.Combine(path, "1", "3")));

            return path;
        }

        [TestMethod]
        public void Readable_folder_contents()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var path = Setup(tk);
            tk.Tell(new SetupComplete());

            tk.Tell(new ListReadableContents(new ReadableFolder(path)));
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
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var path = Setup(tk);
            tk.Tell(new SetupComplete());

            tk.Tell(new ListWritableContents(new WritableFolder(path)));
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
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var path = Setup(tk);
            tk.Tell(new SetupComplete());

            tk.Tell(new ListDeletableContents(new DeletableFolder(path)));
            var result = ExpectMsg<FolderDeletableContents>();
            Assert.AreEqual(2, result.Folders.Count);
            Assert.AreEqual(3, result.Files.Count);
            Assert.IsTrue(result.Folders[0].Path.EndsWith("A"));
            Assert.IsTrue(result.Folders[1].Path.EndsWith("B"));
            Assert.IsTrue(result.Files[0].Path.EndsWith("1"));
            Assert.IsTrue(result.Files[1].Path.EndsWith("2"));
            Assert.IsTrue(result.Files[2].Path.EndsWith("3"));
        }

        [TestMethod]
        public void Readable_missing_contents()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            tk.Tell(new SetupComplete());

            tk.Tell(new ListReadableContents(new ReadableFolder(@"C:\missing")));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }

        [TestMethod]
        public void Writable_missing_contents()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            tk.Tell(new SetupComplete());

            tk.Tell(new ListWritableContents(new WritableFolder(@"C:\missing")));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }

        [TestMethod]
        public void Deletable_missing_contents()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            tk.Tell(new SetupComplete());

            tk.Tell(new ListDeletableContents(new DeletableFolder(@"C:\missing")));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
