namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 単票出力
    /// </summary>
    internal abstract class OutputSingleSheetData : ExcelApp
    {
        /// <summary>
        /// エクセルにデータを出力します
        /// </summary>
        public void DataOutput()
        {
            SetSheetStyle();
            _ = myWorksheet.PageSetup.SetPaperSize(SheetPaperSize());
            SetMargins();
            SetMerge();
            double[] RowSizes = SetRowSizes();
            double[] ColumnSizes = SetColumnSizes();

            for (int i = 0; i < RowSizes.Length; i++) { myWorksheet.Row(i + 1).Height = RowSizes[i]; }

            for (int i = 0; i < ColumnSizes.Length; i++)
            { myWorksheet.Column(i + 1).Width = ColumnSizes[i]; }
            myWorksheet.Style.Font.FontName = SetSheetFontName();
            SetBorderStyle();
            SetCellsStyle();
            SetDataStrings();
            myWorkbook.SaveAs(openPath);
            ExcelOpen();
        }
        /// <summary>
        /// シートにデータの文字列を書き込みます
        /// </summary>
        protected abstract void SetDataStrings();
    }
}
