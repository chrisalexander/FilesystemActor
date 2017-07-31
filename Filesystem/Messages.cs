namespace Filesystem
{
    public class FolderExists
    {
        public FolderExists(Folder folder) => this.Folder = folder;

        public Folder Folder { get; }
    }

    public class CreateFolder
    {
        public CreateFolder(WriteableFolder folder, string folderName)
        {
            this.Folder = folder;
            this.FolderName = FolderName;
        }

        public WriteableFolder Folder { get; }

        public string FolderName { get; }
    }
}
