using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Filesystem.Akka.TestKit
{
    public class Folder
    {
        public Folder(string Name) => this.Name = Name;

        public string Name { get; }

        public ConcurrentDictionary<string, Folder> Folders = new ConcurrentDictionary<string, Folder>();

        public ConcurrentDictionary<string, File> Files = new ConcurrentDictionary<string, File>();

        public void CreateFolder(string[] folderPath)
        {
            if (folderPath.Length < 1)
            {
                return;
            }

            var subfolder = this.Folders.GetOrAdd(folderPath[0], name => new Folder(name));

            if (folderPath.Length > 1)
            {
                subfolder.CreateFolder(folderPath.Skip(1).ToArray());
            }
        }

        public File CreateFile(string[] folderPath, string fileName)
        {
            if (folderPath.Length > 0)
            {
                var subfolder = this.Folders.GetOrAdd(folderPath[0], name => new Folder(name));
                return subfolder.CreateFile(folderPath.Skip(1).ToArray(), fileName);
            }
            else
            {
                return this.Files.GetOrAdd(fileName, name => new File(name));
            }
        }
    }

    public class File
    {
        public File(string Name) => this.Name = Name;

        public string Name { get; }

        public bool Locked { get; set; }
    }

    public class LocationDefinition
    {
        public LocationDefinition(string fullPath)
        {
            string[] components;

            if (fullPath.Substring(1, 2).Equals(@":\"))
            {
                Drive = fullPath.Substring(0, 1).ToUpper();
                components = fullPath.Substring(3).Split('\\').ToArray();
            }
            else if (fullPath.Substring(0, 2).Equals(@"\\"))
            {
                components = fullPath
                                    .Substring(2)
                                    .Split('\\')
                                    .ToArray();

                Drive = components[0];
                components = components.Skip(1).ToArray();
            }
            else
            {
                throw new InvalidOperationException("Unrecognised path format - is neither UNC or a drive");
            }

            if (string.IsNullOrWhiteSpace(components[components.Length - 1]))
            {
                Filename = null;
            }
            else
            {
                Filename = components[components.Length - 1];
                components = components.Take(components.Length - 1).ToArray();
            }

            Folders = components.Where(c => !string.IsNullOrWhiteSpace(c)).ToArray();
        }

        public string Drive { get; }

        public string[] Folders { get; }

        public string Filename { get; }
    }
}
