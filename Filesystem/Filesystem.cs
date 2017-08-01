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
        }
    }
}
