using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// お布施一覧作成ViewModel
    /// </summary>
    public class CreateCondolencesViewModel : BaseViewModel,ICondolenceObserver
    {
        #region Properties
        #region Ints
        private int TotalPageCount;
        /// <summary>
        /// リストデータの総数
        /// </summary>
        private int RowCount;
        /// <summary>
        /// リストのページ
        /// </summary>
        private int PageCount;
        #endregion
        /// <summary>
        /// リストのページの総数
        /// </summary>
        private string listPageInfo;
        private string outputButtonContent="出力";
        private bool isPrevPageEnabled;
        private bool isNextPageEnabled;
        private ObservableCollection<Condolence> condolences;
        private ObservableCollection<Condolence> AllList;
        private Condolence selectedCondolence;
        private readonly CondolenceOperation condolenceOperation;
        private DateTime searchStartDate;
        private DateTime searchEndDate;
        #endregion

        public CreateCondolencesViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            condolenceOperation = CondolenceOperation.GetInstance();
            condolenceOperation.Add(this);
            SearchStartDate = DateTime.Today.AddDays(1 * (-1 * (DateTime.Today.Day - 1)));
            SearchEndDate = DateTime.Today;
            ShowRegistrationViewCommand = new DelegateCommand
                (() => ShowRegistrationView(), () => true);
            NextPageListExpressCommand = new DelegateCommand
                (() => NextPageListExpress(), () => true);
            PrevPageListExpressCommand = new DelegateCommand
                (() => PrevPageListExpress(), () => true);
            ShowUpdateViewCommand = new DelegateCommand
                (() => ShowUpdateView(), () => true);
            OutputCommand = new DelegateCommand
                (() => Output(), () => true);
        }
        public CreateCondolencesViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        public DelegateCommand OutputCommand { get; set; }
        private void Output()
        {

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
        /// 前の10件を表示するコマンド
        /// </summary>
        public DelegateCommand PrevPageListExpressCommand { get; set; }
        private void PrevPageListExpress()
        {
            if (PageCount == 0) return;
            if (PageCount > 1) PageCount--;
            CreateCondolences(false);
        }
        /// <summary>
        /// 次の10件を表示するコマンド
        /// </summary>
        public DelegateCommand NextPageListExpressCommand { get; }
        private void NextPageListExpress()
        {
            if (PageCount == 0) return;
            PageCount += PageCount == TotalPageCount ? 0 : 1;
            CreateCondolences(false);
        }
        /// <summary>
        /// データ登録画面を表示するコマンド
        /// </summary>
        public DelegateCommand ShowRegistrationViewCommand { get; }
        private void ShowRegistrationView() => 
            CreateShowWindowCommand(ScreenTransition.CondolenceOperation());
        /// <summary>
        /// お布施一覧のアイテムリスト
        /// </summary>
        public ObservableCollection<Condolence> Condolences
        {
            get => condolences;
            set
            {
                condolences = value;
                SetListPageInfo();
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
                if (value < SearchEndDate) CreateCondolences(true);
                CallPropertyChanged();
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
                if (value > SearchStartDate) CreateCondolences(true);
                CallPropertyChanged();
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
        /// 前の10件ボタンのEnabled
        /// </summary>
        public bool IsPrevPageEnabled
        {
            get => isPrevPageEnabled;
            set
            {
                isPrevPageEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 次の10件ボタンのEnabled
        /// </summary>
        public bool IsNextPageEnabled
        {
            get => isNextPageEnabled;
            set
            {
                isNextPageEnabled = value;
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


        private void SetListPageInfo()
        {
            int i = RowCount / 10;
            i += RowCount % 10 == 0 ? 0 : 1;
            TotalPageCount = i;
            ListPageInfo = $"{PageCount}/{i}";
            IsPrevPageEnabled = PageCount > 1;
            IsNextPageEnabled = PageCount != i;
        }
        public string ConvertContentText(bool isMemorialService) => isMemorialService ? "法事" : "葬儀";
        /// <summary>
        /// 御布施一覧リストを検索して生成します
        /// </summary>
        private void CreateCondolences(bool isPageCountReset)
        {
            PageCount = isPageCountReset ? 1 : PageCount;

            Condolences =
                DataBaseConnect.ReferenceCondolence(SearchStartDate, SearchEndDate, PageCount).List;
            RowCount =
                DataBaseConnect.ReferenceCondolence(SearchStartDate, SearchEndDate,PageCount).TotalRows;
            AllList =
                DataBaseConnect.ReferenceCondolence(SearchStartDate, SearchEndDate);

            if (AllList.Count == 0) PageCount = 0;
            SetListPageInfo();
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
            throw new System.NotImplementedException();
        }

        protected override void SetWindowDefaultTitle() => 
            DefaultWindowTitle = $"お布施一覧データ出力 : {AccountingProcessLocation.Location}";

        public void CondolenceNotify() => CreateCondolences(true);
    }
}
