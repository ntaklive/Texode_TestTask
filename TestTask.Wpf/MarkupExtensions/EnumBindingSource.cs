using System;
using System.Windows.Markup;

namespace TestTask.Wpf.MarkupExtensions;

public class EnumBindingSource : MarkupExtension
{
    public Type EnumType { get; }

    public EnumBindingSource(Type enumType)
    {
        if (enumType is null || !enumType.IsEnum )
        {
            throw new ArgumentException("The argument is not an Enum");
        }
        
        EnumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enum.GetValues(EnumType);
    }
}