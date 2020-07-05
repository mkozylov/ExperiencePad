using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace ExperiencePad
{
    public class HighlightingDefinitionConverter : IValueConverter
    {
        private static readonly HighlightingDefinitionTypeConverter Converter = new HighlightingDefinitionTypeConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.ConvertFrom(value ?? "text");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.ConvertToString(value);
        }
    }
}
