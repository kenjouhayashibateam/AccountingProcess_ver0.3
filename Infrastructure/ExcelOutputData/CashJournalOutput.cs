using ClosedXML.Excel;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using static Domain.Entities.Helpers.TextHelper;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// 出納データ出力
    /// </summary>
    internal class CashJournalOutput : OutputList
    {
        /// <summary>
        /// エクセルに出力する出納データの入出金日
        /// </summary>
        private DateTime CurrentDate;
        /// <summary>
        /// 1ページあたりの行数
        /// </summary>
        private readonly int OnePageRowCount = 54;
        /// <summary>
        /// 出納データリスト
        /// </summary>
        private ObservableCollection<ReceiptsAndExpenditure> ReceiptsAndExpenditures;
        /// <summary>
        /// 前日残高
        /// </summary>
        private int PreviousDayBalance;
        private readonly IDataBaseConnect DataBaseConnect;

        public CashJournalOutput
            (ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures)
                : base(receiptsAndExpenditures)
        {
            DataBaseConnect = DefaultInfrastructure.GetDefaultDataBaseConnect();
            SetSheetStyle();
        }

        public override void Output()
        {
            int payment = 0;
            int withdrawal = 0;
            int pageRowCount = 1;
            int pagePayment = default;
            int pageWithdrawal = default;
            int pageBalance = default;
            int slipPayment = default;
            int slipWithdrawal = default;
            bool firstPage = true;
            bool isPageMove = false;
            string detailString = default;
            string currentSubjectCode = string.Empty;
            string currentDept = string.Empty;
            DateTime currentActivityDate = DefaultDate;
            string currentLocation = string.Empty;

            foreach (ReceiptsAndExpenditure rae in ReceiptsAndExpenditures.OrderBy(r => r.OutputDate)
                .ThenByDescending(r => r.IsPayment)
                .ThenBy(r => r.Content.AccountingSubject.SubjectCode)
                .ThenBy(r => r.Content.AccountingSubject.Subject)
                .ThenBy(r => r.CreditDept.ID)
                .ThenBy(r => r.Location))
            {
                if (isPageMove) PageMove();

                if (pageRowCount == 1)
                {
                    CurrentDate = rae.OutputDate;
                    SetMerge();
                    SetTitle();
                    SetStyleAndNextIndex();
                    pageRowCount++;
                    SetPageBalance();
                    SetStyleAndNextIndex();
                    pageRowCount++;
                }
                else if (CurrentDate != rae.OutputDate)
                {
                    SetBalance();
                    myWorksheet.Cell(StartRowPosition, 2).Value = CurrentDate.Day;
                }

                SetItem();

                isPageMove = pageRowCount == OnePageRowCount;

                void PageMove()
                {
                    if (IsSameSlip()) return;
                    myWorksheet.Cell(StartRowPosition, 4).Value = "計";
                    myWorksheet.Cell(StartRowPosition, 7).Value = CommaDelimitedAmount(pagePayment);
                    myWorksheet.Cell(StartRowPosition, 8).Value = CommaDelimitedAmount(pageWithdrawal);
                    pageBalance += payment - withdrawal;
                    myWorksheet.Cell(StartRowPosition, 9).Value =
                        CommaDelimitedAmount(pageBalance);
                    payment = pagePayment = withdrawal = pageWithdrawal = 0;
                    SetStyleAndNextIndex();
                    pageRowCount = 1;
                    MySheetCellRange(StartRowPosition - 1, 1, StartRowPosition - 1, 9).Style
                        .Border.SetTopBorder(XLBorderStyleValues.Double);
                    PreviousDayBalance = pageBalance;
                    SetStyleAndNextIndex();
                    isPageMove = false;
                    NextPage();
                }

                void SetPageBalance()
                {
                    if (firstPage)
                    {
                        myWorksheet.Cell(StartRowPosition + 1, 1).Value = CurrentDate.Month;
                        myWorksheet.Cell(StartRowPosition + 1, 2).Value = CurrentDate.Day;
                        myWorksheet.Cell(StartRowPosition, 4).Value = "前月より繰越";
                        PreviousDayBalance = DataBaseConnect.CallFinalAccountPerMonth
                            (rae.OutputDate);
                        myWorksheet.Cell(StartRowPosition, 9).Value =
                            CommaDelimitedAmount(PreviousDayBalance);
                        pageBalance += PreviousDayBalance;
                        payment = pagePayment = withdrawal = pageWithdrawal = 0;
                        firstPage = false;
                    }
                    else
                    {
                        myWorksheet.Cell(StartRowPosition, 4).Value = "前頁より繰越";
                        myWorksheet.Cell(StartRowPosition, 9).Value =
                            CommaDelimitedAmount(pageBalance);
                        myWorksheet.Cell(StartRowPosition + 1, 1).Value = CurrentDate.Month;
                        myWorksheet.Cell(StartRowPosition + 1, 2).Value = CurrentDate.Day;
                    }
                }

                void SetItem()
                {
                    if (IsSameSlip())
                    {
                        slipPayment += rae.IsPayment ? rae.Price : 0;
                        slipWithdrawal += rae.IsPayment ? 0 : rae.Price;
                        myWorksheet.Cell(StartRowPosition - 1, 6).Value = $"{detailString}他{ItemIndex}件";
                        SetPrice(StartRowPosition - 1);
                        ItemIndex++;
                    }
                    else
                    {
                        currentActivityDate = rae.AccountActivityDate;
                        slipPayment = rae.IsPayment ? rae.Price : 0;
                        slipWithdrawal = rae.IsPayment ? 0 : rae.Price;
                        currentLocation = rae.Location;
                        currentSubjectCode = rae.Content.AccountingSubject.SubjectCode;
                        currentDept = rae.CreditDept.Dept;
                        if (rae.IsPayment) slipPayment = rae.Price;
                        else slipWithdrawal = rae.Price;
                        ItemIndex = 1;
                        detailString = rae.Detail;
                        myWorksheet.Cell(StartRowPosition, 3).Value = rae.Content.AccountingSubject.SubjectCode;
                        myWorksheet.Cell(StartRowPosition, 4).Value = rae.Content.AccountingSubject.Subject;
                        myWorksheet.Cell(StartRowPosition, 5).Value = rae.Content.Text;
                        myWorksheet.Cell(StartRowPosition, 6).Value = detailString;
                        SetPrice(StartRowPosition);
                        SetStyleAndNextIndex();
                        pageRowCount++;
                    }

                }
                bool IsSameSlip()
                {
                    return currentDept == rae.CreditDept.Dept &&
                        currentSubjectCode == rae.Content.AccountingSubject.SubjectCode &&
                        ItemIndex < 9 && currentLocation == rae.Location;
                }

                void SetBalance()
                {
                    if (CurrentDate != rae.OutputDate)
                    {
                        CurrentDate = rae.OutputDate;
                        pageBalance += payment - withdrawal;
                        PreviousDayBalance += payment - withdrawal;
                        myWorksheet.Cell(StartRowPosition - 1, 9).Value =
                            CommaDelimitedAmount(PreviousDayBalance);
                        payment = 0;
                        withdrawal = 0;
                    }
                }

                void SetPrice(int position)
                {
                    if (rae.IsPayment)
                    {
                        myWorksheet.Cell(position, 7).Value = CommaDelimitedAmount(slipPayment);
                        payment += rae.Price;
                        pagePayment += rae.Price;
                    }
                    else
                    {
                        myWorksheet.Cell(position, 8).Value = CommaDelimitedAmount(slipWithdrawal);
                        withdrawal += rae.Price;
                        pageWithdrawal += rae.Price;
                    }
                }
            }

            if (pageRowCount == 1)
            {
                SetTitle();
                SetStyleAndNextIndex();
                myWorksheet.Cell(StartRowPosition, 4).Value = "前頁より繰越";
                myWorksheet.Cell(StartRowPosition, 9).Value =
                    CommaDelimitedAmount(pageBalance);
                PreviousDayBalance = pageBalance;
                SetStyleAndNextIndex();
                pageRowCount++;
            }
            else myWorksheet.Cell(StartRowPosition - 1, 9).Value =
                CommaDelimitedAmount(pageBalance + payment - withdrawal);

            myWorksheet.Cell(StartRowPosition, 4).Value = "計";
            myWorksheet.Cell(StartRowPosition, 7).Value = CommaDelimitedAmount(pagePayment);
            myWorksheet.Cell(StartRowPosition, 8).Value = CommaDelimitedAmount(pageWithdrawal);
            PreviousDayBalance += payment - withdrawal;
            myWorksheet.Cell(StartRowPosition, 9).Value = CommaDelimitedAmount(PreviousDayBalance);
            SetStyleAndNextIndex();
            _ = MySheetCellRange(StartRowPosition - 1, 1, StartRowPosition - 1, 9).Style
                .Border.SetTopBorder(XLBorderStyleValues.Double);
            myWorksheet.Cell(StartRowPosition, 4).Value = "翌月へ繰越";
            myWorksheet.Cell(StartRowPosition, 9).Value = CommaDelimitedAmount(PreviousDayBalance);
            SetStyleAndNextIndex();
            ExcelOpen();

            void SetTitle()
            {
                SetMerge();
                _ = MySheetCellRange(StartRowPosition, 1, StartRowPosition, SetColumnSizes().Length).Style
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    .Border.SetRightBorder(XLBorderStyleValues.None)
                    .Border.SetLeftBorder(XLBorderStyleValues.None)
                    .Border.SetTopBorder(XLBorderStyleValues.None);
                myWorksheet.Cell(StartRowPosition, 1).Value =
                    $"{CurrentDate.ToString($"gg{Space}y{Space}年", JapanCulture)}";
                StartRowPosition++;
                pageRowCount++;
                myWorksheet.Cell(StartRowPosition, 1).Value = "日付";
                myWorksheet.Cell(StartRowPosition, 3).Value = "コード";
                myWorksheet.Cell(StartRowPosition, 4).Value = "勘定科目";
                myWorksheet.Cell(StartRowPosition, 5).Value = "内容";
                myWorksheet.Cell(StartRowPosition, 6).Value = "詳細";
                myWorksheet.Cell(StartRowPosition, 7).Value = "入金";
                myWorksheet.Cell(StartRowPosition, 8).Value = "出金";
                myWorksheet.Cell(StartRowPosition, 9).Value = "差引残高";
            }
        }
        /// <summary>
        ///  インデックスに値を加える際に、データのセルのスタイルを設定します
        ///  </summary>
        private void SetStyleAndNextIndex()
        {
            if (StartRowPosition == 1) return;
            SetBorderStyle();
            SetCellsStyle();
            SetMargins();
            _ = myWorksheet.Style.Alignment.SetShrinkToFit(true);
            StartRowPosition++;
        }

        protected override void SetBorderStyle()
        {
            _ = MySheetCellRange(StartRowPosition , 1, StartRowPosition , 9).Style
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin);
            _ = myWorksheet.Cell(StartRowPosition, 1).Style.Border.SetRightBorder(XLBorderStyleValues.Dashed);
            _ = myWorksheet.Cell(StartRowPosition, 2).Style.Border.SetLeftBorder(XLBorderStyleValues.Dashed);
            _ = myWorksheet.Cell(StartRowPosition, 2).Style.Border.SetRightBorder(XLBorderStyleValues.Double);
            _ = myWorksheet.Cell(StartRowPosition, 3).Style.Border.SetLeftBorder(XLBorderStyleValues.Double);
            _ = myWorksheet.Cell(StartRowPosition, 6).Style.Border.SetRightBorder(XLBorderStyleValues.Double);
            _ = myWorksheet.Cell(StartRowPosition, 7).Style.Border.SetLeftBorder(XLBorderStyleValues.Double);
            _ = myWorksheet.Cell(StartRowPosition, 7).Style.Border.SetRightBorder(XLBorderStyleValues.Double);
            _ = myWorksheet.Cell(StartRowPosition, 8).Style.Border.SetLeftBorder(XLBorderStyleValues.Double);
            _ = myWorksheet.Cell(StartRowPosition, 8).Style.Border.SetRightBorder(XLBorderStyleValues.Double);
            _ = myWorksheet.Cell(StartRowPosition, 9).Style.Border.SetLeftBorder(XLBorderStyleValues.Double);
        }

        protected override void SetCellsStyle()
        {
            _ = MySheetCellRange(StartRowPosition, 1, StartRowPosition, 2).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = myWorksheet.Cell(StartRowPosition , 3).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = MySheetCellRange(StartRowPosition , 4, StartRowPosition , 6).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            _ = MySheetCellRange(StartRowPosition, 7, StartRowPosition, 9).Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        }

        protected override double[] SetColumnSizes() => new double[]
            { 2.64,2.64, 4.71, 16.43, 16.43, 16.43, 10,10, 10 };

        protected override double SetMaeginsBottom() => ToInch(1);

        protected override double SetMaeginsLeft() => ToInch(0.7);

        protected override double SetMaeginsRight() => ToInch(0.7);

        protected override double SetMaeginsTop() => ToInch(1);

        protected override void SetMerge()
        {
            _ = MySheetCellRange(StartRowPosition, 1, StartRowPosition, SetColumnSizes().Length).Merge();
            _ = MySheetCellRange(StartRowPosition + 1, 1, StartRowPosition + 1, 2).Merge();
            _ = MySheetCellRange(StartRowPosition + 1, 5, StartRowPosition + 1, 6).Merge();
        }

        protected override double[] SetRowSizes()
        {
            double[] d = new double[OnePageRowCount];
            for (int i = 0; i < d.Length; i++) { d[i] = 15; }
            return d;
        }

        protected override void SetSheetStyle()
        {
            myWorksheet.Style.Font.FontSize = 11;
            myWorksheet.Style.NumberFormat.Format = "@";
        }

        protected override XLPaperSize SheetPaperSize() => XLPaperSize.A4Paper;

        protected override void PageStyle()
        {
            SetStyleAndNextIndex();
        }

        protected override void SetList(IEnumerable outputList) =>
            ReceiptsAndExpenditures = (ObservableCollection<ReceiptsAndExpenditure>)outputList;
        
        protected override string SetSheetFontName() => "ＭＳ Ｐゴシック";
    }
}
