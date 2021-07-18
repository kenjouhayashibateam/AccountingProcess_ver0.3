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
        /// 収支日報を出力します
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
    }
}
