using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TablicaDIM.Converts
{
    public class HolidaysToAcceptConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "Urlopy")
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#ff0000"); //  Red if toolate          
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White normal
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
