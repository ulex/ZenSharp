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
            var tree = new LtgParser().ParseAll(LtgSamples.jack);
            var mr = new LiveTemplateMatcher(tree).Match("hs", "house");
            Console.WriteLine(mr);
        }

        [Test]
        public void TestBase()
        {
            var tree = new LtgParser().ParseAll(LtgSamples.jack);
            var mr = new LiveTemplateMatcher(tree).Match("titCat", "house");
            Console.WriteLine(mr);
        }
    }
}