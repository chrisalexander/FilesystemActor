using System.Text;
using Filesystem.Akka;

namespace Filesystem
{
    public static class FileContentsExtensions
    {
        public static string AsAscii(this FileContents FileContents) => Encoding.ASCII.GetString(FileContents.Bytes);

        public static string AsUtf8(this FileContents FileContents) => Encoding.UTF8.GetString(FileContents.Bytes);
    }
}
