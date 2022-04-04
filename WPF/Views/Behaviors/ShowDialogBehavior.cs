using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using WPF.Views.Datas;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// Window.ShowDialog動作
    /// </summary>
    public class ShowDialogBehavior : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// Window.ShowDialogを実行します
        /// </summary>
        /// <param name="parameter">実行するウィンドウデータ</param>
        protected override void Invoke(object parameter)
        {
            DependencyPropertyChangedEventArgs e = (DependencyPropertyChangedEventArgs)parameter;
            ShowWindowData showForm = (ShowWindowData)e.NewValue;
            //親画面を代入
            showForm.WindowData.Owner =
                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive); ;
            showForm.WindowData.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            _ = showForm.WindowData.ShowDialog();
        }
    }
}