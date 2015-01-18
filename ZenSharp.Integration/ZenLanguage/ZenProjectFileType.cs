using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace Github.Ulex.ZenSharp.Integration.ZenLanguage
{
    [ProjectFileTypeDefinition(ZenLanguage.ZenSharp)]
    public class ZenProjectFileType : ProjectFileType
    {
        public static ZenProjectFileType Instance;

        private static readonly string[] _extensions = new []{".ltg"};

        public ZenProjectFileType() : base(ZenLanguage.ZenSharp, ZenLanguage.ZenPresentableName, _extensions)
        {
            
        }

        public ZenProjectFileType(string name) : base(name)
        {
        }

        public ZenProjectFileType(string name, string presentableName) : base(name, presentableName)
        {
        }

        public ZenProjectFileType(string name, string presentableName, IEnumerable<string> extensions) : base(name, presentableName, extensions)
        {
        }
    }
}