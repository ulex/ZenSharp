using System;

using Github.Ulex.ZenSharp.Core;

using NUnit.Framework;

namespace ZenSharp.Core.Tests
{
    [TestFixture]
    internal sealed class TestTemplates
    {
        private GenerateTree _tree;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            // TOdo: move it to auto generated test
            //_tree = new LtgParser().ParseAll(LtgSamples.Templates);
        }

        [Test]
        public void TestExpandsMalt()
        {
            VerifySameExpand("m", "public void $name$($END$) {}");
        }

        [Test]
        public void TestExpandsM()
        {
            VerifySameExpand("M", "public static void $name$($END$) {}");
        }

        [Test]
        public void TestExpands_M()
        {
            VerifySameExpand("_M", "public static void $name$($END$) {}");
        }


        [Test]
        public void TestExpandsMiTest()
        {
            VerifySameExpand("MiTest", "public static int Test($END$) {}");
        }

        [Test]
        public void TestExpands_Miei()
        {
            VerifySameExpand("_M~i", "private static System.Collections.Generic.IEnumerable<int> $name$($END$) {}");
        }

        [Test]
        public void TestExpands_Miei2()
        {
            VerifySameExpand("_M~sEnu", "private static System.Collections.Generic.IEnumerable<string> Enu($END$) {}");
        }

        [Test]
        public void TestExpandsMMainitest()
        {
            VerifySameExpand("MMain,itest", "public static void Main(int test) {}");
        }

        [Test]
        public void TestExpandsMMainoitest()
        {
            VerifySameExpand("MMain,oitest", "public static void Main(out int test) {}");
        }

        [Test]
        public void TestExpandsMMainiiitest()
        {
            VerifySameExpand("MMain`i,i,itest", "public static void Main<int,int>(int test) {}");
        }

        [Test]
        public void TestExpandsmdHellodetestsbi()
        {
            VerifySameExpand("m~dHello,detest,sbi", "public IEnumerable<double> Hello(decimal test, StringBuiler i){}");
        }

        private void VerifySameExpand(string input, string expectedExpand)
        {
            var ltm = new LiveTemplateMatcher(_tree);
            var m = ltm.Match(input, "InCSharpTypeMember");
            var expand = m.Expand(input);
            Console.WriteLine("{0}, {1}, {2}", m.Success, m.Tail, expand);

            Assert.AreEqual(expectedExpand, expand);
        }
    }
}