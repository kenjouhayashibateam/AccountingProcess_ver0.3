using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 出納データ操作画面ViewModel
    /// </summary>
    public class ReceiptsAndExpenditureOperationViewModel : DataOperationViewModel,
        IReceiptsAndExpenditureOperationObserver
    {
        #region Properties
        #region Strings
        private string receiptsAndExpenditureIDFieldText;
        private string detailTitle;
        private string depositAndWithdrawalContetnt;
        private string comboCreditDeptText;
        private string comboAccountingSubjectCode;
        private string comboAccountingSubjectText;
        private string comboContentText;
        private string otherDescription;
        private string detailText;
        private string price;
        private string slipOutputDateTitle;
        private string dataOperationButtonContent;
        private string supplement;
        private string supplementInfo;
        /// <summary>
        /// 補足に入る必須入力文字
        /// </summary>
        private string SupplementRequiredString;
        #endregion
        #region Bools
        private bool isValidity;
        private bool isValidityEnabled;
        private bool isReducedTaxRate;
        private bool isOutput;
        private bool isOutputCheckEnabled;
        private bool isDataOperationButtonEnabled;
        private bool isPaymentCheck;
        private bool isSupplementVisiblity;
        private bool isReducedTaxRateVisiblity;
        private bool isPaymentCheckEnabled;
        private bool isInputWizardEnabled;
        #endregion
        #region ObservableCollections
        private ObservableCollection<CreditDept> comboCreditDepts;
        private ObservableCollection<AccountingSubject> comboAccountingSubjectCodes;
        private ObservableCollection<AccountingSubject> comboAccountingSubjects;
        private ObservableCollection<Content> comboContents;
        #endregion
        #region ValueObjects
        private CreditDept selectedCreditDept;
        private AccountingSubject selectedAccountingSubjectCode;
        private AccountingSubject selectedAccountingSubject;
        private Content selectedContent;
        private Rep operationRep;
        #endregion
        #region DateTimes
        private DateTime accountActivityDate;
        private DateTime registrationDate;
        private DateTime slipOutputDate;
       #endregion
        private SolidColorBrush detailBackGroundColor;
        private readonly ReceiptsAndExpenditureOperation OperationData;
        private int receiptsAndExpenditureIDField;
        readonly LoginRep loginRep = LoginRep.GetInstance();
        #endregion

        public ReceiptsAndExpenditureOperationViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            OperationData = ReceiptsAndExpenditureOperation.GetInstance();
            OperationData.Add(this);
            if (OperationData.Data != null)
            {
                SetDataUpdateCommand.Execute();
                SetReceiptsAndExpenditureProperty();
            }
            else
            {
                SetDataRegistrationCommand.Execute();
                IsPaymentCheck = true;
                FieldClear();
            } 
        }
        public ReceiptsAndExpenditureOperationViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        public DelegateCommand ShowWizardCommand { get; set; }
        private void ShowWizard()
        {
            CreateShowWindowCommand(ScreenTransition.ReceiptsAndExpenditureRegistrationHelper());
            SetReceiptsAndExpenditureProperty();
        }
        /// <summary>
        /// 勘定科目一覧PDFを開くコマンド
        /// </summary>
        public DelegateCommand ShowAccountTitleListPDFFileCommand { get; set; }
        private void ShowAccountTitleListPDFFile() =>
            System.Diagnostics.Process.Start(".\\files\\AccountTitleList.pdf");
        /// <summary>
        /// 詳細メニューのプロパティをセットします
        /// </summary>
        private void SetDetailFieldProperty()
        {
            switch(OperationData.GetOperationType())
            {
                case ReceiptsAndExpenditureOperation.OperationType.ReceiptsAndExpenditure:
                    SetDetailTitle("その他詳細", string.Empty);
                    break;
                case ReceiptsAndExpenditureOperation.OperationType.Voucher:
                    IsPaymentCheck = true;
                    SetDetailTitle("宛名", string.Empty);
                    break;
            }
            IsPaymentCheckEnabled =
                OperationData.GetOperationType() == 
                ReceiptsAndExpenditureOperation.OperationType.ReceiptsAndExpenditure;
        }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand ReceiptsAndExpenditureDataOperationCommand { get; set; }
        /// <summary>
        /// 出納データを登録、更新します
        /// </summary>
        private void ReceiptsAndExpenditureDataOperation()
        {
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    DataRegistration();
                    break;
                case DataOperation.更新:
                    DataUpdate();
                    break;
            }
        }
        /// <summary>
        /// 出納データを更新します
        /// </summary>
        private async void DataUpdate()
        {
            IsDataOperationButtonEnabled = false;
            if (OperationData.Data == null) return;

            string UpdateCotent = string.Empty;

            if (OperationData.Data.IsPayment != IsPaymentCheck)
            {
                string formCheck = IsPaymentCheck ? "入金" : "出金";
                string instanceCheck = OperationData.Data.IsPayment ? "入金" : "出金";
                UpdateCotent += $"入出金 : { instanceCheck} → { formCheck }";
            }

            if (OperationData.Data.AccountActivityDate != AccountActivityDate)
                UpdateCotent +=
                    $"入出金日 : {OperationData.Data.AccountActivityDate.ToShortDateString()} → " +
                    $"{AccountActivityDate.ToShortDateString()}\r\n";

            if (OperationData.Data.CreditDept.Dept != ComboCreditDeptText)
                UpdateCotent +=
                    $"貸方勘定 : {OperationData.Data.CreditDept.Dept} → {ComboCreditDeptText}\r\n";

            if (OperationData.Data.Content.AccountingSubject.SubjectCode != ComboAccountingSubjectCode)
                UpdateCotent +=
                    $"勘定科目コード : {OperationData.Data.Content.AccountingSubject.SubjectCode} → " +
                    $"{ComboAccountingSubjectCode}\r\n";

            if (OperationData.Data.Content.AccountingSubject.Subject != ComboAccountingSubjectText)
                UpdateCotent += $"勘定科目 : {OperationData.Data.Content.AccountingSubject.Subject} → " +
                    $"{ComboAccountingSubjectText}\r\n";

            if (OperationData.Data.Content.Text != ComboContentText)
                UpdateCotent += $"内容 : {OperationData.Data.Content.Text} → {ComboContentText}\r\n";

            if (OperationData.Data.Detail !=JoinDetail())
                UpdateCotent += $"詳細 : {OperationData.Data.Detail} → {JoinDetail()}\r\n";

            if (OperationData.Data.Price != IntAmount(price))
                UpdateCotent += $"金額 : {AmountWithUnit(OperationData.Data.Price)} → " +
                    $"{AmountWithUnit(IntAmount(Price))}\r\n";

            if (OperationData.Data.IsValidity != IsValidity)
                UpdateCotent += $"有効性 : {OperationData.Data.IsValidity} → {IsValidity}\r\n";

            if (OperationData.Data.OutputDate != SlipOutputDate)
                UpdateCotent += $"出力日 : {OperationData.Data.OutputDate.ToShortDateString()} → " +
                    $"{(SlipOutputDate==DefaultDate?"未出力":SlipOutputDate.ToShortDateString())}\r\n";

            if (OperationData.Data.IsReducedTaxRate != IsReducedTaxRate)
                UpdateCotent +=
                    $"軽減税率データ : {OperationData.Data.IsReducedTaxRate} → {IsReducedTaxRate}\r\n";

            if (UpdateCotent.Length == 0)
            {
                CallNoRequiredUpdateMessage();
                IsDataOperationButtonEnabled = true;
                return;
            }

            if (CallConfirmationDataOperation
                ($"{UpdateCotent}\r\n\r\n更新しますか？", "伝票") ==
                System.Windows.MessageBoxResult.Cancel)
            {
                SetReceiptsAndExpenditureProperty();
                IsDataOperationButtonEnabled = true;
                return;
            }

            ReceiptsAndExpenditure updateData =
                new ReceiptsAndExpenditure
                (ReceiptsAndExpenditureIDField, RegistrationDate, OperationRep,
                OperationData.Data.Location, SelectedCreditDept, SelectedContent,
                JoinDetail(), IntAmount(price), IsPaymentCheck, IsValidity, 
                AccountActivityDate, SlipOutputDate, IsReducedTaxRate);

            DataOperationButtonContent = "更新中";
            await Task.Run(() => DataBaseConnect.Update(updateData));
            OperationData.SetData(updateData);
            OperationData.Notify();
            CallCompletedUpdate();
            DataOperationButtonContent = "更新";

            IsDataOperationButtonEnabled = true;
        }
        /// <summary>
        /// DetailとSupplementを結合して返します
        /// </summary>
        /// <returns></returns>
        private string JoinDetail() => $"{DetailText}{Space}{Supplement}".Trim();
        /// <summary>
        /// 出納データを登録します
        /// </summary>
        private async void DataRegistration()
        {
            IsDataOperationButtonEnabled = false;
            ReceiptsAndExpenditure rae =
                new ReceiptsAndExpenditure
                (0, DateTime.Now, LoginRep.Rep, AccountingProcessLocation.Location,
                SelectedCreditDept, SelectedContent,JoinDetail(), IntAmount(price), IsPaymentCheck, IsValidity,
                AccountActivityDate, DefaultDate, IsReducedTaxRate);

            string depositAndWithdrawalText = IsPaymentCheck ? "入金" : "出金";
            if (CallConfirmationDataOperation
                   (
                       $"経理担当場所\t : {rae.Location}\r\n" +
                       $"入出金日\t\t : {rae.AccountActivityDate.ToShortDateString()}\r\n" +
                       $"入出金\t\t : {depositAndWithdrawalText}\r\n" +
                       $"貸方勘定\t\t : {rae.CreditDept.Dept}\r\n" +
                       $"コード\t\t : {rae.Content.AccountingSubject.SubjectCode}\r\n" +
                       $"勘定科目\t\t : {rae.Content.AccountingSubject.Subject}\r\n" +
                       $"内容\t\t : {rae.Content.Text}\r\n" +
                       $"詳細\t\t : {JoinDetail()}\r\n金額\t\t : {AmountWithUnit(rae.Price)}\r\n" +
                       $"軽減税率\t\t : {rae.IsReducedTaxRate}\r\n" +
                       $"有効性\t\t : {rae.IsValidity}\r\n\r\n登録しますか？", "伝票"
                   )
                   == System.Windows.MessageBoxResult.Cancel)
            {
                IsDataOperationButtonEnabled = true;
                return;
            }

            DataOperationButtonContent = "登録中";
            await Task.Run(() => DataBaseConnect.Registration(rae));
            OperationData.SetData(rae);
            OperationData.Notify();
            CallShowMessageBox = true;
            FieldClear();
            CallCompletedRegistration();
            DataOperationButtonContent = "登録";
        }
        /// <summary>
        /// フィールドの値をクリアします
        /// </summary>
        private void FieldClear()
        {
            IsValidity = true;
            OperationData.SetData(null);
            ComboCreditDeptText = string.Empty;
            ComboAccountingSubjectCodes.Clear();
            SelectedAccountingSubjectCode = null;
            ComboAccountingSubjectCode = string.Empty;
            if (SelectedAccountingSubject != null) SelectedAccountingSubject = null;
            ComboAccountingSubjectText = string.Empty;
            SelectedContent = null;
            ComboContentText = string.Empty;
            DetailText = string.Empty;
            Price = string.Empty;
            AccountActivityDate = DateTime.Today;
            RegistrationDate = DateTime.Today;
            OperationRep = loginRep.Rep;
            if (SelectedCreditDept == null) SelectedCreditDept = ComboCreditDepts[0];
            SlipOutputDate = DefaultDate;
            IsOutput = false;
            IsOutputCheckEnabled = false;
            SetDetailFieldProperty();
        }
        /// <summary>
        /// フィールドにプロパティをセットします
        /// </summary>
        private void SetReceiptsAndExpenditureProperty()
        {
            OperationRep = OperationData.Data.RegistrationRep ?? loginRep.Rep;
            ReceiptsAndExpenditureIDField = OperationData.Data.ID;
            IsValidity = OperationData.Data.IsValidity;
            IsPaymentCheck = OperationData.Data.IsPayment;
            SlipOutputDate = OperationData.Data.OutputDate;
            IsOutput = OperationData.Data.OutputDate != DefaultDate;
            IsOutputCheckEnabled = OperationData.Data.ID > 0;
            SelectedCreditDept = OperationData.Data.CreditDept;
            ComboCreditDeptText = OperationData.Data.CreditDept.Dept;
            ComboAccountingSubjectText = OperationData.Data.Content.AccountingSubject.Subject;
            ComboContentText = OperationData.Data.Content.Text;
            ComboAccountingSubjectCode = OperationData.Data.Content.AccountingSubject.SubjectCode;
            DetailText = OperationData.Data.Detail;
            Price = SetPrice();
            AccountActivityDate = OperationData.Data.AccountActivityDate;
            RegistrationDate = OperationData.Data.RegistrationDate;
            IsReducedTaxRate = OperationData.Data.IsReducedTaxRate;
            //補足が入力されるContentの場合に、各フィールドに値を振り分ける。
            //現状管理料のみだが、他に出てきた時にはelse if句で対応する
            SetDetailFieldProperty();
            if (OperationData.Data.Content.Text.Contains("管理料")) ManagementFeeTextAllocation();
            else
            {
                DetailText = OperationData.Data.Detail;
                Supplement = string.Empty;
            }

            string SetPrice()
            {
                if (OperationData.Data.Price == 0)
                    return OperationData.Data.Content.FlatRate > 0 ? CommaDelimitedAmount(OperationData.Data.Content.FlatRate) : string.Empty;

                return CommaDelimitedAmount(OperationData.Data.Price);
            }
        }
        /// <summary>
        /// 管理料のDetailを分割して、年度分の文字列をSupplementに配分します
        /// </summary>
        private void ManagementFeeTextAllocation()
        {
            string s = OperationData.Data.Detail.Replace(SpaceF,Space);
            string[] detailArray = s.Split(' ');
            DetailText = string.Empty;
            Supplement = string.Empty;

            foreach (string t in detailArray)
            {
                if (t.Contains("年度分")) Supplement = t;
                else DetailText += t;
            }
            SetDetailTitle("支払者名", "必ず「年度分」の文字を入力");
        }
        /// <summary>
        /// その他詳細、補足のタイトルをセットします
        /// </summary>
        /// <param name="otherDescriptionString">その他詳細</param>
        /// <param name="supplementInfoString">補足</param>
        private void SetDetailTitle(string otherDescriptionString,string supplementInfoString)
        {
            SupplementInfo = supplementInfoString;
            OtherDescription = otherDescriptionString;
        }
        /// <summary>
        /// 出納IDフィールド
        /// </summary>
        private int ReceiptsAndExpenditureIDField
        {
            get => receiptsAndExpenditureIDField;
            set
            {
                receiptsAndExpenditureIDField = value;
                if (value == 0) ReceiptsAndExpenditureIDFieldText = string.Empty;
                else ReceiptsAndExpenditureIDFieldText = $"データID : {value}";
            }
        }
        /// <summary>
        /// 金額に000を付け足すコマンド
        /// </summary>
        public DelegateCommand ZeroAddCommand { get; set; }
        /// <summary>
        /// 金額に000を付け足す
        /// </summary>
        private void ZeroAdd()
        {
            int i = IntAmount(Price);
            i *= 1000;
            Price = CommaDelimitedAmount(i);
        }
        /// <summary>
        /// 出納データIDの表示Text
        /// </summary>
        public string ReceiptsAndExpenditureIDFieldText
        {
            get => receiptsAndExpenditureIDFieldText;
            set
            {
                receiptsAndExpenditureIDFieldText = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納データの入出金を表すタイトル
        /// </summary>
        public string DetailTitle
        {
            get => detailTitle;
            set
            {
                detailTitle = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金に応じて、詳細の色を決める
        /// </summary>
        public SolidColorBrush DetailBackGroundColor
        {
            get => detailBackGroundColor;
            set
            {
                detailBackGroundColor = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity
        {
            get => isValidity;
            set
            {
                isValidity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金のトグルボタンのContent
        /// </summary>
        public string DepositAndWithdrawalContetnt
        {
            get => depositAndWithdrawalContetnt;
            set
            {
                depositAndWithdrawalContetnt = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方部門のリスト
        /// </summary>
        public ObservableCollection<CreditDept> ComboCreditDepts
        {
            get => comboCreditDepts;
            set
            {
                comboCreditDepts = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 貸方部門コンボボックスのText
        /// </summary>
        public string ComboCreditDeptText
        {
            get => comboCreditDeptText;
            set
            {
                if (comboCreditDeptText == value) return;
                comboCreditDeptText = value;
                SelectedCreditDept = ComboCreditDepts.FirstOrDefault(c => c.Dept == value);
                ValidationProperty(nameof(ComboCreditDeptText), value);
                SetDataOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された貸方部門
        /// </summary>
        public CreditDept SelectedCreditDept 
        { 
            get => selectedCreditDept; 
            set 
            {
                selectedCreditDept = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コードのコンボボックスのText
        /// </summary>
        public string ComboAccountingSubjectCode
        {
            get => comboAccountingSubjectCode;
            set
            {
                comboAccountingSubjectCode = string.Empty;
                
                if (string.IsNullOrEmpty(value)&&OperationData.Data==null)
                {
                    SetField();
                    ValidationProperty(nameof(ComboAccountingSubjectCode), comboAccountingSubjectCode);
                    CallPropertyChanged();
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"\d"))
                    comboAccountingSubjectCode = string.Empty;
                else comboAccountingSubjectCode = value;

                if (comboAccountingSubjectCode.Length == 3) ComboAccountingSubjects =
                           DataBaseConnect.ReferenceAccountingSubject(value, string.Empty, true);
                else
                {
                    SetField();
                    ValidationProperty(nameof(ComboAccountingSubjectCode), comboAccountingSubjectCode);
                    CallPropertyChanged();
                    return;
                }

                if (ComboAccountingSubjects.Count != ComboAccountingSubjectCodes.Count &&
                    ComboAccountingSubjects.Count!=0)
                {
                    comboAccountingSubjectCode = ComboAccountingSubjects[0].SubjectCode;
                    ComboAccountingSubjectText = ComboAccountingSubjects[0].Subject;
                }
                else
                {
                    comboAccountingSubjectCode = value;
                    if (SelectedAccountingSubject != null) SelectedAccountingSubject = null;
                    ComboAccountingSubjectText = string.Empty;
                    ComboContentText = string.Empty;
                    ComboContents.Clear();
                }

                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboAccountingSubjectCode), comboAccountingSubjectCode);
                CallPropertyChanged();
                
                void SetField()
                {
                    ComboAccountingSubjects =
                        DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
                    if (SelectedAccountingSubject != null) SelectedAccountingSubject = null;
                    ComboAccountingSubjectText = string.Empty;
                    ComboContents.Clear();
                    if (SelectedContent != null) SelectedContent = null;
                    SetDataOperationButtonEnabled();
                }
            }
        }
        /// <summary>
        /// 勘定科目コードリスト
        /// </summary>
        public ObservableCollection<AccountingSubject> ComboAccountingSubjectCodes
        {
            get => comboAccountingSubjectCodes;
            set
            {
                comboAccountingSubjectCodes = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された勘定科目コード
        /// </summary>
        public AccountingSubject SelectedAccountingSubjectCode
        {
            get => selectedAccountingSubjectCode;
            set
            {
                if (selectedAccountingSubjectCode != null && selectedAccountingSubjectCode.Equals(value)) return;
                selectedAccountingSubjectCode = value;
                if (value == null) ComboAccountingSubjectCode = string.Empty;
                else
                {
                    ComboAccountingSubjectCode = value.SubjectCode;
                    SelectedAccountingSubject = value;
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 勘定科目コンボボックスのText
        /// </summary>
        public string ComboAccountingSubjectText
        {
            get => comboAccountingSubjectText;
            set
            {
                if (comboAccountingSubjectText == value) return;
                
                if(string.IsNullOrEmpty(value))
                {
                    comboAccountingSubjectText = value;
                    ValidationProperty(nameof(ComboAccountingSubjectText), value);
                    CallPropertyChanged();
                    return;
                }

                AccountingSubject accountingSubject = 
                    ComboAccountingSubjects.FirstOrDefault(r => r.Subject == value);

                if (accountingSubject == null)
                {
                    comboAccountingSubjectText = string.Empty;
                    ValidationProperty(nameof(ComboAccountingSubjectText), comboAccountingSubjectText);
                    return;
                }
                else comboAccountingSubjectText = accountingSubject.Subject;

                if (string.IsNullOrEmpty(value))
                {
                    PropertyClear();
                    return;
                }

                SetDataOperationButtonEnabled();

                ComboContents =                         
                    DataBaseConnect.ReferenceContent(string.Empty, string.Empty, value, true);

                if (ComboContents.Count > 0) ComboContentText = ComboContents[0].Text;
                else ComboContentText = string.Empty;

                comboAccountingSubjectText = accountingSubject.Subject;
                ValidationProperty(nameof(ComboAccountingSubjectText), value);
                CallPropertyChanged();

                void PropertyClear()
                {
                    if(accountingSubject.SubjectCode == ComboAccountingSubjectCode) return;

                    comboAccountingSubjectText = string.Empty;
                    ComboAccountingSubjects =
                        DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
                    ComboAccountingSubjectCode = string.Empty;
                    ComboContents.Clear();
                    ValidationProperty(nameof(ComboAccountingSubjectText), value);
                }
            }
        }
        /// <summary>
        /// 勘定科目のリスト
        /// </summary>
        public ObservableCollection<AccountingSubject> ComboAccountingSubjects
        {
            get => comboAccountingSubjects;
            set
            {
                comboAccountingSubjects = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された勘定科目
        /// </summary>
        public AccountingSubject SelectedAccountingSubject
        {
            get => selectedAccountingSubject;
            set
            {
                selectedAccountingSubject = value;
                SelectedAccountingSubjectCode = value;
                ComboContents = DataBaseConnect.ReferenceContent
                    (string.Empty, ComboAccountingSubjectCode,
                        ComboAccountingSubjectText ?? string.Empty, true);
                if (ComboContents.Count > 0) ComboContentText =
                        ComboContents.Count != 0 ? ComboContents[0].Text : string.Empty;
                else ComboContentText = string.Empty;

                ComboAccountingSubjectText = value == null ? string.Empty : value.Subject;

                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容コンボボックスのText
        /// </summary>
        public string ComboContentText
        {
            get => comboContentText;
            set
            {
                SelectedContent = ComboContents.FirstOrDefault(c => c.Text == value);
                if (SelectedContent == null)
                {
                    comboContentText = string.Empty;
                    Price = string.Empty;
                }
                else comboContentText = SelectedContent.Text;

                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboContentText), comboContentText);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容コンボボックスリスト
        /// </summary>
        public ObservableCollection<Content> ComboContents
        {
            get => comboContents;
            set
            {
                comboContents = value;
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
                if (value != null) SetContentProperty();
                else
                {
                    Supplement = string.Empty;
                    IsSupplementVisiblity = false;
                    Price = string.Empty;
                    IsReducedTaxRateVisiblity = !IsSupplementVisiblity;
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// Contentを参照して定額、低減税率チェックに値を入力します
        /// </summary>
        private void SetContentProperty()
        {
            if (SelectedContent.Text.Contains("管理料")) SupplementRequiredString = "年度分";
            else SupplementRequiredString = string.Empty;

            IsSupplementVisiblity = SelectedContent != null && SelectedContent.Text.Contains("管理料");

            IsReducedTaxRateVisiblity = !IsSupplementVisiblity;

            if(IsReducedTaxRateVisiblity) IsReducedTaxRate = SelectedContent.Text == "供物";
            
            if ( selectedContent.FlatRate > 0)
                Price = selectedContent.FlatRate.ToString();
            else Price = string.Empty;

            if (SelectedAccountingSubject == null) return;

            if (IsSupplementVisiblity)
            {
                Supplement = SelectedAccountingSubject.Number == 26 ?
                    $"{DateTime.Now.Year}年度分" :
                    $"{DateTime.Now.Year},{DateTime.Now.Year + 1}年度分";
                SetDetailTitle("支払者名", $"必ず「{SupplementRequiredString}」の文字を入力");
            }
            else SetDetailTitle("その他詳細", string.Empty);
        }
        /// <summary>
        /// 詳細テキストブロックに表示するタイトル
        /// </summary>
        public string OtherDescription
        {
            get => otherDescription;
            set
            {
                otherDescription = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 詳細欄のText
        /// </summary>
        public string DetailText
        {
            get => detailText;
            set
            {
                detailText = value.Replace('　',' ');
                SetDataOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 軽減税率かのチェック
        /// </summary>
        public bool IsReducedTaxRate
        {
            get => isReducedTaxRate;
            set
            {
                isReducedTaxRate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納金額
        /// </summary>
        public string Price
        {
            get => price;
            set
            {
                price = CommaDelimitedAmount(value);
                ValidationProperty(nameof(Price), price);
                SetDataOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金日
        /// </summary>
        public DateTime AccountActivityDate
        {
            get => accountActivityDate;
            set
            {
                accountActivityDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出納データ登録日
        /// </summary>
        public DateTime RegistrationDate
        {
            get => registrationDate;
            set
            {
                registrationDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票出力日のタイトル
        /// </summary>
        public string SlipOutputDateTitle
        {
            get => slipOutputDateTitle;
            set
            {
                slipOutputDateTitle = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票出力日
        /// </summary>
        public DateTime SlipOutputDate
        {
            get => slipOutputDate;
            set
            {
                slipOutputDate = value;
                if (slipOutputDate == DefaultDate) SlipOutputDateTitle = "伝票出力日（未出力）";
                else SlipOutputDateTitle = "伝票出力日";
                CallPropertyChanged();
            }
        }
        /// <summary> 
        /// データ操作担当者
        /// </summary>
        public Rep OperationRep
        {
            get => operationRep;
            set
            {
                operationRep = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力されたかのチェック
        /// </summary>
        public bool IsOutput
        {
            get => isOutput;
            set
            {
                isOutput = value;
                SetOutputDate(value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作ボタンのContent
        /// </summary>
        public string DataOperationButtonContent
        {
            get => dataOperationButtonContent;
            set
            {
                dataOperationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作ボタンのEnable
        /// </summary>
        public bool IsDataOperationButtonEnabled
        {
            get => isDataOperationButtonEnabled;
            set
            {
                isDataOperationButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金チェック　入金がTrue
        /// </summary>
        public bool IsPaymentCheck
        {
            get => isPaymentCheck;
            set
            {
                isPaymentCheck = value;
                if (value)
                {
                    DepositAndWithdrawalContetnt = "出金に変更";
                    DetailTitle = "入金伝票";
                    DetailBackGroundColor = new SolidColorBrush(Colors.MistyRose);
                }
                else
                {
                    DepositAndWithdrawalContetnt = "入金に変更";
                    DetailTitle = "出金伝票";
                    DetailBackGroundColor = new SolidColorBrush(Colors.LightCyan);
                }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 補足説明のVisiblity
        /// </summary>
        public bool IsSupplementVisiblity
        {
            get => isSupplementVisiblity;
            set
            {
                isSupplementVisiblity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 軽減税率チェックのVisiblity
        /// </summary>
        public bool IsReducedTaxRateVisiblity
        {
            get => isReducedTaxRateVisiblity;
            set
            {
                isReducedTaxRateVisiblity = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 補足
        /// </summary>
        public string Supplement
        {
            get => supplement;
            set
            {
                supplement = value;
                ValidationProperty(nameof(Supplement), value);
                SetDataOperationButtonEnabled();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 有効性チェックのEnabled
        /// </summary>
        public bool IsValidityEnabled
        {
            get => isValidityEnabled;
            set
            {
                isValidityEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入出金トグルボタンのEnabled
        /// </summary>
        public bool IsPaymentCheckEnabled
        {
            get => isPaymentCheckEnabled;
            set
            {
                isPaymentCheckEnabled = value;
                CallPropertyChanged();               
            }
        }
        /// <summary>
        /// 印刷済みフラグチェックのEnabled
        /// </summary>
        public bool IsOutputCheckEnabled
        {
            get => isOutputCheckEnabled;
            set
            {
                isOutputCheckEnabled = value;
                CallPropertyChanged();
            }
        }
        private void SetOutputDate(bool value)
        {
            if (value) SlipOutputDate =
                     OperationData.Data.OutputDate == DefaultDate ?
                         DateTime.Today : OperationData.Data.OutputDate;
            else SlipOutputDate = DefaultDate;
        }
        /// <summary>
        /// 補足に促す注意
        /// </summary>
        public string SupplementInfo
        {
            get => supplementInfo;
            set
            {
                supplementInfo = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録ウィザードボタンのEnabled
        /// </summary>
        public bool IsInputWizardEnabled
        {
            get => isInputWizardEnabled;
            set
            {
                isInputWizardEnabled = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// データ操作ボタンのEnabledを設定します
        /// </summary>
        public void SetDataOperationButtonEnabled() =>
            IsDataOperationButtonEnabled = CanOperation();
        /// <summary>
        /// 出納データ操作時の必須のフィールドにデータが入力されているかを確認し、判定結果を返します
        /// </summary>
        /// <returns>判定結果</returns>
        private bool CanOperation()
        {
            bool b =!HasErrors && !string.IsNullOrEmpty(ComboCreditDeptText) &
                !string.IsNullOrEmpty(ComboContentText) &
                !string.IsNullOrEmpty(ComboAccountingSubjectText) &
                !string.IsNullOrEmpty(ComboAccountingSubjectCode) &
                !string.IsNullOrEmpty(Price) && 0 < IntAmount(price);

            if (!b) return false;

            b=SlipOutputDate == DefaultDate;
            if (!b) b = LoginRep.GetInstance().Rep.IsAdminPermisson &&
                     $"{SlipOutputDate.Year}{SlipOutputDate.Month}" ==
                     $"{DateTime.Today.Year}{DateTime.Today.Month}";

            if (!b) DataOperationButtonContent =
                    "更新は管理者権限所有者が、今月中の出力データでのみ許可されます";

            return b;
        }

        public void ReceiptsAndExpenditureOperationNotify() => SetReceiptsAndExpenditureProperty();        

        public override void SetRep(Rep rep)
        {
            if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
            else
            {
                IsAdminPermisson = rep.IsAdminPermisson;
                WindowTitle = $"{DefaultWindowTitle}（ログイン : {GetFirstName(rep.Name)}）";
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(ComboContentText):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ComboAccountingSubjectText):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ComboAccountingSubjectCode):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    string s = (string)value;
                    ErrorsListOperation(s.Length != 3, propertyName, "コードは3桁で入力してください");
                    break;
                case nameof(Price):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    break;
                case nameof(ComboCreditDeptText):
                    SetNullOrEmptyError(propertyName, value.ToString());
                    if (!string.IsNullOrEmpty(value.ToString())) ErrorsListOperation
                        (SelectedCreditDept == null, propertyName, "貸方部門をリストから選択してください");
                    break;
                case nameof(Supplement):
                    if(ComboContentText!=null && ComboContentText.Contains("管理料"))
                    {
                        SetNullOrEmptyError(propertyName, value.ToString());
                        ErrorsListOperation(!((string)value).Contains(SupplementRequiredString), 
                            propertyName, $"{SupplementRequiredString}を入力してください");
                    }
                    break;
            }            
        }

        protected override void SetWindowDefaultTitle()
        {
            string genre = OperationData == null ? "登録" : "更新";
            DefaultWindowTitle = $"出納データ{genre} : {AccountingProcessLocation.Location}";
        }

        protected override void SetDetailLocked()
        {
            IsValidityEnabled = CurrentOperation == DataOperation.更新;
            IsInputWizardEnabled = CurrentOperation == DataOperation.登録;

            if (IsValidityEnabled)
                ComboCreditDeptText = OperationData.Data.CreditDept.Dept;
            else ComboCreditDeptText = ComboCreditDepts[0].Dept;
        }

        protected override void SetDataOperationButtonContent(DataOperation operation) =>
            DataOperationButtonContent = operation.ToString();

        protected override void SetDataList()
        {
            ComboContents = new ObservableCollection<Content>();
            ComboAccountingSubjectCodes =
                DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            ComboAccountingSubjects =
                DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            ComboCreditDepts = DataBaseConnect.ReferenceCreditDept(string.Empty, true, false);
        }

        protected override void SetDelegateCommand()
        {
            ZeroAddCommand = new DelegateCommand
                (() => ZeroAdd(), () => true);
            ReceiptsAndExpenditureDataOperationCommand = new DelegateCommand
                (() => ReceiptsAndExpenditureDataOperation(), () => true);
            ShowAccountTitleListPDFFileCommand = new DelegateCommand
                (() => ShowAccountTitleListPDFFile(), () => true);
            ShowWizardCommand = new DelegateCommand
                (() => ShowWizard(), () => true);
        }
    }
}
