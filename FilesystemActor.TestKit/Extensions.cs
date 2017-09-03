using System.IO;
using Akka.Actor;

namespace Filesystem.Akka.TestKit
{
    public static class FilesystemFailures
    {
        public static Failure IOException() => new Failure() { Exception = new IOException("Filesystem TestKit exceptions are not completely faithful representations of what you might find in production use") };
    }
}
