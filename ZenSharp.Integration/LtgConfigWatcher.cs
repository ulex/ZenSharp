using System;
using System.IO;
using System.Reflection;

using Github.Ulex.ZenSharp.Core;
using Github.Ulex.ZenSharp.Integration.Properties;

using JetBrains.Application;
using JetBrains.Util;

namespace Github.Ulex.ZenSharp.Integration
{
    [ShellComponent]
    internal sealed class LtgConfigWatcher : IDisposable
    {
        private readonly FileSystemWatcher _watcher;

        private GenerateTree _tree;

        public LtgConfigWatcher()
        {
            _watcher = new FileSystemWatcher(AssemblyDir, "*.ltg")
                {
                    EnableRaisingEvents = true,
                    NotifyFilter = NotifyFilters.LastWrite
                };
            _watcher.Changed += (sender, args) => Reload(true);
            Reload(false);
        }

        public GenerateTree Tree
        {
            get
            {
                return _tree;
            }
        }

        private string ConfigPath
        {
            get
            {
                var path = Settings.Default.ConfigPath;
                if (Path.IsPathRooted(path))
                {
                    return path;
                }
                else
                {
                    return Path.Combine(AssemblyDir, path);
                }
            }
        }

        private static string AssemblyDir
        {
            get
            {
                var curpath = Assembly.GetExecutingAssembly().Location;
                var assemblyDir = Path.GetDirectoryName(curpath);
                return assemblyDir;
            }
        }

        private void Reload(bool showmsg)
        {
            try
            {
                _tree = new LtgParser().ParseAll(File.ReadAllText(ConfigPath));
                if (showmsg) MessageBox.ShowInfo("Config updated");
            }
            catch (Exception e)
            {
                if (showmsg) MessageBox.ShowError(e.ToString(), e.Source);   
            }
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}