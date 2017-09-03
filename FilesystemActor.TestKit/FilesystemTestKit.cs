using System;
using System.Collections.Concurrent;
using System.IO;
using Akka.Actor;
using Filesystem.Akka.TestKit;

namespace FilesystemActor.TestKit
{
    public class FilesystemTestKit : ReceiveActor
    {
        private readonly ConcurrentDictionary<string, Folder> drives = new ConcurrentDictionary<string, Folder>();

        public FilesystemTestKit() => Become(Configuring);

        private void Configuring()
        {
            Receive<SetupComplete>(msg => Become(Filesystem));

            Receive<CreateTestFolder>(msg =>
            {
                var location = new LocationDefinition(msg.Path);
                var root = this.drives.GetOrAdd(location.Drive, name => new Folder(name));
                root.CreateFolder(location.Folders);
            });

            Receive<CreateTestFile>(msg =>
            {
                var location = new LocationDefinition(msg.Path);
                var root = this.drives.GetOrAdd(location.Drive, name => new Folder(name));
                var file = root.CreateFile(location.Folders, location.Filename);
                file.Contents = msg.Contents;
                file.Locked = msg.Locked;
            });

            Receive<LockTestFile>(msg =>
            {
                var location = new LocationDefinition(msg.Path);
                this.drives.TryGetValue(location.Drive, out var root);
                var file = root.GetFile(location.Folders, location.Filename);
                file.Locked = true;
            });

            Receive<UnlockTestFile>(msg =>
            {
                var location = new LocationDefinition(msg.Path);
                this.drives.TryGetValue(location.Drive, out var root);
                var file = root.GetFile(location.Folders, location.Filename);
                file.Locked = false;
            });
        }

        private void Filesystem()
        {
            Receive<EnterSetup>(msg => Become(Configuring));

            Receive<FolderExists>(msg =>
            {
                var location = new LocationDefinition(msg.Folder.Path);

                try
                {
                    var root = this.drives[location.Drive];
                    foreach (var subfolder in location.Folders)
                    {
                        root = root.Folders[subfolder];
                    }
                    Sender.Tell(true);
                }
                catch (Exception)
                {
                    Sender.Tell(false);
                }
            });

            Receive<FileExists>(msg =>
            {
                var location = new LocationDefinition(msg.File.Path);

                try
                {
                    var root = this.drives[location.Drive];
                    foreach (var subfolder in location.Folders)
                    {
                        root = root.Folders[subfolder];
                    }
                    
                    Sender.Tell(root.Files.ContainsKey(location.Filename));
                }
                catch (Exception)
                {
                    Sender.Tell(false);
                }
            });

            Receive<CreateFolder>(msg =>
            {
                var location = new LocationDefinition(msg.Folder.Path);

                try
                {
                    var root = this.drives.GetOrAdd(location.Drive, n => new Folder(n));
                    root.CreateFolder(location.Folders);
                    Sender.Tell(true);
                }
                catch (Exception)
                {
                    Sender.Tell(false);
                }
            });

            Receive<WriteFile>(msg =>
            {
                var location = new LocationDefinition(msg.File.Path);

                try
                {
                    var root = this.drives.GetOrAdd(location.Drive, n => new Folder(n));

                    TestKit.File existing = null;
                    try
                    {
                        existing = root.GetFile(location.Folders, location.Filename);
                    } catch (Exception) { }

                    if (existing != null)
                    {
                        throw new Exception("Write to existing files should fail");
                    }

                    var file = root.CreateFile(location.Folders, location.Filename);
                    var ms = new MemoryStream();
                    msg.Stream.CopyTo(ms);
                    file.Contents = ms.ToArray();
                    Sender.Tell(true);
                }
                catch (Exception)
                {
                    Sender.Tell(FilesystemFailures.IOException());
                }
            });

            Receive<OverwriteFile>(msg =>
            {
                var location = new LocationDefinition(msg.File.Path);

                try
                {
                    var root = this.drives.GetOrAdd(location.Drive, n => new Folder(n));
                    
                    var existing = root.GetFile(location.Folders, location.Filename);
                    if (existing != null && existing.Locked)
                    {
                        throw new Exception("Overwrite locked files should fail");
                    }

                    var file = root.CreateFile(location.Folders, location.Filename);
                    var ms = new MemoryStream();
                    msg.Stream.CopyTo(ms);
                    file.Contents = ms.ToArray();
                    Sender.Tell(true);
                }
                catch (Exception)
                {
                    Sender.Tell(FilesystemFailures.IOException());
                }
            });

            Receive<DeleteFolder>(msg =>
            {

            });

            Receive<EmptyFolder>(msg =>
            {

            });

            Receive<DeleteFile>(msg =>
            {

            });

            Receive<ListReadableContents>(msg =>
            {

            });

            Receive<ListWritableContents>(msg =>
            {

            });

            Receive<ListDeletableContents>(msg =>
            {

            });

            Receive<CopyFolder>(msg =>
            {

            });

            Receive<CopyFolderContents>(msg =>
            {

            });

            Receive<CopyFile>(msg =>
            {

            });

            Receive<ReadFile>(msg =>
            {
                var location = new LocationDefinition(msg.Target.Path);

                try
                {
                    var root = this.drives[location.Drive];
                    foreach (var subfolder in location.Folders)
                    {
                        root = root.Folders[subfolder];
                    }

                    var file = root.Files[location.Filename];
                    Sender.Tell(new FileContents(file.Contents));
                }
                catch (Exception)
                {
                    Sender.Tell(false);
                }
            });
        }
    }
}
