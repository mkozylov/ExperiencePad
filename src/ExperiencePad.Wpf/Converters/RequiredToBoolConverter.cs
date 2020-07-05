using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using NWrath.Synergy.Common.Extensions;

namespace ExperiencePad
{
    public class RequiredToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var needInvert = (parameter as string)?.Equals("invert", StringComparison.OrdinalIgnoreCase) ?? false;

            var result = value != null;

            result = needInvert ? !result : result;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool?)value ?? false;
        }
    }
}
