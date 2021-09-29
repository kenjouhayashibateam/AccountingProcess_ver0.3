using System.Windows;
using WPF.ViewModels.Commands;

namespace WPF.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WavSoundPlayCommand.Play("Startup.wav");
        }
    }
}
