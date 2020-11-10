using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Views.Behaviors
{
    public static class ComboBoxAttachment
    {
        /// <summary>
        /// GotFocus時にIMEをOnにした状態にします
        /// </summary>
        /// <param name="obj">対象のコンボボックス</param>
        /// <param name="value"></param>
        public static void SetIsIMEModeOnGotFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SetIsIMEModeOnProperty, value);
        }

        public static DependencyProperty SetIsIMEModeOnProperty =
            DependencyProperty.RegisterAttached("SetIsIMEModeOnProperty", typeof(bool), typeof(ComboBoxAttachment), new PropertyMetadata(false, (d, e) =>
            {
                if (!(d is ComboBox cb)) return;

                if (!(e.NewValue is bool isIMEMode)) { return; }
                cb.GotFocus -= OnComboBoxGotFocus;
                cb.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                if (isIMEMode)
                {
                    cb.GotFocus += OnComboBoxGotFocus;
                    cb.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                }
            }
            ));

        public static bool GetIsIMEModeOnGotFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetIsIMEModeOnProperty);
        }

        private static void OnComboBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is ComboBox cb)) { return; }

            var isIMEModeOnGotFocus = GetIsIMEModeOnGotFocus(cb);
            var tb = cb.Template.FindName("PART_EditableTextBox", cb) as TextBox;

            if (isIMEModeOnGotFocus)
            {
                InputMethod.SetPreferredImeState(tb, InputMethodState.On);
                InputMethod.SetPreferredImeConversionMode(tb, ImeConversionModeValues.FullShape | ImeConversionModeValues.Native);
            }
        }
        private static void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is ComboBox cb)) { return; }

            if (cb.IsFocused) { return; }
            cb.Focus();
            e.Handled = false;
        }
    }
}
