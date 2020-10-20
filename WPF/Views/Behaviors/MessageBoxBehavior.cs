using System.Windows;
using System.Windows.Interactivity;
using WPF.Views.Datas;

namespace WPF.Views.Behaviors
{
    public class MessageBoxBehavior : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (parameter is DependencyPropertyChangedEventArgs e
                && e.NewValue is MessageBoxInfo info)
            {
                info.Result = MessageBox.Show(info.Message, info.Title, info.Button, info.Image, info.DefaultResult, info.Options);
            }
        }
    }
}
