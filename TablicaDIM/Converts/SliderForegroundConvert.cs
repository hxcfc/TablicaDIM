using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TablicaDIM.Converts
{
    public class SliderForegroundConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((double)value == 0)
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#e6e6fa"); //  
                }
                else if ((double)value == 25)
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#c2e5f0"); // 
                }
                else if ((double)value == 50)
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#87ceeb"); // 
                }
                else if ((double)value == 75)
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#79c0f7"); //            
                }
                else
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3"); //            
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
