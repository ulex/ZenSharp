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
            Assert.AreEqual(rule.Match("short7ending").Short, "short7");
            Assert.AreEqual(rule.Match("short7ending").Expand, "expand");
            Assert.AreEqual(rule.Match("short7").Short, "short7");
            Assert.AreEqual(rule.Match("short7short7").Short, "short7");
            Assert.IsFalse(rule.Match("wr").Success);
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
            Assert.AreEqual(rule.Match("short7").Short, "short7");
            Assert.AreEqual(rule.Match("wr").Short, "wr");
            Assert.AreEqual(rule.Match("wr,aa").Short, "wr");
            Assert.AreEqual(rule.Match("wr`aa").Short, "wr");
            Assert.AreEqual(rule.Match("wr~aa").Short, "wr");
        }


        [Test]
        public void TestString()
        {
            var rule = new LeafRule.String("short7");
            Assert.IsTrue(rule.Match("wr").Success);
            Assert.AreEqual(rule.Match("wr").Expand, "short7");
            Assert.AreEqual(rule.Match("wr").Short, "");
        }
    }
}