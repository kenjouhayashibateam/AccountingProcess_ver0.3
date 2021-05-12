using Domain.Entities;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.ObjectModel;
using Domain.Repositories;
using static Domain.Entities.Helpers.TextHelper;
using Infrastructure;
using System.Collections.Generic;
using WPF.ViewModels.Commands;
using System.Threading.Tasks;
using WPF.Views.Datas;
using System.Windows;
using WPF.ViewModels.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// お布施一覧登録ViewModel
    /// </summary>
    public class CondolenceOperationViewModel:DataOperationViewModel,ICondolenceObserver
    {
        #region Properties
        #region Strings
        private string carTip = string.Empty;
        private string ownerName = string.Empty;
        private string almsgiving = string.Empty;
        private string mealTip = string.Empty;
        private string carAndMealTip = string.Empty;
        private string socialGathering = string.Empty;
        private string totalAmount = string.Empty;
        private string contentText = string.Empty;
        private string soryoName = string.Empty;
        private string note = string.Empty;
        private string dataOperationButtonContent;
        private string counterReceiver;
        private string mailRepresentative;
        /// <summary>
        /// 検索する勘定科目コード
        /// </summary>
        private string SearchAccountingSubjectCode { get; set; } = string.Empty;
        private string searchGanreContent;
        #endregion
        #region Bools
        private bool isMemorialService;
        private bool isAlmsgivingSearch = true;
        private bool isTipSearch;
        private bool isSocalGatheringSearch;
        private bool isOperationButtonEnabled;
        #endregion
        private Dictionary<int, string> soryoList;
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures;
        private DateTime receiptsAndExpenditureSearchDate=DefaultDate;
        private DateTime accountActivityDate;
        private ReceiptsAndExpenditure selectedReceiptsAndExpenditure;
        private readonly CondolenceOperation condolenceOperation;
        private Condolence OperationCondolence;
        private int ID { get; set; }
        private Pagination pagination;
        #endregion

        public CondolenceOperationViewModel(IDataBaseConnect dataBaseConnect):base(dataBaseConnect)
        {
            condolenceOperation = CondolenceOperation.GetInstance();
            condolenceOperation.Add(this);
            Pagination = new Pagination();
            ReceiptsAndExpenditureSearchDate = DateTime.Today;
            IsMemorialService = true;
            if(condolenceOperation.GetData()==null)
            {
                SetDataRegistrationCommand.Execute();
                FieldClear();
            }
            else
            {
                SetDataUpdateCommand.Execute();
                SetProperty();
            }
        }
        public CondolenceOperationViewModel() : 
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 懇志検索コマンド
        /// </summary>
        public DelegateCommand SocialGatheringSearchCommand { get; set; }
        private void SocialGatheringSearch()
        {
            IsSocalGatheringSearch = true;
            SearchAccountingSubjectCode = "831";
            SetReceiptsAndExpenditures(true);
        }
        /// <summary>
        /// 志納金検索コマンド
        /// </summary>
        public DelegateCommand TipSearchCommand { get; set; }
        private void TipSearch()
        {
            IsTipSearch = true;
            SearchAccountingSubjectCode = "832";
            SetReceiptsAndExpenditures(true);
        }
        /// <summary>
        /// 御布施検索コマンド
        /// </summary>
        public DelegateCommand AlmsgivingSearchCommand { get; set; }
        private void AlmsgivingSearch()
        {
            IsAlmsgivingSearch = true;
            SearchAccountingSubjectCode = "815";
            SetReceiptsAndExpenditures(true);
        }
        /// <summary>
        /// 前の10件を表示するコマンド
        /// </summary>
        public DelegateCommand PrevPageListExpressCommand { get; set; }
        private void PrevPageListExpress()
        {
            if(Pagination.CanPageCountSubtractAndCanPrevPageExpress()) SetReceiptsAndExpenditures(false);
        }
        /// <summary>
        /// 次の10件を表示するコマンド
        /// </summary>
        public DelegateCommand NextPageListExpressCommand { get; set; }
        private void NextPageListExpress()
        {
            if(Pagination.CanPageCountAddAndCanNextPageExpress()) SetReceiptsAndExpenditures(false);
        }
        /// <summary>
        /// プロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            Condolence condolence = condolenceOperation.GetData();
            ID = condolence.ID;
            AccountActivityDate = condolence.AccountActivityDate;
            OwnerName = condolence.OwnerName;
            IsMemorialService = condolence.IsMemorialService;
            SoryoName = condolence.SoryoName;
            Almsgiving = AmountWithUnit(condolence.Almsgiving);
            CarTip = AmountWithUnit(condolence.CarTip);
            MealTip = AmountWithUnit(condolence.MealTip);
            CarAndMealTip = AmountWithUnit(condolence.CarAndMealTip);
            SocialGathering = AmountWithUnit(condolence.SocialGathering);
            Note = condolence.Note;
            CounterReceiver = condolence.CounterReceiver;
            MailRepresentative = condolence.MailRepresentative;
        }
        /// <summary>
        /// プロパティをクリアします
        /// </summary>
        private void FieldClear()
        {
            ID = 0;
            AccountActivityDate = DefaultDate;
            OwnerName = string.Empty;
            SoryoName = string.Empty;
            Almsgiving = string.Empty;
            CarTip = string.Empty;
            MealTip = string.Empty;
            CarAndMealTip = string.Empty;
            SocialGathering = string.Empty;
            Note = string.Empty;
            CounterReceiver = string.Empty;
            MailRepresentative = string.Empty;
        }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand OperationDataCommand { get; set; }
        private void OperationData()
        {
            OperationCondolence = new Condolence
                (ID, OwnerName, GetFirstName(SoryoName), isMemorialService, IntAmount(Almsgiving),
                    IntAmount(CarTip), IntAmount(MealTip), IntAmount(CarAndMealTip), IntAmount(SocialGathering),
                    Note, AccountActivityDate, CounterReceiver, MailRepresentative);
            
            switch(CurrentOperation)
            {
                case DataOperation.更新:
                    DataUpdate();
                    break;
                case DataOperation.登録:
                    DataRegistration();
                    break;
            }
        }
        /// <summary>
        /// データを更新します
        /// </summary>
        private async void DataUpdate()
        {
            IsOperationButtonEnabled = false;
            string updateContent=string.Empty;
            Condolence condolence = condolenceOperation.GetData();

            if (OperationCondolence.AccountActivityDate != condolence.AccountActivityDate)
                updateContent +=
                    $"入金日{Space}:{Space}{condolence.AccountActivityDate.ToShortDateString()}" +
                    $"{Space}→{Space}{OperationCondolence.AccountActivityDate.ToShortDateString()}\r\n";

            if (OperationCondolence.OwnerName != condolence.OwnerName)
                updateContent +=
                    $"施主名{Space}:{Space}{condolence.OwnerName}{Space}→{Space}" +
                    $"{OperationCondolence.OwnerName}\r\n";

            if (condolence.SoryoName != OperationCondolence.SoryoName)
                updateContent +=
                    $"担当僧侶{Space}:{Space}{condolence.SoryoName}{Space}→{Space}" +
                    $"{OperationCondolence.SoryoName}\r\n";

            if (condolence.IsMemorialService != OperationCondolence.IsMemorialService)
                updateContent +=
                    $"内容{Space}:{Space}{(condolence.IsMemorialService ? "法事" : "葬儀")}{Space}→{Space}" +
                    $"{(OperationCondolence.IsMemorialService ? "法事" : "葬儀")}\r\n";

            if (condolence.CarTip != OperationCondolence.CarTip)
                updateContent +=
                    $"御車代{Space}:{Space}{AmountWithUnit(condolence.CarTip)}{Space}→{Space}" +
                    $"{AmountWithUnit(OperationCondolence.CarTip)}\r\n";

            if (condolence.MealTip != OperationCondolence.MealTip)
                updateContent +=
                    $"御膳料{Space}:{Space}{AmountWithUnit(condolence.MealTip)}{Space}→{Space}" +
                    $"{AmountWithUnit(OperationCondolence.MealTip)}\r\n";

            if (condolence.CarAndMealTip != OperationCondolence.CarAndMealTip)
                updateContent +=
                    $"御車代御膳料{Space}:{Space}{AmountWithUnit(condolence.CarAndMealTip)}{Space}→" +
                    $"{Space}{AmountWithUnit(OperationCondolence.CarAndMealTip)}\r\n";

            if (condolence.SocialGathering != OperationCondolence.SocialGathering)
                updateContent +=
                    $"懇志{Space}:{Space}{AmountWithUnit(condolence.SocialGathering)}{Space}→" +
                    $"{Space}{AmountWithUnit(OperationCondolence.SocialGathering)}\r\n";

            if (condolence.Note != OperationCondolence.Note)
                updateContent +=
                    $"備考{Space}:{Space}{condolence.Note}{Space}→{Space}{OperationCondolence.Note}\r\n";

            if (condolence.CounterReceiver != OperationCondolence.CounterReceiver)
                updateContent +=
                    $"窓口受付者{Space}:{Space}{condolence.CounterReceiver}{Space}→" +
                    $"{Space}{OperationCondolence.CounterReceiver}\r\n";

            if (condolence.MailRepresentative != OperationCondolence.MailRepresentative)
                updateContent +=
                    $"郵送担当者{Space}:{Space}{condolence.MailRepresentative}{Space}→" +
                    $"{Space}{OperationCondolence.MailRepresentative}\r\n";

            if(string.IsNullOrEmpty(updateContent))
            {
                CallNoRequiredUpdateMessage();
                return;
            }

            if(CallConfirmationDataOperation($"{updateContent}\r\n\r\n更新しますか？","御布施一覧データ")
                ==MessageBoxResult.Cancel)
            {
                SetProperty();
                IsOperationButtonEnabled = true;
                return;
            }

            DataOperationButtonContent = "更新中";
            await Task.Run(() => DataBaseConnect.Update(OperationCondolence));
            condolenceOperation.SetData(OperationCondolence);
            CallCompletedUpdate();
            DataOperationButtonContent = "更新";
            isOperationButtonEnabled = true;
        }
        /// <summary>
        /// データを登録します
        /// </summary>
        private async void DataRegistration()
        {         
            string content=OperationCondolence.IsMemorialService?"法事":"葬儀";
            
            IsOperationButtonEnabled = false;
            
            MessageBox = new MessageBoxInfo()
            {

                Message =
                    $"日付\t\t:{Space}{OperationCondolence.AccountActivityDate.ToShortDateString()}\r\n" +
                    $"施主名\t\t:{Space}{OperationCondolence.OwnerName}\r\n" +
                    $"担当僧侶\t\t:{Space}{OperationCondolence.SoryoName}\r\n" +
                    $"内容\t\t:{Space}{content}\r\n" +
                    $"合計金額\t\t:{Space}{TotalAmount}\r\n" +
                    $"御布施\t\t:{Space}{Almsgiving}\r\n" +
                    $"御車代\t\t:{Space}{CarTip}\t\n" +
                    $"御膳料\t\t:{Space}{MealTip}\t\n" +
                    $"御車代御膳料\t:{Space}{CarAndMealTip}\t\n" +
                    $"懇志\t\t;{Space}{SocialGathering}\r\n" +
                    $"備考\t\t:{Space}{Note}\r\n" +
                    $"窓口受付者\t\t:{Space}{CounterReceiver}\r\n" +
                    $"郵送担当者\t\t:{Space}{MailRepresentative}\r\n\r\n登録しますか？",
                Button = MessageBoxButton.OKCancel,
                Image = MessageBoxImage.Question,
                Title = "登録確認"
            };

            if (MessageBox.Result == MessageBoxResult.Cancel) return;

            DataOperationButtonContent = "登録中";
            LoginRep loginRep = LoginRep.GetInstance();
            await Task.Run(() => DataBaseConnect.Registration(OperationCondolence));
            condolenceOperation.SetData(OperationCondolence);
            condolenceOperation.Notify();
            FieldClear();
            CallCompletedRegistration();
            IsOperationButtonEnabled = true;
            DataOperationButtonContent = "登録";
        }
        /// <summary>
        /// 出納データの情報を登録データに入力するコマンド
        /// </summary>
        public DelegateCommand InputPropertyCommand { get; set; }
        private async void InputProperty()
        {
            await Task.Delay(1);
            AccountActivityDate = SelectedReceiptsAndExpenditure.AccountActivityDate;
            OwnerName = SelectedReceiptsAndExpenditure.Detail;

            switch(SearchAccountingSubjectCode)
            {
                case "815":
                    Almsgiving = SelectedReceiptsAndExpenditure.PriceWithUnit;
                    break;
                case "832":
                    SetAmount();
                    break;
                case "831":
                    SocialGathering = SelectedReceiptsAndExpenditure.PriceWithUnit;
                    break;
            }
        }

        private void SetAmount()
        {
            switch(SelectedReceiptsAndExpenditure.Content.Text)
            {
                case "御車代":
                    CarTip = SelectedReceiptsAndExpenditure.PriceWithUnit;
                    break;
                case "御膳料":
                    MealTip = SelectedReceiptsAndExpenditure.PriceWithUnit;
                    break;
                case "御車代御膳料":
                    CarAndMealTip = SelectedReceiptsAndExpenditure.PriceWithUnit;
                    break;
            }
        }
        private void SetReceiptsAndExpenditures(bool isCountReset)
        {
            Pagination.CountReset(isCountReset);

            var(totalRow,list) =
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                    (DefaultDate, DateTime.Today, string.Empty, "法務部", string.Empty, string.Empty,
                        string.Empty, SearchAccountingSubjectCode, true, true, true, true, 
                        ReceiptsAndExpenditureSearchDate, receiptsAndExpenditureSearchDate, DefaultDate,
                        DateTime.Today,Pagination.PageCount);
            ReceiptsAndExpenditures = list;
            Pagination.TotalRowCount = totalRow;
            Pagination.SetProperty();
        }
        /// <summary>
        /// 出納データリスト
        /// </summary>
        public ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures
        {
            get => receiptsAndExpenditures;
            set 
            {
                receiptsAndExpenditures = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索日時
        /// </summary>
        public DateTime ReceiptsAndExpenditureSearchDate
        {
            get => receiptsAndExpenditureSearchDate; 
            set
            {
                receiptsAndExpenditureSearchDate = value;
                SetReceiptsAndExpenditures(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された出納データ
        /// </summary>
        public ReceiptsAndExpenditure SelectedReceiptsAndExpenditure
        {
            get => selectedReceiptsAndExpenditure;
            set
            {
                selectedReceiptsAndExpenditure = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する日時
        /// </summary>
        public DateTime AccountActivityDate
        {
            get => accountActivityDate;
            set
            {
                accountActivityDate = value;
                ValidationProperty(nameof(AccountActivityDate), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する御車代
        /// </summary>
        public string CarTip
        {
            get => carTip;
            set
            {
                carTip = value;
                SetTotalAmount();
                CallPropertyChanged();
            }
        }
        /// <summary> 
        /// 登録する施主名
        /// </summary>
        public string OwnerName
        {
            get => ownerName;
            set
            {
                if(ConfirmationOwnerName(ownerName,value)==MessageBoxResult.Yes) ownerName = value;
                ValidationProperty(nameof(OwnerName), value);
                CallPropertyChanged();
            }
        }
        private MessageBoxResult ConfirmationOwnerName(string oldValue,string newValue)
        {
            if (oldValue == newValue) return MessageBoxResult.Yes;
            if (string.IsNullOrEmpty(newValue)) return MessageBoxResult.Yes;
            if (string.IsNullOrEmpty(oldValue)) return MessageBoxResult.Yes;

            MessageBox = new MessageBoxInfo()
            {
                Message = $"施主名が違います。\r\n" +
                    $"旧施主名{Space}:{Space}{oldValue}\t新施主名{Space}:{Space}{newValue}\r\n\r\n" +
                    $"よろしいですか？",
                Image = MessageBoxImage.Question,
                Button = MessageBoxButton.YesNo,
                Title = "施主名確認"
            };
            return MessageBox.Result;
        }
        /// <summary>
        /// 登録するお布施
        /// </summary>
        public string Almsgiving
        {
            get => almsgiving;
            set
            {
                almsgiving = value;
                SetTotalAmount();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する御膳料
        /// </summary>
        public string MealTip
        {
            get => mealTip;
            set
            {
                mealTip = value;
                SetTotalAmount();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する御車代御膳料
        /// </summary>
        public string CarAndMealTip
        {
            get => carAndMealTip;
            set
            {
                carAndMealTip = value;
                SetTotalAmount();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する合計金額
        /// </summary>
        public string TotalAmount
        {
            get => totalAmount;
            set
            {
                totalAmount = value;
                ValidationProperty(nameof(TotalAmount), value);
                CallPropertyChanged();
            }
        }
        private void SetTotalAmount()
        {
            TotalAmount =
                AmountWithUnit(IntAmount(Almsgiving) + IntAmount(CarTip) + IntAmount(MealTip) +
                    IntAmount(CarAndMealTip) + IntAmount(SocialGathering));
        }
        /// <summary>
        /// 内容が法事かのチェック　Trueが法事、Falseが葬儀
        /// </summary>
        public bool IsMemorialService
        {
            get => isMemorialService;
            set
            {
                isMemorialService = value;
                ContentText = value ? "法事" : "葬儀";
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 内容のText
        /// </summary>
        public string ContentText
        {
            get => contentText;
            set
            {
                contentText = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 担当僧侶リスト
        /// </summary>
        public Dictionary<int, string> SoryoList
        {
            get => soryoList;
            set
            {
                soryoList = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する僧侶名
        /// </summary>
        public string SoryoName
        {
            get => soryoName;
            set
            {
                soryoName = value;
                ValidationProperty(nameof(SoryoName), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する備考
        /// </summary>
        public string Note
        {
            get => note;
            set
            {
                note = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作ボタン
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
        /// 御布施検索チェック
        /// </summary>
        public bool IsAlmsgivingSearch
        {
            get => isAlmsgivingSearch;
            set
            {
                isAlmsgivingSearch = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索するジャンルのContent
        /// </summary>
        public string SearchGanreContent
        {
            get => searchGanreContent;
            set
            {
                searchGanreContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// データ操作ボタンのEnable
        /// </summary>
        public bool IsOperationButtonEnabled
        {
            get => isOperationButtonEnabled;
            set
            {
                isOperationButtonEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 窓口受付者
        /// </summary>
        public string CounterReceiver
        {
            get => counterReceiver;
            set
            {
                counterReceiver = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 郵送担当者
        /// </summary>
        public string MailRepresentative
        {
            get => mailRepresentative;
            set
            {
                mailRepresentative = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ページネーション
        /// </summary>
        public Pagination Pagination
        {
            get => pagination;
            set
            {
                pagination = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録する懇志
        /// </summary>
        public string SocialGathering
        {
            get => socialGathering;
            set
            {
                socialGathering = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 志納金検索チェック
        /// </summary>
        public bool IsTipSearch
        {
            get => isTipSearch;
            set
            {
                isTipSearch = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 懇志検索チェック
        /// </summary>
        public bool IsSocalGatheringSearch
        {
            get => isSocalGatheringSearch;
            set
            {
                isSocalGatheringSearch = value;
                CallPropertyChanged();
            }
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
            switch(propertyName)
            {
                case nameof(OwnerName):
                    SetNullOrEmptyError(propertyName, (string)value);
                    break;
                case nameof(SoryoName):
                    SetNullOrEmptyError(propertyName, (string)value);
                    break;
                case nameof(TotalAmount):
                    SetNullOrEmptyError(propertyName, (string)value);
                    if (GetErrors(propertyName) == null)
                        ErrorsListOperation
                            (IntAmount((string)value) == 0, propertyName, "金額が入力されていません");
                    break;
            }
            SetIsOperationButtonEnabled();
        }

        private void SetIsOperationButtonEnabled() =>
            IsOperationButtonEnabled = !string.IsNullOrEmpty(OwnerName) &&
                !string.IsNullOrEmpty(SoryoName) && !string.IsNullOrEmpty(TotalAmount) && 
                IntAmount(TotalAmount) != 0;

        protected override void SetDataList()
        {
            SoryoList = DataBaseConnect.GetSoryoList();
        }

        protected override void SetDataOperationButtonContent(DataOperation operation) =>
            DataOperationButtonContent = operation.ToString();

        protected override void SetDelegateCommand()
        {
            InputPropertyCommand = new DelegateCommand(() => InputProperty(), () => true);
            OperationDataCommand = new DelegateCommand(() => OperationData(), () => true);
            PrevPageListExpressCommand = new DelegateCommand(() => PrevPageListExpress(), () => true);
            NextPageListExpressCommand = new DelegateCommand(() => NextPageListExpress(), () => true);
            AlmsgivingSearchCommand = new DelegateCommand(() => AlmsgivingSearch(), () => true);
            TipSearchCommand = new DelegateCommand(() => TipSearch(), () => true);
            SocialGatheringSearchCommand = new DelegateCommand(() => SocialGatheringSearch(), () => true);
        }

        protected override void SetDetailLocked() { }

        protected override void SetWindowDefaultTitle() =>
            WindowTitle = $"お布施一覧データ出力 : {AccountingProcessLocation.Location}";

        public void CondolenceNotify() => FieldClear();
    }
}
