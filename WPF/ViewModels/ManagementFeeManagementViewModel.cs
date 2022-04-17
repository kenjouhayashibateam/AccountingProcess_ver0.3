using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 管理料管理画面
    /// </summary>
    public class ManagementFeeManagementViewModel : DataOperationViewModel
    {
        private decimal area;
        private string amount;
        private bool canDataOperation;
        private KeyValuePair<decimal, int> selectedFee;
        private ObservableCollection<KeyValuePair<decimal, int>> managementFeeList;

        public ManagementFeeManagementViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            SetDataList();
            PropertyClear();
            RegistrationCommand = new DelegateCommand(() => Registration(), () => true);
        }
        public ManagementFeeManagementViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        private void PropertyClear()
        {
            Area = 0;
            Amount = "0";
        }
        /// <summary>
        /// 登録コマンド
        /// </summary>
        public DelegateCommand RegistrationCommand { get; }
        private async void Registration()
        {
            CanDataOperation = false;

            await Task.Run(() => _ = DataBaseConnect.Registration(Area, IntAmount(Amount)));

            PropertyClear();
        }
        /// <summary>
        /// 管理料リスト
        /// </summary>
        public ObservableCollection<KeyValuePair<decimal, int>> ManagementFeeList
        {
            get => managementFeeList;
            set
            {
                managementFeeList = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 面積
        /// </summary>
        public decimal Area
        {
            get => area;
            set
            {
                area = value;
                ValidationProperty(nameof(Area), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作できるか
        /// </summary>
        public bool CanDataOperation
        {
            get => canDataOperation;
            set
            {
                canDataOperation = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 金額
        /// </summary>
        public string Amount
        {
            get => amount;
            set
            {
                amount = CommaDelimitedAmount(value);
                ValidationProperty(nameof(Amount), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された管理料
        /// </summary>
        public KeyValuePair<decimal, int> SelectedFee
        {
            get => selectedFee;
            set
            {
                selectedFee = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(Area):
                    SetNullOrEmptyError(propertyName, value);
                    if (GetErrors(propertyName) == null)
                    {
                        ErrorsListOperation(!double.TryParse(value.ToString(), out _), propertyName, "数字を入力してください");
                    }
                    break;
                case nameof(Amount):
                    SetNullOrEmptyError(propertyName, value);
                    break;
            }

            CanDataOperation = Area > 0 && IntAmount(amount) > 0;
        }

        protected override void SetDataList()
        {
            ManagementFeeList = DataBaseConnect.GetManagementFeeList();
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        {
        }

        protected override void SetDelegateCommand()
        {
        }

        protected override void SetDetailLocked()
        {
            
        }

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = "管理料管理"; }
    }
}
