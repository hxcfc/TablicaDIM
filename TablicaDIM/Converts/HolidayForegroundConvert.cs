using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TablicaDIM.Converts
{
    public class HolidayForegroundConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString().Contains("TODAY"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
                }
                else if (value.ToString().Contains("POSTOJ"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
                }
                else if (value.ToString().Contains("- So."))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White
                }
                else if (value.ToString().Contains("- Ni."))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White
                }
                else if (value.ToString().Contains("So."))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3"); // White
                }
                else if (value.ToString().Contains("Ni."))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3"); // White
                }
                else if (value.ToString().Contains(" - "))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#000000"); // Black
                }
                else if (value.ToString().Contains("Urlop"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3"); // Blue
                }
                else if (value.ToString().Contains("Krew"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ff0000"); // Red
                }
                else if (value.ToString().Contains("Szkolenie"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#CDDC39"); // Lime
                }
                else if (value.ToString().Contains("Opieka"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#4CAF50"); // Green
                }
                else if (value.ToString().Contains("Postój"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#000000"); // Black
                }
                else if (value.ToString().Contains("Odbiórka"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#FF9800"); // Orange
                }
                else if (value.ToString().Contains("Choroba"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#9C27B0"); // Purple
                }
                else if (value.ToString().Contains("Wniosek"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White
                }
                else if (value.ToString().Contains("Święto"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White
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
