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
    public class RequiredVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var needInvert = (parameter as string)?.Equals("invert", StringComparison.OrdinalIgnoreCase) ?? false;

            if (value is string)
            {
                return BoolToVisibility(string.IsNullOrEmpty(value?.ToString()), needInvert);
            }

            return BoolToVisibility(value == null, needInvert);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private Visibility BoolToVisibility(bool predicate, bool needInvert)
        {
            predicate = needInvert ? !predicate : predicate;

            return predicate ? Visibility.Collapsed
                             : Visibility.Visible;
        }
    }
}
