using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// お布施一覧登録ViewModel
    /// </summary>
    public class CondolenceOperationViewModel : DataOperationViewModel, ICondolenceObserver,
        IPagenationObserver
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
        private string dataOperationButtonContent = string.Empty;
        private string counterReceiver = string.Empty;
        private string mailRepresentative = string.Empty;
        private string fixToggleContent = string.Empty;
        private string condolenceContent = "法事";
        private string Location = string.Empty;
        /// <summary>
        /// 検索する勘定科目コード
        /// </summary>
        private string SearchAccountingSubjectCode { get; set; } = string.Empty;
        private string searchGanreContent = string.Empty;
        #endregion
        #region Bools
        private bool isAlmsgivingSearch = true;
        private bool isTipSearch = false;
        private bool isSocalGatheringSearch = false;
        private bool isOperationButtonEnabled = false;
        private bool isReceptionBlank = false;
        private bool fixToggle = true;
        private bool isFixToggleEnabled = false;
        private bool isDeleteButtonVisibility = true;
        #endregion
        private Dictionary<int, string> soryoList = new Dictionary<int, string>();
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures =
            new ObservableCollection<ReceiptsAndExpenditure>();
        private DateTime receiptsAndExpenditureSearchDate = DateTime.Today;
        private DateTime accountActivityDate = DefaultDate;
        private ReceiptsAndExpenditure selectedReceiptsAndExpenditure =
            new ReceiptsAndExpenditure(0, DefaultDate, LoginRep.GetInstance().Rep, string.Empty,
                new CreditDept(string.Empty, string.Empty, true, true),
                    new Content(string.Empty, new AccountingSubject(string.Empty, string.Empty, string.Empty, true), 0, string.Empty, true),
                        string.Empty, 0, true, true, DefaultDate, DefaultDate, false);
        private Condolence OperationCondolence = new Condolence(0, string.Empty, string.Empty, string.Empty, string.Empty,
            0, 0, 0, 0, 0, string.Empty, DefaultDate, string.Empty, string.Empty);
        private int ID { get; set; } = 0;
        private Pagination pagination = Pagination.GetPagination();
        public Dictionary<int, string> NoteStrings => new Dictionary<int, string>() { { 0, "佐野商店" }, { 1, "徳島" }, { 2, "緑山メモリアルパーク" } };
        public Dictionary<int, string> ContentStrings => new Dictionary<int, string>() { { 0, "法事" }, { 1, "葬儀" }, { 2, "法名授与" } };
        private readonly LoginRep GetLoginRep = LoginRep.GetInstance();
        #endregion

        public CondolenceOperationViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            if (GetLoginRep.Rep == null) GetLoginRep.SetRep(new Rep(string.Empty, string.Empty, string.Empty, true, true));
            CondolenceOperation.GetInstance().Add(this);
            Pagination = Pagination.GetPagination();
            Pagination.Add(this);
            SetReceiptsAndExpenditures(true);
            AlmsgivingSearchCommand.Execute();
            if (CondolenceOperation.GetInstance().GetData() == null)
            {
                SetDataRegistrationCommand.Execute();
                FieldClear();
                InputProperty();
            }
            else
            {
                SetDataUpdateCommand.Execute();
                SetProperty();
            }
        }
        public CondolenceOperationViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// お布施一覧データ削除コマンド
        /// </summary>
        public DelegateCommand DeleteCondolenceCommand { get; set; }
        private void DeleteCondolence()
        {
            OperationCondolence = new Condolence
                (ID, Location, OwnerName, SoryoName, ContentText, IntAmount(Almsgiving), IntAmount(CarTip), 
                    IntAmount(MealTip), IntAmount(CarAndMealTip), IntAmount(SocialGathering), Note, AccountActivityDate,
                    CounterReceiver, MailRepresentative);
            if (DeleteConfirmation() == MessageBoxResult.No) return;
            DataBaseConnect.DeleteCondolence(ID);
            MessageBox = new MessageBoxInfo()
            {
                Message = "削除しました。このまま新規登録ができます。",
                Image = MessageBoxImage.Information,
                Button = MessageBoxButton.OK,
                Title = "削除完了"
            };
            CallPropertyChanged(nameof(MessageBox));
            CondolenceOperation.GetInstance().Notify();
            SetDataRegistrationCommand.Execute();
            FieldClear();
            InputProperty();
        }
        private  MessageBoxResult DeleteConfirmation()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = $"ID{Space}:{Space}{ID}\r\n{PropertyContentsMessage()}" +
                    $"\r\n\r\nデータを削除します。元に戻せませんがよろしいですか？",
                Image = MessageBoxImage.Question,
                Button = MessageBoxButton.YesNo,
                Title = "削除確認"
            };
            CallPropertyChanged(nameof(MessageBox));
            return MessageBox.Result;
        }
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
            SetReceiptsAndExpenditures(true);
        }
        /// <summary>
        /// プロパティをセットします
        /// </summary>
        private void SetProperty()
        {
            Condolence condolence = CondolenceOperation.GetInstance().GetData();
            ID = condolence.ID;
            if (ID == 0) { SetDataOperation(DataOperation.登録); }
            AccountActivityDate = condolence.AccountActivityDate;
            Location = condolence.Location;
            OwnerName = condolence.OwnerName;
            CondolenceContent = condolence.Content;
            SoryoName = condolence.SoryoName;
            Almsgiving = AmountWithUnit(condolence.Almsgiving);
            CarTip = AmountWithUnit(condolence.CarTip);
            MealTip = AmountWithUnit(condolence.MealTip);
            CarAndMealTip = AmountWithUnit(condolence.CarAndMealTip);
            SocialGathering = AmountWithUnit(condolence.SocialGathering);
            Note = condolence.Note;
            FixToggle = string.IsNullOrEmpty(condolence.MailRepresentative);
            CounterReceiver = condolence.CounterReceiver;
            MailRepresentative = condolence.MailRepresentative;
            IsAlmsgivingSearch = true;
            IsReceptionBlank = string.IsNullOrEmpty(condolence.CounterReceiver) &&
                string.IsNullOrEmpty(condolence.MailRepresentative);
            IsDeleteButtonVisibility = true;
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
            FixToggle = true;
            IsAlmsgivingSearch = true;
            ContentText = ContentStrings[0];
            IsReceptionBlank = false;
            IsDeleteButtonVisibility = false;
        }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand OperationDataCommand { get; set; }
        private void OperationData()
        {
            OperationCondolence = new Condolence
                (ID, AccountingProcessLocation.Location, OwnerName, GetFirstName(SoryoName),
                    ContentText, IntAmount(Almsgiving), IntAmount(CarTip), IntAmount(MealTip),
                    IntAmount(CarAndMealTip), IntAmount(SocialGathering), Note, AccountActivityDate,
                    IsReceptionBlank ? string.Empty : CounterReceiver,
                    IsReceptionBlank ? string.Empty : MailRepresentative);
            
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
            string updateContent = string.Empty;
            Condolence condolence = CondolenceOperation.GetInstance().GetData();

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

            if (condolence.Content != OperationCondolence.Content)
                updateContent +=
                    $"内容{Space}:{Space}{condolence.Content}{Space}→{Space}" +
                    $"{OperationCondolence.Content}\r\n";

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

            if (string.IsNullOrEmpty(updateContent))
            {
                CallNoRequiredUpdateMessage();
                SetProperty();
                return;
            }

            if (CallConfirmationDataOperation($"{updateContent}\r\n\r\n更新しますか？", "御布施一覧データ")
                == MessageBoxResult.Cancel)
            {
                SetProperty();
                IsOperationButtonEnabled = true;
                return;
            }

            DataOperationButtonContent = "更新中";
            await Task.Run(() => DataBaseConnect.Update(OperationCondolence));
            CondolenceOperation.GetInstance().SetData(OperationCondolence);
            CondolenceOperation.GetInstance().Notify();
            CallCompletedUpdate();
            SetProperty();
            DataOperationButtonContent = "更新";
            isOperationButtonEnabled = true;
        }
        /// <summary>
        /// データを登録します
        /// </summary>
        private async void DataRegistration()
        {
            IsOperationButtonEnabled = false;
            
            MessageBox = new MessageBoxInfo()
            {

                Message = $"{PropertyContentsMessage()}\r\n\r\n登録しますか？",
                Button = MessageBoxButton.OKCancel,
                Image = MessageBoxImage.Question,
                Title = "登録確認"
            };

            if (MessageBox.Result == MessageBoxResult.Cancel)
            {
                IsOperationButtonEnabled = true;
                return;
            }

            DataOperationButtonContent = "登録中";
            await Task.Run(() => DataBaseConnect.Registration(OperationCondolence));
            CondolenceOperation.GetInstance().SetData(OperationCondolence);
            CondolenceOperation.GetInstance().Notify();
            FieldClear();
            CallCompletedRegistration();
            IsOperationButtonEnabled = true;
            DataOperationButtonContent = "登録";
        }
        private string PropertyContentsMessage()
        {
            return $"日付\t\t:{Space}{OperationCondolence.AccountActivityDate.ToShortDateString()}\r\n" +
                $"施主名\t\t:{Space}{OperationCondolence.OwnerName}\r\n" +
                $"担当僧侶\t\t:{Space}{OperationCondolence.SoryoName}\r\n" +
                $"内容\t\t:{Space}{OperationCondolence.Content}\r\n" +
                $"合計金額\t\t:{Space}{AmountWithUnit(OperationCondolence.TotalAmount)}\r\n" +
                $"御布施\t\t:{Space}{AmountWithUnit(OperationCondolence.Almsgiving)}\r\n" +
                $"御車代\t\t:{Space}{AmountWithUnit(OperationCondolence.CarTip)}\t\n" +
                $"御膳料\t\t:{Space}{AmountWithUnit(OperationCondolence.MealTip)}\t\n" +
                $"御車代御膳料\t:{Space}{AmountWithUnit(OperationCondolence.CarAndMealTip)}\t\n" +
                $"懇志\t\t;{Space}{AmountWithUnit(OperationCondolence.SocialGathering)}\r\n" +
                $"備考\t\t:{Space}{OperationCondolence.Note}\r\n" +
                $"窓口受付者\t:{Space}{OperationCondolence.CounterReceiver}\r\n" +
                $"郵送担当者\t:{Space}{OperationCondolence.MailRepresentative}";
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
                    ( DefaultDate, DateTime.Today, string.Empty, "法務部", string.Empty, string.Empty,
                        string.Empty, SearchAccountingSubjectCode, true, true, true, true, 
                        ReceiptsAndExpenditureSearchDate, receiptsAndExpenditureSearchDate, DefaultDate,
                        DateTime.Today, Pagination.PageCount, "ID", false);
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
                if (ConfirmationOwnerName(ownerName, value) == MessageBoxResult.Yes) ownerName = value;
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
        public string CondolenceContent
        {
            get => condolenceContent;
            set
            {
                condolenceContent = value;
                ContentText = value;
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
                if(value) SearchAccountingSubjectCode = "815";
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
                if (value) SearchAccountingSubjectCode = "832";
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
                if (value) SearchAccountingSubjectCode = "831";
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 窓口、郵送欄を空欄にするかチェック
        /// </summary>
        public bool IsReceptionBlank
        {
            get => isReceptionBlank;
            set
            {
                isReceptionBlank = value;
                IsFixToggleEnabled = !value;
                if (value) FixToggle = true;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 窓口、郵送対応トグル　窓口がtrue
        /// </summary>
        public bool FixToggle
        {
            get => fixToggle;
            set
            {
                fixToggle = value;
                MailRepresentative = value ? string.Empty : GetLoginRep.Rep.Name;
                CounterReceiver = value ? GetLoginRep.Rep.Name : string.Empty;
                FixToggleContent = value ? "窓口受付" : "郵送対応";
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 窓口、郵送トグルのContent
        /// </summary>
        public string FixToggleContent
        {
            get => fixToggleContent;
            set
            {
                fixToggleContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 窓口、郵送トグルのEnabled
        /// </summary>
        public bool IsFixToggleEnabled
        {
            get => isFixToggleEnabled;
            set
            {
                isFixToggleEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 削除ボタンVisibility
        /// </summary>
        public bool IsDeleteButtonVisibility
        {
            get => isDeleteButtonVisibility;
            set
            {
                isDeleteButtonVisibility = value;
                CallPropertyChanged();
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
            AlmsgivingSearchCommand = new DelegateCommand(() => AlmsgivingSearch(), () => true);
            TipSearchCommand = new DelegateCommand(() => TipSearch(), () => true);
            SocialGatheringSearchCommand = new DelegateCommand
                (() => SocialGatheringSearch(), () => true);
            DeleteCondolenceCommand = new DelegateCommand(() => DeleteCondolence(), () => true);
        }

        protected override void SetDetailLocked() { }

        protected override void SetWindowDefaultTitle() =>
            WindowTitle = $"お布施一覧データ出力 : {AccountingProcessLocation.Location}";

        public void CondolenceNotify() => FieldClear();

        public void SortNotify() => SetReceiptsAndExpenditures(true);

        public void PageNotify() => SetReceiptsAndExpenditures(false);

        public void SetSortColumns() =>
            Pagination.SortColumns = new Dictionary<int, string>()
            {
                {0,"ID" },{1,"入金日"}
            };
    }
}
