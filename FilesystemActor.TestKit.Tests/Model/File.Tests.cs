using System;
using Filesystem.Akka.TestKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.Model
{
    [TestClass]
    public class FileTests
    {
        [TestMethod]
        public void Create_file_name()
        {
            var name = Guid.NewGuid().ToString();
            var file = new File(name);
            Assert.AreEqual(name, file.Name);
        }
    }
}
