using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.IDE;
using JetBrains.Util;

using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

#if RESHARPER_90 || RESHARPER_91
using JetBrains.UI.MenuGroups;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.UI.ActionsRevised;
using JetBrains.ReSharper.Feature.Services.Menu;
#endif


namespace Github.Ulex.ZenSharp.Integration
{
#if RESHARPER_90 || RESHARPER_91
    [ActionGroup(ActionGroupInsertStyles.Embedded)]
    public class ZenSharpGroup : IAction, IInsertLast<VsMainMenuGroup>
    {
        public ZenSharpGroup(Separator sep, EditConfigActionHandler handler)
        {
        }
    }

    [Action("Edit ZenSharp templates", Id = 11122233)]
    public class EditConfigActionHandler : IExecutableAction
#else

    [ActionHandler("ZenSharp.EditConfig")]
    public class EditConfigActionHandler : IActionHandler
#endif
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
                .OpenFile(FileSystemPath.CreateByCanonicalPath(ZenSharpSettings.GetTreePath(settings.TreeFilename)), true, TabOptions.Default);
        }
    }
}