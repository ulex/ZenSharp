using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.Util;

namespace Github.Ulex.ZenSharp.Integration
{
    [ActionHandler("ZenSharp.EditConfig")]
    public class EditConfigActionHandler : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            //ConfigurationManager.RefreshSection("Github.Ulex.ZenSharp.Integration.Properties.Settings");
            //var component = context.GetComponent<LtgConfigWatcher>();
            //MessageBox.ShowInfo("Hello" + component + component.ConfigPath);
            var curpath = Assembly.GetExecutingAssembly().Location;
            var assemblyDir = Path.GetDirectoryName(curpath);
            Process.Start(assemblyDir);
        }
    }
}