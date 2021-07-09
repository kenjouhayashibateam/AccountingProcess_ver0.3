using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using WPF.Win32;
using WPF.Win32.Api;
using This = WPF.Views.Behaviors.SystemMenuBehavior;

namespace WPF.Views.Behaviors
{
    public class SystemMenuBehavior : Behavior<Window>
    {
        public bool? IsVisible
        {
            get => (bool?)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register
            ("IsVisible", typeof(bool?), typeof(This), new PropertyMetadata(null, OnPropertyChanged));

        public bool? CanMinimize
        {
            get => (bool?)GetValue(CanMinimizeProperty);
            set => SetValue(CanMinimizeProperty, value);
        }
        public static readonly DependencyProperty CanMinimizeProperty =
            DependencyProperty.Register
            ("CanMinimize", typeof(bool?), typeof(This), new PropertyMetadata(null, OnPropertyChanged));
        
        public bool? CanMaximize
        {
            get => (bool?)GetValue(CanMaximizeProperty);
            set => SetValue(CanMaximizeProperty, value);
        }
        private static readonly DependencyProperty CanMaximizeProperty =
            DependencyProperty.Register
            ("CanMaximize", typeof(bool?), typeof(This), new PropertyMetadata(null, OnPropertyChanged));

        public bool EnableAltF4
        {
            get => (bool)GetValue(EnableAltF4Property);
            set => SetValue(EnableAltF4Property, value);
        }
        public static readonly DependencyProperty EnableAltF4Property =
            DependencyProperty.Register
            ("EnableAltF4", typeof(bool), typeof(This), new PropertyMetadata(true));

        private static void OnPropertyChanged
            (DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is This self) { self.Apply(); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.SourceInitialized += OnSourceInitialized;
            base.OnAttached();
        }

        private void OnSourceInitialized(object sender, EventArgs e) { Apply(); }

        private void Apply()
        {
            if (AssociatedObject == null) { return; }
            //スタイル
            IntPtr hwnd = new WindowInteropHelper(AssociatedObject).Handle;
            int style = User32.GetWindowLong(hwnd, Constant.GWL_STYLE);

            if (IsVisible.HasValue)
            {
                if (IsVisible.Value) { style |= Constant.WS_SYSMENU; }
                else { style &= ~Constant.WS_SYSMENU; }
            }
            if (CanMinimize.HasValue)
            {
                if (CanMinimize.Value) { style |= Constant.WS_MINIMIZEBOX; }
                else { style &= ~Constant.WS_MINIMIZEBOX; }
            }
            if (CanMaximize.HasValue)
            {
                if (CanMaximize.Value) { style |= Constant.WS_MAXIMIZEBOX; }
                else { style &= ~Constant.WS_MAXIMIZEBOX; }
            }
            _ = User32.SetWindowLong(hwnd, Constant.GWL_STYLE, style);
        }
    }
}

