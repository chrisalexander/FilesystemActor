using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Filesystem.Akka
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
        public WriteFile(WritableFile File, string Text)
        {
            this.File = File;
            this.Stream = new MemoryStream(Encoding.UTF8.GetBytes(Text));
        }

        public WriteFile(WritableFile File, Stream Stream)
        {
            this.File = File;
            this.Stream = Stream;
        }

        public WritableFile File { get; }

        public Stream Stream { get; }
    }

    public class OverwriteFile
    {
        public OverwriteFile(OverwritableFile File, string Text)
        {
            this.File = File;
            this.Stream = new MemoryStream(Encoding.UTF8.GetBytes(Text));
        }

        public OverwriteFile(OverwritableFile File, Stream Stream)
        {
            this.File = File;
            this.Stream = Stream;
        }

        public OverwritableFile File { get; }

        public Stream Stream { get; }
    }

    public class DeleteFolder
    {
        public DeleteFolder(DeletableFolder Folder) => this.Folder = Folder;

        public DeletableFolder Folder { get; }

        public bool Recursive { get; set; }
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

    public class ListReadableContents
    {
        public ListReadableContents(ReadableFolder Folder) => this.Folder = Folder;

        public ReadableFolder Folder { get; }
    }

    public class ListWritableContents
    {
        public ListWritableContents(WritableFolder Folder) => this.Folder = Folder;

        public WritableFolder Folder { get; }
    }

    public class ListDeletableContents
    {
        public ListDeletableContents(DeletableFolder Folder) => this.Folder = Folder;

        public DeletableFolder Folder { get; }
    }
    
    public class FolderReadableContents
    {
        public FolderReadableContents(List<ReadableFolder> Folders, List<ReadableFile> Files)
        {
            this.Folders = Folders;
            this.Files = Files;
        }

        public List<ReadableFolder> Folders { get; }

        public List<ReadableFile> Files { get; }
    }
    
    public class FolderWritableContents
    {
        public FolderWritableContents(List<WritableFolder> Folders, List<WritableFile> Files)
        {
            this.Folders = Folders;
            this.Files = Files;
        }

        public List<WritableFolder> Folders { get; }

        public List<WritableFile> Files { get; }
    }
    
    public class FolderDeletableContents
    {
        public FolderDeletableContents(List<DeletableFolder> Folders, List<DeletableFile> Files)
        {
            this.Folders = Folders;
            this.Files = Files;
        }

        public List<DeletableFolder> Folders { get; }

        public List<DeletableFile> Files { get; }
    }

    public class CopyFolder
    {
        public CopyFolder(ReadableFolder Source, WritableFolder Target)
        {
            this.Source = Source;
            this.Target = Target;
        }

        public ReadableFolder Source { get; }

        public WritableFolder Target { get; }
    }

    public class CopyFolderContents
    {
        public CopyFolderContents(ReadableFolder Source, WritableFolder Target)
        {
            this.Source = Source;
            this.Target = Target;
        }

        public ReadableFolder Source { get; }

        public WritableFolder Target { get; }
    }

    public class CopyFile
    {
        public CopyFile(ReadableFile Source, WritableFolder Target)
        {
            this.Source = Source;
            this.Target = Target;
        }

        public ReadableFile Source { get; }

        public WritableFolder Target { get; }
    }
}
