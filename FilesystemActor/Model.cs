namespace FilesystemActor
{
    /// <summary>
    /// Wraps a folder that is readable.
    /// </summary>
    public class ReadableFolder
    {
        /// <summary>
        /// Create a reference to a readable folder at the specified path.
        /// </summary>
        /// <param name="Path">Path to the directory.</param>
        public ReadableFolder(string Path) => this.Path = Path;

        public string Path { get; }

        public ReadableFile File(string name) => new ReadableFile(System.IO.Path.Combine(Path, name));

        public ReadableFolder ChildFolder(string name) => new ReadableFolder(System.IO.Path.Combine(Path, name));
    }

    /// <summary>
    /// Wraps a readable file.
    /// </summary>
    public class ReadableFile
    {
        /// <summary>
        /// Create a reference to a readable file at the specified path.
        /// </summary>
        /// <param name="Path">Path to the file.</param>
        public ReadableFile(string Path) => this.Path = Path;

        public string Path { get; }
    }

    /// <summary>
    /// Wraps a folder that is readable and writable.
    /// </summary>
    public class WritableFolder : ReadableFolder
    {
        /// <summary>
        /// Create a reference to a writable file.
        /// </summary>
        /// <param name="Path">Path to the folder.</param>
        public WritableFolder(string Path) : base(Path) { }

        public WritableFile WriteableFile(string name) => new WritableFile(System.IO.Path.Combine(Path, name));

        public WritableFolder ChildWriteableFolder(string name) => new WritableFolder(System.IO.Path.Combine(Path, name));
    }

    /// <summary>
    /// Wraps a file that is readable and writable.
    /// </summary>
    public class WritableFile : ReadableFile
    {
        /// <summary>
        /// Create a reference to a writable file.
        /// </summary>
        /// <param name="Path">Path to the file.</param>
        public WritableFile(string Path) : base(Path) { }
    }

    /// <summary>
    /// Wraps a file that can be overwritten.
    /// </summary>
    public class OverwritableFile : WritableFile
    {
        /// <summary>
        /// Create a reference to an overwritable file.
        /// </summary>
        /// <param name="Path">Path to the file.</param>
        public OverwritableFile(string Path) : base(Path) { }
    }

    /// <summary>
    /// Wraps a folder that can be deleted.
    /// </summary>
    public class DeletableFolder : WritableFolder
    {
        /// <summary>
        /// Creates a reference to a deletable folder.
        /// </summary>
        /// <param name="Path">Path to the folder.</param>
        public DeletableFolder(string Path) : base(Path) { }
    }

    /// <summary>
    /// Wraps a file that can be deleted.
    /// </summary>
    public class DeletableFile : WritableFile
    {
        /// <summary>
        /// Create a reference to a deletable file.
        /// </summary>
        /// <param name="Path">Path to the file.</param>
        public DeletableFile(string Path) : base(Path) { }
    }
}
