using Github.Ulex.ZenSharp.Core;

using NUnit.Framework;

namespace ZenSharp.Core.Tests
{
    internal sealed class TestParser
    {
        public const string grammar1 = @"
house ::= ""that lay in the house that Jack built""=h

scope ""some"" {
  start ::= <house>
}
";

        [Test]
        public void TestTestParser()
        {
            var parser = new LtgParser();
        }
    }
}
