using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace WPF.Views.Behaviors
{
    public class WindowCloseBehavior : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        { Window.GetWindow(AssociatedObject).Close(); }
    }
}
