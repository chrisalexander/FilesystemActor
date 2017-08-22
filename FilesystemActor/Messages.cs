using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FilesystemActor
{
    /// <summary>
    /// Check whether the specified Readable Folder exists.
    /// </summary>
    public class FolderExists
    {
        /// <summary>
        /// /// Check whether the specified Readable Folder exists.
        /// </summary>
        /// <param name="Folder">The folder.</param>
        public FolderExists(ReadableFolder Folder) => this.Folder = Folder;

        public ReadableFolder Folder { get; }
    }

    /// <summary>
    /// Check whether the specified Readable File exists.
    /// </summary>
    public class FileExists
    {
        /// <summary>
        /// Check whether the specified Readable File exists.
        /// </summary>
        /// <param name="File">The file.</param>
        public FileExists(ReadableFile File) => this.File = File;

        public ReadableFile File { get; }
    }

    /// <summary>
    /// Create a folder within a specified Writable Folder.
    /// </summary>
    public class CreateFolder
    {
        /// <summary>
        /// Create a folder within a specified Writable Folder.
        /// </summary>
        /// <param name="Folder">A writable folder.</param>
        /// <param name="FolderName">The name of the folder to create.</param>
        public CreateFolder(WritableFolder Folder, string FolderName)
        {
            this.Folder = Folder;
            this.FolderName = FolderName;
        }

        public WritableFolder Folder { get; }

        public string FolderName { get; }
    }

    /// <summary>
    /// Write data to a file.
    /// </summary>
    public class WriteFile
    {
        /// <summary>
        /// Write data to a file.
        /// </summary>
        /// <param name="File">The Writable File to write to.</param>
        /// <param name="Text">The text to write.</param>
        public WriteFile(WritableFile File, string Text)
        {
            this.File = File;
            Stream = new MemoryStream(Encoding.UTF8.GetBytes(Text));
        }

        /// <summary>
        /// Write data to a file.
        /// </summary>
        /// <param name="File">The Writable File to write to.</param>
        /// <param name="Stream">The stream of bytes to write.</param>
        public WriteFile(WritableFile File, Stream Stream)
        {
            this.File = File;
            this.Stream = Stream;
        }

        public WritableFile File { get; }

        public Stream Stream { get; }
    }

    /// <summary>
    /// Overwrite the specified file.
    /// </summary>
    public class OverwriteFile
    {
        /// <summary>
        /// Overwrite the specified file.
        /// </summary>
        /// <param name="File">The Overwritable File to overwrite.</param>
        /// <param name="Text">The text to write to the file.</param>
        public OverwriteFile(OverwritableFile File, string Text)
        {
            this.File = File;
            Stream = new MemoryStream(Encoding.UTF8.GetBytes(Text));
        }

        /// <summary>
        /// Overwrite the specified file.
        /// </summary>
        /// <param name="File">The Overwritable File to overwrite.</param>
        /// <param name="Stream">The stream of bytes to write.</param>
        public OverwriteFile(OverwritableFile File, Stream Stream)
        {
            this.File = File;
            this.Stream = Stream;
        }

        public OverwritableFile File { get; }

        public Stream Stream { get; }
    }

    /// <summary>
    /// Delete a folder.
    /// </summary>
    public class DeleteFolder
    {
        /// <summary>
        /// Delete a folder.
        /// </summary>
        /// <param name="Folder">The Deletable Folder to delete.</param>
        public DeleteFolder(DeletableFolder Folder) => this.Folder = Folder;

        public DeletableFolder Folder { get; }

        public bool Recursive { get; set; }
    }

    /// <summary>
    /// Empty a folder.
    /// </summary>
    public class EmptyFolder
    {
        /// <summary>
        /// Empty a folder.
        /// </summary>
        /// <param name="Folder">The Deletable Folder to empty.</param>
        public EmptyFolder(DeletableFolder Folder) => this.Folder = Folder;

        public DeletableFolder Folder { get; }
    }

    /// <summary>
    /// Delete a file.
    /// </summary>
    public class DeleteFile
    {
        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="File">The Deletable File to delete.</param>
        public DeleteFile(DeletableFile File) => this.File = File;

        public DeletableFile File { get; }
    }

    /// <summary>
    /// List the contents of a Readable Folder.
    /// </summary>
    public class ListReadableContents
    {
        /// <summary>
        /// List the contents of a Readable Folder.
        /// </summary>
        /// <param name="Folder">The Readable Folder to list the contents of.</param>
        public ListReadableContents(ReadableFolder Folder) => this.Folder = Folder;

        public ReadableFolder Folder { get; }
    }

    /// <summary>
    /// List the contents of a Writable Folder.
    /// </summary>
    public class ListWritableContents
    {
        /// <summary>
        /// List the contents of a Writable Folder.
        /// </summary>
        /// <param name="Folder">The Writable Folder to list the contents of.</param>
        public ListWritableContents(WritableFolder Folder) => this.Folder = Folder;

        public WritableFolder Folder { get; }
    }

    /// <summary>
    /// List the contents of a Deletable Folder.
    /// </summary>
    public class ListDeletableContents
    {
        /// <summary>
        /// List the contents of a Deletable Folder.
        /// </summary>
        /// <param name="Folder">The Deletable Folder to list the contents of.</param>
        public ListDeletableContents(DeletableFolder Folder) => this.Folder = Folder;

        public DeletableFolder Folder { get; }
    }
    
    /// <summary>
    /// Provides the contents of a Readable Folder.
    /// </summary>
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
    
    /// <summary>
    /// Provides the contents of a Writable Folder.
    /// </summary>
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
    
    /// <summary>
    /// Provides the contents of a Deletable Folder.
    /// </summary>
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

    /// <summary>
    /// Copy a source Readable Fodler to a target Writable Folder.
    /// </summary>
    public class CopyFolder
    {
        /// <summary>
        /// Copy a source Readable Fodler to a target Writable Folder.
        /// </summary>
        /// <param name="Source">The source Readable Folder.</param>
        /// <param name="Target">The target Writable Folder to place the source folder in.</param>
        public CopyFolder(ReadableFolder Source, WritableFolder Target)
        {
            this.Source = Source;
            this.Target = Target;
        }

        public ReadableFolder Source { get; }

        public WritableFolder Target { get; }
    }

    /// <summary>
    /// Copy the contents of a Readable Folder into a Writable Folder.
    /// </summary>
    public class CopyFolderContents
    {
        /// <summary>
        /// Copy the contents of a Readable Folder into a Writable Folder.
        /// </summary>
        /// <param name="Source">The source Readable Folder.</param>
        /// <param name="Target">The target Writable Folder to put the contents in.</param>
        public CopyFolderContents(ReadableFolder Source, WritableFolder Target)
        {
            this.Source = Source;
            this.Target = Target;
        }

        public ReadableFolder Source { get; }

        public WritableFolder Target { get; }
    }

    /// <summary>
    /// Copy a file.
    /// </summary>
    public class CopyFile
    {
        /// <summary>
        /// Copy a file from the Readable File source on top of the Overwritable File target.
        /// </summary>
        /// <param name="Source">The source Readable File.</param>
        /// <param name="FileTarget">The target Overwritable File.</param>
        public CopyFile(ReadableFile Source, OverwritableFile FileTarget)
        {
            this.Source = Source;
            this.FileTarget = FileTarget;
            IsFolderMode = false;
        }

        /// <summary>
        /// Copy a file from the Readable File source into the Writable Folder target.
        /// </summary>
        /// <param name="Source">The source Readable File.</param>
        /// <param name="FolderTarget">The target Writable Folder to put the file in.</param>
        public CopyFile(ReadableFile Source, WritableFolder FolderTarget)
        {
            this.Source = Source;
            this.FolderTarget = FolderTarget;
            IsFolderMode = true;
        }

        public ReadableFile Source { get; }

        public WritableFolder FolderTarget { get; }

        public OverwritableFile FileTarget { get; }

        public bool IsFolderMode { get; }
    }

    /// <summary>
    /// Read the contents of a Readable File.
    /// </summary>
    public class ReadFile
    {
        /// <summary>
        /// Read the contents of a Readable File.
        /// </summary>
        /// <param name="Target">The Readable File.</param>
        public ReadFile(ReadableFile Target) => this.Target = Target;

        public ReadableFile Target { get; }
    }

    /// <summary>
    /// The contents of a file.
    /// </summary>
    public class FileContents
    {
        public FileContents(byte[] Contents) => this.Bytes = Contents;

        public byte[] Bytes { get; }
    }
}
