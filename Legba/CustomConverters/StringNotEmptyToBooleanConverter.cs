using System.Globalization;
using System.Windows.Data;

namespace Legba.CustomConverters;

public sealed class StringNotEmptyToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string str && !string.IsNullOrWhiteSpace(str);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException($"{nameof(StringNotEmptyToBooleanConverter)} does not support ConvertBack.");
    }
}