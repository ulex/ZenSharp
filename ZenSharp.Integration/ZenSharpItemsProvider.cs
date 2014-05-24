using System;
using System.Linq;
using System.Net.Mime;

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.LiveTemplates.Templates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;

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
            var iconManager = context.PsiModule.GetSolution().GetComponent<PsiIconManager>();

            var ltgConfig = context.PsiModule.GetSolution().GetComponent<LtgConfigWatcher>();
            if (ltgConfig.Tree != null)
            {
                var template = new Template("shortcut", "desctiption", "text", true, true, false, TemplateApplicability.Live)
                    {
                        UID = Guid.NewGuid()
                    };
                collector.AddToTop(new ZenSharpLookupItem(iconManager, template, ltgConfig.Tree));
                return true;
            }

            return false;
        }

        protected override void TransformItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            foreach (var item in collector.Items.OfType<ZenSharpLookupItem>())
            {
                //item.Template.Shortcut = "sss" + new Random().Next();
            }
        }
    }
}