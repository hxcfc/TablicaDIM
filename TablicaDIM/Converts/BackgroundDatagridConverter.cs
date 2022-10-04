using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TablicaDIM.Converts
{
    public class BackgroundDatagridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Remove(10) == DateTime.Now.ToShortDateString())
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#FF9800"); // Orange  if today          
            }
            else if (DateTime.Parse(value.ToString().Remove(10)) > DateTime.Now && (DateTime.Parse(value.ToString().Remove(10)) <= DateTime.Now.AddDays(7)))
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF99"); //  Yellow if next 7 days          
            }
            else if (DateTime.Parse(value.ToString().Remove(10)) < DateTime.Now)
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
