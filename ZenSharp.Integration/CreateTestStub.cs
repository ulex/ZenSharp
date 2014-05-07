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
            collector.AddToTop(new MyLookupItem(iconManager, template, false, "IgnoreSoftOnSpace", ltgConfig.Tree) { IgnoreSoftOnSpace = false });
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

        private readonly string _her;

        private RichText _displayName;

        public bool _dynamic = true;

        public MatchingResult _matchingResult;

        private string _prefix;

        private Template _template;

        private GenerateTree _tree;

        public MyLookupItem(PsiIconManager psiIconManager, Template template, bool showDescription, string her, GenerateTree tree)
            : base(psiIconManager, template, showDescription)
        {
            _tree = tree;
            _template = template;
            _matchingResult = new MatchingResult(3, "dd", 1000);
            _psiIconManager = psiIconManager;
            _her = her;
        }

        bool ILookupItem.IsDynamic
        {
            get
            {
                return _dynamic;
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
                return new RichText(_template.Text) + _prefix;
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
            _prefix = matcher.Match(prefix, "InCSharpTypeMember").Expand(prefix);
            return _matchingResult;
        }
    }
}