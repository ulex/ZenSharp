using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.Components;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.LiveTemplates;
using JetBrains.Util;

namespace Github.Ulex.ZenSharp.Integration
{
#if !RESHARPER_71
    [ActionHandler("ZenSharp.UpdateMacroDefinition")]
    public class UpdateMacroDefinition : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var viewer = Shell.Instance.GetComponents<IMacroDefinition>();
            using (var f = File.CreateText("c:\\out.txt"))
                foreach (var macroDefinition in viewer)
                {
                    var attr = MacroDescriptionFormatter.GetMacroAttribute(macroDefinition);
                    if (attr != null)
                    {
                        f.WriteLine("// {0}", attr.Name);
                        f.WriteLine("// {0}", attr.ShortDescription);
                        f.WriteLine("// {0}", attr.LongDescription);
                        foreach (var parameterInfo in macroDefinition.Parameters)
                        {
                            f.WriteLine("// P: {0}", parameterInfo.ParameterType.ToString());
                        }
                        f.WriteLine();
                    }
                }
        }
    }

#endif
}
