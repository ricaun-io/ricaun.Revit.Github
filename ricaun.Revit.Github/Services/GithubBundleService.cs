using ricaun.Revit.Github.Data;
using System;
using System.Linq;
using System.Reflection;

namespace ricaun.Revit.Github.Services
{
    public class GithubBundleService : GithubService
    {
        #region const
        private const string CONST_BUNDLE_ZIP = ".bundle.zip";
        #endregion
        public GithubBundleService(string user, string repo) : base(user, repo)
        {

        }

        public string GetBundleDownloadUrlLatest(string versionAssembly = null)
        {
            return GetBundleDownloadUrl(null, versionAssembly);
        }

        public string GetBundleDownloadUrl(string name, string versionAssembly = null)
        {
            var model = GetGithubModel(name);

            if (IsVersionModel(model, versionAssembly))
                return GetAssetBundle(model)?.browser_download_url;

            return null;
        }

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
        #region internal
        internal Asset GetAssetBundle(GithubModel githubModel)
        {
            var asset = githubModel?.assets.FirstOrDefault(e => e.name.EndsWith(CONST_BUNDLE_ZIP));
            return asset;
        }
        #endregion
    }
}
