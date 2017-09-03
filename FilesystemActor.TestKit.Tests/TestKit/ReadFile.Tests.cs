using System.IO;
using System.Linq;
using System.Text;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class ReadFileTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Read_existing_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new ReadableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new CreateTestFile(file.Path, Encoding.ASCII.GetBytes("Contents")));
            tk.Tell(new SetupComplete());

            tk.Tell(new ReadFile(file));
            Assert.IsTrue(Encoding.ASCII.GetBytes("Test").SequenceEqual(ExpectMsg<FileContents>().Bytes));
        }

        [TestMethod]
        public void Read_missing_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));
            var file = new ReadableFile(@"C:\users\test\folder\file.txt");
            tk.Tell(new SetupComplete());

            tk.Tell(new ReadFile(file));
            Assert.IsTrue(ExpectMsg<Failure>().Exception is IOException);
        }
    }
}
