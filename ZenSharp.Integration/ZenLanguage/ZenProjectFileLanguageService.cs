using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.IL;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.UI.Icons;

namespace Github.Ulex.ZenSharp.Integration.ZenLanguage
{
    [ProjectFileType(typeof(ZenProjectFileType))]
    public class ZenProjectFileLanguageService : ProjectFileLanguageService
    {
        public ZenProjectFileLanguageService(ProjectFileType projectFileType) : base(projectFileType)
        {
        }

        public override ILexerFactory GetMixedLexerFactory(ISolution solution, IBuffer buffer, IPsiSourceFile sourceFile = null)
        {
            return new ZenLexerFactory();
        }

        protected override PsiLanguageType PsiLanguageType
        {
            get { return ZenLanguage.Instance; }
        }

        public override IconId Icon
        {
            get { return null; }
        }
    }
}