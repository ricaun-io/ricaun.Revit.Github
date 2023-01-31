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
        /// Initialize
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task Initialize(Action<string> action = null)
        {
            var task = Task.Run(async () =>
                {
                    action?.Invoke("Initialize");

                    var bundleModels = await githubBundleService.GetBundleModelsAsync();
                    action?.Invoke($"BundleModels: {bundleModels.Count()}");

                    if (bundleModels.Any() == false) return;

                    var bundleModelLatest = bundleModels.FirstOrDefault();

                    action?.Invoke($"BundleModel: {bundleModelLatest}");

                    if (pathBundleService.TryGetPath(out string folder))
                    {
                        action?.Invoke($"DownloadBundle: {bundleModelLatest.DownloadUrl}");
                        await downloadBundleService.DownloadBundleAsync(folder, bundleModelLatest.DownloadUrl);
                        action?.Invoke($"DownloadBundle: {folder}");
                    }
                    action?.Invoke($"Finish");
                });

            return task;
        }

    }
}
