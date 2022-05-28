using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace ricaun.Revit.Github.Services
{
    /// <summary>
    /// DownloadBundleService
    /// </summary>
    public class DownloadBundleService
    {
        #region const
        private const string CONST_BUNDLE = ".bundle";
        #endregion

        #region Download Events
        /// <summary>
        /// DownloadProgressChanged
        /// </summary>
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;
        /// <summary>
        /// DownloadFileCompleted
        /// </summary>
        public event AsyncCompletedEventHandler DownloadFileCompleted;
        /// <summary>
        /// DownloadFileException
        /// </summary>
        public event Action<Exception> DownloadFileException;
        #endregion

        #region Download
        /// <summary>
        /// Download and unzip Bundle
        /// </summary>
        /// <param name="extractPath"></param>
        /// <param name="address"></param>
        public void DownloadBundle(string extractPath, string address)
        {
            var fileName = Path.GetFileName(address);
            var zipPath = Path.Combine(extractPath, fileName);

            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent", $"{this.GetType().Assembly.GetName().Name}");
                client.DownloadProgressChanged += DownloadProgressChanged;
                client.DownloadFileCompleted += (s, e) =>
                {
                    DownloadFileCompleted?.Invoke(s, e);
                    if (e.Cancelled) return;
                    try
                    {
                        ExtractBundleZipToDirectory(zipPath, extractPath);
                        File.Delete(zipPath);
                    }
                    catch (Exception ex)
                    {
                        DownloadFileException?.Invoke(ex);
                    }
                };
                client.DownloadFileAsync(new Uri(address), zipPath);
            }
        }
        #endregion

        #region BundleZip
        /// <summary>
        /// ExtractToDirectory with overwrite enable
        /// </summary>
        /// <param name="archiveFileName"></param>
        /// <param name="destinationDirectoryName"></param>
        private void ExtractBundleZipToDirectory(string archiveFileName, string destinationDirectoryName)
        {
            if (Path.GetExtension(archiveFileName) != ".zip") return;
            using (var archive = ZipFile.OpenRead(archiveFileName))
            {
                string baseDirectory = null;
                foreach (var file in archive.Entries)
                {
                    if (baseDirectory == null)
                        baseDirectory = Path.GetDirectoryName(file.FullName);
                    if (baseDirectory.EndsWith(CONST_BUNDLE) == false)
                        baseDirectory = "";

                    var fileFullName = file.FullName.Substring(baseDirectory.Length).TrimStart('/');

                    var completeFileName = Path.Combine(destinationDirectoryName, fileFullName);
                    var directory = Path.GetDirectoryName(completeFileName);

                    Debug.WriteLine($"{fileFullName} |\t {baseDirectory} |\t {completeFileName}");

                    if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
                        Directory.CreateDirectory(directory);

                    if (file.Name != "")
                        file.ExtractToFile(completeFileName, true);
                }
            }

        }
        #endregion
    }
}
