using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Legba.CustomConverters;

public class RelativeWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length != 2)
        {
            return DependencyProperty.UnsetValue;
        }

        if (values[0] is double parentWidth &&
            values[1] != null &&
            double.TryParse(values[1].ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double percentage))
        {
            return parentWidth * percentage;
        }

        return DependencyProperty.UnsetValue;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}