using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ricaun.Revit.Github.Services
{
    /// <summary>
    /// PathBundleService
    /// </summary>
    public class PathBundleService
    {
        #region Constructor
        private readonly Assembly assembly;
        /// <summary>
        /// PathBundleService
        /// </summary>
        /// <param name="assembly"></param>
        public PathBundleService(Assembly assembly)
        {
            this.assembly = assembly;
        }
        /// <summary>
        /// PathBundleService
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public PathBundleService()
        {
            this.assembly = Assembly.GetCallingAssembly();
        }
        /// <summary>
        /// GetAssembly
        /// </summary>
        /// <returns></returns>
        public Assembly GetAssembly() => assembly;
        #endregion

        /// <summary>
        /// GetAssemblyPath
        /// </summary>
        /// <returns></returns>
        public string GetAssemblyPath()
        {
            string ExecutingAssemblyPath = GetAssembly().Location;
            string folder = Path.GetDirectoryName(ExecutingAssemblyPath);
            return folder;
        }

        /// <summary>
        /// Try to get path Bundle
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool TryGetPath(out string path)
        {
            path = GetPath();
            return path != null;
        }

        /// <summary>
        /// Get Bundle Path
        /// </summary>
        /// <param name="upPath"></param>
        /// <returns></returns>
        public string GetPath(int upPath = 0)
        {
            string path = GetAssemblyPath();
            string bundle = path;

            if (path == null) return null;
            if (path == "") return null;

            int maxPath = 6;
            string endWith = ".bundle";

#if DEBUG
            endWith = ".Console"; // Find Temp Folder ricaun.Console
#endif

            try
            {
                int i = 0;
                for (i = 0; i < maxPath; i++)
                {
                    FileInfo fileInfo = new FileInfo(bundle);
                    if (fileInfo.Name.EndsWith(endWith))
                    {
                        break;
                    }
                    bundle = fileInfo.DirectoryName;
                }

                if (i < maxPath)
                {
                    bundle = path;
                    for (int j = 1 + upPath; j < i; j++)
                    {
                        bundle = Path.Combine(bundle, "..");
                    }
                    FileInfo fileInfo = new FileInfo(bundle);

                    bundle = fileInfo.DirectoryName;

                    return bundle;
                }

            }
            catch { }
            return null;
        }
    }
}
