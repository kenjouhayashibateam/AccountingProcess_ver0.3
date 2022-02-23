using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels.Datas
{
    /// <summary>
    /// 保持している振替伝票データの通知インフラストラクチャ
    /// </summary>
    public interface ITransferReceiptsAndExpenditureOperationObserver
    {
        void TransferReceiptsAndExpenditureOperationNotify();
    }
    /// <summary>
    /// 操作する振替伝票データをメモリに保持するクラス
    /// </summary>
    public sealed class TransferReceiptsAndExpenditureOperation
    {
        private readonly List<ITransferReceiptsAndExpenditureOperationObserver> observers =
            new List<ITransferReceiptsAndExpenditureOperationObserver>();

        private static TransferReceiptsAndExpenditure operationData;

        private static readonly TransferReceiptsAndExpenditureOperation _TransferReceiptsAndExpenditureOperation =
            new TransferReceiptsAndExpenditureOperation();

        public static TransferReceiptsAndExpenditureOperation GetInstance()
        { return _TransferReceiptsAndExpenditureOperation; }

        public void SetData(TransferReceiptsAndExpenditure transferReceiptsAndExpenditure)
        { operationData = transferReceiptsAndExpenditure; }

        public TransferReceiptsAndExpenditure GetData() => operationData;

        public void Add(ITransferReceiptsAndExpenditureOperationObserver observer)
        { observers.Add(observer); }

        public void Remove(ITransferReceiptsAndExpenditureOperationObserver ovserver)
        { observers.Remove(ovserver); }

        public void Notify()
        {
            foreach (ITransferReceiptsAndExpenditureOperationObserver observer in observers)
            { observer.TransferReceiptsAndExpenditureOperationNotify(); }
        }
    }
}
