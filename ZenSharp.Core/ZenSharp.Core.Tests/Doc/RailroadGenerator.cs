using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Github.Ulex.ZenSharp.Core;

using Nemerle.Core;

using NUnit.Framework;

namespace ZenSharp.Core.Tests.Doc
{
    [TestFixture]
    [Explicit]
    internal sealed class RailroadGenerator
    {
        private GenerateTree _tree;

        private IEnumerable<Rule> _globalRules;

        private readonly HashSet<string> _alwaysExpandRules = new HashSet<string>()
        {
            "space",
            "methodBody",
            "methodArgs",
            "access",
            "SCG",
            //"type",
            "primType",
            "generic",
            "suggType",
            "propertyBody",
            "cursor",
            "classBody"
        };

        private readonly HashSet<string> _ignoredStrings = new HashSet<string>() { " " };

        private const string TemplatesRelativePath = SolutionDir + @"ZenSharp.Integration\Templates.ltg";

        private const string SolutionDir = @"..\..\";

        public const string OutFile = DocDir + "out.html";

        private const string DocDir = SolutionDir + "doc\\";

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _tree = new LtgParser().Parse(File.ReadAllText(TemplatesRelativePath)).Value;
        }

        [Test]
        public void DumpNodes()
        {
            using (var dump = new StreamWriter(OutFile))
            {
                dump.WriteLine(@"<!DOCTYPE html>");
                dump.WriteLine(@"<link rel='stylesheet' href='railroad-diagrams.css'>");
                dump.WriteLine(@"<script src='railroad-diagrams.js'></script>");
                dump.WriteLine(@"<body>");

                _globalRules = _tree.GlobalRules;

                foreach (var scope in _tree.Scopes)
                {
                    _globalRules = scope.Rules.Concat(_tree.GlobalRules);
                    
                    dump.WriteLine("<h1>{0}</h1>", scope.Name);
                    foreach (var rule in scope.Rules.Where(f => !_alwaysExpandRules.Contains(f.Name)))
                    {
                        WriteH2Script(dump, rule.Name, DumpConcatRules(rule.Rules));
                    }

                }

                dump.WriteLine("<h1>Global</h1>");
                foreach (var rule in _globalRules)
                {
                    WriteH2Script(dump, rule.Name, DumpConcatRules(rule.Rules));
                }

                dump.WriteLine(@"</body>");
            }
        }

        private void WriteH2Script(StreamWriter dump, string name, string script)
        {
            dump.WriteLine("<h2>{0}</h2>", name);
            dump.WriteLine("<script>");
            dump.WriteLine("Diagram({0}).addTo();", script);
            dump.WriteLine("</script>");
        }

        public string DumpConcatRules(IEnumerable<ConcatRule> concatRules)
        {
            var list = new List<string>();
            foreach (var concatRule in concatRules)
            {
                list.Add(DumpConcatRule(concatRule));
            }
            return string.Format("Choice(0, {0})", string.Join(",", list));
        }

        private string DumpConcatRule(ConcatRule concatRule)
        {
            var listlr = new List<string>();
            foreach (var leafRule in concatRule.Rules)
            {
                var item = Dump(leafRule as LeafRule.String) ??
                              Dump(leafRule as LeafRule.Substitution) ??
                              Dump(leafRule as LeafRule.ExpandRule) ??
                              Dump(leafRule as LeafRule.InsideRule) ??
                              Dump(leafRule as LeafRule.NonTerminal);
                if (item != null) listlr.Add(item);
            }
            return string.Format("Sequence({0})", string.Join(",", listlr));
        }

        private string Dump(LeafRule.InsideRule leafRule)
        {
            if (leafRule == null) return null;
            return DumpConcatRules(leafRule.Rules.ToArray());
            // return string.Format("NonTerminal('{0}')", leafRule.)
        }

        private string Dump(LeafRule.NonTerminal leafRule)
        {
            if (leafRule == null) return null;
            if (_alwaysExpandRules.Contains(leafRule.Value))
            {
                return DumpConcatRules(_globalRules.First(r => r.Name == leafRule.Value).Rules);
            }
            return string.Format("NonTerminal('{0}')", leafRule.Value);
        }

        private string Dump(LeafRule.ExpandRule leafRule)
        {
            if (leafRule == null) return null;
            return string.Format("NonTerminal('{0} = {1}')", leafRule.Expand, leafRule.Short);
        }

        private string Dump(LeafRule.String leafRule)
        {
            if (leafRule == null) return null;

            if (_ignoredStrings.Contains(leafRule.Value))
            {
                return null;
            }

            var escapeNsNames = Regex.Replace(leafRule.Value, "[\\w.]+\\.", String.Empty);
            return string.Format("Terminal('{0}')", escapeNsNames);
        }

        private string Dump(LeafRule.Substitution leafRule)
        {
            if (leafRule == null) return null;
            return string.Format("NonTerminal('{0}{1}')", leafRule.Expand, leafRule.Short);
        }
    }
}