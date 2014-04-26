using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FunctionPlotter.Converters
{
    public class EditBrushConverter : IValueConverter
    {
        public Brush EditModeBrush { get; set; }

        public Brush NonEditModeBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isInEditMode = (bool)value;

            if (isInEditMode)
                return this.EditModeBrush;
            else
                return this.NonEditModeBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
