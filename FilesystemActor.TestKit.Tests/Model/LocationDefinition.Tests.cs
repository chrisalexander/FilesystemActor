using System;
using System.Linq;
using Filesystem.Akka.TestKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.TestKit.Tests.Model
{
    [TestClass]
    public class LocationDefinitionTests
    {
        [TestMethod]
        public void Folder_location_definition()
        {
            var definition = new LocationDefinition(@"C:\users\test\directory\");
            Assert.AreEqual("C", definition.Drive);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "users", "test", "directory" }, definition.Folders));
            Assert.IsNull(definition.Filename);
        }

        [TestMethod]
        public void File_location_definition()
        {
            var definition = new LocationDefinition(@"C:\users\test directory\file.txt");
            Assert.AreEqual("C", definition.Drive);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "users", "test directory" }, definition.Folders));
            Assert.AreEqual("file.txt", definition.Filename);
        }

        [TestMethod]
        public void Network_location_definition()
        {
            var definition = new LocationDefinition(@"\\shareddrive\users\test\directory\");
            Assert.AreEqual("shareddrive", definition.Drive);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "users", "test", "directory" }, definition.Folders));
            Assert.IsNull(definition.Filename);
        }

        [TestMethod]
        public void Network_file_location_definition()
        {
            var definition = new LocationDefinition(@"\\shareddrive\users\test directory\file.txt");
            Assert.AreEqual("shareddrive", definition.Drive);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "users", "test directory" }, definition.Folders));
            Assert.AreEqual("file.txt", definition.Filename);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Not_a_location()
        {
            var definition = new LocationDefinition("Something something something");
        }
    }
}
