using System.IO;
using Akka.Actor;

namespace Filesystem
{
    public class Filesystem : ReceiveActor
    {
        public Filesystem()
        {
            Receive<FolderExists>(m =>
            {
                Sender.Tell(Directory.Exists(m.Folder.Path));
            });

            Receive<CreateFolder>(m =>
            {
                var folder = m.Folder.ChildWriteableFolder(m.FolderName);
                Directory.CreateDirectory(folder.Path);
                Sender.Tell(folder);
            });
        }
    }
}
