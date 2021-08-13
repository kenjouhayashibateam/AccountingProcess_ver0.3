using System.Collections.Generic;
using WPF.ViewModels.Commands;

namespace WPF.ViewModels.Datas
{
    public interface IPagenationObserver
    {
        void SortNotify();
        void PageNotify();
        void SetSortColumns();
    }
    /// <summary>
    /// ページネーションクラス
    /// </summary>
    public sealed class Pagination : NotifyPropertyChanged
    {
        public static Pagination GetPagination() { return new Pagination(); }
        private int totalRowCount;
        private int pageCount;
        private int totalPageCount;
        private string listPageInfo = "0/0";
        private string sortDirectionContent = string.Empty;
        private string selectedSortColumn = string.Empty;
        private const string ASCCONTENT = "降順で検索（現在昇順）";
        private const string DESCCONTENT = "昇順で検索（現在降順）";
        private bool isPrevPageEnabled = true;
        private bool isNextPageEnabled = true;
        private bool sortDirectionIsASC = true;
        private Dictionary<int, string> sortColumns;
        private readonly List<IPagenationObserver> pagenationObservers = new List<IPagenationObserver>();

        private Pagination()
        {
            PrevPageListExpressCommand = new DelegateCommand
                (() => PrevPageListExpress(), () => true);
            NextPageListExpressCommand = new DelegateCommand
                (() => NextPageListExpress(), () => true);
            MaxPageExpressCommand = new DelegateCommand
                (() => MaxPageExpress(), () => true);
            MinPageExpressCommand = new DelegateCommand
                (() => MinPageExpress(), () => true);
        }
        /// <summary>
        /// 最初のページを表示するコマンド
        /// </summary>
        public DelegateCommand MinPageExpressCommand { get; set; }
        private void MinPageExpress()
        {
            if (PageCount == 0) { return; }
            PageCount = 1;
            PageNotification();
        }
        /// <summary>
        /// 最終ページを表示するコマンド
        /// </summary>
        public DelegateCommand MaxPageExpressCommand { get; set; }
        private void MaxPageExpress()
        {
            if (PageCount == 0) { return; }
            PageCount = TotalPageCount;
            PageNotification();
        }
        /// <summary>
        /// 次の10件を表示するコマンド
        /// </summary>
        public DelegateCommand NextPageListExpressCommand { get; set; }
        private void NextPageListExpress() { PageCountAdd(); }
        /// <summary>
        /// 前の10件を表示するコマンド
        /// </summary>
        public DelegateCommand PrevPageListExpressCommand { get; set; }
        private void PrevPageListExpress() { PageCountSubtract(); }
        /// <summary>
        /// ソートする方向トグルのContent
        /// </summary>
        public string SortDirectionContent
        {
            get => sortDirectionContent;
            set
            {
                sortDirectionContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ItemSourceのトータルのレコード数
        /// </summary>
        public int TotalRowCount
        {
            get => totalRowCount;
            set
            {
                totalRowCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ページカウント
        /// </summary>
        public int PageCount
        {
            get => pageCount;
            set
            {
                pageCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// トータルのページカウント
        /// </summary>
        public int TotalPageCount
        {
            get => totalPageCount;
            set
            {
                totalPageCount = value;
                if (value == 0) { PageCount = 0; }
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 表示されているページ数と総ページ数のText
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
            get => isNextPageEnabled; set
            {
                isNextPageEnabled = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// DataGridをソートするカラムリスト
        /// </summary>
        public Dictionary<int, string> SortColumns
        {
            get => sortColumns;
            set
            {
                sortColumns = value;
                SelectedSortColumn = sortColumns[0];
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 総レコード数から総ページ数を算出して、各プロパティに値を入力します
        /// </summary>
        public void SetProperty()
        {
            int i = TotalRowCount / 10;
            i += TotalRowCount % 10 == 0 ? 0 : 1;
            TotalPageCount = i;
            ListPageInfo = $"{PageCount}/{i}";
            IsPrevPageEnabled = PageCount > 1;
            IsNextPageEnabled = PageCount != i;
        }
        /// <summary>
        /// ページ数をマイナスした時に1以上になっているかを検証してページを通知します
        /// </summary>
        public void PageCountSubtract()
        {
            if (PageCount == 0) { return; }
            if (PageCount > 1) { PageCount--; }
            PageNotification();
        }
        /// <summary>
        /// ページ数をプラスした時に1以上かつ総ページ数を超えていないかを検証してページを通知します
        /// </summary>
        /// <returns></returns>
        public void PageCountAdd()
        {
            if (PageCount == 0) { return; }
            PageCount += PageCount == TotalPageCount ? 0 : 1;
            PageNotification();
        }
        /// <summary>
        /// ページ数をリセットします
        /// </summary>
        /// <param name="isReset">リセットするかのチェック</param>
        public void CountReset(bool isReset) { if (isReset) { PageCount = 1; } }
        /// <summary>
        /// ソート方向トグル　昇順がTrue
        /// </summary>
        public bool SortDirectionIsASC
        {
            get => sortDirectionIsASC;
            set
            {
                sortDirectionIsASC = value;
                SortDirectionContent = value ?
                    ASCCONTENT : DESCCONTENT;
                SortNotification();
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択されたソートするカラム名
        /// </summary>
        public string SelectedSortColumn
        {
            get => selectedSortColumn;
            set
            {
                selectedSortColumn = value;
                SortNotification();
                CallPropertyChanged();
            }
        }

        public void Add(IPagenationObserver pagenationObserver)
        {
            pagenationObservers.Add(pagenationObserver);
            foreach (IPagenationObserver po in pagenationObservers) { po.SetSortColumns(); }
        }

        private void PageNotification()
        {
            foreach (IPagenationObserver po in pagenationObservers) { po.PageNotify(); }
        }

        private void SortNotification()
        {
            foreach (IPagenationObserver po in pagenationObservers) { po.SortNotify(); }
        }
    }
}
