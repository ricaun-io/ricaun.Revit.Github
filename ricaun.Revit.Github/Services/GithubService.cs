using ricaun.Revit.Github.Data;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ricaun.Revit.Github.Services
{

    /// <summary>
    /// GithubService
    /// </summary>
    public class GithubService
    {
        #region private
        private readonly JsonService jsonService;
        private readonly string user;
        private readonly string repo;
        #endregion

        /// <summary>
        /// GithubService
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repo"></param>
        public GithubService(string user, string repo)
        {
            this.user = user;
            this.repo = repo;
            jsonService = new JsonService();
        }

        /// <summary>
        /// User
        /// </summary>
        public string User => this.user;
        /// <summary>
        /// Repository
        /// </summary>
        public string Repository => this.repo;

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
        /// GetGithubModels
        /// </summary>
        /// <returns>Empty Array if throw</returns>
        internal GithubModel[] GetGithubModels()
        {
            var task = Task.Run(async () =>
                {
                    return await GetGithubModelsAsync();
                });
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// GetGithubModelsAsync
        /// </summary>
        /// <returns>Empty Array if throw</returns>
        internal Task<GithubModel[]> GetGithubModelsAsync()
        {
            return Task.Run(async () =>
                {
                    var json = await DownloadStringAsync(UrlReleases);
                    if (json is null)
                        return new GithubModel[] { };
                    return jsonService.DeserializeObject<GithubModel[]>(json);
                });

        }
        #endregion

        #region WebClient
        /// <summary>
        /// Download String Async
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Thrown return null</returns>
        public Task<string> DownloadStringAsync(string address)
        {
            return Task.Run<string>(async () =>
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        client.Headers.Add("User-Agent", GetType().Assembly.GetName().Name);
                        client.Encoding = System.Text.Encoding.UTF8;
                        return await client.DownloadStringTaskAsync(address);
                    }
                }
                catch { }
                return null;
            });
        }

        /// <summary>
        /// DownloadStringAsyncHttp
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Thrown return null</returns>
        private Task<string> DownloadStringAsyncHttp(string address)
        {
            return Task.Run<string>(async () =>
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("User-Agent", GetType().Assembly.GetName().Name);
                        return await httpClient.GetStringAsync(address);
                    }
                }
                catch { }
                return null;
            });
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
