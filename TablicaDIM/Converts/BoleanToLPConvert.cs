using System;
using System.Globalization;
using System.Windows.Data;

namespace TablicaDIM.Converts
{
    public class BoleanToLPConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value != false)
            {
                return "Oczczekuje na zatwierdzenie";
            }
            else
            {
                return "Nie dotyczy";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}


