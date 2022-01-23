using Domain.Entities.Helpers;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPF.Views.Behaviors
{
    public class OutputDateConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = (DateTime)value;

            return dateTime == DataHelper.DefaultDate ? "未出力" : dateTime.ToString("yy/MM/dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { return null; }

        private static OutputDateConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new OutputDateConverter();
        }
    }
}
