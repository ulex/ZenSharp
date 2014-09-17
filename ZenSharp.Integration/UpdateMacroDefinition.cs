using System;

using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.LiveTemplates;

using NLog;

namespace Github.Ulex.ZenSharp.Integration
{
    [ActionHandler("ZenSharp.UpdateMacroDefinition")]
    public class UpdateMacroDefinition : IActionHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            try
            {
                var viewer = Shell.Instance.GetComponents<IMacroDefinition>();
                foreach (var macroDefinition in viewer)
                {
                    var attr = MacroDescriptionFormatter.GetMacroAttribute(macroDefinition);
                    if (attr != null)
                    {
                        Log.Info("// {0}", attr.Name);
                        Log.Info("// {0}", attr.ShortDescription);
                        Log.Info("// {0}", attr.LongDescription);
                        foreach (var parameterInfo in macroDefinition.Parameters)
                        {
                            Log.Info("// P: {0}", parameterInfo.ParameterType);
                        }
                        Log.Info(string.Empty);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("IMacro definition enumeration failed", e);
            }
        }
    }
}
