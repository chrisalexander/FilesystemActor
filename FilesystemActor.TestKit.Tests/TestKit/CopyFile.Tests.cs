using System.IO;
using System.Text;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.TestKit
{
    [TestClass]
    public class CopyFileTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Copy_file_to_file()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new CreateTestFile(sourceFile.Path, Encoding.ASCII.GetBytes("TestString")));
            tk.Tell(new CreateTestFolder(@"\\shared\folder\"));
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsTrue(result2);

            tk.Tell(new ReadFile(targetFile));
            var result3 = ExpectMsg<FileContents>();
            Assert.AreEqual("TestString", result3.AsAscii());
        }

        [TestMethod]
        public void Copy_file_to_file_conflict()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new CreateTestFile(sourceFile.Path, Encoding.ASCII.GetBytes("TestString")));
            tk.Tell(new CreateTestFile(targetFile.Path, Encoding.ASCII.GetBytes("ConflictTestString")));
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsTrue(result2);

            tk.Tell(new ReadFile(targetFile));
            var result3 = ExpectMsg<FileContents>();
            Assert.AreEqual("TestString", result3.AsAscii());
        }

        [TestMethod]
        public void Copy_file_to_file_source_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new CreateTestFolder(@"\\shared\folder\"));
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void Copy_file_to_file_target_folder_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new CreateTestFile(sourceFile.Path, Encoding.ASCII.GetBytes("TestString")));
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void Copy_file_to_file_both_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsFalse(result2);
        }
    }

    [TestClass]
    public class CopyFileToFolderTests : Akka.TestKit.VsTest.TestKit
    {
        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestMethod]
        public void Copy_file_to_folder()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new CreateTestFile(sourceFile.Path, Encoding.ASCII.GetBytes("TestString")));
            tk.Tell(new CreateTestFolder(@"\\shared\folder\"));
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsTrue(result2);

            tk.Tell(new ReadFile(targetFile));
            var result3 = ExpectMsg<FileContents>();
            Assert.AreEqual("TestString", result3.AsAscii());
        }

        [TestMethod]
        public void Copy_file_to_folder_conflict()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new CreateTestFile(sourceFile.Path, Encoding.ASCII.GetBytes("TestString")));
            tk.Tell(new CreateTestFile(targetFile.Path, Encoding.ASCII.GetBytes("ConflictTestString")));
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsTrue(result2);

            tk.Tell(new ReadFile(targetFile));
            var result3 = ExpectMsg<FileContents>();
            Assert.AreEqual("TestString", result3.AsAscii());
        }

        [TestMethod]
        public void Copy_file_to_folder_file_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new CreateTestFolder(@"\\shared\folder\"));
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void Copy_file_to_folder_folder_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new CreateTestFile(sourceFile.Path, Encoding.ASCII.GetBytes("TestString")));
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void Copy_file_to_folder_both_missing()
        {
            var tk = Sys.ActorOf(Props.Create(() => new FilesystemTestKit()));

            var sourceFile = new ReadableFile(@"C:\users\test\folder\file.txt");
            var targetFile = new OverwritableFile(@"\\shared\folder\file2.txt");
            tk.Tell(new SetupComplete());

            tk.Tell(new CopyFile(sourceFile, targetFile));
            var result = ExpectMsg<Failure>();
            Assert.IsTrue(result.Exception is IOException);

            tk.Tell(new FileExists(targetFile));
            var result2 = ExpectMsg<bool>();
            Assert.IsFalse(result2);
        }
    }
}
