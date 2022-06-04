using ricaun.Revit.Github.Services;
using System;

namespace ricaun.Revit.Github
{

    /// <summary>
    /// GithubRequestService
    /// </summary>
    public class GithubRequestService
    {
        #region private readonly

        private readonly PathBundleService pathBundleService;
        private readonly DownloadBundleService downloadBundleService;
        private readonly GithubBundleService githubBundleService;
        #endregion

        /// <summary>
        /// GithubRequestService
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repo"></param>
        public GithubRequestService(string user, string repo)
        {
            this.githubBundleService = new GithubBundleService(user, repo);
            this.pathBundleService = new PathBundleService();
            this.downloadBundleService = new DownloadBundleService();
        }

        /// <summary>
        /// DownloadLast
        /// </summary>
        /// <returns></returns>
        public Data.BundleModel DownloadLast()
        {
            if (pathBundleService.TryGetPath(out string path))
            {
                return GetDownloadFile(path);
            }
            return null;
        }

        private Data.BundleModel GetDownloadFile(string folder)
        {
            var bundleModel = githubBundleService.GetBundleModelLatest();

            if (bundleModel is not null)
                downloadBundleService.DownloadBundleAsync(folder, bundleModel.DownloadUrl);

            return bundleModel;
        }

    }
}
