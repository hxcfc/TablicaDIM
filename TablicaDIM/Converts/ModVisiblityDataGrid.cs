using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TablicaDIM.Converts
{
    public class ModVisiblityDataGrid : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

