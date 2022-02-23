using Domain.Entities;
using System;
using System.Collections.ObjectModel;

namespace Domain.Repositories
{
    /// <summary>
    /// データ出力
    /// </summary>
    public interface IDataOutput
    {
        /// <summary>
        /// 金庫データを出力します
        /// </summary>
        void CashboxData();
        /// <summary>
        /// 春秋苑収支日報を出力します
        /// </summary>
        /// <param name="previousDayFinalAccountWithUnit">前日決算</param>
        /// <param name="paymentWithUnit">入金</param>
        /// <param name="withdrawalWithUnit">出金</param>
        /// <param name="tranceferAmountWithUnit">社内振替</param>
        /// <param name="todayFinalAccountWithUnit">残高</param>
        /// <param name="yokohamaBankAmountWithUnit">横浜銀行残高</param>
        /// <param name="ceresaAmountWithUnit">セレサ川崎残高</param>
        /// <param name="wizeCoreAmountWithUnit">ワイズコア仮受金</param>
        void BalanceFinalAccount
            (string previousDayFinalAccountWithUnit, string paymentWithUnit, string withdrawalWithUnit,
                string tranceferAmountWithUnit, string todayFinalAccountWithUnit,
                string yokohamaBankAmountWithUnit, string ceresaAmountWithUnit,
                string wizeCoreAmountWithUnit, bool yokohamaBankCheck, bool ceresaCheck);
        /// <summary>
        /// ワイズコア収支日報を出力します
        /// </summary>
        /// <param name="rengeanPreviousDayFinalAccount">蓮華庵前日繰越</param>
        /// <param name="rengeanPayment">蓮華庵入金</param>
        /// <param name="rengeanWithdrawal">蓮華庵出金</param>
        /// <param name="rengeanTranceferAmount">蓮華庵銀行振替</param>
        /// <param name="rengeanShunjuenTranceferAmount">蓮華庵春秋苑振替</param>
        /// <param name="shunjuanPreviousDayFinalAccount">春秋庵前日繰越</param>
        /// <param name="shunjuanPayment">春秋庵入金</param>
        /// <param name="shunjuanWithdrawal">春秋庵出金</param>
        /// <param name="shunjuanTranceferAmount">春秋庵銀行振替</param>
        /// <param name="shunjuanShunjuenTranceferAmount">春秋庵春秋苑振替</param>
        /// <param name="kougePreviousDayFinalAccount">香華前日繰越</param>
        /// <param name="kougePayment">香華入金</param>
        /// <param name="kougeWithdrawal">香華出金</param>
        /// <param name="kougeTranceferAmount">香華銀行振替</param>
        /// <param name="kougeShunjuenTranceferAmount">香華春秋苑振替</param>
        /// <param name="yokohamaBankAmount">横浜銀行残高</param>
        /// <param name="shunjuenAmount">春秋苑仮払金</param>
        void BalanceFinalAccount
            (int rengeanPreviousDayFinalAccount, int rengeanPayment, int rengeanWithdrawal,
                int rengeanTranceferAmount, int rengeanShunjuenTranceferAmount,
                int shunjuanPreviousDayFinalAccount, int shunjuanPayment, int shunjuanWithdrawal,
                int shunjuanTranceferAmount, int shunjuanShunjuenTranceferAmount,
                int kougePreviousDayFinalAccount, int kougePayment, int kougeWithdrawal,
                int kougeTranceferAmount, int kougeShunjuenTranceferAmount, int yokohamaBankAmount,
                int shunjuenAmount);
        /// <summary>
        /// 出納データを出力します
        /// </summary>
        /// <param name="receiptsAndExpenditures">出力する出納データ</param>
        /// <param name="previousDayBalance">出力する出納データの前日残高</param>
        void ReceiptsAndExpenditureData
            (ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures);
        /// <summary>
        /// 入出金伝票を出力します
        /// </summary>
        /// <param name="receiptsAndExpenditures">伝票出力する出納データ</param>
        /// <param name="loginRep">ログインしている担当者</param>
        /// <param name="isPayment">伝票の種類。入金チェック</param>
        void PaymentAndWithdrawalSlips(ObservableCollection<ReceiptsAndExpenditure>
            receiptsAndExpenditures, bool isPayment, bool isPreviousDay);
        /// <summary>
        /// 受納証を出力します
        /// </summary>
        /// <param name="voucher">出力する受納証</param>
        void VoucherData(Voucher voucher, bool isReissueText, DateTime prepaidDate);
        /// <summary>
        /// 御布施一覧データを出力します
        /// </summary>
        /// <param name="condolences"></param>
        void Condolences(ObservableCollection<Condolence> condolences);

        void TransferSlips(ObservableCollection<TransferReceiptsAndExpenditure>
            transferReceiptsAndExpenditures);
    }
}
