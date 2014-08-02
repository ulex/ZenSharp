using System.Collections.Generic;
using System.Linq;

using Github.Ulex.ZenSharp.Core;

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Scope;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Util;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.ReSharper.LiveTemplates.Templates;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace Github.Ulex.ZenSharp.Integration
{
    internal class ZenSharpLookupItem : TemplateLookupItem, ILookupItem
    {
        private readonly IEnumerable<string> _scopes;

        private readonly MatchingResult _matchingResult;

        private readonly PsiIconManager _psiIconManager;

        private readonly Template _template;

        private readonly GenerateTree _tree;

        private string _matchExpand;
        
        public ZenSharpLookupItem(PsiIconManager psiIconManager, Template template, GenerateTree tree, IEnumerable<ITemplateScopePoint> scopePoints)
            : base(psiIconManager, template, true)
        {
            _tree = tree;
            _scopes = scopePoints.Select(sp => sp.GetType().Name).ToArray();
            _template = template;
            _matchingResult = new MatchingResult(3, "dd", 10000);
            _psiIconManager = psiIconManager;
            //IgnoreSoftOnSpace = true;
        }

        RichText ILookupItem.DisplayName
        {
            get
            {
                return new RichText(_template.Text );
            }
        }

        IconId ILookupItem.Image
        {
            get
            {
                return _psiIconManager.ExtendToTypicalSize(ServicesThemedIcons.LiveTemplate.Id);
            }
        }

#if !RESHARPER_71
        bool ILookupItem.IsDynamic
        {
            get
            {
                return true;
            }
        }
#endif

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
            string scopeName = null;
            foreach (var scope1 in _scopes)
            {
                scopeName = scopeName ?? (_tree.IsScopeExist(scope1) ? scope1 : null);
            }

            if (scopeName == null)
            {
                return null;
            }

            var matchResult = matcher.Match(prefix, scopeName);
            if (matchResult.Success)
            {
                _matchExpand = matchResult.Expand(prefix);
                _template.Text = _matchExpand;
                var appliedRules = matchResult.ReMatchLeafs(prefix);
                foreach (var subst in appliedRules.Where(ar => ar.This is LeafRule.Substitution))
                {
                    var rule = (LeafRule.Substitution)subst.This;
                    var macros = rule.Macros.Replace("\\0", subst.Short);
                    if (string.IsNullOrEmpty(macros))
                    {
                        macros = "complete()";
                    }
                    _template["$" + rule.Name  + "$"] = macros;
                }
                return _matchingResult;
            }
            else
            {
                return null;
            }
        }
    }
}