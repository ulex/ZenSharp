using System;
using System.IO;
using System.Reflection;

using JetBrains.Application.Settings;
using JetBrains.ReSharper.Settings;

namespace Github.Ulex.ZenSharp.Integration
{
    [SettingsKey(typeof(PatternsAndTemplatesSettingsKey), "ZenSharp settings")]
    public sealed class ZenSharpSettings
    {
        [SettingsEntry("Templates.ltg", "Path to ltg file")]
        public string TreeFilename { get; set; }

        public string GetTreePath
        {
            get
            {
                var path = TreeFilename;
                if (Path.IsPathRooted(path))
                {
                    return path;
                }
                else
                {
                    return Path.Combine(DefaultDir, path);
                }
            }
        }

        public static string DefaultDir
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }
    }
}