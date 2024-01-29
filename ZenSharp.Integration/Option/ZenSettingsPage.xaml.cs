using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionPages;
using JetBrains.Application.UI.Options.Options.ThemedIcons;
using JetBrains.DataFlow;
using JetBrains.Lifetimes;
using JetBrains.Util;
using JetBrains.Util.Logging;
using Microsoft.Win32;

namespace Github.Ulex.ZenSharp.Integration
{
    /// <summary>
    ///     Interaction logic for ZenSettingsPage.xaml
    /// </summary>
    [OptionsPage(pageId, "ZenSharp", typeof(OptionsThemedIcons.Plugins), ParentId = EnvironmentPage.Pid)]
    public partial class ZenSettingsPage : IOptionsPage
    {
        private const string pageId = "ZenSettingsPageId";

        private static readonly ILogger Log = Logger.GetLogger(typeof(ZenSettingsPage));

        public IProperty<string> Path { get; private set; }

        public ZenSettingsPage(Lifetime lifetime, OptionsSettingsSmartContext settings)
        {
            Path = new Property<string>("Path");
            settings.SetBinding(lifetime, (ZenSharpSettings s) => s.TreeFilename, Path);
            InitializeComponent();
            Id = pageId;
            // show exception info if any
            OnOk();
        }

        public string Id { get; }

        public bool OnOk()
        {
            return true;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog() { CheckFileExists = true, DefaultExt = "ltg", ValidateNames = true };
                if (dialog.ShowDialog() == true)
                {
                    Path.Value = dialog.FileName;
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

        public event PropertyChangedEventHandler PropertyChanged;

        // [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
