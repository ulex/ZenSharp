using JetBrains.ReSharper.Psi;

namespace Github.Ulex.ZenSharp.Integration.ZenLanguage
{
    [LanguageDefinition(ZenSharp)]
    public class ZenLanguage : KnownLanguage
    {
        public static ZenLanguage Instance;

        public const string ZenPresentableName = "ZenTemplates";
        public const string ZenSharp = "ZenSharp";

        protected ZenLanguage() : base(ZenSharp, ZenPresentableName)
        {
        }

        protected ZenLanguage(string name) : base(name)
        {
        }

        protected ZenLanguage(string name, string presentableName) : base(name, presentableName)
        {
        }
    }
}