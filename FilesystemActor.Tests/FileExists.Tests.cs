﻿using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.VsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilesystemActor.Tests
{
    [TestClass]
    public class FileExistsExistingTests : TestKit
    {
        private string existingFile;

        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
            File.Delete(this.existingFile);
        }

        [TestInitialize]
        public void Initialise()
        {
            this.existingFile = Path.Combine(Path.GetTempPath(), "exists_" + Guid.NewGuid().ToString());
            File.WriteAllText(this.existingFile, "Test");
        }

        [TestMethod]
        public void File_exists()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new FileExists(new ReadableFile(this.existingFile)));
            var result = ExpectMsg<bool>();
            Assert.IsTrue(result);
        }
    }

    [TestClass]
    public class FileExistsMissingTests : TestKit
    {
        private string missingFile;

        [TestCleanup]
        public void Cleanup() => Shutdown();

        [TestInitialize]
        public void Initialise() => this.missingFile = Path.Combine(Path.GetTempPath(), "missing_" + Guid.NewGuid().ToString());

        [TestMethod]
        public void File_missing()
        {
            var fs = Sys.ActorOf(Props.Create(() => new Filesystem()));
            fs.Tell(new FileExists(new ReadableFile(this.missingFile)));
            var result = ExpectMsg<bool>();
            Assert.IsFalse(result);
        }
    }
}
