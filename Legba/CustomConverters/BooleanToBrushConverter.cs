using System.Globalization;
using System.Windows.Data;

namespace Legba.CustomConverters;

public class BooleanToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value
            ? System.Windows.Media.Brushes.LightBlue
            : System.Windows.Media.Brushes.LightGray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}