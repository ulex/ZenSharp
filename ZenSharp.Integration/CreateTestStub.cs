using System;
using System.Linq;
using System.Net.Mime;

using Github.Ulex.ZenSharp.Core;

using JetBrains.ProjectModel;
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

namespace Github.Ulex.ZenSharp.Integration
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
            //collector.AddAtDefaultPlace(context.LookupItemsFactory.InitializeLookupItem(myLookupItem));

            //collector.AddToTop(new MyLookupItem(iconManager, template, true, "with desc"));
            //collector.AddToTop(new MyLookupItem(iconManager, template, false, "with no desc"));
            var ltgConfig = context.PsiModule.GetSolution().GetComponent<LtgConfigWatcher>();
            if (ltgConfig.Tree != null)
            {
                collector.AddToTop(new MyLookupItem(iconManager, template, false, ltgConfig.Tree) { IgnoreSoftOnSpace = false });
            }
            return true;
        }

        protected override void TransformItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            foreach (var item in collector.Items.OfType<MyLookupItem>())
            {
                //item.Template.Shortcut = "sss" + new Random().Next();
            }
        }
    }

    internal class MyLookupItem : TemplateLookupItem, ILookupItem
    {
        private readonly PsiIconManager _psiIconManager;

        public MatchingResult _matchingResult;

        private string _matchExpand;

        private Template _template;

        private GenerateTree _tree;

        public MyLookupItem(PsiIconManager psiIconManager, Template template, bool showDescription, GenerateTree tree)
            : base(psiIconManager, template, showDescription)
        {
            _tree = tree;
            _template = template;
            _matchingResult = new MatchingResult(3, "dd", 1000);
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
                return new RichText(_template.Text + "|" + _matchExpand);
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
            if (_tree == null)
            {
                return null;
            }
            var matcher = new LiveTemplateMatcher(_tree);
            _matchExpand = matcher.Match(prefix, "InCSharpTypeMember").Expand(prefix);
            if (!string.IsNullOrEmpty(_matchExpand))
            {
                _template.Text = _matchExpand;
            }
            return _matchingResult;
        }
    }
}