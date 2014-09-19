using System;
using System.IO;

using Github.Ulex.ZenSharp.Core;

using JetBrains.Application;
using JetBrains.Application.Settings;

using NLog;

namespace Github.Ulex.ZenSharp.Integration
{
    [ShellComponent]
    internal sealed class LtgConfigWatcher : IDisposable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly FileSystemWatcher _watcher;

        private GenerateTree _tree;

        private readonly IContextBoundSettingsStore _boundSettings;
        
        public LtgConfigWatcher(ISettingsStore settingsStore)
        {
            _boundSettings = settingsStore.BindToContextTransient(ContextRange.ApplicationWide);

            // todo: support change dir
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(ZenSettings.GetTreePath), "*.ltg")
                {
                    EnableRaisingEvents = true,
                    NotifyFilter = NotifyFilters.LastWrite
                };
            _watcher.Changed += (sender, args) => Reload();
            Reload();
        }

        private ZenSharpSettings ZenSettings
        {
            get
            {
                return _boundSettings.GetKey<ZenSharpSettings>(SettingsOptimization.DoMeSlowly);
            }
        }

        public GenerateTree Tree
        {
            get
            {
                return _tree;
            }
        }

        private void Reload()
        {
            try
            {
                var path = ZenSettings.GetTreePath;
                _tree = new LtgParser().ParseAll(File.ReadAllText(path));
                Log.Info("Config reloaded from {0}", path);
            }
            catch (Exception e)
            {
                Log.Error("Error loading config", e);
                throw;
            }
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}