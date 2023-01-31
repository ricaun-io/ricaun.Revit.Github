namespace ricaun.Revit.Github.Example.Revit
{
    public static class InfoCenterUtils
    {
        public static void ShowBalloon(string title, string category = null)
        {
            if (title == null) return;
            Autodesk.Internal.InfoCenter.ResultItem ri = new Autodesk.Internal.InfoCenter.ResultItem();
            ri.Category = category ?? typeof(InfoCenterUtils).Assembly.GetName().Name;
            ri.Title = title.Trim();
            Autodesk.Windows.ComponentManager.InfoCenterPaletteManager.ShowBalloon(ri);
        }
    }
}