using System.Collections;

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
        /// データを出力するページの一番上のRow
        /// </summary>
        protected int StartRowPosition;

        protected OutputList(IEnumerable outputList)
        {
            SetList(outputList);
            ItemIndex = 0;
            PageCount = 0;
            StartRowPosition = 1;
            myWorksheet.PageSetup.PaperSize = SheetPaperSize();
            myWorksheet.Style.Font.FontName = SetSheetFontName();
            NextPage();
        }
        /// <summary>
        /// 出力するリストを保持します
        /// </summary>
        /// <param name="outputList">出力リスト</param>
        protected abstract void SetList(IEnumerable outputList);
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
            PageCount++;
            StartRowPosition = SetRowSizes().Length * (PageCount - 1) + 1;
            for (int i = 0; i < SetRowSizes().Length; i++) 
                myWorksheet.Row(StartRowPosition + i).Height = SetRowSizes()[i];
            for (int i = 0; i < SetColumnSizes().Length; i++) 
                myWorksheet.Column(i + 1).Width = SetColumnSizes()[i];
            PageStyle();
        }
    }
}
