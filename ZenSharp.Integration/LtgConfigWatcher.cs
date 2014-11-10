using System;
using System.IO;
using System.Linq;
using System.Windows.Annotations;

using Github.Ulex.ZenSharp.Core;

using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.TextControl.Coords;
using JetBrains.Util;

using NLog;

namespace Github.Ulex.ZenSharp.Integration
{
    [SolutionComponent]
    internal sealed class LtgConfigWatcher : IDisposable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IContextBoundSettingsStore _boundSettings;

        private FileSystemWatcher _watcher;

        private GenerateTree _tree;

        public LtgConfigWatcher(ISettingsStore settingsStore)
        {
            _boundSettings = settingsStore.BindToContextTransient(ContextRange.ApplicationWide);
            var path = ZenSettings.GetTreePath;
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
                NotifyFilter = NotifyFilters.LastWrite
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
            catch (ParsingException e)
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
}