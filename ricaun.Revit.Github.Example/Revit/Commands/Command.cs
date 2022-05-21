using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.Github;
using System;
using System.Diagnostics;

namespace ricaun.Revit.Github.Example.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            var request = new GithubRequestService("ricaun-io", "RevitAddin.TemporaryGraphicsExample");
            request.DownloadProgressChanged += (s, e) =>
            {
                //Console.WriteLine($"--- Download: {e.ProgressPercentage}%");
            };

            request.DownloadFileCompleted += (s, e) =>
            {
                Console.WriteLine($"Download Complete");
            };

            request.DownloadFileException += (ex) =>
            {
                Console.WriteLine($"Exception {ex}");
            };

            //Process.Start(pathBundleService.GetAssemblyPath());
            string text = request.DownloadLast();
            System.Windows.MessageBox.Show(text);

            return Result.Succeeded;
        }
    }
}
