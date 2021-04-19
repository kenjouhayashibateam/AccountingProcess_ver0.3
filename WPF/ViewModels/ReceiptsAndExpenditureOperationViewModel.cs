﻿using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;
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
        #endregion
        #region Bools
        private bool isValidity;
        private bool isReducedTaxRate;
        private bool isOutput;
        private bool isDataOperationButtonEnabled;
        private bool isPaymentCheck;
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
        #endregion

        public ReceiptsAndExpenditureOperationViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            OperationData = ReceiptsAndExpenditureOperation.GetInstance();
            SetDetailText();
            OperationData.Add(this);
            if (OperationData.Data != null)
            {
                SetDataUpdateCommand.Execute();
                SetReceiptsAndExpenditureProperty();
            }
            else
            {
                SetDataRegistrationCommand.Execute();
                FieldClear();
            } 
        }
        public ReceiptsAndExpenditureOperationViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 詳細テキストブロックのタイトルを設定します
        /// </summary>
        private void SetDetailText()
        {
            switch(OperationData.GetOperationType())
            {
                case ReceiptsAndExpenditureOperation.OperationType.ReceiptsAndExpenditure:
                    OtherDescription = "その他詳細";
                    break;
                case ReceiptsAndExpenditureOperation.OperationType.Voucher:
                    OtherDescription = "宛名";
                    break;
            }
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
        private void DataUpdate()
        {
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
                    $"入出金日 : {OperationData.Data.AccountActivityDate} → {AccountActivityDate}\r\n";

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

            if (OperationData.Data.Detail != DetailText)
                UpdateCotent += $"詳細 : {OperationData.Data.Detail} → {DetailText}\r\n";

            if (OperationData.Data.Price != IntAmount(price))
                UpdateCotent += $"金額 : {AmountWithUnit(OperationData.Data.Price)} → " +
                    $"{AmountWithUnit(IntAmount(Price))}\r\n";

            if (OperationData.Data.IsValidity != IsValidity)
                UpdateCotent += $"有効性 : {OperationData.Data.IsValidity} → {IsValidity}\r\n";

            if (OperationData.Data.OutputDate != SlipOutputDate)
                UpdateCotent += $"出力日 : {OperationData.Data.OutputDate} → {SlipOutputDate}\r\n";

            if (OperationData.Data.IsReducedTaxRate != IsReducedTaxRate)
                UpdateCotent +=
                    $"軽減税率データ : {OperationData.Data.IsReducedTaxRate} → {IsReducedTaxRate}\r\n";

            if (UpdateCotent.Length == 0)
            {
                CallNoRequiredUpdateMessage();
                return;
            }

            if (CallConfirmationDataOperation
                ($"{UpdateCotent}\r\n\r\n更新しますか？", "伝票") ==
                System.Windows.MessageBoxResult.Cancel)
            {
                SetReceiptsAndExpenditureProperty();
                return;
            }

            ReceiptsAndExpenditure updateData =
                new ReceiptsAndExpenditure
                (ReceiptsAndExpenditureIDField, RegistrationDate, OperationRep,
                OperationData.Data.Location, SelectedCreditDept, SelectedContent, DetailText,
                IntAmount(price), IsPaymentCheck, IsValidity, AccountActivityDate, SlipOutputDate, 
                IsReducedTaxRate);

            DataBaseConnect.Update(updateData);
            OperationData.SetData(updateData);
            OperationData.Notify();
            MessageBox = new MessageBoxInfo
            {
                Button = System.Windows.MessageBoxButton.OK,
                Image = System.Windows.MessageBoxImage.Information,
                Title = "更新完了",
                Message = "更新しました"
            };
            
            CallShowMessageBox = true;
        }
        /// <summary>
        /// 出納データを登録します
        /// </summary>
        private void DataRegistration()
        {
            ReceiptsAndExpenditure rae =
                new ReceiptsAndExpenditure
                (0, DateTime.Now, LoginRep.Rep, AccountingProcessLocation.Location,
                SelectedCreditDept, SelectedContent, DetailText, IntAmount(price), IsPaymentCheck,
                IsValidity, AccountActivityDate, DefaultDate, IsReducedTaxRate);

            string depositAndWithdrawalText = IsPaymentCheck ? "入金" : "出金";
            if (CallConfirmationDataOperation
                ($"経理担当場所\t : {rae.Location}\r\n" +
                    $"入出金日\t\t : {rae.AccountActivityDate.ToShortDateString()}\r\n" +
                    $"入出金\t\t : {depositAndWithdrawalText}\r\n" +
                    $"貸方勘定\t\t : {rae.CreditDept.Dept}\r\n" +
                    $"コード\t\t : {rae.Content.AccountingSubject.SubjectCode}\r\n" +
                    $"勘定科目\t\t : {rae.Content.AccountingSubject.Subject}\r\n" +
                    $"内容\t\t : {rae.Content.Text}\r\n" +
                    $"詳細\t\t : {rae.Detail}\r\n金額\t\t : {AmountWithUnit(rae.Price)}\r\n" +
                    $"軽減税率\t\t : {rae.IsReducedTaxRate}\r\n" +
                    $"有効性\t\t : {rae.IsValidity}\r\n\r\n登録しますか？", "伝票")
                == System.Windows.MessageBoxResult.Cancel) return;

            DataBaseConnect.Registration(rae);
            OperationData.SetData(rae);
            OperationData.Notify();
            MessageBox = new MessageBoxInfo
            {
                Button = System.Windows.MessageBoxButton.OK,
                Image = System.Windows.MessageBoxImage.Information,
                Title = "登録完了",
                Message = "登録しました"
            };
            CallShowMessageBox = true;

            FieldClear();
        }
        /// <summary>
        /// フィールドの値をクリアします
        /// </summary>
        private void FieldClear()
        {
            IsValidity = true;
            IsPaymentCheck = true;
            ComboAccountingSubjectText = string.Empty;
            SelectedAccountingSubject = null;
            ComboAccountingSubjectCode = string.Empty;
            ComboContentText = string.Empty;
            DetailText = string.Empty;
            Price = string.Empty;
            AccountActivityDate = DateTime.Today;
            RegistrationDate = DateTime.Today;
            LoginRep loginRep = LoginRep.GetInstance();
            OperationRep = loginRep.Rep;
            SelectedCreditDept = ComboCreditDepts[0];
            ComboCreditDeptText = SelectedCreditDept.Dept;
        }
        /// <summary>
        /// フィールドにプロパティをセットします
        /// </summary>
        private void SetReceiptsAndExpenditureProperty()
        {
            ReceiptsAndExpenditureIDField = OperationData.Data.ID;
            IsValidity = OperationData.Data.IsValidity;
            IsPaymentCheck = OperationData.Data.IsPayment;
            SlipOutputDate = OperationData.Data.OutputDate;
            IsOutput = OperationData.Data.OutputDate != DefaultDate;
            SelectedCreditDept = OperationData.Data.CreditDept;
            ComboCreditDeptText = OperationData.Data.CreditDept.Dept;
            SelectedAccountingSubjectCode = OperationData.Data.Content.AccountingSubject;
            ComboAccountingSubjectCode = OperationData.Data.Content.AccountingSubject.SubjectCode;
            ComboAccountingSubjectText = OperationData.Data.Content.AccountingSubject.Subject;
            ComboContentText = OperationData.Data.Content.Text;
            DetailText = OperationData.Data.Detail;
            Price = OperationData.Data.Price.ToString();
            AccountActivityDate = OperationData.Data.AccountActivityDate;
            RegistrationDate = OperationData.Data.RegistrationDate;
            IsReducedTaxRate = OperationData.Data.IsReducedTaxRate;
            OperationRep = OperationData.Data.RegistrationRep;
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
                if (string.IsNullOrEmpty(value)) value = comboCreditDeptText;
                SelectedCreditDept = ComboCreditDepts.FirstOrDefault(c => c.Dept == value);
                if (SelectedCreditDept == null) comboCreditDeptText = string.Empty;
                else comboCreditDeptText = SelectedCreditDept.Dept;

                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboCreditDeptText), value);
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
                if (selectedCreditDept != null && selectedCreditDept.Equals(value)) return;
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
                SetDataOperationButtonEnabled();
                if (string.IsNullOrEmpty(value))
                {
                    ComboAccountingSubjects.Clear();
                    ComboAccountingSubjectText = string.Empty;
                }
                else ComboAccountingSubjects = 
                        DataBaseConnect.ReferenceAccountingSubject(value, string.Empty, true);

                if (ComboAccountingSubjects.Count > 0)
                {
                    comboAccountingSubjectCode = ComboAccountingSubjects[0].SubjectCode;
                    ComboAccountingSubjectText = ComboAccountingSubjects[0].Subject;
                }
                else
                {
                    comboAccountingSubjectCode = string.Empty;
                    ComboAccountingSubjectText = string.Empty;
                    ComboContentText = string.Empty;
                    ComboContents.Clear();
                }
                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(ComboAccountingSubjectCode), value);
                CallPropertyChanged();
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
                selectedAccountingSubjectCode = value;
                if (value == null) ComboAccountingSubjectCode = string.Empty;
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
                SetDataOperationButtonEnabled();
                if (string.IsNullOrEmpty(value))
                {
                    ComboContents.Clear();
                }
                else ComboContents = 
                        DataBaseConnect.ReferenceContent(string.Empty, string.Empty, value, true);

                if (ComboContents.Count > 0) ComboContentText = ComboContents[0].Text;
                else ComboContentText = string.Empty;
                comboAccountingSubjectText = value;
                ValidationProperty(nameof(ComboAccountingSubjectText), value);
                CallPropertyChanged();
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
                ComboContents = DataBaseConnect.ReferenceContent
                    (string.Empty, ComboAccountingSubjectCode, ComboAccountingSubjectText, true);
                if (ComboContents.Count > 0) ComboContentText =
                        ComboContents.Count != 0 ? ComboContents[0].Text : string.Empty;
                else ComboContentText = string.Empty;

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
                comboContentText = value;
                SelectedContent = ComboContents.FirstOrDefault(c => c.Text == comboContentText);
                if (SelectedContent == null) comboContentText = string.Empty;
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
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// Contentを参照して定額、低減税率チェックに値を入力します
        /// </summary>
        private void SetContentProperty()
        {
            if (selectedContent != null && selectedContent.FlatRate > 0)
                Price = selectedContent.FlatRate.ToString();
            else Price = string.Empty;

            IsReducedTaxRate = SelectedContent.Text == "供物";
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
                detailText = value;
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
                SetDataOperationButtonEnabled();
                ValidationProperty(nameof(Price), price);
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
                if (slipOutputDate == DefaultDate) SlipOutputDateTitle = "伝票出力日（見出力）";
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
        /// データ操作ボタンのEnabledを設定します
        /// </summary>
        public void SetDataOperationButtonEnabled() => IsDataOperationButtonEnabled = CanOperation();
        /// <summary>
        /// 出納データ操作時の必須のフィールドにデータが入力されているかを確認し、判定結果を返します
        /// </summary>
        /// <returns>判定結果</returns>
        private bool CanOperation() =>
            !string.IsNullOrEmpty(ComboCreditDeptText) & !string.IsNullOrEmpty(ComboContentText) &
            !string.IsNullOrEmpty(ComboAccountingSubjectText) &
            !string.IsNullOrEmpty(ComboAccountingSubjectCode) &
            !string.IsNullOrEmpty(Price) && 0 < IntAmount(price) & !IsOutput;

        public void Notify()
        {
            SetReceiptsAndExpenditureProperty();
        }

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
                    ErrorsListOperation
                        (SelectedCreditDept == null, propertyName, "貸方部門をリストから選択してください");
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
            if (CurrentOperation == DataOperation.更新)
                ComboCreditDeptText = OperationData.Data.CreditDept.Dept;
            else
            ComboCreditDeptText = ComboCreditDepts[0].Dept;
        }

        protected override void SetDataOperationButtonContent(DataOperation operation) =>
            DataOperationButtonContent = operation.ToString();

        protected override void SetDataList()
        {
            ComboContents =
                DataBaseConnect.ReferenceContent(string.Empty, string.Empty, string.Empty, true);
            ComboAccountingSubjects =
                DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            ComboAccountingSubjectCodes =
                DataBaseConnect.ReferenceAccountingSubject(string.Empty, string.Empty, true);
            ComboCreditDepts = DataBaseConnect.ReferenceCreditDept(string.Empty, true, false);
        }

        protected override void SetDelegateCommand()
        {
            ZeroAddCommand = new DelegateCommand
                (() => ZeroAdd(), () => true);
            ReceiptsAndExpenditureDataOperationCommand = new DelegateCommand
                (() => ReceiptsAndExpenditureDataOperation(), () => true);
        }
    }
}
