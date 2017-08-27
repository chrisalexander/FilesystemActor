using System.IO;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class EmptyFolderTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Can_empty_empty_folder()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new DeletableFolder(@"c:\data\folder");
            tk.Tell(new CreateTestFolder(folder.Path));
            tk.Tell(new SetupComplete());

            tk.Tell(new ListReadableContents(folder));
            var result = ExpectMsg<FolderReadableContents>();
            Assert.AreEqual(0, result.Folders);
            Assert.AreEqual(0, result.Files);

            tk.Tell(new EmptyFolder(folder));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(folder));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ListReadableContents(folder));
            var result2 = ExpectMsg<FolderReadableContents>();
            Assert.AreEqual(0, result2.Folders);
            Assert.AreEqual(0, result2.Files);
        }

        [TestMethod]
        public void Can_empty_missing_folder()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new DeletableFolder(@"c:\data\folder");
            tk.Tell(new SetupComplete());

            tk.Tell(new FolderExists(folder));
            Assert.IsFalse(ExpectMsg<bool>());

            tk.Tell(new EmptyFolder(folder));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(folder));
            Assert.IsFalse(ExpectMsg<bool>());
        }

        [TestMethod]
        public void Can_empty_populated_folder()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new DeletableFolder(@"c:\data\folder");
            tk.Tell(new CreateTestFolder(folder.Path));
            tk.Tell(new CreateTestFolder(Path.Combine(folder.Path, "A")));
            tk.Tell(new CreateTestFile(Path.Combine(folder.Path, "A", "file1.txt")));
            tk.Tell(new CreateTestFile(Path.Combine(folder.Path, "file2.txt")));
            tk.Tell(new SetupComplete());

            tk.Tell(new ListReadableContents(folder));
            var result = ExpectMsg<FolderReadableContents>();
            Assert.AreEqual(1, result.Folders);
            Assert.AreEqual(1, result.Files);

            tk.Tell(new EmptyFolder(folder));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(folder));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(folder.ChildFolder("A")));
            Assert.IsFalse(ExpectMsg<bool>());

            tk.Tell(new FileExists(folder.File("file2.txt")));
            Assert.IsFalse(ExpectMsg<bool>());

            tk.Tell(new ListReadableContents(folder));
            var result2 = ExpectMsg<FolderReadableContents>();
            Assert.AreEqual(0, result2.Folders);
            Assert.AreEqual(0, result2.Files);
        }

        [TestMethod]
        public void Cant_empty_locked_folder()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new DeletableFolder(@"c:\data\folder");
            tk.Tell(new CreateTestFolder(folder.Path));
            tk.Tell(new CreateTestFile(Path.Combine(folder.Path, "file.txt"), string.Empty, true));
            tk.Tell(new SetupComplete());

            tk.Tell(new ListReadableContents(folder));
            var result = ExpectMsg<FolderReadableContents>();
            Assert.AreEqual(0, result.Folders);
            Assert.AreEqual(1, result.Files);

            tk.Tell(new EmptyFolder(folder));
            Assert.IsTrue(ExpectMsg<Failure>().Exception is IOException);

            tk.Tell(new FolderExists(folder));
            Assert.IsTrue(ExpectMsg<bool>());
            
            tk.Tell(new FileExists(folder.File("file.txt")));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ListReadableContents(folder));
            var result2 = ExpectMsg<FolderReadableContents>();
            Assert.AreEqual(0, result2.Folders);
            Assert.AreEqual(1, result2.Files);
        }
    }
}
