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

            var pathBundleService = new PathBundleService();
            var request = new GithubRequestService();

            //Process.Start(pathBundleService.GetAssemblyPath());
            string text = "Bundle no Found";
            if (pathBundleService.TryGetPath(out string path))
            {
                Console.WriteLine($"Bundle: {path }");
                text = request.GetDownloadFile(path);
            }
            System.Windows.MessageBox.Show(text);

            return Result.Succeeded;
        }
    }
}
