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
        /// <returns></returns>
        public Task Initialize()
        {
            var task = Task.Run(async () =>
                {
                    var bundleModels = await githubBundleService.GetBundleModelsAsync();
                    if (bundleModels.Any() == false) return;
                    var bundleModel = bundleModels.FirstOrDefault();
                    Console.WriteLine(bundleModel);

                    if (pathBundleService.TryGetPath(out string folder))
                    {
                        await downloadBundleService.DownloadBundleAsync(folder, bundleModel.DownloadUrl);
                    }

                    Console.WriteLine($"Initialize Finish");
                });

            return task;
        }

    }
}
