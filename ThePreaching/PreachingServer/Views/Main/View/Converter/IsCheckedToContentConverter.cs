using System;
using System.Globalization;
using System.Windows.Data;

namespace PreachingServer.Views.Main.View.Converter
{
    public class IsCheckedToContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool) value;
            if (isChecked == null || !isChecked)
                return "Starten";
            return "Stoppen";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}