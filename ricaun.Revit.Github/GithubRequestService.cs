using ricaun.Revit.Github.Data;
using ricaun.Revit.Github.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ricaun.Revit.Github
{
    /// <summary>
    /// GithubRequestService
    /// </summary>
    public class GithubRequestService
    {
        #region CONST
        private const string BUNDLE_ZIP = "bundle.zip";
        #endregion

        #region private readonly
        private readonly HttpClient client;
        private readonly JsonService jsonService;
        private readonly PathBundleService pathBundleService;
        private readonly string user;
        private readonly string repo;
        private readonly Assembly assembly;
        #endregion

        #region Download Events
        /// <summary>
        /// DownloadProgressChanged
        /// </summary>
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;
        /// <summary>
        /// DownloadFileCompleted
        /// </summary>
        public event AsyncCompletedEventHandler DownloadFileCompleted;
        /// <summary>
        /// DownloadFileException
        /// </summary>
        public event Action<Exception> DownloadFileException;
        #endregion


        /// <summary>
        /// GithubRequestService
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repo"></param>
        public GithubRequestService(string user, string repo)
        {
            this.user = user;
            this.repo = repo;

            this.assembly = Assembly.GetCallingAssembly();

            this.client = new HttpClient();
            this.client.Timeout = TimeSpan.FromMilliseconds(5000);
            this.jsonService = new JsonService();
            this.pathBundleService = new PathBundleService();
        }

        private string UrlReleasesLatest => GenereteApiGithubLatest(user, repo);
        private string GenereteApiGithubLatest(string user, string repo)
        {
            return $"https://api.github.com/repos/{user}/{repo}/releases/latest";
        }

        private string GetAssemblyVersion() => this.assembly.GetName().Version.ToString(3);

        /// <summary>
        /// DownloadLast
        /// </summary>
        /// <returns></returns>
        public string DownloadLast()
        {
            if (pathBundleService.TryGetPath(out string path))
            {
                return GetDownloadFile(path);
            }
            return null;
        }

        private GithubModel GetGithubModelLast()
        {
            var json = GetString(UrlReleasesLatest);
            if (json == null)
                return null;

            return this.jsonService.DeserializeObject<GithubModel>(json);
        }

        private bool IsVersionModel(GithubModel model)
        {
            var versionAssembly = GetAssemblyVersion();
            var versionModel = model.name;
            try
            {
                return (new Version(versionAssembly) < new Version(versionModel));
            }
            catch { }
            return false;
        }

        private string GetDownloadFile(string folder)
        {
            var model = GetGithubModelLast();

            if (model == null) return null;

            if (IsVersionModel(model) == false) return null;

            var bundle = model.assets.FirstOrDefault(e => e.name.EndsWith(BUNDLE_ZIP));

            if (bundle == null) return null;

            DownloadFile(folder, bundle.browser_download_url);

            return model.body;
        }

        #region Get String

        #region Remove???

        private async Task<string> GetStringAsync(string endpoint)
        {
            var request = this.GetRequestMessage(HttpMethod.Get, endpoint);

            var response = await this.client.SendAsync(request);

            var text = await response.Content.ReadAsStringAsync();
            return text;
        }

        private async Task<T> GetAsync<T>(string endpoint) where T : class
        {
            var request = this.GetRequestMessage(HttpMethod.Get, endpoint);

            var response = await this.client.SendAsync(request);

            var text = await response.Content.ReadAsStringAsync();
            return this.ConvertResponseToType<T>(text);
        }


        private HttpRequestMessage GetRequestMessage(HttpMethod httpMethod, string endpoint, HttpContent content = null)
        {
            var request = new HttpRequestMessage(httpMethod, endpoint);

            foreach (var header in this.client.DefaultRequestHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            if (content != null)
            {
                request.Content = content;
            }

            return request;
        }


        private T ConvertResponseToType<T>(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new Exception("Response json is empty");
                }
                var data = this.jsonService.DeserializeObject<T>(json);
                return data;
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Could not convert response to {typeof(T).Name}");
            }
        }
        #endregion

        private string GetString(string address)
        {
            if (IsConnectedToInternet() == false)
                return null;

            try
            {
                using (var client = new System.Net.WebClient())
                {
                    client.Headers.Add("User-Agent", $"{this.GetType().Assembly.GetName().Name}");
                    client.Encoding = System.Text.Encoding.UTF8;
                    return client.DownloadString(address);
                }
            }
            catch { }

            return null;
        }

        #endregion

        #region Download
        private void DownloadFile(string extractPath, string address)
        {
            var fileName = Path.GetFileName(address);
            var zipPath = Path.Combine(extractPath, fileName);

            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent", $"{this.GetType().Assembly.GetName().Name}");
                client.DownloadProgressChanged += DownloadProgressChanged;
                client.DownloadFileCompleted += (s, e) =>
                {
                    DownloadFileCompleted?.Invoke(s, e);
                    if (e.Cancelled) return;
                    try
                    {
                        if (Path.GetExtension(zipPath) == ".zip")
                        {
                            ZipFileHelper.ExtractToDirectory(zipPath, extractPath);
                            File.Delete(zipPath);
                            Console.WriteLine($"Extract: {fileName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        DownloadFileException?.Invoke(ex);
                    }
                };
                client.DownloadFileAsync(new Uri(address), zipPath);
            }
        }
        #endregion

        #region IsConnectedToInternet
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
