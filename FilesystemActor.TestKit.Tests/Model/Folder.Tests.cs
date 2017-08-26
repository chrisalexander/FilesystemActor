using System;
using Filesystem.Akka.TestKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.Model
{
    [TestClass]
    public class FolderTests
    {
        [TestMethod]
        public void Create_folder_name()
        {
            var name = Guid.NewGuid().ToString();
            var folder = new Folder(name);
            Assert.AreEqual(name, folder.Name);
        }

        [TestMethod]
        public void Create_no_subfolder()
        {
            var folder = new Folder("Root");
            folder.CreateFolder(new string[] { });

            Assert.AreEqual(0, folder.Folders.Count);
        }

        [TestMethod]
        public void Create_simple_subfolder()
        {
            var folder = new Folder("Root");
            folder.CreateFolder(new[] { "Subfolder" });

            Assert.IsTrue(folder.Folders.ContainsKey("Subfolder"));
            Assert.AreEqual("Subfolder", folder.Folders["Subfolder"].Name);
            Assert.AreEqual(1, folder.Folders.Count);
            Assert.AreEqual(0, folder.Files.Count);
        }

        [TestMethod]
        public void Create_recursive_subfolder()
        {
            var folder = new Folder("Root");
            folder.CreateFolder(new[] { "Subfolder1", "Subfolder2", "Subfolder3" });

            Assert.IsTrue(folder.Folders.ContainsKey("Subfolder1"));
            Assert.AreEqual("Subfolder1", folder.Folders["Subfolder1"].Name);
            Assert.AreEqual(1, folder.Folders.Count);
            Assert.AreEqual(0, folder.Files.Count);

            var subfolder1 = folder.Folders["Subfolder1"];
            Assert.IsTrue(subfolder1.Folders.ContainsKey("Subfolder2"));
            Assert.AreEqual("Subfolder2", subfolder1.Folders["Subfolder2"].Name);
            Assert.AreEqual(1, subfolder1.Folders.Count);
            Assert.AreEqual(0, subfolder1.Files.Count);

            var subfolder2 = subfolder1.Folders["Subfolder2"];
            Assert.IsTrue(subfolder2.Folders.ContainsKey("Subfolder3"));
            Assert.AreEqual("Subfolder3", subfolder2.Folders["Subfolder3"].Name);
            Assert.AreEqual(1, subfolder2.Folders.Count);
            Assert.AreEqual(0, subfolder2.Files.Count);

            var subfolder3 = subfolder2.Folders["Subfolder3"];
            Assert.AreEqual(0, subfolder3.Folders.Count);
            Assert.AreEqual(0, subfolder3.Files.Count);
        }

        [TestMethod]
        public void Create_simple_file()
        {
            var folder = new Folder("Root");
            folder.CreateFile(new string[] { }, "TestFile.txt");

            Assert.IsTrue(folder.Files.ContainsKey("TestFile.txt"));
            Assert.AreEqual("TestFile.txt", folder.Files["TestFile.txt"].Name);
            Assert.AreEqual(1, folder.Files.Count);
            Assert.AreEqual(0, folder.Folders.Count);
        }

        [TestMethod]
        public void Create_recursive_file()
        {
            var folder = new Folder("Root");
            var file = folder.CreateFile(new[] { "Subfolder1", "Subfolder2", "Subfolder3" }, "TestFile.zip");

            Assert.IsTrue(folder.Folders.ContainsKey("Subfolder1"));
            Assert.AreEqual("Subfolder1", folder.Folders["Subfolder1"].Name);
            Assert.AreEqual(1, folder.Folders.Count);
            Assert.AreEqual(0, folder.Files.Count);

            var subfolder1 = folder.Folders["Subfolder1"];
            Assert.IsTrue(subfolder1.Folders.ContainsKey("Subfolder2"));
            Assert.AreEqual("Subfolder2", subfolder1.Folders["Subfolder2"].Name);
            Assert.AreEqual(1, subfolder1.Folders.Count);
            Assert.AreEqual(0, subfolder1.Files.Count);

            var subfolder2 = subfolder1.Folders["Subfolder2"];
            Assert.IsTrue(subfolder2.Folders.ContainsKey("Subfolder3"));
            Assert.AreEqual("Subfolder3", subfolder2.Folders["Subfolder3"].Name);
            Assert.AreEqual(1, subfolder2.Folders.Count);
            Assert.AreEqual(0, subfolder2.Files.Count);

            var subfolder3 = subfolder2.Folders["Subfolder3"];
            Assert.AreEqual(0, subfolder3.Folders.Count);
            Assert.AreEqual(1, subfolder3.Files.Count);

            Assert.IsTrue(subfolder3.Files.ContainsKey("TestFile.zip"));
            Assert.AreEqual("TestFile.zip", subfolder3.Files["TestFile.zip"].Name);
            Assert.AreSame(file, subfolder3.Files["TestFile.zip"]);
        }
    }
}
