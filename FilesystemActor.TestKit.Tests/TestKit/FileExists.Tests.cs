using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class FileExistsTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void File_doesnt_exist()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new ReadableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new SetupComplete());
            tk.Tell(new FileExists(file));
            var result = ExpectMsg<bool>();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void File_exists()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new ReadableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new CreateTestFile(file.Path));
            tk.Tell(new SetupComplete());
            tk.Tell(new FileExists(file));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
        }
    }
}
