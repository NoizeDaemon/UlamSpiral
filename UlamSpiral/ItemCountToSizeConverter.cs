using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UlamSpiral
{
    public class ItemCountToSizeConverter : IMultiValueConverter
    {
        static readonly ItemCountToSizeConverter Instance = new();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values[0] is int itemCount && values[0] is double availableSpace)
            {
                var root = Math.Ceiling(Math.Sqrt(itemCount));
                var radius = availableSpace / root;

                return radius;
            }
            return BindingOperations.DoNothing;
        }
    }
}
