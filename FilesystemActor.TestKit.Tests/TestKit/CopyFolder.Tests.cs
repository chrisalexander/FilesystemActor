using System.IO;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class CopyFolderTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Copy_populated_folder()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var rootFolder = new ReadableFolder(@"\\shared\source\folder\");
            var targetFolder = new WritableFolder(@"D:\Target\");
            tk.Tell(new CreateTestFolder(rootFolder.Path));
            tk.Tell(new CreateTestFolder(targetFolder.Path));
            tk.Tell(new CreateTestFolder(Path.Combine(rootFolder.Path, "A", "1")));
            tk.Tell(new CreateTestFolder(Path.Combine(rootFolder.Path, "A", "2")));
            tk.Tell(new CreateTestFolder(Path.Combine(rootFolder.Path, "B", "1")));
            tk.Tell(new CreateTestFolder(Path.Combine(rootFolder.Path, "B", "2")));
            tk.Tell(new CreateTestFile(Path.Combine(rootFolder.Path, "A", "1", "one.txt"), "One"));
            tk.Tell(new CreateTestFile(Path.Combine(rootFolder.Path, "A", "two.txt"), "Two"));
            tk.Tell(new CreateTestFile(Path.Combine(rootFolder.Path, "three.txt"), "Three"));

            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFolder(rootFolder, targetFolder));

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(targetFolder.Path, "folder", "A", "1"))));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(targetFolder.Path, "folder", "A", "2"))));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(targetFolder.Path, "folder", "A"))));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(targetFolder.Path, "folder", "B", "1"))));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(targetFolder.Path, "folder", "B", "2"))));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(targetFolder.Path, "folder", "B"))));
            Assert.IsTrue(ExpectMsg<bool>());

            var fileOne = new ReadableFile(Path.Combine(targetFolder.Path, "folder", "A", "1", "one.txt"));
            tk.Tell(new FileExists(fileOne));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(fileOne));
            Assert.AreEqual("One", ExpectMsg<FileContents>().AsAscii());

            var fileTwo = new ReadableFile(Path.Combine(targetFolder.Path, "folder", "A", "two.txt"));
            tk.Tell(new FileExists(fileTwo));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(fileTwo));
            Assert.AreEqual("Two", ExpectMsg<FileContents>().AsAscii());

            var fileThree = new ReadableFile(Path.Combine(targetFolder.Path, "folder", "three.txt"));
            tk.Tell(new FileExists(fileThree));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(fileThree));
            Assert.AreEqual("Three", ExpectMsg<FileContents>().AsAscii());
        }

        [TestMethod]
        public void Copy_folder_same()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var targetFolder = new WritableFolder(@"D:\Target\");
            tk.Tell(new CreateTestFolder(targetFolder.Path));
            tk.Tell(new CreateTestFile(Path.Combine(targetFolder.Path, "A", "1", "one.txt"), "One"));

            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFolder(targetFolder, targetFolder));

            // Just check the file is still there
            tk.Tell(new ReadFile(new ReadableFile(Path.Combine(targetFolder.Path, "A", "1", "one.txt"))));
            Assert.AreEqual("One", ExpectMsg<FileContents>().AsAscii());
        }

        [TestMethod]
        public void Copy_folder_source_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var rootFolder = new ReadableFolder(@"\\shared\source\folder\");
            var targetFolder = new WritableFolder(@"D:\Target\");

            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFolder(rootFolder, targetFolder));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }

        [TestMethod]
        public void Copy_folder_target_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var rootFolder = new ReadableFolder(@"\\shared\source\folder\");
            var targetFolder = new WritableFolder(@"D:\Target\");
            tk.Tell(new CreateTestFolder(rootFolder.Path));
            tk.Tell(new CreateTestFolder(Path.Combine(rootFolder.Path, "A", "1")));
            tk.Tell(new CreateTestFile(Path.Combine(rootFolder.Path, "A", "1", "one.txt"), "One"));

            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFolder(rootFolder, targetFolder));

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(targetFolder.Path, "folder", "A", "1"))));
            Assert.IsTrue(ExpectMsg<bool>());
            
            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(targetFolder.Path, "folder", "A"))));
            Assert.IsTrue(ExpectMsg<bool>());

            var fileOne = new ReadableFile(Path.Combine(targetFolder.Path, "folder", "A", "1", "one.txt"));
            tk.Tell(new FileExists(fileOne));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(fileOne));
            Assert.AreEqual("One", ExpectMsg<FileContents>().AsAscii());
        }

        [TestMethod]
        public void Copy_folder_both_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var rootFolder = new ReadableFolder(@"\\shared\source\folder\");
            var targetFolder = new WritableFolder(@"D:\Target\");

            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFolder(rootFolder, targetFolder));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
