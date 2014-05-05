using System;

using Github.Ulex.ZenSharp.Core;

using NUnit.Framework;

namespace ZenSharp.Core.Tests
{
    [TestFixture, Explicit]
    internal sealed class DevTests
    {
        [Test]
        public void TestDevTests()
        {
            var gt = new LtgParser().ParseAll(LtgSamples.Templates);
            var ltm = new LiveTemplateMatcher(gt);
            var input = "mSomeThing";
            var m = ltm.Match(input, "InCSharpTypeMember");
            Console.WriteLine(m.Expand(input));
        }
    }
}