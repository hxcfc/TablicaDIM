using System;
using System.Globalization;
using System.Windows.Data;

namespace TablicaDIM.Converts
{
    public class PermisionIDtoNameConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case 1:
                    return "Właściciel";
                case 2:
                    return "Administrator";
                case 3:
                    return "Mistrz";
                case 4:
                    return "Technik";
            }
            return "Błąd";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Właściciel":
                    return 1;
                case "Administrator":
                    return 2;
                case "Mistrz":
                    return 3;
                case "Technik":
                    return 4;
            }
            return 0;
        }
    }
}

