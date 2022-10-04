using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TablicaDIM.Converts
{
    public class WeekendBackgroundConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString().Contains("TODAY"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#74c84c");
                }
                else if (value.ToString().Contains("POSTOJ"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#BFE04343");
                }
                else if (value.ToString().Contains("So"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3");
                }
                else if (value.ToString().Contains("Ni"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3");
                }
                else if (value.ToString().Contains("Wniosek"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
                }
                else if (value.ToString().Contains("Święto"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ff0000");
                }
                else
                {
                    return Brushes.White;
                }
            }
            else
            {
                return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

