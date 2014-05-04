using System;

using Github.Ulex.ZenSharp.Core;

using NUnit.Framework;

namespace ZenSharp.Core.Tests
{
    [TestFixture]
    internal sealed class LiveTemplateMatcherTest
    {
        [Test]
        public void Test()
        {
            var tree = new LtgParser().Parse(TestParser.grammar1).Value;
            var mr = new LiveTemplateMatcher(tree).Match("h", "some");
            Console.WriteLine(mr);
        }
    }
}