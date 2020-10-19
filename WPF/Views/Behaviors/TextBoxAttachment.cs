using System.Windows;
using System.Windows.Controls;

namespace WPF.Behavior
{
    public static class TextBoxAttachment
    {
        public static bool GetIsSelectAllOnGotFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectAllOnGotFocusProperty);
        }

        public static void SetIsSelectAllOnGotFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectAllOnGotFocusProperty, value);
        }

        public static readonly DependencyProperty IsSelectAllOnGotFocusProperty =
            DependencyProperty.RegisterAttached("IsSelectAllOnGotFocus", typeof(bool), typeof(TextBoxAttachment), new PropertyMetadata(false, (d, e) =>
            {
                if (!(d is TextBox tb)) { return; }
                if (!(e.NewValue is bool isSelectAll)) { return; }

                tb.GotFocus -= OnTextBoxGotFocus;
                tb.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                if (isSelectAll)
                {
                    tb.GotFocus += OnTextBoxGotFocus;
                    tb.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                }
            }));

        private static void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox tb)) { return; }
            var isSelectAllOnGotFocus = GetIsSelectAllOnGotFocus(tb);

            if (isSelectAllOnGotFocus)
            {
                tb.SelectAll();
            }
        }

        private static void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is TextBox tb)) { return; }

            if (tb.IsFocused) { return; }
            tb.Focus();
            e.Handled = true;
        }
    }
}
