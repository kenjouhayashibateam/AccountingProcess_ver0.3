using Domain.Entities.ValueObjects;
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
using static Domain.Entities.Helpers.DataHelper;
using static Domain.Entities.Helpers.TextHelper;

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
        private string area;
        private bool isSingleYear;
        private bool isDoubleYear;
        private string code;
        private string payer;
        private string fee;
        private ObservableCollection<AccountingSubject> accounts;
        private ObservableCollection<Content> contents;
        private AccountingSubject selectedAccountingSubject;
        private Content selectedContent;
        private string paymentFiscalYear;

        public ManagementFeePaymentRegistrationViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
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
        /// 面積
        /// </summary>
        public string Area
        {
            get => area;
            set
            {
                area = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 一年度分チェック
        /// </summary>
        public bool IsSingleYear
        {
            get => isSingleYear;
            set
            {
                isSingleYear = value;
                CallPropertyChanged();
                if(value) { SetDetail(); }
            }
        }
        /// <summary>
        /// 二年度分チェック
        /// </summary>
        public bool IsDoubleYear
        {
            get => isDoubleYear;
            set
            {
                isDoubleYear = value;
                CallPropertyChanged();
                if (value) { SetDetail(); }
            }
        }

        private void SetDetail()
        {
            string y1;
            string y2;

            if (DateTime.Now.Month < 4)
            {
                y1 = DateTime.Now.AddYears(1).Year.ToString(); 
                y2 = DateTime.Now.AddYears(2).Year.ToString();
            }
            else
            {
                y1 = DateTime.Now.Year.ToString(); 
                y2= DateTime.Now.AddYears(1).Year.ToString();
            }

            PaymentFiscalYear = y1;

            if (IsDoubleYear) { PaymentFiscalYear += $"、{y2}"; }
            SetCode();
        }
        /// <summary>
        /// 科目コード
        /// </summary>
        public string Code
        {
            get => code;
            set 
            {
                code = value;
                CallPropertyChanged();
                if (value.Length == 3)
                { Accounts = DataBaseConnect.ReferenceAccountingSubject(value, string.Empty, true, true); }
            } 
        }
        /// <summary>
        /// 科目リスト
        /// </summary>
        public ObservableCollection<AccountingSubject> Accounts
        {
            get => accounts;
            set
            {
                accounts = value;
                CallPropertyChanged();
                if (value.Count > 0)
                {
                    SelectedAccountingSubject=value[0];
                }
            } 
        }
        /// <summary>
        /// 選択された科目
        /// </summary>
        public AccountingSubject SelectedAccountingSubject
        {
            get => selectedAccountingSubject;
            set
            {
                selectedAccountingSubject = value;
                CallPropertyChanged();
                if (value != null)
                { Contents = DataBaseConnect.ReferenceContent
                        (string.Empty, value.SubjectCode, value.Subject, true, true); }
            }
        }
        /// <summary>
        /// 伝票内容リスト
        /// </summary>
        public ObservableCollection<Content> Contents
        {
            get => contents;
            set
            {
                contents = value;
                CallPropertyChanged();
                if(value.Count > 0) { SelectedContent=value[0]; }
            }
        }
        /// <summary>
        /// 支払者
        /// </summary>
        public string Payer
        {
            get => payer;
            set
            {
                payer = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 料金
        /// </summary>
        public string Fee
        {
            get => fee;
            set
            {
                fee = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 支払年度
        /// </summary>
        public string PaymentFiscalYear
        {
            get => paymentFiscalYear;
            set
            {
                paymentFiscalYear = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票内容
        /// </summary>
        public Content SelectedContent
        {
            get => selectedContent;
            set
            {
                selectedContent = value;
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
            Area = $"{lessee.Area}{Space}㎡";
            if (string.IsNullOrEmpty( lessee.ReceiverName)) { AddresseeName=lessee.LesseeName; }
            else { AddresseeName=lessee.ReceiverName; }
            Payer = AddresseeName;
            Fee = ReturnManagementFee(lessee.Area);
            SetCode();
        }

        private void SetCode()
        {
            if (DateTime.Now.Month < 4)
            { Code = "421"; }
            else
            {
                YearCheckSetCode();
            }

            void YearCheckSetCode()
            {
                if (IsSingleYear) { Code = "851"; }
                else { Code = "421"; }

                if(IsDoubleYear) { Code = "421"; }
                else { Code = "851"; }
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
        }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = $"管理料登録：{AccountingProcessLocation.Location}"; }
    }
}
