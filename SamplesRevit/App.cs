using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Reflection;

namespace RevitSamples
{
    public class App : IExternalApplication

    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication app)
        {
            string tabName = "tab";
            string panelName = "panel";
            app.CreateRibbonTab(tabName);
            var panel = app.CreateRibbonPanel(tabName, panelName);
            var assembly = Assembly.GetExecutingAssembly().Location;
            panel.AddItem(new PushButtonData("btn", "Отправить в ПИЛОТ!", assembly, nameof(RevitSamples) + "." + nameof(Command)));
            return Result.Succeeded;
        }
    }
}
