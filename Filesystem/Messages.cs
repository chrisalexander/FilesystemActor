namespace Filesystem
{
    public class FolderExists
    {
        public FolderExists(Folder Folder) => this.Folder = Folder;

        public Folder Folder { get; }
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
}
