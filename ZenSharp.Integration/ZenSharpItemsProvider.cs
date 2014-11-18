using System;
using System.Linq;

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Context;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;

using NLog;
#if RESHARPER_82
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.LiveTemplates.Templates;
#endif

#if RESHARPER_90
using JetBrains.Util;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Templates;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
#endif

namespace Github.Ulex.ZenSharp.Integration
{
    [Language(typeof(CSharpLanguage))]
    internal class ZenSharpItemsProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            return true;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            Log.Info("Add lookupitems");

            var solution = context.PsiModule.GetSolution();
            if (solution == null) return false;

            var ltgConfig = solution.GetComponent<LtgConfigWatcher>();

            var iconManager = solution.GetComponent<PsiIconManager>();
            var provider = solution.GetComponent<CSharpExtendedScopeProvider>();
            var textControl = context.BasicContext.TextControl;

            var offset = textControl.Caret.Offset();
#if RESHARPER_90
            var templateContext = new TemplateAcceptanceContext(solution, textControl.Document, offset, new TextRange(offset));
#else
            var templateContext = new TemplateAcceptanceContext(solution, textControl.Document, offset);
#endif

            var scopePoints = provider.ProvideScopePoints(templateContext);
            if (ltgConfig.Tree != null)
            {
                var template = new Template("", "", "", true, true, false, TemplateApplicability.Live)
                {
                    UID = Guid.NewGuid()
                };
                var scopes = scopePoints.ToList();
                Log.Debug("Current scopes: {0}", string.Join(",", scopes));
                var iconId = iconManager.ExtendToTypicalSize(ServicesThemedIcons.LiveTemplate.Id);
                collector.AddAtDefaultPlace(new ZenSharpLookupItem(template, ltgConfig.Tree, scopes, iconId));
                return true;
            }
            else
            {
                Log.Warn("Lookup item for completion is not added, because ZenSharp expand tree is not loaded.");
            }

            return false;
        }
    }
}