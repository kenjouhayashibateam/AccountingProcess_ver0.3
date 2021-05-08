using ClosedXML.Excel;
using Domain.Entities;
using System;
using System.Collections.ObjectModel;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 御布施一覧データ出力クラス
    /// </summary>
    internal class CondolencesOutput : OutputSingleSheetData
    {
        private readonly ObservableCollection<Condolence> Condolences;
        private int MaxRow = 2;

        public CondolencesOutput(ObservableCollection<Condolence> condolences) => Condolences = condolences;

        protected override void SetBorderStyle()
        {
            MySheetCellRange(2, 1, MaxRow, 13).Style
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin);     
        }

        protected override void SetCellsStyle()
        {
            throw new NotImplementedException();
        }

        protected override double[] SetColumnSizes()
        {
            throw new NotImplementedException();
        }

        protected override void SetDataStrings()
        {
            throw new NotImplementedException();
        }

        protected override double SetMaeginsBottom()
        {
            throw new NotImplementedException();
        }

        protected override double SetMaeginsLeft()
        {
            throw new NotImplementedException();
        }

        protected override double SetMaeginsRight()
        {
            throw new NotImplementedException();
        }

        protected override double SetMaeginsTop()
        {
            throw new NotImplementedException();
        }

        protected override void SetMerge()
        {
            throw new NotImplementedException();
        }

        protected override double[] SetRowSizes()
        {
            throw new NotImplementedException();
        }

        protected override string SetSheetFontName()
        {
            throw new NotImplementedException();
        }

        protected override void SetSheetStyle()
        {
            throw new NotImplementedException();
        }

        protected override XLPaperSize SheetPaperSize()
        {
            throw new NotImplementedException();
        }
    }
}
