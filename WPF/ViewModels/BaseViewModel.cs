using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// ビューモデルの共通処理クラス
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand ShowFormCommand { get; set; }
        private bool callShowForm;

        public ShowFormData ShowForm { get; set; }

        public bool CallShowForm
        {
            get => callShowForm;
            set
            {
                if(callShowForm==value)
                {
                    return;
                }
                callShowForm = value;
                CallPropertyChanged();
                callShowForm = false;
            }
        }

        protected void CallPropertyChanged()
        {
            StackFrame caller = new StackFrame(1);
            string[] methodNames = caller.GetMethod().Name.Split('_');
            int i = methodNames.Length - 1;
            string propertyName = methodNames[i];

            CallPropertyChanged(propertyName);
        }
        protected void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void CreateShowFormCommand(Window myForm)
        {
            ShowFormCommand = new DelegateCommand(() =>SetShowForm(myForm) , () => true);
            CallPropertyChanged(nameof(ShowFormCommand));
            CallShowForm = true;
        }

        private void SetShowForm(Window myForm)
        {
            ShowForm = new ShowFormData()
            {
                FormData = myForm
            };

            CallPropertyChanged(nameof(ShowForm));
        }
    }
}
