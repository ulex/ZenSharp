using System;
using System.Collections.Generic;

using NUnit.Framework;
using Github.Ulex.ZenSharp.Core;
using N = Nemerle.Builtins;

namespace ZenSharp.Core.Tests
{
    internal sealed class LeafRulesMatchTest
    {
        [Test]
        public void TestExpandRule()
        {
            var rule = new LeafRule.ExpandRule("short7", "expand");
            Assert.AreEqual(rule.Match("short7ending").Value, "ending");
            Assert.AreEqual(rule.Match("short7").Value, "");
            Assert.AreEqual(rule.Match("short7short7").Value, "short7");
            Assert.IsTrue(rule.Match("wr").IsNone);
        }

        [Test, ExpectedException(typeof(NotImplementedException))]
        public void TestNonTerminal()
        {
            var rule = new LeafRule.NonTerminal("short7");
            rule.Match("wr");
        }

        [Test]
        public void TestSubstitution()
        {
            var rule = new LeafRule.Substitution("hello", new List<N.Tuple<string, string>>());
            Assert.AreEqual(rule.Match("short7").Value, "");
            Assert.AreEqual(rule.Match("wr").Value, "");
            Assert.AreEqual(rule.Match("wr,aa").Value, ",aa");
            Assert.AreEqual(rule.Match("wr`aa").Value, "`aa");
            Assert.AreEqual(rule.Match("wr~aa").Value, "~aa");
        }
    }
}