using System.Globalization;
using System.Windows.Data;

namespace Legba.CustomConverters;

public class BooleanInverterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool booleanValue)
        {
            return !booleanValue; // Invert the boolean value
        }
        return value; // Return the original value if it's not a boolean
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool booleanValue)
        {
            return !booleanValue; // Invert back the boolean value
        }
        return value; // Return the original value if it's not a boolean
    }
}
