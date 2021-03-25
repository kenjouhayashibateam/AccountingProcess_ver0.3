using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// コンボボックスの動作クラス
    /// </summary>
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

        public static bool GetIsIMEModeTrueOnGotFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetIsIMEModeOnProperty);
        }

        private static void OnComboBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is ComboBox cb)) { return; }

            var isIMEModeOnGotFocus = GetIsIMEModeTrueOnGotFocus(cb);

            if (!(cb.Template.FindName("PART_EditableTextBox", cb) is TextBox tb)) return;

            if (isIMEModeOnGotFocus)
            {
                InputMethod.SetPreferredImeState(tb, InputMethodState.On);
                InputMethod.SetPreferredImeConversionMode(tb, ImeConversionModeValues.FullShape | ImeConversionModeValues.Native);
            }
            else InputMethod.SetPreferredImeState(tb, InputMethodState.Off);
        }
        private static void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is ComboBox cb)) { return; }

            if (cb.IsFocused) { return; }
            cb.Focus();
            e.Handled = false;
        }
        /// <summary>
        /// GotFocus時にIMEをOnにした状態にします
        /// </summary>
        /// <param name="obj">対象のコンボボックス</param>
        /// <param name="value"></param>
        public static void SetIsIMEModeOffGotFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SetIsIMEModeOffProperty, value);
        }

        public static DependencyProperty SetIsIMEModeOffProperty =
            DependencyProperty.RegisterAttached("SetIsIMEModeOffProperty", typeof(bool), typeof(ComboBoxAttachment), new PropertyMetadata(false, (d, e) =>
            {
                if (!(d is ComboBox cb)) return;

                if (!(e.NewValue is bool isIMEMode)) { return; }
                cb.GotFocus -= OffComboBoxGotFocus;
                cb.PreviewMouseLeftButtonDown -= OffMouseLeftButtonDown;
                if (isIMEMode)
                {
                    cb.GotFocus += OffComboBoxGotFocus;
                    cb.PreviewMouseLeftButtonDown += OffMouseLeftButtonDown;
                }
            }
            ));

        public static bool GetIsIMEModeTrueOffGotFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetIsIMEModeOffProperty);
        }

        private static void OffComboBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is ComboBox cb)) { return; }

            var isIMEModeOffGotFocus = GetIsIMEModeTrueOffGotFocus(cb);
            var tb = cb.Template.FindName("PART_EditableTextBox", cb) as TextBox;

            if (isIMEModeOffGotFocus)
            {
                InputMethod.SetPreferredImeState(tb, InputMethodState.Off);
            }
        }
        private static void OffMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is ComboBox cb)) { return; }

            if (cb.IsFocused) { return; }
            cb.Focus();
            e.Handled = false;
        }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached("MaxLength", typeof(int), typeof(ComboBoxAttachment), new UIPropertyMetadata(1, OnMaxLengthChanged));

        public static void OnMaxLengthChanged(DependencyObject obj,DependencyPropertyChangedEventArgs args)
        {
            if (!(obj is ComboBox comboBox)) return;
            comboBox.Loaded +=
                (s, e) =>
                {
                    if (!(comboBox.Template.FindName("PART_EditableTextBox", comboBox) is TextBox textBox)) return;
                    textBox.MaxLength = (int)args.NewValue;
                };
        }
        public static int GetMaxLength(DependencyObject obj0) => (int)obj0.GetValue(MaxLengthProperty);
        public static void SetMaxLength(DependencyObject obj, int value) => obj.SetValue(MaxLengthProperty, value);
    }
}

