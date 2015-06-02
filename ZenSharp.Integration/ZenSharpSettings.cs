using JetBrains.Util;

using System;
using System.IO;
using System.Reflection;

using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace Github.Ulex.ZenSharp.Integration
{
    [SettingsKey(typeof(PatternsAndTemplatesSettingsKey), "ZenSharp settings")]
    public sealed class ZenSharpSettings
    {
        [SettingsEntry("Templates.ltg", "Path to ltg file")]
        public string TreeFilename { get; set; }

        public static string GetTreePath(string treeFilename)
        {
            if (treeFilename.IsNullOrEmpty())
            {
                return null;
            }
            if (!string.IsNullOrEmpty(treeFilename) && Path.IsPathRooted(treeFilename))
            {
                return treeFilename;
            }
            else
            {
                return Path.Combine(DefaultDir, treeFilename);
            }
        }

        private static string DefaultDir
        {
            get
            {
                // todo: store ltg iside ReSharper config
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }
    }
}