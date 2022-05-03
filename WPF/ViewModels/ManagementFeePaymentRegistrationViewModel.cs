using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.DataHelper;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 管理料登録画面
    /// </summary>
    public class ManagementFeePaymentRegistrationViewModel : JustRegistraterDataViewModel, IClosing
    {
        #region Properties
        #region Strings
        private string managementNumber;
        private string graveNumber;
        private string addresseeName;
        private string area;
        private string code;
        private string payer;
        private string fee;
        private string paymentFiscalYear;
        private string operationContent = "登録";
        #endregion
        #region Bools
        private bool isSingleYear;
        private bool isDoubleYear;
        private bool canOperation;
        private bool canClose;
        private bool isAccountsReceivableOther;
        #endregion
        private ObservableCollection<AccountingSubject> accounts;
        private ObservableCollection<Content> contents;
        private AccountingSubject selectedAccountingSubject;
        private Content selectedContent;
        private DateTime activityDate = DateTime.Today;
        #endregion

        public ManagementFeePaymentRegistrationViewModel(IDataBaseConnect dataBaseConnect) :
            base(dataBaseConnect)
        {
            SetCode();
            IsDoubleYear = true;
            DataRegistrationCommand = new DelegateCommand(() => DataRegistriation(), () => true);
            ManagementNumber = string.Empty;
            Fee = string.Empty;
        }
        public ManagementFeePaymentRegistrationViewModel() : 
            this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 登録コマンド
        /// </summary>
        public DelegateCommand DataRegistrationCommand { get; }
        private async void DataRegistriation()
        {
            StringBuilder info=new StringBuilder();

            info.AppendLine($"管理番号\t{ManagementNumber}");
            info.AppendLine($"墓地番号\t{GraveNumber}");
            info.AppendLine($"面積\t{Area}");
            info.AppendLine($"支払年度\t{PaymentFiscalYear}");
            info.AppendLine($"勘定科目\t{SelectedAccountingSubject.Subject}");           
            info.AppendLine($"管理料\t{Fee}{Space}円");
            info.AppendLine($"支払人\t{Payer}");
            info.AppendLine();
            info.AppendLine("登録しますか？");

            if (CallConfirmationDataOperation(info.ToString(), "管理料入金") == System.Windows.MessageBoxResult.Cancel) { return; }
            CanClose = false;
            OperationContent = "登録中";

            await Task.Run(() =>Registration());
            CallCompletedRegistration();
            OperationContent = "登録";
            CanClose = true;

            void Registration()
            {
                CreditDept creditDept = DataBaseConnect.ReferenceCreditDept("春秋苑", true, true)[0];
                string s = $"{Payer}{Space}{ManagementNumber}{Space}{PaymentFiscalYear}年度分";
                _ = DataBaseConnect.Registration(new ReceiptsAndExpenditure(0, DateTime.Now, LoginRep.GetInstance().Rep,
                    AccountingProcessLocation.Location.ToString(), creditDept, SelectedContent, s,IntAmount(Fee), true, true, 
                    ActivityDate, DefaultDate, false));
                ManagementNumber=string.Empty;
                ClearProperty();
            }
        }
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
                else { ClearProperty(); }
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
                if(value) { SetProperty(); }
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
                if (value) { SetProperty(); }
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

            PaymentFiscalYear = IsAccountsReceivableOther ? string.Empty : y1;
            if (IsDoubleYear) { PaymentFiscalYear += $"、{y2}"; }
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
                ValidationProperty(nameof(Code), code);
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
                ValidationProperty(nameof(Payer), value);
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
                fee = CommaDelimitedAmount(value);
                ValidationProperty(nameof(Fee), fee);
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
                ValidationProperty(nameof(PaymentFiscalYear), paymentFiscalYear);
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
        /// データ操作できるか
        /// </summary>
        public bool CanOperation
        {
            get => canOperation;
            set
            {
                canOperation = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ウインドウを閉じさせるか
        /// </summary>
        public bool CanClose
        {
            get => canClose;
            set
            {
                canClose = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入金日
        /// </summary>
        public DateTime ActivityDate
        {
            get => activityDate;
            set
            {
                activityDate = value;
                CallPropertyChanged();
                SetProperty();
            }
        }
        /// <summary>
        /// 未収入金チェック
        /// </summary>
        public bool IsAccountsReceivableOther
        {
            get => isAccountsReceivableOther;
            set
            {
                isAccountsReceivableOther = value;
                CallPropertyChanged();
                if (value) { SetProperty(); }
            }
        }
        /// <summary>
        /// 登録する際のボタンの文字列
        /// </summary>
        public string OperationContent
        {
            get => operationContent;
            set
            {
                operationContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// プロパティをクリアします
        /// </summary>
        private void ClearProperty()
        {
            GraveNumber = string.Empty;
            Area = string.Empty;
            AddresseeName = string.Empty;
            Payer = string.Empty;
            Fee = string.Empty;
            IsDoubleYear = true;
        }
        /// <summary>
        /// プロパティをセットします
        /// </summary>
        private async void SetProperty()
        {
            await Task.Delay(1);
            SetCode();
            SetDetail();

            Lessee lessee = DataBaseConnect.ReferenceLessee(ManagementNumber);
            if (lessee == null) { return; }

            //名義人情報を各プロパティに代入
            GraveNumber =lessee.GraveNumber;
            Area = $"{lessee.Area}{Space}㎡";
            AddresseeName = lessee.LesseeName;
            //送付先に名前があればそちらを優先して支払人に代入
            Payer = string.IsNullOrEmpty(lessee.ReceiverName) ?
                lessee.LesseeName.Replace(SpaceF, Space) : 
                lessee.ReceiverName.Replace(SpaceF,Space);
            Fee = IsSingleYear ? CommaDelimitedAmount(ReturnManagementFee(lessee.Area)) :
                CommaDelimitedAmount(ReturnManagementFee(lessee.Area) * 2);
        }
        /// <summary>
        /// 勘定科目コードを設定します
        /// </summary>
        private void SetCode()
        {
            //3月以前は前受け金
            if (ActivityDate.Month < 4)
            { Code = "421"; }
            else
            { YearCheckSetCode(); }

            async void YearCheckSetCode()
            {
                await Task.Delay(1);
                //単年度は管理料
                if (IsSingleYear) { Code = "851"; }
                //2年度分は複合勘定
                if(IsDoubleYear)
                {
                    Code = "527";
                    SelectedContent = Contents.Where(c => c.Text.Contains("管理料")).First();
                }
                //未収入金
                if (IsAccountsReceivableOther)
                {
                    Code = "161";
                    SelectedContent = Contents.Where(c => c.Text.Contains("管理料")).First();
                }
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(PaymentFiscalYear):
                    SetNullOrEmptyError(propertyName, value);
                    if (GetErrors(propertyName) == null)
                    {
                        //2022、2023を区切る
                        string[] s = value.ToString().Split('、');
                        //要素が4桁の数字でなければ西暦ではないと判断する
                        foreach (string t in s) { DigitNumberVerification(t); }
                    }
                    break;
                case nameof(Fee):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(Payer):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(Code):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                default:
                    break;
            }

            CanOperation = !HasErrors;

            void DigitNumberVerification(string s)
            {
                if (GetErrors(propertyName) != null) { return; }
                
                ErrorsListOperation
                    (!new Regex(@"\d{4}").IsMatch(s), propertyName, "西暦４桁になっていません"); 
            }
        }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = $"管理料登録：{AccountingProcessLocation.Location}"; }

        public bool CancelClose() => CanClose;
    }
}
