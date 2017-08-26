using System.Collections.Generic;
using Akka.Actor;

namespace FilesystemActor.TestKit
{
    public class FilesystemTestKit : ReceiveActor
    {
        private readonly Dictionary<string, object> drives = new Dictionary<string, object>();

        public FilesystemTestKit() => Become(Configuring);

        private void Configuring()
        {
            Receive<SetupComplete>(msg => Become(Filesystem));

            Receive<CreateTestFolder>(msg =>
            {

            });

            Receive<CreateTestFile>(msg =>
            {

            });

            Receive<LockTestFile>(msg =>
            {

            });
        }

        private void Filesystem()
        {
            Receive<EnterSetup>(msg => Become(Configuring));

            Receive<FolderExists>(msg =>
            {

            });

            Receive<FileExists>(msg =>
            {

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
