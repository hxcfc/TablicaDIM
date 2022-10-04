using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TablicaDIM.DBModels;

namespace TablicaDIM.Converts
{
    public class BackgroundTechConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Any(x => x == DependencyProperty.UnsetValue))
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White normal
            int result = 0;
            int TechAl = 0;
            int TechWa = 0;
            if(values != null)
            {

            if (int.TryParse(values[1].ToString(), out result)) // TryParse returns a boolean showing whether the parse worked
            {
                 TechAl = Int32.Parse(values[1].ToString());
                 TechWa = Int32.Parse(values[2].ToString());
                if (values[0].ToString() != "Weekend")
                {
                    int GetedValue = Int32.Parse(values[0].ToString());
                    if (TechWa >= GetedValue && TechAl < GetedValue)
                    {
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF99"); // Yellow  if today          
                    }
                    else if (TechAl >= GetedValue)
                    {
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#ff0000"); // Red normal
                    }
                    else
                    {
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White normal
                    }
                }
                else
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3"); // White Blue

                }
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White normal
            }
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"); // White normal
            }
        }
        public object[] ConvertBack(
            object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}