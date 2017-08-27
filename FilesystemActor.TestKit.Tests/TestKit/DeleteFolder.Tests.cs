using System.IO;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class DeleteFolderTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Can_delete_empty_folder_non_recursive()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new DeletableFolder(@"\\data\test\folder\");
            tk.Tell(new CreateTestFolder(folder.Path));
            tk.Tell(new SetupComplete());

            tk.Tell(new DeleteFolder(folder));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(folder));
            Assert.IsFalse(ExpectMsg<bool>());
        }

        [TestMethod]
        public void Can_delete_empty_folder_recursive()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new DeletableFolder(@"\\data\test\folder\");
            tk.Tell(new CreateTestFolder(folder.Path));
            tk.Tell(new SetupComplete());

            tk.Tell(new DeleteFolder(folder) { Recursive = true });
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(folder));
            Assert.IsFalse(ExpectMsg<bool>());
        }

        [TestMethod]
        public void Cant_delete_populated_folder_non_recursive()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new DeletableFolder(@"\\data\test\folder\");
            tk.Tell(new CreateTestFolder(folder.Path));
            tk.Tell(new CreateTestFolder(Path.Combine(folder.Path, "A")));
            tk.Tell(new CreateTestFile(Path.Combine(folder.Path, "A", "file.txt")));
            tk.Tell(new SetupComplete());

            tk.Tell(new DeleteFolder(folder));
            Assert.IsTrue(ExpectMsg<Failure>().Exception is IOException);

            tk.Tell(new FolderExists(folder));
            Assert.IsTrue(ExpectMsg<bool>());
        }

        [TestMethod]
        public void Can_delete_populated_folder_recursive()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new DeletableFolder(@"\\data\test\folder\");
            tk.Tell(new CreateTestFolder(folder.Path));
            tk.Tell(new CreateTestFolder(Path.Combine(folder.Path, "A")));
            tk.Tell(new CreateTestFile(Path.Combine(folder.Path, "A", "file.txt")));
            tk.Tell(new SetupComplete());

            tk.Tell(new DeleteFolder(folder) { Recursive = true });
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(folder));
            Assert.IsFalse(ExpectMsg<bool>());

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(folder.Path, "A"))));
            Assert.IsFalse(ExpectMsg<bool>());

            tk.Tell(new FileExists(new ReadableFile(Path.Combine(folder.Path, "A", "file.txt"))));
            Assert.IsFalse(ExpectMsg<bool>());
        }
    }
}
