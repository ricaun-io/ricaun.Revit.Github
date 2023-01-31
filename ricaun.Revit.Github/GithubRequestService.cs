using ricaun.Revit.Github.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
        [MethodImpl(MethodImplOptions.NoInlining)]
        public GithubRequestService(string user, string repo)
        {
            var assembly = Assembly.GetCallingAssembly();
            this.githubBundleService = new GithubBundleService(user, repo);
            this.pathBundleService = new PathBundleService(assembly);
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
                downloadBundleService.DownloadBundle(folder, bundleModel.DownloadUrl);

            return bundleModel;
        }

        /// <summary>
        /// Initialize and download the latest version if it is greater than the current version
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>Is downloaded a new version</returns>
        public Task<bool> Initialize(Action<string> callback = null)
        {
            var task = Task.Run(async () =>
                {
                    var bundleModels = await githubBundleService.GetBundleModelsAsync();
                    callback?.Invoke($"Bundles: [{string.Join(" , ", bundleModels)}]");

                    bundleModels = bundleModels
                        .Where(e => githubBundleService.IsVersionModel(e, pathBundleService.GetAssembly()));

                    if (bundleModels.Any() == false) return false;

                    var bundleModelLatest = bundleModels.FirstOrDefault();

                    callback?.Invoke($"Download: {bundleModelLatest}");

                    var result = false;
                    if (pathBundleService.TryGetPath(out string folder))
                    {
                        callback?.Invoke($"Download: {bundleModelLatest.DownloadUrl}");
                        result = await downloadBundleService.DownloadBundleAsync(folder, bundleModelLatest.DownloadUrl);
                        callback?.Invoke($"Download: {folder}");
                    }
                    callback?.Invoke($"Download: {result}");
                    return result;
                });

            return task;
        }

    }
}
