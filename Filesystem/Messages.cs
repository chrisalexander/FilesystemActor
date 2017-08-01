namespace Filesystem
{
    public class FolderExists
    {
        public FolderExists(ReadOnlyFolder Folder) => this.Folder = Folder;

        public ReadOnlyFolder Folder { get; }
    }

    public class FileExists
    {
        public FileExists(ReadOnlyFile File) => this.File = File;

        public ReadOnlyFile File { get; }
    }

    public class CreateFolder
    {
        public CreateFolder(WriteableFolder Folder, string FolderName)
        {
            this.Folder = Folder;
            this.FolderName = FolderName;
        }

        public WriteableFolder Folder { get; }

        public string FolderName { get; }
    }

    public class CreateFile
    {
        public CreateFile(WriteableFile File) => this.File = File;

        public WriteableFile File { get; }
    }

    public class CreateFileInFolder
    {
        public CreateFileInFolder(WriteableFolder Folder, string FileName)
        {
            this.Folder = Folder;
            this.FileName = FileName;
        }

        public WriteableFolder Folder { get; }

        public string FileName { get; }
    }

    public class DeleteFolder
    {
        public DeleteFolder(WriteableFolder Folder) => this.Folder = Folder;

        public WriteableFolder Folder { get; }
    }
}
