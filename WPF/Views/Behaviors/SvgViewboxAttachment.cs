using SharpVectors.Converters;
using System;
using System.Windows;

namespace WPF.Views.Behaviors
{
    public class SvgViewboxAttachment
    {
        private static readonly string ResourceFullPath = System.IO.Path.GetFullPath("./SvgFiles");

        public static readonly DependencyProperty SouceProperty =
            DependencyProperty.RegisterAttached("SetSource", typeof(string), typeof(SvgViewboxAttachment),
                new PropertyMetadata(string.Empty, (s, v) =>
              {
                  if (!(s is SvgViewbox svg)) return;

                  svg.Source = new Uri($"{ResourceFullPath}/{v.NewValue}");
              }));

        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SouceProperty, value);
        }
    }
}
