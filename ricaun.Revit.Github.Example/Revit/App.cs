using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.UI;
using System;

namespace ricaun.Revit.Github.Example.Revit
{
    [AppLoader]
    public class App : IExternalApplication
    {
        private static RibbonPanel ribbonPanel;
        public Result OnStartup(UIControlledApplication application)
        {

            ribbonPanel = application.CreatePanel("Github");
            ribbonPanel.CreatePushButton<Commands.CommandUpdate>("Update")
                .SetLargeImage("https://img.icons8.com/material-outlined/32/000000/github.png")
                .SetToolTip(GetToopTip());

#if DEBUG
            ribbonPanel.GetRibbonPanel().CustomPanelTitleBarBackground = System.Windows.Media.Brushes.Salmon;
#endif

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel?.Remove();
            return Result.Succeeded;
        }

        private string GetToopTip()
        {
            var assembly = this.GetType().Assembly;
            var assemblyName = assembly.GetName();
            var result = $"App: {assemblyName.Name}\n";
            result += $"Version: {assemblyName.Version.ToString(3)}\n";
            result += $"Location: {assembly.Location}";

            return result;
        }
    }
}