using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.ObjectModel;
using static Infrastructure.ExcelOutputData.SlipOutput;

namespace Infrastructure.ExcelOutputData
{
    /// <summary>
    /// エクセル出力クラス
    /// </summary>
    public class ExcelOutputInfrastructure : IDataOutput
    {
        public void BalanceFinalAccount
            (string previousDayFinalAccountWithUnit, string paymentWithUnit, string withdrawalWithUnit, 
                string tranceferAmountWithUnit, string todayFinalAccountWithUnit,
                string yokohamaBankAmountWithUnit, string ceresaAmountWithUnit,
                string wizeCoreAmountWithUnit, bool yokohamaBankCheck, bool ceresaCheck)
        {
            ShunjuenBalanceFinalAccountOutput bfao =
                new ShunjuenBalanceFinalAccountOutput
                    (previousDayFinalAccountWithUnit, paymentWithUnit, withdrawalWithUnit,
                        tranceferAmountWithUnit, todayFinalAccountWithUnit, yokohamaBankAmountWithUnit,
                        ceresaAmountWithUnit, wizeCoreAmountWithUnit, yokohamaBankCheck, ceresaCheck);
            bfao.DataOutput();
        }

        public void BalanceFinalAccount
            (string rengeanPreviousDayFinalAccountWithUnit, string rengeanPaymentWithUnit,
                string rengeanWithdrawalWithUnit, string rengeanTranceferAmountWithUnit,
                string shunjuanPreviousDayFinalAccountWithUnit, string shunjuanPaymentWithUnit,
                string shunjuanWithdrawalWithUnit, string shunjuanTranceferAmountWithUnit,
                string kougePreviousDayFinalAccountWithUnit, string kougePaymentWithUnit,
                string kougeWithdrawalWithUnit, string kougeTranceferAmountWithUnit,
                string yokohamaBankAmountWithUnit, string shunjuenAmountWithUnit)
        {
            WizeCoreBalanceFinalAccountOutput wcbfa =
                new WizeCoreBalanceFinalAccountOutput
                    (rengeanPreviousDayFinalAccountWithUnit, rengeanPaymentWithUnit, rengeanWithdrawalWithUnit,
                        rengeanTranceferAmountWithUnit, shunjuanPreviousDayFinalAccountWithUnit,
                        shunjuanPaymentWithUnit, shunjuanWithdrawalWithUnit, shunjuanTranceferAmountWithUnit,
                        kougePreviousDayFinalAccountWithUnit, kougePaymentWithUnit, kougeWithdrawalWithUnit,
                        kougeTranceferAmountWithUnit, yokohamaBankAmountWithUnit, shunjuenAmountWithUnit);
            wcbfa.DataOutput();
        }

        public void CashboxData()
        {
            CashBoxOutput cbo = new CashBoxOutput();
            cbo.DataOutput();
        }

        public void Condolences(ObservableCollection<Condolence> condolences)
        {
            CondolencesOutput co = new CondolencesOutput(condolences);
            co.DataOutput();
        }

        public void PaymentAndWithdrawalSlips
            (ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures, bool isPayment,
                bool isPreviousDay)
        {
            SlipType st = isPayment ? SlipType.Payment : SlipType.Withdrawal;
            LoginRep loginRep = LoginRep.GetInstance();
            SlipOutput pso = new SlipOutput(receiptsAndExpenditures, loginRep.Rep, st, isPreviousDay);
            pso.Output();
        }

        public void ReceiptsAndExpenditureData
            (ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures)
        {
            CashJournalOutput raeo =
                new CashJournalOutput(receiptsAndExpenditures);
            raeo.Output();
        }

        public void VoucherData(Voucher voucher, bool isReissue, DateTime prepaidDate)
        {
            VoucherOutput vo = new VoucherOutput(voucher, isReissue, prepaidDate);
            vo.DataOutput();
        }
    }
}
