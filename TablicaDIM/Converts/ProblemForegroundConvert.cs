using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TablicaDIM.Converts
{
    public class ProblemForegroundConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString().Contains("EWO"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3"); // Blue 
                }
                else if (value.ToString().Contains("Czynność planowana"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#4CAF50"); // Green
                }
                else if (value.ToString().Contains("Pozostałe"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#000000"); // Black
                }
                else if (value.ToString().Contains("Problem"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#DE000000"); // Black           
                }
                else if (value.ToString().Contains("Weekend"))
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White           
                }
                else
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#DE000000"); // Black           
                }
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#DE000000");
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
