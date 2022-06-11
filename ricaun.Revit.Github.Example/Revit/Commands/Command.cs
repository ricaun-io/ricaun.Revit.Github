using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.Github;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ricaun.Revit.Github.Example.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            var request = new GithubRequestService("ricaun-io", "ricaun.Nuke.PackageBuilder");

            request.Initialize();

            //var text = request.DownloadLast();
            //System.Windows.MessageBox.Show($">>> {text}");

            //var githubBundleService = new Services.GithubBundleService("ricaun-io", "ricaun.Nuke.PackageBuilder");

            //foreach (var item in githubBundleService.GetBundleModels(Assembly.GetExecutingAssembly()))
            //{
            //    Console.WriteLine(item);
            //}



            //var a = Assembly.GetExecutingAssembly();
            //var version = a.GetName().Version.ToString();



            var githubService = new Github.Services.GithubBundleService("ricaun-io", "ricaun.Nuke.PackageBuilder2");
            var task = Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(100);
                        var text = await githubService.DownloadStringAsync("https://api.github.com/repos/ricaun-io/ricaun.Nuke.PackageBuilder/releases/latest");
                        Console.WriteLine($"Result {text}");


                        var bundleModels = await githubService.GetBundleModelsAsync();
                        Console.WriteLine(bundleModels.Count());
                        foreach (var bundleModel in bundleModels)
                        {
                            Console.WriteLine(bundleModel);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });


            //TaskDialog.Show("Hello", "Luiz");

            //task.GetAwaiter().GetResult();

            //var service = new Services.GithubBundleService("ricaun-io", "ricaun.Nuke.PackageBuilder");
            //Console.WriteLine($"1.2.4 - {version} {service.GetBundleDownloadUrl("1.2.4", version)}");
            //Console.WriteLine($"0.0.10 - {version} {service.GetBundleDownloadUrl("0.0.10", version)}");
            //Console.WriteLine($"0.0.10 {service.GetBundleDownloadUrl("0.0.10")}");

            return Result.Succeeded;
        }
    }
}
