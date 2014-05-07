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
            _watcher = new FileSystemWatcher(AssemblyDir, "*.ltg");
            _watcher.EnableRaisingEvents = true;
            _watcher.Changed += (sender, args) => Reload();
            Reload();
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

        private void Reload()
        {
            try
            {
                _tree = new LtgParser().ParseAll(File.ReadAllText(ConfigPath));
            }
            catch (Exception e)
            {
                MessageBox.ShowError(e.ToString(), e.Source);   
            }
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}