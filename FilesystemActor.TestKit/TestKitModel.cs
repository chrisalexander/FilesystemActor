using System;
using System.Collections.Generic;
using System.Linq;

namespace Filesystem.Akka.TestKit
{
    public class Folder
    {
        public Folder(string Name) => this.Name = Name;

        public string Name { get; }

        public Dictionary<string, Folder> Folders = new Dictionary<string, Folder>();

        public Dictionary<string, File> Files = new Dictionary<string, File>();

        public void CreateFolder(string[] folderPath)
        {
            if (folderPath.Length < 1)
            {
                return;
            }

            var subfolder = GetSubFolder(folderPath[0]);

            if (folderPath.Length > 1)
            {
                subfolder.CreateFolder(folderPath.Skip(1).ToArray());
            }
        }

        public File CreateFile(string[] folderPath, string fileName)
        {
            if (folderPath.Length > 0)
            {
                var subfolder = GetSubFolder(folderPath[0]);
                return subfolder.CreateFile(folderPath.Skip(1).ToArray(), fileName);
            }
            else
            {
                if (!this.Files.TryGetValue(fileName, out var file))
                {
                    file = new File(fileName);
                    this.Files[fileName] = file;
                }

                return file;
            }
        }

        private Folder GetSubFolder(string folderName)
        {
            if (!this.Folders.TryGetValue(folderName, out var subfolder))
            {
                subfolder = new Folder(folderName);
                this.Folders[folderName] = subfolder;
            }

            return subfolder;
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
