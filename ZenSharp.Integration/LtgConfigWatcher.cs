/*using System;
using System.IO;

using Github.Ulex.ZenSharp.Core;

using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace Github.Ulex.ZenSharp.Integration
{
    [ShellComponent]
    internal sealed class LtgConfigWatcher : IDisposable
    {
        private static readonly ILogger Log = Logger.GetLogger(typeof(LtgConfigWatcher));

        private readonly IContextBoundSettingsStore _boundSettings;

        private FileSystemWatcher _watcher;

        private GenerateTree _tree;

        public LtgConfigWatcher(ISettingsStore settingsStore)
        {
            _boundSettings = settingsStore.BindToContextTransient(ContextRange.ApplicationWide);
            Initialize();
        }

        private void Initialize()
        {
            var path = ZenSharpSettings.GetTreePath(ZenSettings.TreeFilename);
            try
            {
                ReinitializeWatcher(path);
                WriteDefaultTemplates(path);
                Reload(path);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void WriteDefaultTemplates(string path)
        {
            if (!File.Exists(path))
            {
                Log.Info("Saving default templates to {0}", path);
                using (var resStream = typeof(LtgConfigWatcher).Assembly.GetManifestResourceStream("Github.Ulex.ZenSharp.Integration.Templates.ltg"))
                {
                    if (resStream != null)
                    {
                        using (var fstream = File.OpenWrite(path))
                        {
                            resStream.CopyTo(fstream);
                        }
                    }
                }
            }
        }

        public void ReinitializeWatcher(string path)
        {
            if (_watcher != null)
            {
                var watcher = _watcher;
                _watcher = null;
                watcher.Dispose();
            }

            var directoryName = Path.GetDirectoryName(path);
            Log.Info("Create file system watcher on directory {0}", directoryName);
            _watcher = new FileSystemWatcher(directoryName, "*.ltg")
            {
                EnableRaisingEvents = true,

                // visual studio 2013 save file to temporary and rename it
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime
            };
            _watcher.Changed += (sender, args) => SafeReload(path);
        }

        private void SafeReload(string path)
        {
            try
            {
                Log.Info("Reloading config from {0}", path);
                Reload(path);
            }
            catch (Exception e)
            {
                Log.Error("Error updating ltg config:");
                MessageBox.ShowError(string.Format("Sorry for this stupid notification type, but some problem occupied when loading ZenSharp config: {0}", e.Message), "ZenSharp error");
            }
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

        public void Reload(string file)
        {
            try
            {
                var path = file;
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
}*/