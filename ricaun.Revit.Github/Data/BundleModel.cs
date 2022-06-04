using System;

namespace ricaun.Revit.Github.Data
{
    /// <summary>
    /// BundleModel
    /// </summary>
    public class BundleModel
    {
        /// <summary>
        /// User
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Repository
        /// </summary>
        public string Repository { get; set; }
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// DownloadUrl
        /// </summary>
        public string DownloadUrl { get; set; }
        /// <summary>
        /// Created
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{User} {Repository} {Version}";
        }
    }
}
