using System;
using System.IO;
using System.Reflection;

using Github.Ulex.ZenSharp.Core;

using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.Util;

namespace Github.Ulex.ZenSharp.Integration
{
    [ShellComponent]
    internal sealed class LtgConfigWatcher : IDisposable
    {
        private readonly FileSystemWatcher _watcher;

        private GenerateTree _tree;

        private readonly ZenSharpSettings _zenSettings;

        public LtgConfigWatcher(ISettingsStore settingsStore)
        {
            var boundSettings = settingsStore.BindToContextTransient(ContextRange.ApplicationWide);
            _zenSettings = boundSettings.GetKey<ZenSharpSettings>(SettingsOptimization.DoMeSlowly);

            // todo: support change dir
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(_zenSettings.TreePath), "*.ltg")
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

        private void Reload(bool showmsg)
        {
            try
            {
                _tree = new LtgParser().ParseAll(File.ReadAllText(_zenSettings.TreePath));
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