namespace Filesystem
{
    public class ReadableFile
    {
        public ReadableFile(string Path) => this.Path = Path;

        public string Path { get; }
    }

    public class ReadableFolder
    {
        public ReadableFolder(string Path) => this.Path = Path;

        public string Path { get; }

        public ReadableFile File(string name) => new ReadableFile(System.IO.Path.Combine(Path, name));

        public ReadableFolder ChildFolder(string name) => new ReadableFolder(System.IO.Path.Combine(Path, name));
    }

    public class WritableFile : ReadableFile
    {
        public WritableFile(string path) : base(path) { }
    }

    public class OverwritableFile : WritableFile
    {
        public OverwritableFile(string path) : base(path) { }
    }

    public class WritableFolder : ReadableFolder
    {
        public WritableFolder(string path) : base(path) { }

        public WritableFile WriteableFile(string name) => new WritableFile(System.IO.Path.Combine(Path, name));

        public WritableFolder ChildWriteableFolder(string name) => new WritableFolder(System.IO.Path.Combine(Path, name));
    }

    public class DeletableFile : WritableFile
    {
        public DeletableFile(string path) : base(path) { }
    }

    public class DeletableFolder : WritableFolder
    {
        public DeletableFolder(string Path) : base(Path) { }
    }
}
