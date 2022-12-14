using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace TestTask.Wpf.Converters;

public class EnumToDescriptionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return string.Empty;
        }

        Type type = value.GetType();

        return
            type.IsEnum
                ? GetDescription(type, value)
                : string.Empty;
    }

    private static string GetDescription(Type enumType, object enumValue)
    {
        var descriptionAttribute =
            enumType
                .GetField(enumValue.ToString()!)!
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

        return
            descriptionAttribute != null
                ? descriptionAttribute.Description
                : enumValue.ToString()!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}