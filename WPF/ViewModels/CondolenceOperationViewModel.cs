using Domain.Entities;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.ObjectModel;
using Domain.Repositories;
using static Domain.Entities.Helpers.TextHelper;
using Infrastructure;
using System.Collections.Generic;
using WPF.ViewModels.Commands;

namespace WPF.ViewModels
{
    /// <summary>
    /// お布施一覧登録ViewModel
    /// </summary>
    public class CondolenceOperationViewModel:DataOperationViewModel
    {
        #region Properties
        #region Strings
        private string carTip;
        private string ownerName;
        private string almsgiving;
        private string mealTip;
        private string carAndMealTip;
        private string totalAmount;
        private string contentText;
        private string soryoName;
        private string note;
        private string dataOperationButtonContent;
        /// <summary>
        /// 検索する伝票内容
        /// </summary>
        private string SearchContent { get; set; }
        #endregion
        #region Bools
        private bool isMemorialService;
        private bool isAlmsgivingCheck;
        private bool isCarTipCheck;
        private bool isMealTipCheck;
        private bool isCarAndMealTipCheck;
        #endregion
        private Dictionary<int, string> soryoList;
        private ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures;
        private DateTime receiptsAndExpenditureSearchDate;
        private DateTime registrationDate;
        private ReceiptsAndExpenditure selectedReceiptsAndExpenditure;
        #endregion

        public CondolenceOperationViewModel(IDataBaseConnect dataBaseConnect):base(dataBaseConnect)
        { 
            ReceiptsAndExpenditureSearchDate = DateTime.Today;
            IsAlmsgivingCheck = true;
        }
        public CondolenceOperationViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 御車代御膳料検索コマンド
        /// </summary>
        public DelegateCommand SearchCarAndMealTipCommand { get; set; }
        private void SearchCarAndMealTip()
        {
            IsCarAndMealTipCheck = true;
            SearchContent = "御車代御膳料";
            SetReceiptsAndExpenditures();
        }
        /// <summary>
        /// 御膳料検索コマンド
        /// </summary>
        public DelegateCommand SearchMealTipCommand { get; set; }
        private void SearchMealTip()
        {
            IsMealTipCheck = true;
            SearchContent = "御膳料";
            SetReceiptsAndExpenditures();
        }
        /// <summary>
        /// 御車代検索コマンド
        /// </summary>
        public DelegateCommand SearchCarTipCommand { get; set; }
        private void SearchCarTip()
        {
            IsCarTipCheck = true;
            SearchContent = "御車代";
            SetReceiptsAndExpenditures();
        }
        /// <summary>
        /// 御布施検索コマンド
        /// </summary>
        public DelegateCommand SearchAlmsgivingCommand { get; set; }
        private void SearchAlmsgiving()
        {
            IsAlmsgivingCheck = true;
            SearchContent = "御布施";
            SetReceiptsAndExpenditures();
        }
        private void SetReceiptsAndExpenditures()
        {
            ReceiptsAndExpenditures =
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                    (DefaultDate, DateTime.Today, string.Empty, "法務部", SearchContent, string.Empty,
                        string.Empty, string.Empty, true, true, true, true, ReceiptsAndExpenditureSearchDate,
                        receiptsAndExpenditureSearchDate, DefaultDate, DateTime.Today);
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
        public DateTime Date
        {
            get => registrationDate;
            set
            {
                registrationDate = value;
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
                ownerName = value;
                CallPropertyChanged();
            }
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
                CallPropertyChanged();
            }
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
            throw new NotImplementedException();
        }

        protected override void SetDataList()
        {
            SoryoList = DataBaseConnect.GetSoryoList();
        }

        protected override void SetDataOperationButtonContent(DataOperation operation) =>
            DataOperationButtonContent = operation.ToString();

        protected override void SetDelegateCommand()
        {
            SearchAlmsgivingCommand = new DelegateCommand(() => SearchAlmsgiving(), () => true);
            SearchCarTipCommand = new DelegateCommand(() => SearchCarTip(), () => true);
            SearchMealTipCommand = new DelegateCommand(() => SearchMealTip(), () => true);
            SearchCarAndMealTipCommand = new DelegateCommand(() => SearchCarAndMealTip(), () => true);
        }

        protected override void SetDetailLocked() { }

        protected override void SetWindowDefaultTitle() =>
            WindowTitle = $"お布施一覧データ出力 : {AccountingProcessLocation.Location}";
    }
}
