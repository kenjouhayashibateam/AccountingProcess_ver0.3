using ClosedXML.Excel;
using Domain.Repositories;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using System;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// エクセルアプリケーション
    /// </summary>
    internal abstract class ExcelApp
    {
        /// <summary>
        /// ClosedXML : ワークブック
        /// </summary>
        protected XLWorkbook myWorkbook;
        /// <summary>
        /// Excel : ワークブック
        /// </summary>
        protected Workbooks myWorkbooks;
        /// <summary>
        /// ClosedXML : ワークシート
        /// </summary>
        protected IXLWorksheet myWorksheet;
        /// <summary>
        /// ログインフラストラクチャ
        /// </summary>
        private readonly ILogger Logger;
        /// <summary>
        /// エクセルアプリケーション
        /// </summary>
        private Application App;
        /// <summary>
        /// エクセルファイルを保存しているフォルダのFullPath
        /// </summary>
        protected readonly string openPath =
            System.IO.Path.GetFullPath(Properties.Resources.SaveFolderPath + Properties.Resources.SaveFile);
        /// <summary>
        /// コンストラクタ　ログ保存のインフラストラクチャを設定します
        /// </summary>
        /// <param name="logger"></param>
        public ExcelApp(ILogger logger)
        {
            Logger = logger;
            ExcelClose();
            myWorkbook = new XLWorkbook();
            myWorksheet = myWorkbook.AddWorksheet(Properties.Resources.SheetName);
        }
        public ExcelApp() : this(new LogFileInfrastructure()) { }

        /// <summary>
        /// デストラクタ　エクセルプロセスを開放します
        /// </summary>
        ~ExcelApp()
        {
            if (App == null) return;
            if (App.Workbooks.Count == 0) App.Quit();
        }
        /// <summary>
        /// データ出力エクセルファイルを開きます
        /// </summary>
        protected void ExcelOpen()
        {
            myWorksheet.Protect("excel");
            myWorkbook.SaveAs(openPath);
            myWorkbooks.Open(Filename: openPath, ReadOnly: true);
            App.Visible = true;
        }
        /// <summary>
        /// エクセルアプリケーションを呼び出します
        /// </summary>
        private void CallExcelApplication()
        {
            try
            {
                App = (Application)Interaction.GetObject(Class: "Excel.Application");
            }
            catch
            {
                App = new Application();
            }
        }
        /// <summary>
        /// データ出力エクセルファイルを閉じます
        /// </summary>
        protected void ExcelClose()
        {
            CallExcelApplication();

            myWorkbooks = App.Workbooks;
            //出力ファイルを検出して閉じる
            foreach (Microsoft.Office.Interop.Excel.Workbook wb in myWorkbooks) if (wb.Name == Properties.Resources.SaveFile) wb.Close(SaveChanges: false);             
            //開いているワークブックがなければエクセルアプリケーションを終了する
            if (myWorkbooks.Count == 0) App.Quit();
        }
        /// <summary>
        /// シートの余白を設定します
        /// </summary>
        protected void SetMargins()
        {
            myWorksheet.PageSetup.Margins
                .SetLeft(SetMaeginsLeft())
                .SetTop(SetMaeginsTop())
                .SetRight(SetMaeginsRight())
                .SetBottom(SetMaeginsBottom());
        }
        /// <summary>
        /// エクセルシートのフォント名を返します
        /// </summary>
        protected abstract string SetSheetFontName();
        /// <summary>
        /// エクセルシートの書式を設定します
        /// </summary>
        protected abstract void SetSheetStyle();
        /// <summary>
        /// エクセルシートの出力用紙を設定します
        /// </summary>
        protected abstract XLPaperSize SheetPaperSize();
        /// <summary>
        /// エクセルシートのTopの余白を設定します
        /// </summary>
        /// <returns></returns>
        protected abstract Double SetMaeginsTop();
        /// <summary>
        /// エクセルシートのLeftの余白を設定します
        /// </summary>
        /// <returns></returns>
        protected abstract Double SetMaeginsLeft();
        /// <summary>
        /// エクセルシートのRightの余白を設定します
        /// </summary>
        /// <returns></returns>
        protected abstract Double SetMaeginsRight();
        /// <summary>
        /// エクセルシートのBottomの余白を設定します
        /// </summary>
        /// <returns></returns>
        protected abstract Double SetMaeginsBottom();
        /// <summary>
        /// エクセルシートのセルの結合を設定します
        /// </summary>
        protected abstract void SetMerge();
        /// <summary>
        /// エクセルシートのRowの値を設定します
        /// </summary>
        protected abstract double[] SetRowSizes();
        /// <summary>
        /// エクセルシートのColumnの値を設定します
        /// </summary>
        protected abstract double[] SetColumnSizes();
        /// <summary>
        /// エクセルシートのボーダースタイルを設定します
        /// </summary>
        protected abstract void SetBorderStyle();
        /// <summary>
        /// セルのStyleを設定します
        /// </summary>
        protected abstract void SetCellsStyle();
        /// <summary>
        /// メートル法の数字をインチ法で返します
        /// </summary>
        /// <param name="x">メートル法での長さ</param>
        /// <returns>インチ法での長さ</returns>
        protected double ToInch(double x) => x * 0.39370;
        /// <summary>
        /// エクセルシートのCellの範囲を指定します
        /// </summary>
        /// <param name="cell1Row">始点CellのRow</param>
        /// <param name="cell1Column">始点CellのCulumn</param>
        /// <param name="cell2Row">終点CellのRow</param>
        /// <param name="cell2Column">終点CellのCulumn</param>
        /// <returns>ClosedXML.Excel.Range</returns>
        protected IXLRange MySheetCellRange(int cell1Row, int cell1Column, int cell2Row, int cell2Column) => myWorksheet.Range(myWorksheet.Cell(cell1Row, cell1Column), myWorksheet.Cell(cell2Row, cell2Column));
    }
}
