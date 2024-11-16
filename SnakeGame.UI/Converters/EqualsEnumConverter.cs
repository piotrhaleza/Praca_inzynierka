using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SnakeGame.UI.Converters
{
    public class EnumToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return DependencyProperty.UnsetValue;

            // Compare the value with the parameter (enum value).
            if (value.GetType().IsEnum && parameter.GetType().IsEnum)
                return Enum.Equals(value, parameter);

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value && parameter != null)
            {
                // If true, return the parameter (which is the enum value bound to the RadioButton)
                return parameter;
            }

            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}
