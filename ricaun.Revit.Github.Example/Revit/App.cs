using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.UI;
using System;
using System.Windows.Media;

namespace ricaun.Revit.Github.Example.Revit
{

    [Console]
    public class App : IExternalApplication
    {
        private static RibbonPanel ribbonPanel;
        public Result OnStartup(UIControlledApplication application)
        {
            ribbonPanel = application.CreatePanel("Github");
            ribbonPanel.AddPushButton<Commands.Command>("Command")
                .SetLargeImage("https://img.icons8.com/material-outlined/32/000000/github.png");

#if DEBUG
            ribbonPanel.GetRibbonPanel().CustomPanelTitleBarBackground = Brushes.Red;
#endif

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel?.Remove();
            return Result.Succeeded;
        }
    }

}