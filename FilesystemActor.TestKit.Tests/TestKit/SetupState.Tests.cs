using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class SetupStateTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Not_setup()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new ReadableFolder(@"C:\users\test\folder");
            tk.Tell(new FolderExists(folder));
            ExpectNoMsg();
        }

        [TestMethod]
        public void Setup()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new ReadableFolder(@"C:\users\test\folder");
            tk.Tell(new SetupComplete());
            tk.Tell(new FolderExists(folder));
            var result = ExpectMsg<bool>();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Un_setup()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var folder = new ReadableFolder(@"C:\users\test\folder");
            tk.Tell(new SetupComplete());
            tk.Tell(new EnterSetup());
            tk.Tell(new FolderExists(folder));
            ExpectNoMsg();
        }
    }
}
