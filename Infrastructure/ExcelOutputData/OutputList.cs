using Domain.Entities;
using System.Collections;
using System.Collections.ObjectModel;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// リスト出力
    /// </summary>
    internal abstract class OutputList : ExcelApp
    {
        /// <summary>
        /// データリストのインデックス
        /// </summary>
        protected int ItemIndex;
        /// <summary>
        /// データ出力のページ数
        /// </summary>
        protected int PageCount;
        /// <summary>
        /// 出力データリスト
        /// </summary>
        protected IEnumerable OutputDatas;
        /// <summary>
        /// データを出力するページの一番上のRow
        /// </summary>
        protected int StartRowPosition;

        protected OutputList(IEnumerable outputDatas)
        {
            OutputDatas = outputDatas;
            ItemIndex = 0;
            PageCount = 0;
            StartRowPosition = 0;
            PageStyle();
        }
        /// <summary>
        /// データリストを出力します
        /// </summary>
        public abstract void Output();
        /// <summary>
        /// エクセルシートの用紙1ページごとのStyleを設定します
        /// </summary>
        protected abstract void PageStyle();
        /// <summary>
        /// 新しいページのStyleを設定します
        /// </summary>
        protected void NextPage()
        {
            StartRowPosition = PageCount + SetRowSizes().Length * PageCount;
            PageCount++;
            for (int i = 0; i < SetRowSizes().Length; i++) myWorksheet.Row(StartRowPosition + i).Height = SetRowSizes()[i];
            for (int i = 0; i < SetColumnSizes().Length; i++) myWorksheet.Column(i + 1).Width = SetColumnSizes()[i];
            PageStyle();
        }
    }
}
