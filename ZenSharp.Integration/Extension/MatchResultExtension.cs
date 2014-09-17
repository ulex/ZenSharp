using Github.Ulex.ZenSharp.Core;

namespace Github.Ulex.ZenSharp.Integration.Extension
{
    internal static class MatchResultExtension
    {
        public static string ExpandDisplay(this LiveTemplateMatcher.MatchResult match, string prefix)
        {
            return match.Expand(prefix).Replace("$END$", string.Empty);
        }
    }
}