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
        private string totalAmount = string.Empty;
        private string contentText = string.Empty;
        private string soryoName = string.Empty;
        private string note = string.Empty;
        private string dataOperationButtonContent;
        /// <summary>
        /// 検索する勘定科目コード
        /// </summary>
        private string SearchAccountingSubjectCode { get; set; }
        private string searchGanreContent;
        #endregion
        #region Bools
        private bool isMemorialService;
        private bool isAlmsgivingCheck;
        private bool isCarTipCheck;
        private bool isMealTipCheck;
        private bool isCarAndMealTipCheck;
        private bool isAlmsgivingSearch;
        private bool isOperationButtonEnabled;
        #endregion
        private Dictionary<int, string> soryoList;
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures;
        private DateTime receiptsAndExpenditureSearchDate=DefaultDate;
        private DateTime accountActivityDate;
        private ReceiptsAndExpenditure selectedReceiptsAndExpenditure;
        private readonly CondolenceOperation OperationCondolence;
        #endregion

        public CondolenceOperationViewModel(IDataBaseConnect dataBaseConnect):base(dataBaseConnect)
        {
            OperationCondolence = CondolenceOperation.GetInstance();
            OperationCondolence.Add(this);
            IsAlmsgivingSearch = true;
            ReceiptsAndExpenditureSearchDate = DateTime.Today;
            IsAlmsgivingCheck = true;
            IsMemorialService = true;
            DataOperationButtonContent = DataOperation.登録.ToString();
        }
        public CondolenceOperationViewModel() : 
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// データ操作コマンド
        /// </summary>
        public DelegateCommand OperationDataCommand { get; set; }
        private async void OperationData()
        {
            Condolence condolence = new Condolence
                (0, OwnerName, SoryoName, isMemorialService, IntAmount(Almsgiving), IntAmount(CarTip),
                    IntAmount(MealTip), IntAmount(CarAndMealTip), Note, AccountActivityDate);
            
            string content=condolence.IsMemorialService?"法事":"葬儀";

            MessageBox = new MessageBoxInfo()
            {
                Message = $"日付\t\t:{Space}{condolence.AccountActivityDate.ToShortDateString()}\r\n" +
                                    $"施主名\t\t:{Space}{condolence.OwnerName}\r\n" +
                                    $"担当僧侶\t\t:{Space}{condolence.SoryoName}\r\n" +
                                    $"内容\t\t:{Space}{content}\r\n" +
                                    $"合計金額\t\t:{Space}{TotalAmount}\r\n" +
                                    $"御布施\t\t:{Space}{Almsgiving}\r\n" +
                                    $"御車代\t\t:{Space}{CarTip}\t\n" +
                                    $"御膳料\t\t:{Space}{MealTip}\t\n" +
                                    $"御車代御膳料\t:{Space}{CarAndMealTip}\t\n" +
                                    $"備考\t\t:{Space}{Note}\r\n\r\n登録しますか？",
                Button = MessageBoxButton.OKCancel,
                Image = MessageBoxImage.Question,
                Title = "登録確認"
            };

            if (MessageBox.Result == MessageBoxResult.Cancel) return;

            IsOperationButtonEnabled = false;
            DataOperationButtonContent = "登録中";
            LoginRep loginRep = LoginRep.GetInstance();
            await Task.Run(() => DataBaseConnect.Registration(condolence));
            MessageBox = new MessageBoxInfo()
            {
                Message = "登録しました。",
                Image = MessageBoxImage.Information,
                Button = MessageBoxButton.OK,
                Title = "登録完了"
            };
            OperationCondolence.SetData(condolence);
            IsOperationButtonEnabled = true;
            DataOperationButtonContent = "登録";
            OperationCondolence.Notify();
        }
        private void FieldClear()
        {
            OwnerName = string.Empty;
            SoryoName = string.Empty;
            Almsgiving = string.Empty;
            CarTip = string.Empty;
            MealTip = string.Empty;
            CarAndMealTip = string.Empty;
            Note = string.Empty;
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
        private void SetReceiptsAndExpenditures()
        {
            ReceiptsAndExpenditures =
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                    (DefaultDate, DateTime.Today, string.Empty, "法務部", string.Empty, string.Empty,
                        string.Empty, SearchAccountingSubjectCode, true, true, true, true, 
                        ReceiptsAndExpenditureSearchDate, receiptsAndExpenditureSearchDate, DefaultDate,
                        DateTime.Today);
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
            get => receiptsAndExpenditureSearchDate; set
            {
                receiptsAndExpenditureSearchDate = value;
                SetReceiptsAndExpenditures();
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
                    IntAmount(CarAndMealTip));
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
        /// お布施チェック
        /// </summary>
        public bool IsAlmsgivingCheck
        {
            get => isAlmsgivingCheck;
            set
            {
                isAlmsgivingCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御車代チェック
        /// </summary>
        public bool IsCarTipCheck
        {
            get => isCarTipCheck;
            set
            {
                isCarTipCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御膳料チェック
        /// </summary>
        public bool IsMealTipCheck
        {
            get => isMealTipCheck;
            set
            {
                isMealTipCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御車代御膳料チェック
        /// </summary>
        public bool IsCarAndMealTipCheck
        {
            get => isCarAndMealTipCheck;
            set
            {
                isCarAndMealTipCheck = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御布施か志納金のどちらで検索するか　御布施がTrue
        /// </summary>
        public bool IsAlmsgivingSearch
        {
            get => isAlmsgivingSearch;
            set
            {
                isAlmsgivingSearch = value;
                SearchGanreContent = value ? "志納金検索" : "御布施検索";
                SearchAccountingSubjectCode = value ? "815" : "832";
                SetReceiptsAndExpenditures();
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
        }

        protected override void SetDetailLocked() { }

        protected override void SetWindowDefaultTitle() =>
            WindowTitle = $"お布施一覧データ出力 : {AccountingProcessLocation.Location}";

        public void CondolenceNotify() => FieldClear();
    }
}
