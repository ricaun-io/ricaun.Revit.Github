using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.Github;
using System;
using System.Diagnostics;
using System.Reflection;

namespace ricaun.Revit.Github.Example.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            var request = new GithubRequestService("ricaun-io", "ricaun.Nuke.PackageBuilder");
            string text = request.DownloadLast();
            System.Windows.MessageBox.Show(text);



            var a = Assembly.GetExecutingAssembly();
            var version = a.GetName().Version.ToString();

            //var service = new Services.GithubBundleService("ricaun-io", "ricaun.Nuke.PackageBuilder");
            //Console.WriteLine($"1.2.4 - {version} {service.GetBundleDownloadUrl("1.2.4", version)}");
            //Console.WriteLine($"0.0.10 - {version} {service.GetBundleDownloadUrl("0.0.10", version)}");
            //Console.WriteLine($"0.0.10 {service.GetBundleDownloadUrl("0.0.10")}");

            return Result.Succeeded;
        }
    }
}
