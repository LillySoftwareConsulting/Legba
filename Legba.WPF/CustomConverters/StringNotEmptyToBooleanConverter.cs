using System.Globalization;
using System.Windows.Data;

namespace Legba.WPF.CustomConverters;

public class StringNotEmptyToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(ReferenceEquals(value, null))
        {
            return false;
        }

        return !string.IsNullOrWhiteSpace(value as string);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}