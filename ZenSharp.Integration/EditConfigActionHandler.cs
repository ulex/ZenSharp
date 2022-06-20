using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.Actions.MenuGroups;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using JetBrains.Diagnostics;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Util;

namespace Github.Ulex.ZenSharp.Integration
{
    [ActionGroup(ActionGroupInsertStyles.Embedded)]
    public class ZenSharpGroup : IAction, IInsertLast<VsMainMenuGroup>
    {
        public ZenSharpGroup(Separator sep, EditConfigActionHandler handler)
        {
        }
    }

    [Action("Edit ZenSharp templates")]
    public class EditConfigActionHandler : IExecutableAction
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            var solution = context.GetData(ProjectModelDataConstants.SOLUTION);
            return solution != null;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var store = Shell.Instance.GetComponent<ISettingsStore>();
            var ctx = store.BindToContextTransient(ContextRange.ApplicationWide);
            var settings = ctx.GetKey<ZenSharpSettings>(SettingsOptimization.DoMeSlowly);

            var solution = context.GetData(ProjectModelDataConstants.SOLUTION).NotNull("solution != null");
            var fsp = VirtualFileSystemPath.CreateByCanonicalPath(ZenSharpSettings.GetTreePath(settings.TreeFilename), InteractionContext.Local);
            solution.GetComponent<IEditorManager>().OpenFileAsync(fsp, OpenFileOptions.DefaultActivate);
        }
    }
}
