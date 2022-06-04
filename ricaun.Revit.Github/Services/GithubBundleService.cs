using ricaun.Revit.Github.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ricaun.Revit.Github.Services
{

    /// <summary>
    /// GithubBundleService
    /// </summary>
    public class GithubBundleService : GithubService
    {
        #region const
        private const string CONST_BUNDLE_ZIP = ".bundle.zip";
        #endregion

        /// <summary>
        /// GithubBundleService
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repo"></param>
        public GithubBundleService(string user, string repo) : base(user, repo)
        {

        }

        ///// <summary>
        ///// GetBundleDownloadUrlLatest
        ///// </summary>
        ///// <param name="versionAssembly"></param>
        ///// <returns></returns>
        //public string GetBundleDownloadUrlLatest(string versionAssembly = null)
        //{
        //    return GetBundleDownloadUrl(null, versionAssembly);
        //}

        ///// <summary>
        ///// GetBundleDownloadUrl
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="versionAssembly"></param>
        ///// <returns></returns>
        //public string GetBundleDownloadUrl(string name, string versionAssembly = null)
        //{
        //    var model = GetGithubModel(name);

        //    if (IsVersionModel(model, versionAssembly))
        //        return GetAssetBundle(model)?.browser_download_url;

        //    return null;
        //}

        /// <summary>
        /// GetBundleModels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BundleModel> GetBundleModels()
        {
            var bundleModels = new List<BundleModel>();
            var models = GetGithubModels();

            foreach (var model in models)
            {
                if (GetAssetBundle(model) is Asset asset)
                {
                    var bundleModel = new BundleModel()
                    {
                        User = this.User,
                        Repository = this.Repository,
                        Version = model.name,
                        Created = model.created_at,
                        DownloadUrl = asset.browser_download_url,
                        Body = model.body,
                    };
                    bundleModels.Add(bundleModel);
                }
            }

            return bundleModels;
        }

        /// <summary>
        /// GetBundleModelLatest
        /// </summary>
        /// <returns></returns>
        public BundleModel GetBundleModelLatest()
        {
            return GetBundleModels().FirstOrDefault();
        }

        /// <summary>
        /// GetBundleModels
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public IEnumerable<BundleModel> GetBundleModels(Assembly assembly)
        {
            return GetBundleModels().Where(e => IsVersionModel(e, assembly));
        }

        /// <summary>
        /// GetBundleModels
        /// </summary>
        /// <param name="versionAssembly"></param>
        /// <returns></returns>
        public IEnumerable<BundleModel> GetBundleModels(string versionAssembly)
        {
            return GetBundleModels().Where(e => IsVersionModel(e, versionAssembly));
        }

        #region BundleModel
        /// <summary>
        /// IsVersionModel
        /// </summary>
        /// <param name="bundleModel"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public bool IsVersionModel(BundleModel bundleModel, Assembly assembly)
        {
            return IsVersionModel(bundleModel, assembly.GetName().Version.ToString(3));
        }
        /// <summary>
        /// IsVersionModel
        /// </summary>
        /// <param name="bundleModel"></param>
        /// <param name="versionAssembly"></param>
        /// <returns></returns>
        public bool IsVersionModel(BundleModel bundleModel, string versionAssembly)
        {
            if (string.IsNullOrEmpty(versionAssembly)) return true;
            var versionModel = bundleModel.Version;
            try
            {
                return (new Version(versionAssembly) < new Version(versionModel));
            }
            catch { }
            return false;
        }
        #endregion

        #region private
        private bool IsVersionModel(GithubModel model, string versionAssembly)
        {
            if (string.IsNullOrEmpty(versionAssembly)) return true;
            var versionModel = model.name;
            try
            {
                return (new Version(versionAssembly) < new Version(versionModel));
            }
            catch { }
            return false;
        }
        private Asset GetAssetBundle(GithubModel githubModel)
        {
            var asset = githubModel?.assets.FirstOrDefault(e => e.name.EndsWith(CONST_BUNDLE_ZIP));
            return asset;
        }
        #endregion
    }
}
