using Akka.Actor;

namespace FilesystemActor.TestKit
{
    public class FilesystemTestKit : ReceiveActor
    {
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
        }
    }
}
