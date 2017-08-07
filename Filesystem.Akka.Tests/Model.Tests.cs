using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filesystem.Akka.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void Create_file_in_readable_folder()
        {
            var rootPath = @"C:\temp";
            var fileName = "File.txt";

            var folder = new ReadableFolder(rootPath);
            var file = folder.File(fileName);

            Assert.AreEqual(file.Path, Path.Combine(rootPath, fileName));
        }

        [TestMethod]
        public void Create_folder_in_readable_folder()
        {
            var rootPath = @"C:\temp";
            var folderName = "Folder";

            var folder = new ReadableFolder(rootPath);
            var subfolder = folder.ChildFolder(folderName);

            Assert.AreEqual(subfolder.Path, Path.Combine(rootPath, folderName));
        }

        [TestMethod]
        public void Create_file_in_writable_folder()
        {
            var rootPath = @"C:\temp";
            var fileName = "File.txt";

            var folder = new WritableFolder(rootPath);
            var file = folder.File(fileName);

            Assert.AreEqual(file.Path, Path.Combine(rootPath, fileName));
        }

        [TestMethod]
        public void Create_writable_file_in_writable_folder()
        {
            var rootPath = @"C:\temp";
            var fileName = "File.txt";

            var folder = new WritableFolder(rootPath);
            var file = folder.WriteableFile(fileName);

            Assert.AreEqual(file.Path, Path.Combine(rootPath, fileName));
        }

        [TestMethod]
        public void Create_folder_in_writable_folder()
        {
            var rootPath = @"C:\temp";
            var folderName = "Folder";

            var folder = new WritableFolder(rootPath);
            var subfolder = folder.ChildFolder(folderName);

            Assert.AreEqual(subfolder.Path, Path.Combine(rootPath, folderName));
        }
    }
}
