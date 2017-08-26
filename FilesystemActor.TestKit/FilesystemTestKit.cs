using System;
using System.Collections.Concurrent;
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
                root.CreateFile(location.Folders, location.Filename);
            });

            Receive<LockTestFile>(msg =>
            {

            });

            Receive<UnlockTestFile>(msg =>
            {

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

            });

            Receive<WriteFile>(msg =>
            {

            });

            Receive<OverwriteFile>(msg =>
            {

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

            });
        }
    }
}
