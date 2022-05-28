using ricaun.Revit.Github.Data;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace ricaun.Revit.Github.Services
{

    public class GithubService
    {
        #region private
        private readonly JsonService jsonService;
        private readonly string user;
        private readonly string repo;
        #endregion

        public GithubService(string user, string repo)
        {
            this.user = user;
            this.repo = repo;
            jsonService = new JsonService();
        }

        public void Show()
        {
            foreach (var item in GetGithubModels())
            {
                System.Console.WriteLine(item);
            }
        }

        #region api.github
        private string UrlReleasesLatest => GenereteApiGithub(user, repo, "latest");
        private string UrlReleases => GenereteApiGithub(user, repo);

        private string GenereteApiGithub(string user, string repo, string id = "")
        {
            if (string.IsNullOrEmpty(id) == false)
                id = $"/{id}";

            return $"https://api.github.com/repos/{user}/{repo}/releases{id}";
        }
        #endregion

        #region GithubModel
        /// <summary>
        /// Get GithubModel with <paramref name="name"/> 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal GithubModel GetGithubModel(string name)
        {
            if (name == null)
                return GetGithubModelLatest();
            return GetGithubModels()?.FirstOrDefault(e => e.name == name);
        }
        internal GithubModel GetGithubModelLatest()
        {
            var json = DownloadString(UrlReleasesLatest);
            if (json == null)
                return null;

            return jsonService.DeserializeObject<GithubModel>(json);
        }

        internal GithubModel[] GetGithubModels()
        {
            var json = DownloadString(UrlReleases);
            if (json == null)
                return null;

            return jsonService.DeserializeObject<GithubModel[]>(json);
        }
        #endregion

        #region WebClient
        private string DownloadString(string address)
        {
            if (IsConnectedToInternet() == false)
                return null;

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", $"{GetType().Assembly.GetName().Name}");
                    client.Encoding = System.Text.Encoding.UTF8;
                    return client.DownloadString(address);
                }
            }
            catch { }

            return null;
        }
        private bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        #endregion

    }
}
