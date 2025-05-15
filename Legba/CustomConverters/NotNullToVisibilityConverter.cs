using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Legba.CustomConverters;

public class NotNullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Return Visible if the value is NOT null
        if (value != null)
        {
            return Visibility.Visible;
        }

        // Otherwise, return Collapsed or Hidden based on your requirement
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
        // ConvertBack is not implemented as visibility to null conversion is typically one-way.
    }
}