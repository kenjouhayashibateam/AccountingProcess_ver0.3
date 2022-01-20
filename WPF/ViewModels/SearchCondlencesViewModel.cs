using Domain.Entities;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 御布施データ閲覧ViewModel
    /// </summary>
    public class SearchCondlencesViewModel : BaseViewModel, IPagenationObserver
    {
        private DateTime searchStartDate = DefaultDate;
        private DateTime searchEndDate = DateTime.Today;
        private Pagination pagination = Pagination.GetPagination();
        private ObservableCollection<Condolence> condolences;

        public SearchCondlencesViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            Pagination.Add(this);
            Pagination.SortDirectionIsASC = true;

            int i = -(int)DateTime.Today.DayOfWeek;

            if (i > -2) { i -= 6; }
            else { i += 1; }

            SearchStartDate = DateTime.Today.AddDays(i);
        }
        public SearchCondlencesViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 御布施一覧リストを生成します
        /// </summary>
        private void CreateCondlences(bool isPageReset)
        {
            Pagination.CountReset(isPageReset);
            (int count, ObservableCollection<Condolence> list) =
                DataBaseConnect.ReferenceCondolence
                    (SearchStartDate, SearchEndDate, string.Empty, Pagination.PageCount, Pagination.CountEachPage);
            Condolences = list;
            Pagination.TotalRowCount = count;
            Pagination.SetProperty();
        }
        /// <summary>
        /// 期間検索の最初の日付
        /// </summary>
        public DateTime SearchStartDate
        {
            get => searchStartDate;
            set
            {
                searchStartDate = SearchEndDate < value ? SearchEndDate : value;
                CreateCondlences(true);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 期間検索の最後の日付
        /// </summary>
        public DateTime SearchEndDate
        {
            get => searchEndDate;
            set
            {
                searchEndDate = SearchStartDate > value ? SearchStartDate : value;
                CreateCondlences(true);
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
        /// 御布施一覧リスト
        /// </summary>
        public ObservableCollection<Condolence> Condolences
        {
            get => condolences;
            set
            {
                condolences = value;
                CallPropertyChanged();
            }
        }

        public void PageNotify() { CreateCondlences(false); }

        public void SetSortColumns()
        {
            Pagination.SortColumns = new Dictionary<int, string>()
            { { 0, "ID" }, { 1, "日付" }, { 2, "担当僧侶" } };
        }

        public void SortNotify() { CreateCondlences(true); }

        public override void ValidationProperty(string propertyName, object value)
        { }

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = "御布施一覧データ"; }

        public void SetCountEachPage() { pagination.CountEachPage = 10; }
    }
}
