using Avalonia.Data.Converters;
using Avalonia.Data;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UlamSpiral
{

    public class ConditionalColorConverter : IValueConverter
    {
        public static readonly ConditionalColorConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool sourceBool && parameter is IBrush targetColor
                && targetType.IsAssignableTo(typeof(IBrush)))
            {
                if (!sourceBool) return Brushes.Transparent;
                else return targetColor;
            }
            // converter used for the wrong type
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
