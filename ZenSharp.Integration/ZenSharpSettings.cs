using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;

namespace Github.Ulex.ZenSharp.Integration
{
    [SettingsKey(typeof(CodeCompletionSettingsKey), "ZenSharp settings")]
    internal sealed class ZenSharpSettings
    {
        [SettingsEntry("Templates.ltg", "Filename")]
        public string FilePath { get; set; }
    }
}