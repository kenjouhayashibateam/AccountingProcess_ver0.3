using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using WPF.Views.Datas;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// Window.ShowDialog動作
    /// </summary>
    public class ShowDiarog : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// Window.ShowDialogを実行します
        /// </summary>
        /// <param name="parameter">実行するウィンドウデータ</param>
        protected override void Invoke(object parameter)
        {
            DependencyPropertyChangedEventArgs e = (DependencyPropertyChangedEventArgs)parameter;
            ShowWindowData showForm = (ShowWindowData)e.NewValue;
            Window Parent = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            showForm.WindowData.Owner = Parent;//親画面を代入
            showForm.WindowData.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //showForm.WindowData.Owner.ShowInTaskbar = false;
            //showForm.WindowData.Owner.Visibility = Visibility.Hidden;//親画面を隠す
            showForm.WindowData.ShowDialog();
            showForm.WindowData.Owner.ShowInTaskbar = true;
            //showForm.WindowData.Owner.Visibility = Visibility.Visible;
            //Application.Current.MainWindow.Visibility = Visibility.Visible;
        }
    }
}