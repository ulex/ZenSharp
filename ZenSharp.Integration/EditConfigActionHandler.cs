using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.IDE;
using JetBrains.Util;

using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

#if RESHARPER_90
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.UI.ActionsRevised;using IActionHandler = JetBrains.UI.ActionsRevised.IAction;
#endif

namespace Github.Ulex.ZenSharp.Integration
{
#if RESHARPER_90
    [Action("ZenSharp.EditConfig")]
#else
    [ActionHandler("ZenSharp.EditConfig")]
#endif
    public class EditConfigActionHandler : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            var solution = context.GetData(DataConstants.SOLUTION);
            return solution != null;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var store = Shell.Instance.GetComponent<ISettingsStore>();
            var ctx = store.BindToContextTransient(ContextRange.ApplicationWide);
            var settings = ctx.GetKey<ZenSharpSettings>(SettingsOptimization.DoMeSlowly);

            var solution = context.GetData(DataConstants.SOLUTION);
            EditorManager.GetInstance(solution)
                .OpenFile(FileSystemPath.CreateByCanonicalPath(settings.GetTreePath), true, TabOptions.Default);
        }
    }
}