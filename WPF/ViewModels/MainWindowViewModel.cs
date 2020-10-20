using System.Linq.Expressions;
using System.Windows.Input;
using WPF.ViewModels.Commands;

namespace WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public DelegateCommand ShowRemainingMoneyCalculation { get; }

        public MainWindowViewModel()
        {
            ShowRemainingMoneyCalculation= new DelegateCommand(                () => SetShowRemainingMoneyCalculationView(),
                () => true
                );
        }

        private void SetShowRemainingMoneyCalculationView()
        {
            CreateShowFormCommand(new Views.RemainingMoneyCalculationView());
            CallPropertyChanged(nameof(ShowRemainingMoneyCalculation));
        }
    }
}