using WPF.ViewModels.Datas;

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
                if (value == 0) PageCount = 0;
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
        /// ページ数をマイナスした時に1以上になっているかを検証しまず
        /// </summary>
        /// <returns>1以上ならTrueを返します</returns>
        public bool CanPageCountSubtractAndCanPrevPageExpress()
        {
            if (PageCount == 0) return false;
            if (PageCount > 1) PageCount--;
            return true;
        }
        /// <summary>
        /// ページ数をプラスした時に1以上かつ総ページ数を超えていないかを検証します
        /// </summary>
        /// <returns></returns>
        public bool CanPageCountAddAndCanNextPageExpress()
        {
            if (PageCount == 0) return false;
            PageCount += PageCount == TotalPageCount ? 0 : 1;
            return true;
        }
        /// <summary>
        /// ページ数をリセットします
        /// </summary>
        /// <param name="isReset">リセットするかのチェック</param>
        public void CountReset(bool isReset)
        {
            if (isReset) PageCount = 1;
        }
    }
}
