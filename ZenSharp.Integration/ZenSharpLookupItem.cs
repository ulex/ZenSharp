using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;

using Github.Ulex.ZenSharp.Core;
using Github.Ulex.ZenSharp.Integration.Extension;

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Util;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.LiveTemplates;
using JetBrains.ReSharper.LiveTemplates.Templates;
using JetBrains.TextControl;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

using NLog;

namespace Github.Ulex.ZenSharp.Integration
{
    /// <summary>
    /// todo: remove inherence
    /// </summary>
    internal class ZenSharpLookupItem : TemplateLookupItem, ILookupItem
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IEnumerable<string> _scopes;

        private readonly MatchingResult _matchingResult;

        /// <summary>
        /// todo: remove
        /// </summary>
        private readonly Template _template;

        private readonly GenerateTree _tree;

        private readonly IconId _iconId;

        private string _displayName;

        public ZenSharpLookupItem(Template template, GenerateTree tree, IEnumerable<string> scopes, IconId iconId)
            : base(null, template, true)
        {
            _tree = tree;
            _scopes = scopes;
            _template = template;
            // todo: what is it?
            _matchingResult = new MatchingResult(3, "dd", 10000);
            Log.Info("Creating ZenSharpLookupItem with template = {0}", template);
            _iconId = iconId;
            _displayName = _template.Text;
        }

        RichText ILookupItem.DisplayName
        {
            get
            {
                return new RichText(_displayName);
            }
        }

        IconId ILookupItem.Image
        {
            get
            {
                return _iconId;
            }
        }

        bool ILookupItem.IsDynamic
        {
            get
            {
                return true;
            }
        }

        bool ILookupItem.AcceptIfOnlyMatched(LookupItemAcceptanceContext itemAcceptanceContext)
        {
            return true;
        }

        MatchingResult ILookupItem.Match(string prefix, ITextControl textControl)
        {
            Log.Info("Match prefix = {0}", prefix);
            if (_tree == null || string.IsNullOrEmpty(prefix))
            {
                Log.Error("Expand tree is null, return.");
                return null;
            }
            var matcher = new LiveTemplateMatcher(_tree);
            string scopeName = null;
            foreach (var scope in _scopes)
            {
                scopeName = scopeName ?? (_tree.IsScopeExist(scope) ? scope : null);
            }
            Log.Debug("Scope name = {0}", scopeName);

            if (scopeName == null)
            {
                return null;
            }
            try
            {
                return GetMatchingResult(prefix, matcher, scopeName);
            }
            catch (Exception e)
            {
                Log.Error("Exception during match", e);
                return null;
            }
        }

        private MatchingResult GetMatchingResult(string prefix, LiveTemplateMatcher matcher, string scopeName)
        {
            var matchResult = matcher.Match(prefix, scopeName);
            if (matchResult.Success)
            {
                FillText(prefix, matchResult);
                Log.Info("Successfull match in scope [{1}]. Return [{0}]", _matchingResult, scopeName);
                return _matchingResult;
            }
            else if(matchResult.Suggestion != null && string.IsNullOrEmpty(matchResult.Suggestion.Tail))
            {
                FillText(prefix, matchResult.Suggestion);
                Log.Info("Suggestion match in scope [{1}] with result [{0}]", _matchingResult, scopeName);

                // todo: review parameters
                return new MatchingResult(prefix.Length, "z", 1);
            }
            else
            {
                Log.Info("No completition found for {0} in scope {1}", prefix, scopeName);
                return null;
            }
        }

        private string FillText(string prefix, LiveTemplateMatcher.MatchResult matchResult)
        {
            var matchExpand = matchResult.Expand(prefix);
            _template.Text = matchExpand;
            Log.Debug("Template text: {0}", matchExpand);
            _displayName = matchResult.ExpandDisplay(prefix);

            FillMacros(prefix, matchResult);
            return matchExpand;
        }

        private void FillMacros(string prefix, LiveTemplateMatcher.MatchResult matchResult)
        {
            _template.Fields.Clear();
            var appliedRules = matchResult.ReMatchLeafs(prefix);
            foreach (var subst in appliedRules.Where(ar => ar.This is LeafRule.Substitution))
            {
                var rule = (LeafRule.Substitution)subst.This;
                var macros = rule.Macros();
                if (string.IsNullOrEmpty(macros))
                {
                    macros = "complete()";
                }
                else
                {
                    macros = macros.Replace("\\0", subst.Short);
                }
                Log.Debug("Place holder macro: {0}, {1}", macros, rule.Name);
                _template.Fields.Add(new TemplateField(rule.Name, macros, 0));
            }
        }
    }
}