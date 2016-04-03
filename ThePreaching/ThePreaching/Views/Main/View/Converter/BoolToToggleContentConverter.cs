using System;
using System.Globalization;
using System.Windows.Data;

namespace ThePreaching.Views.Main.View.Converter
{
    public class BoolToToggleContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? boolValue = (bool?)value;
            if (boolValue == null || boolValue == false)
                return "Starten";
            return "Stoppen";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}