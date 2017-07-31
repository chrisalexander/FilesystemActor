namespace Filesystem
{
    public class File
    {
        public File(string path) => this.Path = path;

        public string Path { get; }
    }

    public class WriteableFile : File
    {
        public WriteableFile(string path) : base(path) { }
    }

    public class Folder
    {
        public Folder(string path) => this.Path = path;

        public string Path { get; }

        public File File(string name) => new File(System.IO.Path.Combine(this.Path, name));

        public Folder ChildFolder(string name) => new Folder(System.IO.Path.Combine(this.Path, name));
    }

    public class WriteableFolder : Folder
    {
        public WriteableFolder(string path) : base(path) { }

        public WriteableFile WriteableFile(string name) => new WriteableFile(System.IO.Path.Combine(this.Path, name));

        public WriteableFolder ChildWriteableFolder(string name) => new WriteableFolder(System.IO.Path.Combine(this.Path, name));
    }
}
