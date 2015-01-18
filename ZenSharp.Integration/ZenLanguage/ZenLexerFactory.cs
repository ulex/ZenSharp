using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Util.Literals;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace Github.Ulex.ZenSharp.Integration.ZenLanguage
{
    public class ZenLexerFactory : ILexerFactory
    {
        public ILexer CreateLexer(IBuffer buffer)
        {
            return new ZenLexer(buffer);
        }
    }

    public class ZenLexer : FilteringLexer
    {
        public ZenLexer(IBuffer buffer) : base(new CSharpLexer(buffer))
        {
        }

        protected override bool Skip(TokenNodeType tokenType)
        {
            return false;
        }
    }
}