using System.IO;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class DeleteFileTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Can_delete_existing_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new DeletableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new CreateTestFile(file.Path));
            tk.Tell(new SetupComplete());

            tk.Tell(new FileExists(file));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new DeleteFile(file));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FileExists(file));
            Assert.IsFalse(ExpectMsg<bool>());
        }

        [TestMethod]
        public void Can_delete_missing_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new DeletableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new CreateTestFolder(@"C:\users\test\folder\"));
            tk.Tell(new SetupComplete());

            tk.Tell(new FileExists(file));
            Assert.IsFalse(ExpectMsg<bool>());

            tk.Tell(new DeleteFile(file));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new FileExists(file));
            Assert.IsFalse(ExpectMsg<bool>());
        }

        [TestMethod]
        public void Cant_delete_file_in_missing_folder()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new DeletableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new SetupComplete());

            tk.Tell(new FileExists(file));
            Assert.IsFalse(ExpectMsg<bool>());

            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);
        }
    }
}
