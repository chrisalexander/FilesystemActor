using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.Tests
{
    [TestClass]
    public class FileContentsExtensionTests
    {
        [TestMethod]
        public void As_ascii()
        {
            var str = "Test Weird ʣ Character";
            var fileContents = new FileContents(Encoding.ASCII.GetBytes(str));
            Assert.AreEqual("Test Weird ? Character", fileContents.AsAscii());
            Assert.AreEqual("Test Weird ? Character", fileContents.AsUtf8());
        }

        [TestMethod]
        public void As_utf8()
        {
            var str = "Test Weird ʣ Character";
            var fileContents = new FileContents(Encoding.UTF8.GetBytes(str));
            Assert.AreEqual("Test Weird ?? Character", fileContents.AsAscii());
            Assert.AreEqual("Test Weird ʣ Character", fileContents.AsUtf8());
        }
    }
}