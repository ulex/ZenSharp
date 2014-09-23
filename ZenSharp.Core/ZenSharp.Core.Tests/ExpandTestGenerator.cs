﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using Github.Ulex.ZenSharp.Core;
using NUnit.Framework;

namespace ZenSharp.Core.Tests
{
    [TestFixture]
    public class TemplatesTests
    {
        #region Input of ltg
        private string _content = @"
// Sample file
space ::= "" ""

// Resharper macros
cursor ::= ""$END$""

// Fields
field ::= accessField """"=f space type ""$name$"" "";""

identifier ::= $name default=""$name$"" macros = ""complete()""$
suggType ::= $type default=""$type$"" macros = ""completeType(""""\0"""")""$ suggTypeFollower
suggTypeFollower ::= """"=""_""

// Methods
method       ::= accessMethod space methodInstStatic methodDecl
methodDecl ::= (type | ""void"") methodDecl2
methodDecl2::= space methodName ""("" methodArgs "") { $END$ }""
methodArgs   ::= cursor
methodName   ::= identifier
methodInstStatic ::= ""static ""=M | """"=m
accessMethod ::= private=_ | protected=pr | ""public""

// Auto properties
property        ::= accessProperty space type space identifier ""{ get; set; }"" ""$END$""
accessProperty  ::= ""public""=p | private=_p | protected=P
lazyPrivateSpec ::= ""private ""=_ | """"

// Plain types
arraySpec  ::= ""[]""=s | """"
type       ::= primType | compType | suggType
primType   ::= string=s | byte=b | double=d | int=i

// Complex types
genericArg  ::= ""<"" primType "">""
scgTypes    ::= IList=l | IEnumerable=""~""
generic2Arg ::= ""<"" primType "">""
scg2Types   ::= SortedList=sl | IDictionary=di
SCG         ::= ""System.Collections.Generic""
compType    ::= SCG ""."" scgTypes genericArg | SCG ""."" scg2Types generic2Arg

scope ""InCSharpTypeMember"" {
  start    ::=  method | property | other
  other ::= ""Verifiers.Verify("" cursor "")""=verify
  // Test: pType_ -> public $type$ $END$

  // Methods:
  // Test: m -> public void $name$($END$) {}
  // Test: M -> public static void $name$($END$) {}
  // Test: prmName -> protected void Name($END$) {}
  // Test: _M -> private static void $name$($END$) {}
  // Test: MiTest -> public static int Test($END$) {}
  // Test: _M~i -> private static System.Collections.Generic.IEnumerable<int> $name$($END$) {}
  // Test: _M~sEnu -> private static System.Collections.Generic.IEnumerable<string> Enu($END$) {}
  // Test: MMain,itest -> public static void Main(int test) {}
  // Test: MMain,oitest -> public static void Main(out int test) {}
  // Test: MMain`i,i,itest -> public static void Main<int,int>(int test) {}
  // Test: m~dHello,detest,sbi -> public IEnumerable<double> Hello(decimal test, StringBuiler i){}

  // Auto-properties
  // Test: pType,Name -> public $type$ Name { get; set; }
}

scope ""InCSharpTypeAndNamespace"" {
  start ::= class | interface | enum
  class ::= ""internal class""=c space classBody
  classBody ::= identifier ""{"" cursor ""}""
}
";
        #endregion Input of ltg
        private GenerateTree _tree;
        private LiveTemplateMatcher _ltm;
        [TestFixtureSetUp]
        public void LoadTree()
        {
            _tree = new LtgParser().ParseAll(_content);
            _ltm = new LiveTemplateMatcher(_tree);
        }


        [Test]
        public void TestpType__publictypeEND()
        {
            string input = @"pType_";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public $type$ $END$", expand, "Expand diffs");
        }

        [Test]
        public void Testm_publicvoidnameEND()
        {
            string input = @"m";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public void $name$($END$) {}", expand, "Expand diffs");
        }

        [Test]
        public void TestM_publicstaticvoidnameEND()
        {
            string input = @"M";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public static void $name$($END$) {}", expand, "Expand diffs");
        }

        [Test]
        public void TestprmName_protectedvoidNameEND()
        {
            string input = @"prmName";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"protected void Name($END$) {}", expand, "Expand diffs");
        }

        [Test]
        public void Test_M_privatestaticvoidnameEND()
        {
            string input = @"_M";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"private static void $name$($END$) {}", expand, "Expand diffs");
        }

        [Test]
        public void TestMiTest_publicstaticintTestEND()
        {
            string input = @"MiTest";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public static int Test($END$) {}", expand, "Expand diffs");
        }

        [Test]
        public void Test_Mi_privatestaticSystemCollectionsGenericIEnumerableintnameEND()
        {
            string input = @"_M~i";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"private static System.Collections.Generic.IEnumerable<int> $name$($END$) {}", expand, "Expand diffs");
        }

        [Test]
        public void Test_MsEnu_privatestaticSystemCollectionsGenericIEnumerablestringEnuEND()
        {
            string input = @"_M~sEnu";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"private static System.Collections.Generic.IEnumerable<string> Enu($END$) {}", expand, "Expand diffs");
        }

        [Test]
        public void TestMMainitest_publicstaticvoidMaininttest()
        {
            string input = @"MMain,itest";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public static void Main(int test) {}", expand, "Expand diffs");
        }

        [Test]
        public void TestMMainoitest_publicstaticvoidMainoutinttest()
        {
            string input = @"MMain,oitest";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public static void Main(out int test) {}", expand, "Expand diffs");
        }

        [Test]
        public void TestMMainiiitest_publicstaticvoidMainintintinttest()
        {
            string input = @"MMain`i,i,itest";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public static void Main<int,int>(int test) {}", expand, "Expand diffs");
        }

        [Test]
        public void TestmdHellodetestsbi_publicIEnumerabledoubleHellodecimaltestStringBuileri()
        {
            string input = @"m~dHello,detest,sbi";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public IEnumerable<double> Hello(decimal test, StringBuiler i){}", expand, "Expand diffs");
        }

        [Test]
        public void TestpTypeName_publictypeNamegetset()
        {
            string input = @"pType,Name";
            var m = _ltm.Match(input, @"InCSharpTypeMember");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"public $type$ Name { get; set; }", expand, "Expand diffs");
        }
    }
    [TestFixture]
    public class jackTests
    {
        #region Input of ltg
        private string _content = @"// This is the house that Jack built.
// This is the cheese that lay in the house that Jack built.
// This is the rat that ate the cheese
// That lay in the house that Jack built.

tjb      ::= ""that Jack built""=tjb
thisis   ::= ""This is the""=tit
s        ::= "" ""
sentence ::= $sencence par=""value"" par2=""sample"" $
sep      ::= "", ""="",""

scope ""house"" {
  base  ::= thisis s sentence
  start ::= base | base sep tjb
  // Test: tithouse -> This is the house
  // Test: tithouse,tjb -> This is the house
}
";
        #endregion Input of ltg
        private GenerateTree _tree;
        private LiveTemplateMatcher _ltm;
        [TestFixtureSetUp]
        public void LoadTree()
        {
            _tree = new LtgParser().ParseAll(_content);
            _ltm = new LiveTemplateMatcher(_tree);
        }


        [Test]
        public void Testtithouse_Thisisthehouse()
        {
            string input = @"tithouse";
            var m = _ltm.Match(input, @"house");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"This is the house", expand, "Expand diffs");
        }

        [Test]
        public void Testtithousetjb_Thisisthehouse()
        {
            string input = @"tithouse,tjb";
            var m = _ltm.Match(input, @"house");
            var expand = m.Expand(input);
			Assert.IsTrue(m.Success);
			Assert.AreEqual(string.Empty, m.Tail, "Tail is not empty");
            Assert.AreEqual(@"This is the house", expand, "Expand diffs");
        }
    }
}


