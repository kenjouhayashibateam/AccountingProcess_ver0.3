using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.DataHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// お布施一覧作成ViewModel
    /// </summary>
    public class CreateCondolencesViewModel : BaseViewModel, ICondolenceObserver,
        IPagenationObserver, IClosing
    {
        #region Properties
        /// <summary>
        /// 現在のページとリストのページの総数
        /// </summary>
        private string listPageInfo;
        private string outputButtonContent = "出力";
        private string locationLimitingContent;
        private bool isOutputButtonEnabled;
        private bool isLocationLimiting;
        private bool isClose = true;
        private ObservableCollection<Condolence> condolences;
        private ObservableCollection<Condolence> AllList;
        private Condolence selectedCondolence;
        private readonly CondolenceOperation condolenceOperation;
        private DateTime searchStartDate = DefaultDate;
        private DateTime searchEndDate = DefaultDate;
        private readonly IDataOutput DataOutput;
        private Pagination pagination;
        #endregion

        public CreateCondolencesViewModel
            (IDataBaseConnect dataBaseConnect, IDataOutput dataOutput) : base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            condolenceOperation = CondolenceOperation.GetInstance();
            condolenceOperation.Add(this);
            Pagination = Pagination.GetPagination();
            Pagination.Add(this);
            LocationLimitingContent = $"{AccountingProcessLocation.Location}のデータのみを表示する";
            DayOfWeek dow = DateTime.Today.DayOfWeek;
            int i = dow == DayOfWeek.Sunday ? -7 : -(int)dow;
            SearchStartDate = DateTime.Today.AddDays(i);
            SearchEndDate = DateTime.Today;
            ShowRegistrationViewCommand = new DelegateCommand
                (() => ShowRegistrationView(), () => true);
            ShowUpdateViewCommand = new DelegateCommand
                (() => ShowUpdateView(), () => true);
            OutputCommand = new DelegateCommand
                (() => Output(), () => true);
        }
        public CreateCondolencesViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect(),
                    DefaultInfrastructure.GetDefaultDataOutput())
        { }
        /// <summary>
        /// 御布施一覧出力コマンド
        /// </summary>
        public DelegateCommand OutputCommand { get; set; }
        private async void Output()
        {
            OutputButtonContent = "出力中";
            IsOutputButtonEnabled = IsClose = false;
            await Task.Run(() => DataOutput.Condolences(AllList));
            OutputButtonContent = "出力";
            IsOutputButtonEnabled = IsClose = true;
        }
        /// <summary>
        /// 更新画面を表示するコマンド
        /// </summary>
        public DelegateCommand ShowUpdateViewCommand { get; set; }
        private async void ShowUpdateView()
        {
            await Task.Delay(1);
            condolenceOperation.SetData(SelectedCondolence);
            CreateShowWindowCommand(ScreenTransition.CondolenceOperation());
        }
        /// <summary>
        /// データ登録画面を表示するコマンド
        /// </summary>
        public DelegateCommand ShowRegistrationViewCommand { get; }
        private void ShowRegistrationView()
        {
            condolenceOperation.SetData(null);
            CreateShowWindowCommand(ScreenTransition.CondolenceOperation());
        }
        /// <summary>
        /// お布施一覧のアイテムリスト
        /// </summary>
        public ObservableCollection<Condolence> Condolences
        {
            get => condolences;
            set
            {
                condolences = value;
                ValidationProperty(nameof(Condolences), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 検索する開始日時
        /// </summary>
        public DateTime SearchStartDate
        {
            get => searchStartDate;
            set
            {
                searchStartDate = value;
                CallPropertyChanged();
                if (value < SearchEndDate) { CreateCondolences(true); }
            }
        }
        /// <summary>
        /// 検索する最終日時
        /// </summary>
        public DateTime SearchEndDate
        {
            get => searchEndDate;
            set
            {
                searchEndDate = value;
                CallPropertyChanged();
                if (value > SearchStartDate) { CreateCondolences(true); }
            }
        }
        /// <summary>
        /// 表示しているリストのページの案内
        /// </summary>
        public string ListPageInfo
        {
            get => listPageInfo;
            set
            {
                listPageInfo = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された御布施一覧データ
        /// </summary>
        public Condolence SelectedCondolence
        {
            get => selectedCondolence;
            set
            {
                selectedCondolence = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力ボタンのContent
        /// </summary>
        public string OutputButtonContent
        {
            get => outputButtonContent;
            set
            {
                outputButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力ボタンのEnabled
        /// </summary>
        public bool IsOutputButtonEnabled
        {
            get => isOutputButtonEnabled;
            set
            {
                isOutputButtonEnabled = value;
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
        /// 表示するデータのLocationを限定するか
        /// </summary>
        public bool IsLocationLimiting
        {
            get => isLocationLimiting;
            set
            {
                isLocationLimiting = value;
                CallPropertyChanged();
                CreateCondolences(true);
            }
        }
        /// <summary>
        /// Location限定チェックのContent
        /// </summary>
        public string LocationLimitingContent 
        {
            get => locationLimitingContent;
            set
            {
                locationLimitingContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ウィンドウを閉じる許可を統括
        /// </summary>
        public bool IsClose
        {
            get => isClose;
            set
            {
                isClose = value;
                CallPropertyChanged();
            }
        }

        /// <summary>
        /// 法事チェックで変換する文字列
        /// </summary>
        /// <param name="isMemorialService"></param>
        /// <returns></returns>
        public string ConvertContentText(bool isMemorialService)
        {
            return isMemorialService ? "法事" : "葬儀";
        }

        /// <summary>
        /// 御布施一覧リストを検索して生成します
        /// </summary>
        private void CreateCondolences(bool isPageCountReset)
        {
            Pagination.CountReset(isPageCountReset);

            string location = IsLocationLimiting ? AccountingProcessLocation.Location.ToString() : string.Empty;

            (int count, ObservableCollection<Condolence> list) =
                DataBaseConnect.ReferenceCondolence
                    (SearchStartDate, SearchEndDate, location, Pagination.PageCount, pagination.CountEachPage);
            Condolences = list;
            Pagination.TotalRowCount = count;

            AllList =
                DataBaseConnect.ReferenceCondolence
                    (SearchStartDate, SearchEndDate, location);

            if (AllList.Count == 0) { Pagination.PageCount = 0; }
            ValidationProperty(nameof(Condolences), AllList);
            IsOutputButtonEnabled = AllList.Count > 0;
            Pagination.SetProperty();
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            ErrorsListOperation
                (((ObservableCollection<Condolence>)value).Count < 1, propertyName,
                    "出力するデータがありません");
        }

        protected override void SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"お布施一覧データ出力 : {AccountingProcessLocation.Location}";
        }

        public void CondolenceNotify() { CreateCondolences(true); }

        public void SortNotify() { CreateCondolences(true); }

        public void PageNotify() { CreateCondolences(false); }

        public void SetSortColumns()
        {
            Pagination.SortColumns = new Dictionary<int, string>()
            {
                {0,"ID" },{1,"日付"},{2,"担当僧侶"}
            };
        }

        public bool CancelClose() { return IsClose; }

        public void RefleshList() { CreateCondolences(true); }

        public int SetCountEachPage() => 10;
    }
}
