using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Views.Behaviors
{
    public class DatePickerAttachment : DatePicker
    {
        /// <summary>
        /// GotFocus時にIMEをoffにした状態にします
        /// </summary>
        /// <param name="obj">対象のコンボボックス</param>
        /// <param name="value"></param>
        public static void SetIsIMEModeOffGotFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SetIsIMEModeOffProperty, value);
        }

        public static DependencyProperty SetIsIMEModeOffProperty =
            DependencyProperty.RegisterAttached("SetIsIMEModeOffProperty", typeof(bool),
                typeof(DatePickerAttachment), new PropertyMetadata(false, (d, e) =>
                {
                    if (!(d is DatePicker dp)) { return; }

                    if (!(e.NewValue is bool isIMEMode)) { return; }
                    dp.GotFocus -= OffDatePickerGotFocus;
                    dp.PreviewMouseLeftButtonDown -= OffMouseLeftButtonDown;
                    if (isIMEMode)
                    {
                        dp.GotFocus += OffDatePickerGotFocus;
                        dp.PreviewMouseLeftButtonDown += OffMouseLeftButtonDown;
                    }
                }));

        public static bool GetIsIMEModeTrueOffGotFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetIsIMEModeOffProperty);
        }

        private static void OffDatePickerGotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is DatePicker dp)) { return; }

            bool isIMEModeOffGotFocus = GetIsIMEModeTrueOffGotFocus(dp);
            TextBox tb = dp.Template.FindName("PART_TextBox", dp) as TextBox;

            if (isIMEModeOffGotFocus)
            {
                InputMethod.SetPreferredImeState(tb, InputMethodState.Off);
            }
        }
        private static void OffMouseLeftButtonDown
            (object sender, MouseButtonEventArgs e)
        {
            if (!(sender is DatePicker dp)) { return; }

            if (dp.IsFocused) { return; }
            _ = dp.Focus();
            e.Handled = false;
        }
    }
}
