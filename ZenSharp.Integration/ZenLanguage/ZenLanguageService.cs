using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Asxx.Parsing;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Parsing;

namespace Github.Ulex.ZenSharp.Integration.ZenLanguage
{
    [Language(typeof(ZenLanguage))]
    public class ZenLanguageService : LanguageService
    {
        public ZenLanguageService(ZenLanguage psiLanguageType, IConstantValueService constantValueService)
            : base(psiLanguageType, constantValueService)
        {
        }

        public override ILexerFactory GetPrimaryLexerFactory()
        {
            return new ZenLexerFactory();
        }

        public override ILexer CreateFilteringLexer(ILexer lexer)
        {
            return null;
        }

        public override IParser CreateParser(ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
        {
            return null;
        }

        public override ILanguageCacheProvider CacheProvider
        {
            get { return null; }
        }

        public override bool IsCaseSensitive
        {
            get { return true; }
        }

        public override bool SupportTypeMemberCache
        {
            get { return false; }
        }

        public override ITypePresenter TypePresenter
        {
            get { return null; }
        }
    }
}
