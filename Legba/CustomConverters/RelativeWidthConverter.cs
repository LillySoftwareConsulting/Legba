using System.Globalization;
using System.Windows.Data;

namespace Legba.CustomConverters;

public class RelativeWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2)
        {
            double parentWidth = (double)values[0];
            double percentage = double.Parse(values[1].ToString());

            return parentWidth * percentage;
        }

        return 0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}