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

            showForm.FormData.Owner = Parent;
            showForm.FormData.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            showForm.FormData.Owner.Visibility = Visibility.Hidden;
            showForm.FormData.ShowDialog();
            showForm.FormData.Owner.Visibility = Visibility.Visible;
        }
    }
}