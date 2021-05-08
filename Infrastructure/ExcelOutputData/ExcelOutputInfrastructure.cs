﻿using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
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
                string wizeCoreAmountWithUnit,bool yokohamaBankCheck,bool ceresaCheck)
        {
            BalanceFinalAccountOutput bfao =
                new BalanceFinalAccountOutput
                    (previousDayFinalAccountWithUnit, paymentWithUnit, withdrawalWithUnit,
                        tranceferAmountWithUnit, todayFinalAccountWithUnit, yokohamaBankAmountWithUnit,
                        ceresaAmountWithUnit, wizeCoreAmountWithUnit,yokohamaBankCheck,ceresaCheck);
            bfao.DataOutput();
        }

        public void CashboxData()
        {
            CashBoxOutput cbo = new CashBoxOutput();
            cbo.DataOutput();
        }

        public void PaymentAndWithdrawalSlips
            (ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures,
                Rep loginRep,bool isPayment,bool isPreviousDay)
        {
            SlipType st = (isPayment) ? SlipType.Payment : SlipType.Withdrawal;
            SlipOutput pso = new SlipOutput(receiptsAndExpenditures,loginRep,st,isPreviousDay);
            pso.Output();
        }

        public void ReceiptsAndExpenditureData
            (ObservableCollection<ReceiptsAndExpenditure> receiptsAndExpenditures, int previousDayBalance)
        {
            ReceiptsAndExpenditureOutput raeo = 
                new ReceiptsAndExpenditureOutput(receiptsAndExpenditures, previousDayBalance);
            raeo.Output();
        }

        public void VoucherData(Voucher voucher)
        {
            VoucherOutput vo = new VoucherOutput(voucher);
            vo.DataOutput();
        }
    }
}
