using System.IO;
using System.Text;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class WriteFileTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Cant_overwrite_existing_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new WritableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new CreateTestFile(file.Path));
            tk.Tell(new SetupComplete());

            tk.Tell(new WriteFile(file, "Test"));
            Assert.IsTrue(ExpectMsg<Failure>().Exception is IOException);
        }

        [TestMethod]
        public void Can_write_absent_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new WritableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new SetupComplete());

            tk.Tell(new WriteFile(file, "Test"));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(file));
            Assert.AreEqual("Test", ExpectMsg<FileContents>().AsAscii());
        }

        [TestMethod]
        public void Can_write_with_stream()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new WritableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new SetupComplete());

            tk.Tell(new WriteFile(file, new MemoryStream(Encoding.ASCII.GetBytes("Test Weird ʣ Character"))));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(file));
            Assert.AreEqual("Test Weird ? Character", ExpectMsg<FileContents>().AsAscii());
        }
    }
}
