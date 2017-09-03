using System.IO;
using System.Text;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class OverwriteFileTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Can_overwrite_existing_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new OverwritableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new CreateTestFile(file.Path, Encoding.ASCII.GetBytes("InitialContents")));
            tk.Tell(new SetupComplete());

            tk.Tell(new FileExists(file));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(file));
            Assert.AreEqual("InitialContents", ExpectMsg<FileContents>().AsAscii());

            tk.Tell(new OverwriteFile(file, "Updated"));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(file));
            Assert.AreEqual("Updated", ExpectMsg<FileContents>().AsAscii());
        }

        [TestMethod]
        public void Can_write_absent_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new OverwritableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new SetupComplete());

            tk.Tell(new FileExists(file));
            Assert.IsFalse(ExpectMsg<bool>());
            
            tk.Tell(new OverwriteFile(file, "Updated"));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(file));
            Assert.AreEqual("Updated", ExpectMsg<FileContents>().AsAscii());
        }

        [TestMethod]
        public void Can_overwrite_with_stream()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new OverwritableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new CreateTestFile(file.Path, Encoding.ASCII.GetBytes("InitialContents")));
            tk.Tell(new SetupComplete());

            tk.Tell(new FileExists(file));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(file));
            Assert.AreEqual("InitialContents", ExpectMsg<FileContents>().AsAscii());

            tk.Tell(new OverwriteFile(file, new MemoryStream(Encoding.ASCII.GetBytes("Test Weird ʣ Character"))));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new ReadFile(file));
            Assert.AreEqual("Test Weird ? Character", ExpectMsg<FileContents>().AsAscii());
        }

        [TestMethod]
        public void Cant_overwrite_locked_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new OverwritableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new CreateTestFile(file.Path, Encoding.ASCII.GetBytes("InitialContents"), true));
            tk.Tell(new SetupComplete());

            tk.Tell(new FileExists(file));
            Assert.IsTrue(ExpectMsg<bool>());

            tk.Tell(new OverwriteFile(file, "Updated"));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);

            tk.Tell(new ReadFile(file));
            Assert.AreEqual("InitialContents", ExpectMsg<FileContents>().AsAscii());
        }
    }
}
