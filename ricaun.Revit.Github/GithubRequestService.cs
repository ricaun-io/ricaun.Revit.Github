using ricaun.Revit.Github.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ricaun.Revit.Github
{
    public class GithubRequestService
    {
        private readonly HttpClient client;
        private readonly JsonService jsonService;
        public GithubRequestService()
        {
            this.client = new HttpClient();
            this.client.Timeout = TimeSpan.FromMilliseconds(5000);
            this.jsonService = new JsonService();
        }

        // ricaun-io/ricaun.Nuke.PackageBuilder
        private string GenereteApiGithub(string user, string repo)
        {
            return $"https://api.github.com/repos/{user}/{repo}/releases/latest";
        }

        public string url => GenereteApiGithub("ricaun-io", "ricaun.Nuke.PackageBuilder");
        //"https://api.github.com/repos/ricaun-io/ricaun.Nuke.PackageBuilder/releases/latest";

        public string GetDownloadFile(string folder)
        {
            var json = GetString(url);
            if (json == null) return "";

            var model = this.jsonService.DeserializeObject<GithubModel>(json);

            Console.WriteLine(model);

            var bundle = model.assets.FirstOrDefault(e => e.name.EndsWith("bundle.zip"));

            if (bundle != null)
            {
                Console.WriteLine($"{bundle.name} {bundle.download_count} {bundle.browser_download_url}");
                DownloadFile(folder, bundle.browser_download_url);
            }

            return model.body;
        }

        public async Task<string> GetStringAsync(string endpoint)
        {
            var request = this.GetRequestMessage(HttpMethod.Get, endpoint);

            var response = await this.client.SendAsync(request);

            var text = await response.Content.ReadAsStringAsync();
            return text;
        }

        public async Task<T> GetAsync<T>(string endpoint) where T : class
        {
            var request = this.GetRequestMessage(HttpMethod.Get, endpoint);

            var response = await this.client.SendAsync(request);

            var text = await response.Content.ReadAsStringAsync();
            return this.ConvertResponseToType<T>(text);
        }

        public string GetString(string address)
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

        public void DownloadFile(string extractPath, string address)
        {
            var fileName = Path.GetFileName(address);
            var zipPath = Path.Combine(extractPath, fileName);
            //var zipPath = $@"D:\Github\{fileName}";
            //var extractPath = Path.GetDirectoryName(zipPath);

            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent", $"{this.GetType().Assembly.GetName().Name}");
                client.DownloadProgressChanged += (s, e) => { Console.WriteLine($"Download: {e.ProgressPercentage}%"); };
                client.DownloadFileCompleted += (s, e) =>
                {
                    Console.WriteLine($"Download Complete: {e.UserState} {e.Cancelled} {e.Error}");
                    if (e.Cancelled) return;
                    try
                    {
                        if (Path.GetExtension(zipPath) == ".zip")
                        {
                            Console.WriteLine("ZipFile Extract");
                            ZipFileHelper.ExtractToDirectory(zipPath, extractPath);
                            File.Delete(zipPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                };
                client.DownloadFileAsync(new Uri(address), zipPath);
            }
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

        #region private
        private bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        #endregion
    }



    public class GithubModel
    {
        public string url { get; set; }
        public string assets_url { get; set; }
        public string upload_url { get; set; }
        public string html_url { get; set; }
        public int id { get; set; }
        public Author author { get; set; }
        public string node_id { get; set; }
        public string tag_name { get; set; }
        public string target_commitish { get; set; }
        public string name { get; set; }
        public bool draft { get; set; }
        public bool prerelease { get; set; }
        public DateTime created_at { get; set; }
        public DateTime published_at { get; set; }
        public Asset[] assets { get; set; }
        public string tarball_url { get; set; }
        public string zipball_url { get; set; }
        public string body { get; set; }

        public override string ToString()
        {
            return $"{name} ({string.Join(" ", assets.Select(e => e.name))}) \r\n{body}";
        }
    }

    public class Author
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Asset
    {
        public string url { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public Uploader uploader { get; set; }
        public string content_type { get; set; }
        public string state { get; set; }
        public int size { get; set; }
        public int download_count { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string browser_download_url { get; set; }
    }

    public class Uploader
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }
}
