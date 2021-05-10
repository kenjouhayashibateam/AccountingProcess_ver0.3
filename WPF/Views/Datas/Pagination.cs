using WPF.ViewModels.Commands;

namespace WPF.Views.Datas
{
    /// <summary>
    /// ページネーションクラス
    /// </summary>
    public class Pagination:NotifyPropertyChanged
    {
        private int totalRowCount;
        private int pageCount;
        private int totalPageCount;
        private string listPageInfo;
        private bool isPrevPageEnabled;
        private bool isNextPageEnabled;

        public int TotalRowCount
        {
            get => totalRowCount;
            set
            {
                totalRowCount = value;
                CallPropertyChanged();
            }
        }
        public int PageCount
        {
            get => pageCount;
            set
            {
                pageCount = value;
                CallPropertyChanged();
            }
        }
        public int TotalPageCount
        {
            get => totalPageCount;
            set
            {
                totalPageCount = value;
                if (value == 0) PageCount = 0;
                CallPropertyChanged();
            }
        }
        public string ListPageInfo { get => listPageInfo; set {
                listPageInfo = value;
                CallPropertyChanged();
            } }
        public bool IsPrevPageEnabled { get => isPrevPageEnabled; set {
                isPrevPageEnabled = value;
                CallPropertyChanged();
            } }
        public bool IsNextPageEnabled { get => isNextPageEnabled; set {
                isNextPageEnabled = value;
                CallPropertyChanged();
            } }
        public void PrevPageListExpress()
        {
            if (PageCount == 0) return;
            if (PageCount > 1) PageCount--;
        }
        public void NextPageListExpress()
        {
            if (PageCount == 0) return;
            PageCount += PageCount == TotalPageCount ? 0 : 1;
        }

        public void SetListPageInfo()
        {
            int i = TotalRowCount / 10;
            i += TotalRowCount % 10 == 0 ? 0 : 1;
            TotalPageCount = i;
            ListPageInfo = $"{PageCount}/{i}";
            IsPrevPageEnabled = PageCount > 1;
            IsNextPageEnabled = PageCount != i;
        }

        public bool PageCountSubtractAndCanPrevPageExpress()
        {
            if (PageCount == 0) return false;
            if (PageCount > 1) PageCount--;
            return true;
        }

        public bool PageCountAddAndCanNextPageExpress()
        {
            if (PageCount == 0) return false;
            PageCount += PageCount == TotalPageCount ? 0 : 1;
            return true;
        }
        public void CountReset(bool isReset)
        {
            if (isReset) PageCount = 1;
        }
    }
}
