using Akka.Actor;

namespace Filesystem.Akka.TestKit
{
    public class FilesystemTestKit : ReceiveActor
    {
        public FilesystemTestKit() => Become(Configuring);

        private void Configuring()
        {
            Receive<SetupComplete>(msg => Become(Filesystem));
        }

        private void Filesystem()
        {
            Receive<EnterSetup>(msg => Become(Configuring));
        }
    }
}
