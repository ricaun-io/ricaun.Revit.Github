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
    public class CommandUpdate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            var request = new GithubRequestService("ricaun-io", "ricaun.Revit.Github");

            Task.Run(async () =>
            {
                var result = await request.Initialize((text) => { Console.WriteLine(text); });
                InfoCenterUtils.ShowBalloon($"Download: {result}");
            });

            return Result.Succeeded;
        }
    }
}
