using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.DataHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 管理料登録画面
    /// </summary>
    public class ManagementFeePaymentRegistrationViewModel : JustRegistraterDataViewModel
    {
        private string managementNumber;
        private string graveNumber;
        private string addresseeName;

        public ManagementFeePaymentRegistrationViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            ShowManagementFeeManagement = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.ManagementFeeManagement()), () => true);
            ManagementNumber = ReturnManagementFee(2.15);
        }
        public ManagementFeePaymentRegistrationViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        public DelegateCommand ShowManagementFeeManagement { get; }
        /// <summary>
        /// 管理番号
        /// </summary>
        public string ManagementNumber
        {
            get => managementNumber;
            set
            {
                managementNumber = value;
                CallPropertyChanged();
                if (value.Length == 6) { SetProperty(); }
            }
        }
        /// <summary>
        /// 墓地番号
        /// </summary>
        public string GraveNumber
        {
            get => graveNumber;
            set
            {
                graveNumber = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 宛名
        /// </summary>
        public string AddresseeName
        {
            get => addresseeName;
            set
            {
                addresseeName = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// プロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            Lessee lessee = DataBaseConnect.ReferenceLessee(ManagementNumber);

            GraveNumber=lessee.GraveNumber;
            if (string.IsNullOrEmpty( lessee.ReceiverName)) { AddresseeName=lessee.LesseeName; }
            else { AddresseeName=lessee.ReceiverName; }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
        }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = $"管理料登録：{AccountingProcessLocation.Location}"; }
    }
}
