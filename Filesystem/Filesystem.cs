using System;
using System.IO;
using Akka.Actor;

namespace Filesystem
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
        }
    }
}
