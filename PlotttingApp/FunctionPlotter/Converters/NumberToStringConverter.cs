using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FunctionPlotter.Converters
{
    public class NumberToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value as string;

            double result = 0.0;
            bool isNumber = false;

            if (!string.IsNullOrEmpty(s))
                isNumber = double.TryParse(s, out result);

            if (isNumber)
                return result;
            else
                throw new FormatException("The provided string is not a number!");
        }
    }
}
