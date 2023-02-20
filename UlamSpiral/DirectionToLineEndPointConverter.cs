using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UlamSpiral.Models;

namespace UlamSpiral
{
    public class DirectionToLineEndPointConverter : IMultiValueConverter
    {
        public static DirectionToLineEndPointConverter Instance = new();

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values[0] is Direction pre && values[1] is Direction post)
            {
                var radius = Double.Parse((string)parameter);

                IList<Point> points = new List<Point>();

                if (pre is Direction.RightOf) points.Add(new Point(0, radius / 2));
                else if (pre is Direction.Above) points.Add(new Point(radius / 2, radius));
                else if (pre is Direction.Below) points.Add(new Point(radius / 2, 0));
                else if (pre is Direction.LeftOf) points.Add(new Point(radius, radius / 2));


                if (pre != post) points.Add(new Point(radius / 2, radius / 2));

                if (post is Direction.Above) points.Add(new Point(radius / 2, 0));
                else if (post is Direction.LeftOf) points.Add(new Point(0, radius / 2));
                else if (post is Direction.RightOf) points.Add(new Point(radius, radius / 2));
                else if (post is Direction.Below) points.Add(new Point(radius / 2, radius));


                return points;
            }
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
