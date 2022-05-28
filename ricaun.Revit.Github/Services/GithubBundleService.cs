using ricaun.Revit.Github.Data;
using System;
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

        /// <summary>
        /// GetBundleDownloadUrlLatest
        /// </summary>
        /// <param name="versionAssembly"></param>
        /// <returns></returns>
        public string GetBundleDownloadUrlLatest(string versionAssembly = null)
        {
            return GetBundleDownloadUrl(null, versionAssembly);
        }

        /// <summary>
        /// GetBundleDownloadUrl
        /// </summary>
        /// <param name="name"></param>
        /// <param name="versionAssembly"></param>
        /// <returns></returns>
        public string GetBundleDownloadUrl(string name, string versionAssembly = null)
        {
            var model = GetGithubModel(name);

            if (IsVersionModel(model, versionAssembly))
                return GetAssetBundle(model)?.browser_download_url;

            return null;
        }

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
