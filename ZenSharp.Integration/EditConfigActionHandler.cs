using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.IDE;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.UI.ActionsRevised;
using JetBrains.UI.MenuGroups;
using JetBrains.Util;

using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace Github.Ulex.ZenSharp.Integration
{
    [ActionGroup(ActionGroupInsertStyles.Embedded)]
    public class ZenSharpGroup : IAction, IInsertLast<VsMainMenuGroup>
    {
        public ZenSharpGroup(Separator sep, EditConfigActionHandler handler)
        {
        }
    }

    [Action("Edit ZenSharp templates", Id = 11122233)]
    public class EditConfigActionHandler : IExecutableAction
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
                .OpenFile(
                    FileSystemPath.CreateByCanonicalPath(ZenSharpSettings.GetTreePath(settings.TreeFilename)),
                    true,
                    TabOptions.Default);
        }
    }
}