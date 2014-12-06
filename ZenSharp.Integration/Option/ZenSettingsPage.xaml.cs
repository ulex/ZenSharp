using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Github.Ulex.ZenSharp.Core;

using JetBrains.Application;
using JetBrains.Application.Components;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;

using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionPages;
using JetBrains.UI.Resources;

using Microsoft.Win32;

using NLog;

#if RESHARPER_82
using JetBrains.UI.Application.PluginSupport;
#endif

using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace Github.Ulex.ZenSharp.Integration
{
    /// <summary>
    /// Interaction logic for ZenSettingsPage.xaml
    /// </summary>
    [OptionsPage(pageId, "ZenSharp", typeof(OptionsThemedIcons.Plugins), ParentId = EnvironmentPage.Pid)]
    public partial class ZenSettingsPage : IOptionsPage
    {
        private const string pageId = "ZenSettingsPageId";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ZenSettingsPage(Lifetime lifetime, OptionsSettingsSmartContext settings, IComponentContainer container)
        {
            settings.SetBinding(lifetime, (ZenSharpSettings s) => s.TreeFilename, this, PathProperty);
            InitializeComponent();
            Id = pageId;
            _zenWatcher = container.GetComponent<LtgConfigWatcher>();
            // show exception info if any
            OnOk();
        }

        private LtgConfigWatcher _zenWatcher;

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(ZenSettingsPage), new PropertyMetadata(default(string)));

        public string Path
        {
            get
            {
                return (string)GetValue(PathProperty);
            }
            set
            {
                SetValue(PathProperty, value);
            }
        }

        public static readonly DependencyProperty PostValidationErrorProperty = DependencyProperty.Register("PostValidationError", typeof(Exception), typeof(ZenSettingsPage), new PropertyMetadata(default(Exception)));

        public Exception PostValidationError
        {
            get
            {
                return (Exception)GetValue(PostValidationErrorProperty);
            }
            set
            {
                SetValue(PostValidationErrorProperty, value);
            }
        }

        public string Id { get; private set; }

        public bool OnOk()
        {
            try
            {
                // todo: use settings notification options
                _zenWatcher.Reload(Path);
                _zenWatcher.ReinitializeWatcher(Path);
            }
            catch(Exception exception)
            {
                PostValidationError = exception;
                return false;
            }
            return true;
        }

        public bool ValidatePage()
        {
            return true;
        }

        public EitherControl Control
        {
            get
            {
                return this;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog() { CheckFileExists = true, DefaultExt = "ltg", ValidateNames = true };
                if (dialog.ShowDialog() == true)
                {
                    Path = dialog.FileName;
                }
            }
            catch (Exception exception)
            {
                Log.Error("Unexcpected error", exception);
            }
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
