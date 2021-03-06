﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using WPF.Views.Datas;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// Windowを閉じるのをキャンセルさせるBehavior
    /// </summary>
    public class WindowClosingCancelBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Closing += Window_Closing;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Window window = sender as Window;

            if (window.DataContext is IClosing) { e.Cancel = (window.DataContext as IClosing).OnClosing(); }
        }
    }
}
