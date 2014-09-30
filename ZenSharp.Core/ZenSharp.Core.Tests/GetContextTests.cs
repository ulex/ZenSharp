using System;

using Github.Ulex.ZenSharp.Core;

using NUnit.Framework;

namespace ZenSharp.Core.Tests
{
    [TestFixture]
    internal sealed class GetContextTests
    {
        string _testFile = new TemplatesTests()._content;
        
        [Test]
        public void TestGetContextForAnyPosition()
        {
            for (int i = 0; i < _testFile.Length + 4; i++)
            {
                Console.WriteLine(ErrorContextLocator.GetContext(_testFile, i));
            }
        }
    }
}