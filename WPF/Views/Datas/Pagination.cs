﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using WPF.ViewModels.Datas;

namespace WPF.Views.Datas
{
    public interface IPagenationObserver
    {
        void SortNotify();
        void PageNotify();
    }
    /// <summary>
    /// ページネーションクラス
    /// </summary>
    public sealed class Pagination:NotifyPropertyChanged
    {
        private readonly static Pagination pagination = new Pagination();
        public static Pagination GetPagination() => pagination;
        private int totalRowCount;
        private int pageCount;
        private int totalPageCount;
        private string listPageInfo;
        private string sortDirectionContent;
        private string selectedSortColumn;
        private const string ASCCONTENT= "降順で検索（現在昇順）";
        private const string DESCCONTENT = "昇順で検索（現在降順）";
        private bool isPrevPageEnabled;
        private bool isNextPageEnabled;
        private bool sortDirectionIsASC;
        private Dictionary<int, string> sortColumns;
        private readonly List<IPagenationObserver> pagenationObservers = new List<IPagenationObserver>();

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
        /// DataGridをソートするカラムリスト
        /// </summary>
        public Dictionary<int, string> SortColumns
        {
            get => sortColumns;
            set
            {
                sortColumns = value;
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
        public void Add(IPagenationObserver pagenationObserver) =>
            pagenationObservers.Add(pagenationObserver);
        private void PageNotification()
        {
            foreach (IPagenationObserver po in pagenationObservers) po.PageNotify();
        }

        private void SortNotification()
        {
            foreach (IPagenationObserver po in pagenationObservers) po.SortNotify();
        }
    }
}
