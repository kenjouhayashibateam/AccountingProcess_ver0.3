using System.Windows.Input;
using WPF.ViewModels.Commands;

namespace WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private bool callRemainingMoneyCalculation=false;
        private ICommand showRemainingMoneyCalculation;

        public bool CallRemainingMoneyCalculation
        {
            get => callRemainingMoneyCalculation;
            set
            {
                callRemainingMoneyCalculation = value;
                CallPropertyChanged();
            }
        }

        public ICommand ShowRemainingMoneyCalculation
        {
            get
            {
                showRemainingMoneyCalculation = new DelegateCommand(() =>
                {
                    CreateShowFormCommand(new Views.RemainingMoneyCalculationView());
                    CallShowForm = true;
                    CallPropertyChanged();
                },
                () => true);
                return showRemainingMoneyCalculation;
            }
            set => showRemainingMoneyCalculation = value;
        }
    }
}