using System;
using System.IO;
using System.IO.Compression;

namespace ricaun.Revit.Github.Services
{
    /// <summary>
    /// ZipFileHelper
    /// </summary>
    public static class ZipFileHelper
    {
        /// <summary>
        /// ExtractToDirectory with overwrite enable
        /// </summary>
        /// <param name="archiveFileName"></param>
        /// <param name="destinationDirectoryName"></param>
        /// <param name="overwrite"></param>
        public static void ExtractToDirectory(string archiveFileName, string destinationDirectoryName, bool overwrite = true)
        {
            if (!overwrite)
            {
                ZipFile.ExtractToDirectory(archiveFileName, destinationDirectoryName);
            }
            else
            {
                using (var archive = ZipFile.OpenRead(archiveFileName))
                {
                    foreach (var file in archive.Entries)
                    {
                        var completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                        var directory = Path.GetDirectoryName(completeFileName);

                        if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
                            Directory.CreateDirectory(directory);

                        if (file.Name != "")
                            file.ExtractToFile(completeFileName, true);
                    }

                }
            }
        }
    }
}
