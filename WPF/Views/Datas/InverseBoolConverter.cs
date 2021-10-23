using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF.Views.Datas
{
    /// <summary>
    /// Bindingしたboolプロパティを反転させるConverter
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { return !(value is bool b) ? null : (object)!b; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { return !(value is bool b) ? null : (object)!b; }
    }
}
