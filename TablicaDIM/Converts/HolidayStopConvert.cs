using System;
using System.Globalization;
using System.Windows.Data;

namespace TablicaDIM.Converts
{
    public class HolidayStopConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return "Postój";
            }
            else
            {
                return "Święto";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

