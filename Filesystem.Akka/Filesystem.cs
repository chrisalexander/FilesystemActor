using System;
using System.IO;
using System.Linq;
using Akka.Actor;

namespace Filesystem.Akka
{
    public class Filesystem : ReceiveActor
    {
        public Filesystem()
        {
            Receive<FolderExists>(msg =>
            {
                Sender.Tell(Directory.Exists(msg.Folder.Path));
            });

            Receive<FileExists>(msg =>
            {
                Sender.Tell(File.Exists(msg.File.Path));
            });

            Receive<CreateFolder>(msg =>
            {
                var folder = msg.Folder.ChildWriteableFolder(msg.FolderName);
                Directory.CreateDirectory(folder.Path);
                Sender.Tell(folder);
            });

            Receive<WriteFile>(msg =>
            {
                try
                {
                    using (var file = File.Open(msg.File.Path, FileMode.CreateNew))
                    {
                        msg.Stream.Seek(0, SeekOrigin.Begin);
                        msg.Stream.CopyTo(file);
                    }

                    Sender.Tell(true);
                }
                catch (Exception e)
                {
                    Sender.Tell(new Failure() { Exception = e });
                }
            });

            Receive<OverwriteFile>(msg =>
            {
                try
                {
                    using (var file = File.Open(msg.File.Path, FileMode.Create))
                    {
                        msg.Stream.Seek(0, SeekOrigin.Begin);
                        msg.Stream.CopyTo(file);
                    }

                    Sender.Tell(true);
                }
                catch (Exception e)
                {
                    Sender.Tell(new Failure() { Exception = e });
                }
            });

            Receive<DeleteFolder>(msg =>
            {
                try
                {
                    Directory.Delete(msg.Folder.Path, msg.Recursive);
                    Sender.Tell(true);
                }
                catch (Exception e)
                {
                    Sender.Tell(new Failure() { Exception = e });
                }
            });

            Receive<EmptyFolder>(msg =>
            {
                try
                {
                    var files = Directory.GetFiles(msg.Folder.Path);

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    Sender.Tell(true);
                }
                catch (Exception e)
                {
                    Sender.Tell(new Failure() { Exception = e });
                }
            });

            Receive<DeleteFile>(msg =>
            {
                try
                {
                    File.Delete(msg.File.Path);

                    Sender.Tell(true);
                }
                catch (Exception e)
                {
                    Sender.Tell(new Failure() { Exception = e });
                }
            });

            Receive<ListReadableContents>(msg =>
            {
                try
                {
                    var directories = Directory.EnumerateDirectories(msg.Folder.Path)
                                        .Select(d => Path.Combine(msg.Folder.Path, d))
                                        .Select(d => new ReadableFolder(d))
                                        .ToList();
                    var files = Directory.EnumerateFiles(msg.Folder.Path)
                                    .Select(f => Path.Combine(msg.Folder.Path, f))
                                    .Select(f => new ReadableFile(f))
                                    .ToList();

                    Sender.Tell(new FolderReadableContents(directories, files));
                }
                catch (Exception e)
                {
                    Sender.Tell(new Failure() { Exception = e });
                }
            });

            Receive<ListWritableContents>(msg =>
            {
                try
                {
                    var directories = Directory.EnumerateDirectories(msg.Folder.Path)
                                        .Select(d => Path.Combine(msg.Folder.Path, d))
                                        .Select(d => new WritableFolder(d))
                                        .ToList();
                    var files = Directory.EnumerateFiles(msg.Folder.Path)
                                    .Select(f => Path.Combine(msg.Folder.Path, f))
                                    .Select(f => new WritableFile(f))
                                    .ToList();

                    Sender.Tell(new FolderWritableContents(directories, files));
                }
                catch (Exception e)
                {
                    Sender.Tell(new Failure() { Exception = e });
                }
            });

            Receive<ListDeletableContents>(msg =>
            {
                try
                {
                    var directories = Directory.EnumerateDirectories(msg.Folder.Path)
                                        .Select(d => Path.Combine(msg.Folder.Path, d))
                                        .Select(d => new DeletableFolder(d))
                                        .ToList();
                    var files = Directory.EnumerateFiles(msg.Folder.Path)
                                    .Select(f => Path.Combine(msg.Folder.Path, f))
                                    .Select(f => new DeletableFile(f))
                                    .ToList();

                    Sender.Tell(new FolderDeletableContents(directories, files));
                }
                catch (Exception e)
                {
                    Sender.Tell(new Failure() { Exception = e });
                }
            });
        }
    }
}
