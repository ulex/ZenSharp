using Github.Ulex.ZenSharp.Core;

namespace Github.Ulex.ZenSharp.Integration.Extension
{
    internal static class LeafSubstitution
    {
        public static string Macros(this LeafRule.Substitution rule)
        {
            return rule["macros"];
        }
    }
}