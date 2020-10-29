using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// ビューモデルの共通処理クラス
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged,INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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

        public bool HasErrors
        {
            get
            {
                return CurrentErrors.Count > 0;
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

        public abstract void ValidationProperty(string propertyName, object value);

        public IEnumerable GetErrors(string propertyName)
        {
            if (!CurrentErrors.ContainsKey(propertyName))
                return null;

            return CurrentErrors[propertyName];
        }

        public Dictionary<string, string> CurrentErrors=new Dictionary<string, string>();

        protected void AddError(string propertyName,string error)
        {
            if (!CurrentErrors.ContainsKey(propertyName))
                CurrentErrors.Add(propertyName, error);

            OnErrorsChanged(propertyName);
        }

        protected void RemoveError(string propertyName)
        {
            if (CurrentErrors.ContainsKey(propertyName))
                CurrentErrors.Remove(propertyName);

            OnErrorsChanged(propertyName);
        }

        protected void OnErrorsChanged(string proeprtyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(proeprtyName));
        }

        protected void ErrorsListOperation(bool hasError, string propertyName,string exeption)
        {
            if(hasError)
            {
                AddError(propertyName, exeption);
            }
            else
            {
                RemoveError(propertyName);
            }
        }
    }
}
