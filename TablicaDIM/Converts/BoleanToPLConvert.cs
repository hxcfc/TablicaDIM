using System;
using System.Globalization;
using System.Windows.Data;

namespace TablicaDIM.Converts
{
    public class BoleanToPLConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return "Oczczekuje na zatwierdzenie";
            }
            else
            {
                return "Zaakceptowany";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

