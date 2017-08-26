using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class FolderExistsTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Folder_exists_not_setup()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new ReadableFolder(@"C:\users\test\folder");
            tk.Tell(new FolderExists(folder));
            ExpectNoMsg();
        }

        [TestMethod]
        public void Folder_doesnt_exist()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new ReadableFolder(@"C:\users\test\folder");
            tk.Tell(new SetupComplete());
            tk.Tell(new FolderExists(folder));
            var result = ExpectMsg<bool>();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Folder_exists()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new ReadableFolder(@"C:\users\test\folder");
            tk.Tell(new CreateTestFolder(folder.Path));
            tk.Tell(new SetupComplete());
            tk.Tell(new FolderExists(folder));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
        }
    }
}
