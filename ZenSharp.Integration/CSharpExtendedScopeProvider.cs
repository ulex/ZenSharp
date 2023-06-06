using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Context;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;

namespace Github.Ulex.ZenSharp.Integration
{
    [ShellComponent]
    public sealed class CSharpExtendedScopeProvider
    {
        /// <summary>
        ///     Too lazy to implement full ITemplateScopePoint
        /// </summary>
        public IEnumerable<string> ProvideScopePoints(TemplateAcceptanceContext tacContext)
        {
            var solution = tacContext.Solution;
            var document = tacContext.SelectionRange.Document;
            if (document == null)
            {
                return Array.Empty<string>();
            }
            var psiSource = tacContext.SourceFile;
            if (psiSource == null)
            {
                return Array.Empty<string>();
            }

            var points = new List<string>();
            using (ReadLockCookie.Create())
            {
                var psiFiles = solution.GetPsiServices().Files;
                if (!psiFiles.AllDocumentsAreCommitted)
                {
                    psiFiles.CommitAllDocuments();
                }
                int caretOffset = tacContext.CaretOffset.Offset;
                string prefix = LiveTemplatesManager.GetPrefix(document, caretOffset);
                var documentRange = new DocumentRange(document, caretOffset - prefix.Length);
                if (!documentRange.IsValid())
                {
                    return Enumerable.Empty<string>();
                }

                points.Add(psiSource.PrimaryPsiLanguage.Name);
                
                var file = psiSource.GetPsiFile<CSharpLanguage>(documentRange);
                if (file == null || !Equals(file.Language, CSharpLanguage.Instance))
                {
                    return points;
                }
                var element = file.FindTokenAt(document, caretOffset - prefix.Length);
                if (element == null)
                {
                    return points;
                }

                points.Add("InCSharpFile");
                var treeNode = element;

                if (treeNode.GetContainingNode<IDocCommentNode>(true) != null) return points;

                if (treeNode is ICSharpCommentNode || treeNode is IPreprocessorDirective)
                {
                    treeNode = treeNode.PrevSibling;
                }
                if (treeNode == null)
                {
                    return points;
                }

                var context = CSharpReparseContext.FindContext(treeNode);
                if (context == null)
                {
                    return points;
                }

                if (treeNode.GetContainingNode<IEnumDeclaration>() != null)
                {
                    points.Add("InCSharpEnum");
                }

                var containingType = treeNode.GetContainingNode<ICSharpTypeDeclaration>(true);
                if (containingType == null && TestNode<ICSharpNamespaceDeclaration>(context, "namespace N {}", false))
                {
                    points.Add("InCSharpTypeAndNamespace");
                }
                else if (TestNode<IMethodDeclaration>(context, "void foo() {}", false))
                {
                    points.Add("InCSharpTypeMember");
                    // Extend here: 
                    // Already in type member, 
                    if (treeNode.GetContainingNode<IInterfaceDeclaration>() != null)
                    {
                        points.Add("InCSharpInterface");
                    }
                    if (treeNode.GetContainingNode<IClassDeclaration>() != null)
                    {
                        points.Add("InCSharpClass");
                    }
                    if (treeNode.GetContainingNode<IStructDeclaration>() != null)
                    {
                        points.Add("InCSharpStruct");
                    }
                    if (treeNode.GetContainingNode<IRecordDeclaration>() != null)
                    {
                        points.Add("InCSharpRecord");
                    }
                }
                else
                {
                    bool acceptsExpression = TestNode<IPostfixOperatorExpression>(context, "a++", true);
                    if (TestNode<IBreakStatement>(context, "break;", false))
                    {
                        points.Add("InCSharpStatement");
                    }
                    else if (acceptsExpression)
                    {
                        points.Add("InCSharpExpression");
                    }
                    if (!acceptsExpression && TestNode<IQuerySelectClause>(context, "select x", false))
                    {
                        points.Add("InCSharpQuery");
                    }
                }
            }

            return points;
        }

        private static bool TestNode<T>(CSharpReparseContext context, string text, bool strictStart = false)
            where T : ITreeNode
        {
            var node = context.Parse(text);
            var tokenAt1 = node.FindTokenAt(new TreeOffset(context.WholeTextLength - 1));
            var treeTextOffset = new TreeOffset(context.OriginalTextLength);
            var tokenAt2 = node.FindTokenAt(treeTextOffset);
            if (tokenAt1 == null || tokenAt2 == null)
            {
                return false;
            }
            var errorNodeFinder = new ErrorNodeFinder(tokenAt1);
            errorNodeFinder.FindLastError(node);
            if (errorNodeFinder.Error != null
                && errorNodeFinder.Error.GetTreeStartOffset().Offset >= context.OriginalTextLength)
            {
                return false;
            }
            ITreeNode commonParent = tokenAt2.FindCommonParent(tokenAt1);
            if (!(commonParent is T))
            {
                return false;
            }
            if (strictStart)
            {
                return commonParent.GetTreeTextRange().StartOffset == treeTextOffset;
            }
            return true;
        }
    }
}
