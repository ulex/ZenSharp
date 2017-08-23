using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Context;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Templates;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace Github.Ulex.ZenSharp.Integration
{
    [Language(typeof(CSharpLanguage))]
    internal class ZenSharpItemsProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        private static readonly ILogger Log = Logger.GetLogger(typeof(ZenSharpItemsProvider));

        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            return context.ReplaceRangeWithJoinedArguments.Length != 0;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            Log.Info("Add lookupitems");

            var solution = context.PsiModule.GetSolution();

            var ltgConfig = Shell.Instance.GetComponent<LtgConfigWatcher>();

            var iconManager = solution.GetComponent<PsiIconManager>();
            var provider = solution.GetComponent<CSharpExtendedScopeProvider>();
            var textControl = context.BasicContext.TextControl;

            var offset = textControl.Caret.Offset();
            var templateContext = new TemplateAcceptanceContext(
                solution,
                new DocumentOffset(textControl.Document, offset), 
                new DocumentRange(textControl.Document, offset));

            var scopePoints = provider.ProvideScopePoints(templateContext);
            if (ltgConfig.Tree != null)
            {
                var template = new Template("", "", "", true, true, false, new[] {TemplateApplicability.Live})
                {
                    UID = Guid.NewGuid()
                };
                var scopes = scopePoints.ToList();
                Log.Trace("Current scopes: {0}", string.Join(",", scopes));

                if (scopes.Any(scopename => ltgConfig.Tree.IsScopeExist(scopename)))
                {
                    var iconId = iconManager.ExtendToTypicalSize(ServicesThemedIcons.LiveTemplate.Id);
                    collector.Add(new ZenSharpLookupItem(template, ltgConfig.Tree, scopes, iconId));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            Log.Warn("Lookup item for completion is not added, because ZenSharp expand tree is not loaded.");
            return false;
        }
    }
}
