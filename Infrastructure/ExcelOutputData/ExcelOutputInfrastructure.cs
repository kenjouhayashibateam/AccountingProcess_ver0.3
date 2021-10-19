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

        public void BalanceFinalAccount(int rengeanPreviousDayFinalAccount, int rengeanPayment, int rengeanWithdrawal, int rengeanTranceferAmount, int shunjuanPreviousDayFinalAccount, int shunjuanPayment, int shunjuanWithdrawal, int shunjuanTranceferAmount, int kougePreviousDayFinalAccount, int kougePayment, int kougeWithdrawal, int kougeTranceferAmount, int yokohamaBankAmount, int shunjuenAmount)
        {
            WizeCoreBalanceFinalAccountOutput wcbfa =
                new WizeCoreBalanceFinalAccountOutput
                    (rengeanPreviousDayFinalAccount, rengeanPayment, rengeanWithdrawal,
                        rengeanTranceferAmount, shunjuanPreviousDayFinalAccount,
                        shunjuanPayment, shunjuanWithdrawal, shunjuanTranceferAmount,
                        kougePreviousDayFinalAccount, kougePayment, kougeWithdrawal,
                        kougeTranceferAmount, yokohamaBankAmount, shunjuenAmount);
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
