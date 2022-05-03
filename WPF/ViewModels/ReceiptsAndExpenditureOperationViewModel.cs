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
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 出納データ操作画面ViewModel
    /// </summary>
    public class ReceiptsAndExpenditureOperationViewModel : DataOperationViewModel,
        IReceiptsAndExpenditureOperationObserver, IClosing
    {
        #region Properties
        #region Strings
        private string receiptsAndExpenditureIDFieldText;
        private string detailTitle;
        private string comboCreditDeptText;
        private string comboAccountingSubjectCode;
        private string comboAccountingSubjectText;
        private string comboContentText = string.Empty;
        private string otherDescription;
        private string detailText;
        private string price;
        private string slipOutputDateTitle;
        private string dataOperationButtonContent;
        private string supplement = string.Empty;
        private string supplementInfo = string.Empty;
        /// <summary>
        /// 補足に入る必須入力文字
        /// </summary>
        private string SupplementRequiredString = string.Empty;
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
        private bool isAccountingSubjectEnabled;
        private bool isContentEnabled;
        private bool IsPassDataNotifyProcess = false;
        private bool CanClose = true;
        /// <summary>
        /// 警告をスルーするか
        /// </summary>
        private bool IsInfoThrough = false;
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
        private int HoldingPrice;
        private ReceiptsAndExpenditure OperationData = ReceiptsAndExpenditureOperation.GetInstance().Data;
        private int receiptsAndExpenditureIDField;
        #endregion

        public ReceiptsAndExpenditureOperationViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            ReceiptsAndExpenditureOperation.GetInstance().Add(this);
            if (ReceiptsAndExpenditureOperation.GetInstance().Data != null)
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
            this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
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
        private void ShowAccountTitleListPDFFile()
        { _ = System.Diagnostics.Process.Start(@".\files\AccountTitleList.pdf"); }
        /// <summary>
        /// 詳細メニューのプロパティをセットします
        /// </summary>
        private void SetDetailFieldProperty()
        {
            switch (ReceiptsAndExpenditureOperation.GetInstance().GetOperationType())
            {
                case ReceiptsAndExpenditureOperation.OperationType.ReceiptsAndExpenditure:
                    SetDetailTitle("その他詳細", string.Empty);
                    break;
                case ReceiptsAndExpenditureOperation.OperationType.Voucher:
                    IsPaymentCheck = true;
                    SetDetailTitle("宛名", string.Empty);
                    break;
                default:
                    break;
            }
            IsPaymentCheckEnabled =
                ReceiptsAndExpenditureOperation.GetInstance().GetOperationType() ==
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
            CanClose = false;
            switch (CurrentOperation)
            {
                case DataOperation.登録:
                    DataRegistration();
                    break;
                case DataOperation.更新:
                    DataUpdate();
                    break;
                default:
                    break;
            }
            CanClose = true;
        }
        /// <summary>
        /// 出納データを更新します
        /// </summary>
        private async void DataUpdate()
        {
            OperationData = ReceiptsAndExpenditureOperation.GetInstance().Data;

            IsDataOperationButtonEnabled = false;
            if (OperationData == null) { return; }

            string UpdateCotent = string.Empty;

            if (OperationData.IsPayment != IsPaymentCheck)
            {
                string formCheck = IsPaymentCheck ? "入金" : "出金";
                string instanceCheck = OperationData.IsPayment ? "入金" : "出金";
                UpdateCotent += $"入出金 : { instanceCheck} → { formCheck }";
            }

            if (OperationData.AccountActivityDate != AccountActivityDate)
            {
                UpdateCotent +=
                    $"入出金日 : {OperationData.AccountActivityDate.ToShortDateString()} → " +
                    $"{AccountActivityDate.ToShortDateString()}\r\n";
            }

            if (OperationData.CreditDept.Dept != ComboCreditDeptText)
            {
                UpdateCotent +=
                    $"貸方勘定 : {OperationData.CreditDept.Dept} → {ComboCreditDeptText}\r\n";
            }

            if (OperationData.Content.AccountingSubject.SubjectCode != ComboAccountingSubjectCode)
            {
                string originalBranchNumber =
                    string.IsNullOrEmpty
                    (DataBaseConnect.GetBranchNumber(OperationData.Content.AccountingSubject)) ?
                    string.Empty : 
                    $"-{DataBaseConnect.GetBranchNumber(OperationData.Content.AccountingSubject)}";
                string updateBranchNumber =
                    string.IsNullOrEmpty(DataBaseConnect.GetBranchNumber(SelectedAccountingSubject)) ?
                    string.Empty : $"-{DataBaseConnect.GetBranchNumber(SelectedAccountingSubject)}";
                UpdateCotent +=
                    $"勘定科目コード : {OperationData.Content.AccountingSubject.SubjectCode}" +
                    $"{originalBranchNumber}{Space}→{Space}{ComboAccountingSubjectCode}" +
                    $"{updateBranchNumber}\r\n";
            }

            if (OperationData.Content.AccountingSubject.Subject != ComboAccountingSubjectText)
            {
                UpdateCotent += $"勘定科目 : {OperationData.Content.AccountingSubject.Subject} → " +
                    $"{ComboAccountingSubjectText}\r\n";
            }

            if (OperationData.Content.Text != ComboContentText)
            { UpdateCotent += $"内容 : {OperationData.Content.Text} → {ComboContentText}\r\n"; }

            if (OperationData.Detail != JoinDetail())
            { UpdateCotent += $"詳細 : {OperationData.Detail} → {JoinDetail()}\r\n"; }

            if (OperationData.Price != IntAmount(price))
            {
                UpdateCotent += $"金額 : {AmountWithUnit(OperationData.Price)} → " +
                    $"{AmountWithUnit(IntAmount(Price))}\r\n";
            }

            if (OperationData.IsValidity != IsValidity)
            { UpdateCotent += $"有効性 : {OperationData.IsValidity} → {IsValidity}\r\n"; }

            if (OperationData.OutputDate != SlipOutputDate)
            {
                UpdateCotent += $"出力日 : {OperationData.OutputDate.ToShortDateString()} → " +
                    $"{(SlipOutputDate == DefaultDate ? "未出力" : SlipOutputDate.ToShortDateString())}\r\n";
            }

            if (OperationData.IsReducedTaxRate != IsReducedTaxRate)
            {
                UpdateCotent +=
                    $"軽減税率データ : {OperationData.IsReducedTaxRate} → {IsReducedTaxRate}\r\n";
            }

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
                OperationData.Location, SelectedCreditDept, SelectedContent,
                JoinDetail(), IntAmount(price), IsPaymentCheck, IsValidity,
                AccountActivityDate, SlipOutputDate, IsReducedTaxRate);

            DataOperationButtonContent = "更新中";
            _ = await Task.Run(() => _ = DataBaseConnect.Update(updateData));
            CallCompletedUpdate();
            ReceiptsAndExpenditureOperation.GetInstance().SetData(updateData);
            ReceiptsAndExpenditureOperation.GetInstance().Notify();
            DataOperationButtonContent = "更新";

            if (OperationData.OutputDate.Month == DateTime.Today.Month) { return; }

            if (OperationData.OutputDate == DefaultDate) { return; }

            CreateResubmitInfo();

            void CreateResubmitInfo()
            {
                MessageBox = new MessageBoxInfo()
                {
                    Message =
                        $"訂正した出納帳（{OperationData.OutputDate.Month}月分）を必ず印刷してください。",
                    Image = System.Windows.MessageBoxImage.Information,
                    Title = "要注意！！！",
                    Button = System.Windows.MessageBoxButton.OK
                };
                CallShowMessageBox = true;
            }
            IsDataOperationButtonEnabled = true;
        }
        /// <summary>
        /// DetailとSupplementを結合して返します
        /// </summary>
        /// <returns></returns>
        private string JoinDetail() { return $"{DetailText}{Space}{Supplement}".Trim(); }
        /// <summary>
        /// 出納データを登録します
        /// </summary>
        private async void DataRegistration()
        {
            IsDataOperationButtonEnabled = false;
            ReceiptsAndExpenditure rae =
                new ReceiptsAndExpenditure
                (0, DateTime.Now, LoginRep.GetInstance().Rep, AccountingProcessLocation.Location.ToString(),
                SelectedCreditDept, SelectedContent, JoinDetail(), IntAmount(price), IsPaymentCheck, IsValidity,
                AccountActivityDate, DefaultDate, IsReducedTaxRate);

            string depositAndWithdrawalText = IsPaymentCheck ? "入金" : "出金";
            if (CallConfirmationDataOperation
                   (
                       $"経理担当場所\t : {rae.Location}\r\n" +
                       $"入出金日\t\t : {rae.AccountActivityDate.ToShortDateString()}\r\n" +
                       $"入出金\t\t : {depositAndWithdrawalText}\r\n" +
                       $"貸方部門\t\t : {rae.CreditDept.Dept}\r\n" +
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
                ReceiptsAndExpenditureOperation.GetInstance().SetData(rae);
                SetReceiptsAndExpenditureProperty();
                ReceiptsAndExpenditureOperation.GetInstance().SetData(null);
                return;
            }

            DataOperationButtonContent = "登録中";
            _ = await Task.Run(() => _ = DataBaseConnect.Registration(rae));
            CallShowMessageBox = true;
            CallCompletedRegistration();
            IsPassDataNotifyProcess = true;
            SetOperationData();
            IsPassDataNotifyProcess = false;
            FieldClear();
            DataOperationButtonContent = "登録";

            void SetOperationData()
            { 
                ReceiptsAndExpenditureOperation.GetInstance().SetData(rae);
                ReceiptsAndExpenditureOperation.GetInstance().Notify();
            }
        }
        /// <summary>
        /// フィールドの値をクリアします
        /// </summary>
        private void FieldClear()
        {
            OperationData = ReceiptsAndExpenditureOperation.GetInstance().Data;

            IsValidity = true;
            ReceiptsAndExpenditureOperation.GetInstance().SetData(null);
            ComboCreditDeptText = string.Empty;
            ComboAccountingSubjectCodes.Clear();
            SelectedAccountingSubjectCode = null;
            ComboAccountingSubjectCode = string.Empty;
            if (SelectedAccountingSubject != null) { SelectedAccountingSubject = null; }
            ComboAccountingSubjectText = string.Empty;
            SelectedContent = null;
            ComboContentText = string.Empty;
            DetailText = string.Empty;
            Price = string.Empty;
            AccountActivityDate = DateTime.Today;
            RegistrationDate = DateTime.Today;
            OperationRep = LoginRep.GetInstance().Rep;
            SlipOutputDate = DefaultDate;
            IsOutput = false;
            IsOutputCheckEnabled = false;
            IsAccountingSubjectEnabled = false;
            IsContentEnabled = false;
            HoldingPrice = -1;
            SetDetailFieldProperty();
        }
        /// <summary>
        /// フィールドにプロパティをセットします
        /// </summary>
        private void SetReceiptsAndExpenditureProperty()
        {
            OperationData = ReceiptsAndExpenditureOperation.GetInstance().Data;

            IsInfoThrough = true;
            OperationRep = OperationData.RegistrationRep ?? LoginRep.GetInstance().Rep;
            ReceiptsAndExpenditureIDField = OperationData.ID;
            IsValidity = OperationData.IsValidity;
            IsPaymentCheck = OperationData.IsPayment;
            SlipOutputDate = OperationData.OutputDate;
            IsOutput = OperationData.OutputDate != DefaultDate;
            IsOutputCheckEnabled = OperationData.ID > 0;
            SelectedAccountingSubjectCode = OperationData.Content.AccountingSubject;
            ComboAccountingSubjectCode = OperationData.Content.AccountingSubject.SubjectCode;
            SelectedAccountingSubject = OperationData.Content.AccountingSubject;
            ComboAccountingSubjectText = OperationData.Content.AccountingSubject.Subject;
            ComboContentText = OperationData.Content.Text;
            SelectedContent = OperationData.Content;
            SelectedCreditDept = OperationData.CreditDept;
            ComboCreditDeptText = OperationData.CreditDept.Dept;
            DetailText = OperationData.Detail;
            Price = SetPrice();
            HoldingPrice = OperationData.Price;
            AccountActivityDate = OperationData.AccountActivityDate;
            RegistrationDate = OperationData.RegistrationDate;
            IsReducedTaxRate = OperationData.IsReducedTaxRate;
            //補足が入力されるContentの場合に、各フィールドに値を振り分ける。
            //現状管理料のみだが、他に出てきた時にはelse if句で対応する
            SetDetailFieldProperty();
            if (OperationData.Content.Text.Contains("管理料")) { ManagementFeeTextAllocation(); }
            else
            {
                DetailText = OperationData.Detail;
                Supplement = string.Empty;
            }

            IsInfoThrough = false;

            string SetPrice()
            {
                return OperationData.Price == 0
                    ? OperationData.Content.FlatRate > 0 ?
                        CommaDelimitedAmount(OperationData.Content.FlatRate) : string.Empty
                    : CommaDelimitedAmount(OperationData.Price);
            }
        }
        /// <summary>
        /// 管理料のDetailを分割して、年度分の文字列をSupplementに配分します
        /// </summary>
        private void ManagementFeeTextAllocation()
        {
            string s = ReceiptsAndExpenditureOperation.GetInstance().Data.Detail.Replace(SpaceF, Space);
            string[] detailArray = s.Split(' ');
            DetailText = string.Empty;
            Supplement = string.Empty;

            foreach (string t in detailArray)
            {
                if (t.Contains("年度分")) { Supplement = t; }
                else { DetailText += t; }
            }
            SetDetailTitle("支払者名", "必ず「年度分」の文字を入力");
        }
        /// <summary>
        /// その他詳細、補足のタイトルをセットします
        /// </summary>
        /// <param name="otherDescriptionString">その他詳細</param>
        /// <param name="supplementInfoString">補足</param>
        private void SetDetailTitle(string otherDescriptionString, string supplementInfoString)
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
                ReceiptsAndExpenditureIDFieldText = value == 0 ? string.Empty : $"データID : {value}";
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
                if (comboCreditDeptText == value)
                {
                    CallPropertyChanged();
                    return;
                }
                comboCreditDeptText = value;
                CallPropertyChanged();
                SelectedCreditDept = ComboCreditDepts.FirstOrDefault(c => c.Dept == value);
                ValidationProperty(nameof(ComboCreditDeptText), value);
                SetDataOperationButtonEnabled();
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
                //ComboCreditDeptText = value == null ? string.Empty : value.Dept;
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

                if (string.IsNullOrEmpty(value) && ReceiptsAndExpenditureOperation.GetInstance().Data == null)
                {
                    SetField();
                    ValidationProperty(nameof(ComboAccountingSubjectCode), comboAccountingSubjectCode);
                    CallPropertyChanged();
                    return;
                }

                comboAccountingSubjectCode =
                    !System.Text.RegularExpressions.Regex.IsMatch(value, @"\d") ? string.Empty : value;

                if (comboAccountingSubjectCode.Length == 3)
                {
                    ComboAccountingSubjects =
                        DataBaseConnect.ReferenceAccountingSubject(value, string.Empty,
                            AccountingProcessLocation.IsAccountingGenreShunjuen, true);
                }
                else
                {
                    SetField();
                    ValidationProperty(nameof(ComboAccountingSubjectCode), comboAccountingSubjectCode);
                    CallPropertyChanged();
                    return;
                }

                if (ComboAccountingSubjects.Count != ComboAccountingSubjectCodes.Count &&
                    ComboAccountingSubjects.Count != 0)
                {
                    comboAccountingSubjectCode = ComboAccountingSubjects[0].SubjectCode;
                    ComboAccountingSubjectText = ComboAccountingSubjects[0].Subject;
                }
                else
                {
                    comboAccountingSubjectCode = value;
                    if (SelectedAccountingSubject != null) { SelectedAccountingSubject = null; }
                    ComboAccountingSubjectText = string.Empty;
                    ComboContentText = string.Empty;
                    ComboContents.Clear();
                }

                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboAccountingSubjectCode), comboAccountingSubjectCode);
                CallPropertyChanged();

                void SetField()
                {
                    ComboAccountingSubjects.Clear();
                    if (SelectedAccountingSubject != null) { SelectedAccountingSubject = null; }
                    ComboAccountingSubjectText = string.Empty;
                    ComboContents.Clear();
                    if (SelectedContent != null) { SelectedContent = null; }
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
                if (selectedAccountingSubjectCode != null && selectedAccountingSubjectCode.Equals(value))
                { return; }
                selectedAccountingSubjectCode = value;
                if (value == null) { ComboAccountingSubjectCode = string.Empty; }
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
                if (comboAccountingSubjectText == value) { return; }

                if (string.IsNullOrEmpty(value))
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
                else { comboAccountingSubjectText = accountingSubject.Subject; }

                if (string.IsNullOrEmpty(value))
                {
                    PropertyClear();
                    return;
                }

                SetDataOperationButtonEnabled();

                comboAccountingSubjectText = accountingSubject.Subject;
                ValidationProperty(nameof(ComboAccountingSubjectText), value);
                CallPropertyChanged();

                if (!ComboContents.Equals(DataBaseConnect.ReferenceContent
                    (string.Empty, ComboAccountingSubjectCode, value,
                        AccountingProcessLocation.IsAccountingGenreShunjuen, 
                        ReceiptsAndExpenditureOperation.GetInstance().Data == null)))
                {
                    ComboContents =
                        DataBaseConnect.ReferenceContent(string.Empty, ComboAccountingSubjectCode, value,
                        AccountingProcessLocation.IsAccountingGenreShunjuen, 
                        ReceiptsAndExpenditureOperation.GetInstance().Data == null);
                    SelectedContent = ComboContents.Count > 0 ? ComboContents[0] : null;
                    ComboContentText = SelectedContent == null ? string.Empty : SelectedContent.Text;
                }

                void PropertyClear()
                {
                    if (accountingSubject.SubjectCode == ComboAccountingSubjectCode) { return; }

                    comboAccountingSubjectText = string.Empty;
                    ComboAccountingSubjects =
                        DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty,
                            AccountingProcessLocation.IsAccountingGenreShunjuen, true);
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
                IsAccountingSubjectEnabled = value.Count > 0;
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
                //SelectedAccountingSubjectCode = value;
                if (value == null)
                { ComboContents.Clear(); }
                else
                {
                    ComboContents = DataBaseConnect.ReferenceContent
                        (string.Empty, ComboAccountingSubjectCode,
                            ComboAccountingSubjectText ?? string.Empty,
                            AccountingProcessLocation.IsAccountingGenreShunjuen,
                            ReceiptsAndExpenditureOperation.GetInstance().Data == null);
                }

                ComboContentText =
                    ComboContents.Count > 0 ? ComboContents.Count != 0 ?
                        ComboContents[0].Text : string.Empty : string.Empty;

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
                if (SelectedContent != ComboContents.FirstOrDefault(c => c.Text == value))
                { SelectedContent = ComboContents.FirstOrDefault(c => c.Text == value); }
                if (SelectedContent == null)
                {
                    comboContentText = string.Empty;
                    Price = string.Empty;
                }
                else { comboContentText = SelectedContent.Text; }

                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboContentText), comboContentText);
                CallPropertyChanged();
                if (!string.IsNullOrEmpty(value)) { SetContentProperty(); }
                else
                {
                    Supplement = string.Empty;
                    IsSupplementVisiblity = false;
                    Price = string.Empty;
                    IsReducedTaxRateVisiblity = !IsSupplementVisiblity;
                }
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
                IsContentEnabled = value.Count > 0;
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
                if (value == null)
                {
                    ComboContentText = string.Empty;
                    return;
                }
                if (value.Text != ComboContentText)
                { ComboContentText = value.Text; }

                SetContentProperty();
            }
        }
        /// <summary>
        /// Contentを参照して各プロパティに値を入力します
        /// </summary>
        private void SetContentProperty()
        {
            if (SelectedContent == null) { return; }

            SupplementRequiredString = SelectedContent.Text.Contains("管理料") ? "年度分" : string.Empty;

            IsSupplementVisiblity = SelectedContent != null && SelectedContent.Text.Contains("管理料");

            IsReducedTaxRateVisiblity = !IsSupplementVisiblity;

            if (IsReducedTaxRateVisiblity) { IsReducedTaxRate = SelectedContent.Text == "供物"; }

            if (HoldingPrice < 0)
            { Price = selectedContent.FlatRate > 0 ? selectedContent.FlatRate.ToString() : string.Empty; }
            else { price = CommaDelimitedAmount(HoldingPrice); }

            if (SelectedAccountingSubject == null) { return; }

            if (IsSupplementVisiblity)
            {
                int i = DateTime.Now.Month < 4 ? DateTime.Now.Year - 1 : DateTime.Now.Year;

                Supplement = SelectedAccountingSubject.Number == 26 ?
                    $"{i}年度分" :
                    $"{i},{i + 1}年度分";
                SetDetailTitle("支払者名", $"必ず「{SupplementRequiredString}」の文字を入力");
            }
            else { SetDetailTitle("その他詳細", string.Empty); }

            CreditDept creditDept = DataBaseConnect.CallContentDefaultCreditDept(SelectedContent);
            if (creditDept == null)
            {
                ComboCreditDeptText = string.Empty;
                SelectedCreditDept = null;
            }
            else
            {
                SelectedCreditDept = DataBaseConnect.CallCreditDept(creditDept.ID);
                ComboCreditDeptText = SelectedCreditDept.Dept;
            }
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
                detailText = value.Replace('　', ' ');
                CallPropertyChanged();
                SetDataOperationButtonEnabled();
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
                if (!value) { Supplement = string.Empty; }
                ValidationProperty(nameof(IsSupplementVisiblity), value);
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
                if (!value) { IsReducedTaxRate = false; }
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
            SlipOutputDate = value
                ? ReceiptsAndExpenditureOperation.GetInstance().Data.OutputDate == DefaultDate ?
                        DateTime.Today : ReceiptsAndExpenditureOperation.GetInstance().Data.OutputDate
                : DefaultDate;
            SlipOutputDateTitle = slipOutputDate == DefaultDate ? "伝票出力日（未出力）" : "伝票出力日";
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
        /// 勘定科目のEnabled
        /// </summary>
        public bool IsAccountingSubjectEnabled
        {
            get => isAccountingSubjectEnabled;
            set
            {
                isAccountingSubjectEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 伝票内容のEnabled
        /// </summary>
        public bool IsContentEnabled
        {
            get => isContentEnabled;
            set
            {
                isContentEnabled = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// データ操作ボタンのEnabledを設定します
        /// </summary>
        public void SetDataOperationButtonEnabled()
        {
            IsDataOperationButtonEnabled = CanOperation();
        }

        /// <summary>
        /// 出納データ操作時の必須のフィールドにデータが入力されているかを確認し、判定結果を返します
        /// </summary>
        /// <returns>判定結果</returns>
        private bool CanOperation()
        {
            //必要なフィールドに値が入っているか
            bool b = !HasErrors && !string.IsNullOrEmpty(ComboCreditDeptText) &
            !string.IsNullOrEmpty(ComboContentText) &
            !string.IsNullOrEmpty(ComboAccountingSubjectText) &
            !string.IsNullOrEmpty(ComboAccountingSubjectCode) &
            (!string.IsNullOrEmpty(Price) && 0 != IntAmount(price) && IntAmount(price) != -1);

            OperationData = ReceiptsAndExpenditureOperation.GetInstance().Data;

            if (!b) { return false; }
            //OperationData.Dataがnullの時は登録なのでtrueを返す
            if (OperationData == null) { return true; }
            //ここまでの条件で伝票出力されていなければtrueを返す
            if (OperationData.OutputDate == DefaultDate) { return true; }
            //伝票出力されたのが今月中ならtrueを返す
            if (OperationData.OutputDate.Month == DateTime.Today.Month) { return true; }
            //伝票出力日が今年度中かをbに代入
            b = IsInCorrectionDeadline();
            //今年度以前ならfalseを返す
            if (!b)
            {
                DataOperationButtonContent =
                    "更新は管理者権限所有者が、今年度中の出力データでのみ許可されます";
                return false;
            }

            b = IsExceptMonthUpdate();

            if (b) 
            {
                DataOperationButtonContent = LoginRep.GetInstance().Rep.IsAdminPermisson ?
                    "更新" : "先月以前のデータは管理者権限をお持ちでないので更新できません";
                b = LoginRep.GetInstance().Rep.IsAdminPermisson; 
            }
            else { DataOperationButtonContent = "訂正期限が過ぎています。"; }

            return b;

            bool IsInCorrectionDeadline()
            {
                //未出力はTrue
                if (SlipOutputDate == DefaultDate) { return true; }
                //今年度以前は今日が4月20日以前かで判断
                if (OperationData.OutputDate < CurrentFiscalYearFirstDate)
                { return DateTime.Today < CurrentFiscalYearFirstDate.AddDays(20); }

                return DateTime.Today > CurrentFiscalYearFirstDate.AddDays(-1);
            }

            bool IsExceptMonthUpdate()
            {
                if (IsInCorrectionDeadline()) { CreateConfirmationMessage(); }

                return IsInCorrectionDeadline();

                void CreateConfirmationMessage()
                {
                    if (IsInfoThrough) { return; }
                    MessageBox = new MessageBoxInfo()
                    {
                        Message = $"今月以外のデータの変更は、出納帳に影響を及ぼします。\r\n" +
                            $"変更後は必ず変更した月の出納帳を出力し、経理責任者に訂正を申し出て下さい。",
                        Image = System.Windows.MessageBoxImage.Information,
                        Button = System.Windows.MessageBoxButton.OK,
                        Title = "変更する際の注意"
                    };
                    CallShowMessageBox = true;
                }
            }
        }

        public void ReceiptsAndExpenditureOperationNotify() 
        { if (!IsPassDataNotifyProcess) { SetReceiptsAndExpenditureProperty(); }}

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(ComboContentText):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(ComboAccountingSubjectText):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(ComboAccountingSubjectCode):
                    SetNullOrEmptyError(propertyName, value);
                    string s = (string)value;
                    ErrorsListOperation(s.Length != 3, propertyName, "コードは3桁で入力してください");
                    break;
                case nameof(Price):
                    SetNullOrEmptyError(propertyName, value);
                    break;
                case nameof(ComboCreditDeptText):
                    SetNullOrEmptyError(propertyName, value);
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        ErrorsListOperation
                            (SelectedCreditDept == null, propertyName, "貸方部門をリストから選択してください");
                    }
                    break;
                case nameof(Supplement):
                    if (ComboContentText.Contains("管理料"))
                    {
                        SetNullOrEmptyError(propertyName, value);
                    }

                    if (GetErrors(propertyName) == null)
                    {
                        SetErrorSupplementRequiredStringContains();
                    }
                    break;
                case nameof(IsSupplementVisiblity):
                    if (!(bool)value)
                    {
                        ErrorsListOperation(!string.IsNullOrEmpty(Supplement), nameof(Supplement),
                            "VisiblityがFalseの場合は空文字にしてください。");
                    }
                    else { SetErrorSupplementRequiredStringContains(); }
                    break;
                default:
                    break;
            }

            void SetErrorSupplementRequiredStringContains()
            {
                ErrorsListOperation(!Supplement.Contains(SupplementRequiredString),
                    nameof(Supplement), $"{SupplementRequiredString}を入力してください");
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
        }

        protected override void SetDataOperationButtonContent(DataOperation operation)
        {
            DataOperationButtonContent = operation.ToString();
        }

        protected override void SetDataList()
        {
            OperationData = ReceiptsAndExpenditureOperation.GetInstance().Data;

            ComboContents = new ObservableCollection<Content>();
            ComboAccountingSubjectCodes =
                DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty,
                    AccountingProcessLocation.IsAccountingGenreShunjuen, OperationData == null);
            ComboAccountingSubjects =
                DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty,
                    AccountingProcessLocation.IsAccountingGenreShunjuen, OperationData == null);
            ComboCreditDepts = DataBaseConnect.ReferenceCreditDept
                (string.Empty, OperationData == null,
                    AccountingProcessLocation.IsAccountingGenreShunjuen);
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

        public bool CancelClose() => !CanClose;
    }
}
