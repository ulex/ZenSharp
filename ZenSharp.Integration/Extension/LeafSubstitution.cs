using Github.Ulex.ZenSharp.Core;

using JetBrains.Util.Special;

namespace Github.Ulex.ZenSharp.Integration.Extension
{
    internal static class LeafSubstitution
    {
        public static string Macro(this LeafRule.Substitution rule)
        {
            return rule["macro"];
        }
    }
}