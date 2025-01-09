using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.Converters
{
    public class FractionToWidthConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double fraction && parameter is string maxWidthStr && double.TryParse(maxWidthStr, out double maxWidth))
            {
                return fraction * maxWidth; // Scale the fraction to the maximum width
            }

            return 0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
