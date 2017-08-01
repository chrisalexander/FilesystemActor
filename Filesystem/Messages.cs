namespace Filesystem
{
    public class FolderExists
    {
        public FolderExists(ReadableFolder Folder) => this.Folder = Folder;

        public ReadableFolder Folder { get; }
    }

    public class FileExists
    {
        public FileExists(ReadableFile File) => this.File = File;

        public ReadableFile File { get; }
    }

    public class CreateFolder
    {
        public CreateFolder(WritableFolder Folder, string FolderName)
        {
            this.Folder = Folder;
            this.FolderName = FolderName;
        }

        public WritableFolder Folder { get; }

        public string FolderName { get; }
    }

    public class WriteFile
    {
        public WriteFile(WritableFile File) => this.File = File;

        public WritableFile File { get; }
    }

    public class OverwriteFile
    {
        public OverwriteFile(OverwritableFile File) => this.File = File;

        public OverwritableFile File { get; }
    }

    public class DeleteFolder
    {
        public DeleteFolder(DeletableFolder Folder) => this.Folder = Folder;

        public DeletableFolder Folder { get; }
    }

    public class EmptyFolder
    {
        public EmptyFolder(DeletableFolder Folder) => this.Folder = Folder;

        public DeletableFolder Folder { get; }
    }

    public class DeleteFile
    {
        public DeleteFile(DeletableFile File) => this.File = File;

        public DeletableFile File { get; }
    }
}
