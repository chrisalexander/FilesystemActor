using System.IO;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class CreateFolderTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Create_folder()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var parentFolder = new WritableFolder(@"C:\users\test\folder\");
            var folderName = "Test";
            tk.Tell(new SetupComplete());

            tk.Tell(new CreateFolder(parentFolder, folderName));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(parentFolder.Path, folderName))));
            Assert.IsTrue(ExpectMsg<bool>());
        }

        [TestMethod]
        public void Create_folder_already_exists()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var parentFolder = new WritableFolder(@"C:\users\test\folder\");
            var folderName = "Test";
            tk.Tell(new CreateTestFolder(Path.Combine(parentFolder.Path, folderName)));
            tk.Tell(new SetupComplete());

            tk.Tell(new CreateFolder(parentFolder, folderName));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);

            tk.Tell(new FolderExists(new ReadableFolder(Path.Combine(parentFolder.Path, folderName))));
            Assert.IsTrue(ExpectMsg<bool>());
        }
    }
}
