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
            var template = new Template("sss" + new Random().Next(), "desctiption", "expand ($END$) end", false, true, false, TemplateApplicability.Live);
            template.UID = Guid.NewGuid();
            var iconManager = context.PsiModule.GetSolution().GetComponent<PsiIconManager>();
            //collector.AddAtDefaultPlace(context.LookupItemsFactory.InitializeLookupItem(myLookupItem));

            //collector.AddToTop(new MyLookupItem(iconManager, template, true, "with desc"));
            //collector.AddToTop(new MyLookupItem(iconManager, template, false, "with no desc"));
            var ltgConfig = context.PsiModule.GetSolution().GetComponent<LtgConfigWatcher>();
            if (ltgConfig.Tree != null)
            {
                collector.AddToTop(new ZenSharpLookupItem(iconManager, template, false, ltgConfig.Tree) { IgnoreSoftOnSpace = false });
            }
            return true;
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