using System;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using WPF.Views.Datas;

namespace WPF.Views.Behaviors
{
    public class ShowDiarog : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            DependencyPropertyChangedEventArgs e = (DependencyPropertyChangedEventArgs)parameter;
            ShowFormData showForm = (ShowFormData)e.NewValue;
            Window Parent = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            Parent.ShowInTaskbar = false;
            showForm.FormData.Owner = Parent;
            showForm.FormData.ShowDialog();
            Parent.ShowInTaskbar = true;
        }
    }
}