using System;
using System.Linq;

using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Util;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.ReSharper.LiveTemplates.Templates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace Serializer
{
    [Language(typeof(CSharpLanguage))]
    internal class CreateTestStub : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
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
            var myLookupItem = new MyLookupItem(iconManager, template, true);
            //collector.AddAtDefaultPlace(context.LookupItemsFactory.InitializeLookupItem(myLookupItem));
            
            collector.AddToTop(myLookupItem);
            return true;
        }

        protected override void TransformItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            foreach (var item in collector.Items.OfType<MyLookupItem>())
            {
                item.Template.Shortcut = "sss" + new Random().Next();
            }
        }
    }

    internal class MyLookupItem : TemplateLookupItem, ILookupItem
    {
        private readonly PsiIconManager _psiIconManager;

        private RichText _displayName;

        public MyLookupItem(PsiIconManager psiIconManager, Template template, bool showDescription)
            : base(psiIconManager, template, showDescription)
        {
            _psiIconManager = psiIconManager;
        }

        bool ILookupItem.IsDynamic
        {
            get
            {
                return true;
            }
        }

        IconId ILookupItem.Image
        {
            get
            {
                return _psiIconManager.ExtendToTypicalSize(ServicesThemedIcons.Recursion2.Id);
            }
        }

        RichText ILookupItem.DisplayName
        {
            get
            {
                return new RichText("hello" + new Random().Next());
            }
        }

        public void Accept(
            ITextControl textControl,
            TextRange nameRange,
            LookupItemInsertType lookupItemInsertType,
            Suffix suffix,
            ISolution solution,
            bool keepCaretStill)
        {
            base.Accept(textControl, nameRange, lookupItemInsertType, suffix, solution, keepCaretStill);
        }

        public bool AcceptIfOnlyMatched(LookupItemAcceptanceContext itemAcceptanceContext)
        {
            return true;
        }

        MatchingResult ILookupItem.Match(string prefix, ITextControl textControl)
        {
            return new MatchingResult(3, "dd", 1000);
        }
    }
}