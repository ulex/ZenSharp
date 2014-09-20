using System;
using System.Globalization;
using System.Windows.Data;

using JetBrains.ReSharper.Psi.BuildScripts.Icons;

namespace Github.Ulex.ZenSharp.Integration
{
    internal sealed class ExceptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var exception = value as Exception;
            if (exception == null || targetType != typeof(string))
            {
                return null;
            }

            return string.Format("{0}{1}{2}", exception.Message, Environment.NewLine, exception.StackTrace);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}