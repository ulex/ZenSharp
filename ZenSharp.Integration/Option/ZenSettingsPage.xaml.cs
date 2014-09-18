using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Github.Ulex.ZenSharp.Core;

using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.UI.Application.PluginSupport;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using JetBrains.UI.Resources;

using Microsoft.Win32;

using NLog;

namespace Github.Ulex.ZenSharp.Integration
{
    /// <summary>
    /// Interaction logic for ZenSettingsPage.xaml
    /// </summary>
    [OptionsPage(pageId, "ZenSharp", typeof(OptionsThemedIcons.Plugins), ParentId = PluginsPage.Pid)]
    public partial class ZenSettingsPage : IOptionsPage
    {
        private const string pageId = "ZenSettingsPageId";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ZenSettingsPage(Lifetime lifetime, OptionsSettingsSmartContext settings)
        {
            settings.SetBinding(lifetime, (ZenSharpSettings s) => s.TreeFilename, this, PathProperty);
            InitializeComponent();
            Id = pageId;
        }

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

        public string Id { get; private set; }

        public bool OnOk()
        {
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
