using System;
using System.Linq;

namespace Filesystem.Akka.TestKit
{
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
