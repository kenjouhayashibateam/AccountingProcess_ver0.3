using System.Windows;
using System.Windows.Controls;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// テキストボックス動作クラス
    /// </summary>
    public static class TextBoxAttachment
    {
        /// <summary>
        /// GotFocus時にテキストをすべて選択にするかを返します
        /// </summary>
        /// <param name="obj">対象のテキストボックス</param>
        /// <returns></returns>
        public static bool GetIsSelectAllOnGotFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectAllOnGotFocusProperty);
        }
        /// <summary>
        /// GotFocus時にテキストをすべて選択した状態にします
        /// </summary>
        /// <param name="obj">対象のテキストボックス</param>
        /// <param name="value"></param>
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
