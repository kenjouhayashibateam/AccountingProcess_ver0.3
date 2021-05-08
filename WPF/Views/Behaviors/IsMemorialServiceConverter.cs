using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// boolを法事、葬儀にConvertするクラス
    /// </summary>
    public class IsMemorialServiceConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return string.Empty;

            return (bool)value ? "法事" : "葬儀";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private static IsMemorialServiceConverter MemorialServiceConverter; 

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return MemorialServiceConverter ??= new IsMemorialServiceConverter();
        }
    }
}
