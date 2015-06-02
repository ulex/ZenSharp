using System;
using System.IO;
using System.Reflection;

using Github.Ulex.ZenSharp.Integration;

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Context;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Scope;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TextControl;
using JetBrains.Util;

using NUnit.Framework;

namespace ZenSharp.Integration.Tests
{
    [TestFixture]
    public class CSharpExtendedScopeProviderTest : BaseTestWithTextControl
    {
        protected override string RelativeTestDataPath
        {
            get
            {
                return "CSharpExtendedScopeProviderTest";
            }
        }

        protected override void DoTest(IProject testProject)
        {
            var caretPosition = GetCaretPosition();
            using (ITextControl textControl = OpenTextControl(testProject, caretPosition))
            {
                var caretOffset = textControl.Caret.Offset();
                var context = new TemplateAcceptanceContext(
                    testProject.GetSolution(),
                    textControl.Document,
                    caretOffset,
                    new TextRange(caretOffset));

                base.ExecuteWithGold(
                    sb =>
                    {
                        var sp = new CSharpExtendedScopeProvider();
                        foreach (string templateScopePoint in sp.ProvideScopePoints(context))
                        {
                            sb.Write(templateScopePoint);
                            sb.WriteLine();
                        }
                    });
            }
        }

        [Test] public void Class1() { DoNamedTest(); }
        [Test] public void InNsEnum() { DoNamedTest(); }
        [Test] public void Interface() { DoNamedTest(); }
        [Test] public void InNamespaceOnly() { DoNamedTest(); }
        [Test] public void InterfaceRoot() { DoNamedTest(); }
    }
}
