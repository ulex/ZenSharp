using System;
using System.Linq;
using System.Net.Mime;

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Context;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.LiveTemplates.CSharp.Scope;
using JetBrains.ReSharper.LiveTemplates.Templates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;

namespace Github.Ulex.ZenSharp.Integration
{
    [Language(typeof(CSharpLanguage))]
    internal class ZenSharpItemsProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            return true;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            var solution = context.PsiModule.GetSolution();
            var iconManager = solution.GetComponent<PsiIconManager>();
            var ltgConfig = solution.GetComponent<LtgConfigWatcher>();
            var provider = solution.GetComponent<CSharpScopeProvider>();
            var textControl = context.BasicContext.TextControl;
            var scopes = provider.ProvideScopePoints(
                new TemplateAcceptanceContext(solution, textControl.Document, textControl.Caret.Offset()));
            if (ltgConfig.Tree != null)
            {
                var template = new Template("shortcut", "desctiption", "text", true, true, false, TemplateApplicability.Live)
                    {
#if !RESHARPER_71
            UID = Guid.NewGuid()
#endif
                    };
                collector.AddToTop(new ZenSharpLookupItem(iconManager, template, ltgConfig.Tree, scopes));
                return true;
            }

            return false;
        }
    }
}