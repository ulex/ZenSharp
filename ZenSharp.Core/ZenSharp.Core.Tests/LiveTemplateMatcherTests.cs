/*using System.Security;

using Github.Ulex.ZenSharp.Core;

using Nemerle.Collections;

using NUnit.Framework;

namespace ZenSharp.Core.Tests
{
    [TestFixture]
    internal sealed class LiveTemplateMatcherTests
    {
        private const string Scopename = "scopename";

        [Test]
        public void TestLiveTemplateMatcherTests()
        {
            var concatRule = new ConcatRule(new LeafRule[] { new LeafRule.ExpandRule("some", "Expand"), new LeafRule.ExpandRule("2", "2") }.NToList());

            var concatRuleAlt = new ConcatRule(new LeafRule[] { new LeafRule.ExpandRule("some", "Expand3"), new LeafRule.ExpandRule("3", "3") }.NToList());

            var mresult = MatcherFromRule(StartRule(concatRule, concatRuleAlt)).Match("some3", Scopename);

            Assert.IsFalse(mresult.Success);
            Assert.IsTrue(string.IsNullOrEmpty(mresult.Tail));
        }

        private static Rule StartRule(params ConcatRule[] concatRules)
        {
            return new Rule("start", concatRules.ToNList());
        }

        private static LiveTemplateMatcher MatcherFromRule(Rule rule)
        {
            var globalRules = NList.FromArray(new Rule[0]);
            var scope = new TreePart.Scope(new[] { rule }.NToList(), Scopename);

            var st = new LiveTemplateMatcher(new GenerateTree(globalRules, new[] { scope }.NToList()));
            return st;
        }
    }
}*/