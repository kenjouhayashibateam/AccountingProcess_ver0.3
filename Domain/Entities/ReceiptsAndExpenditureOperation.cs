using System.Collections.Generic;

namespace Domain.Entities
{
    public interface IReceiptsAndExpenditureOperationObserver
    {
        void ReceiptsAndExpenditureOperationNotify();
    }
    /// <summary>
    /// 操作する出納データをメモリに保持するクラス
    /// </summary>
    public sealed class ReceiptsAndExpenditureOperation
    {
        /// <summary>
        /// 出納データを何に使うか
        /// </summary>
        public enum OperationType
        {
            /// <summary>
            /// 伝票操作
            /// </summary>
            ReceiptsAndExpenditure,
            /// <summary>
            /// 受納証操作
            /// </summary>
            Voucher
        }

        private OperationType operationType = OperationType.ReceiptsAndExpenditure;

        private readonly List<IReceiptsAndExpenditureOperationObserver> observers =
            new List<IReceiptsAndExpenditureOperationObserver>();

        private static ReceiptsAndExpenditure OperationData;
        
        private readonly static ReceiptsAndExpenditureOperation _receiptsAndExpenditureOperation
            = new ReceiptsAndExpenditureOperation();

        public OperationType GetOperationType() => operationType;

        public static ReceiptsAndExpenditureOperation GetInstance() => _receiptsAndExpenditureOperation;

        public void SetOperationType(OperationType type) => operationType = type;       

        public void SetData(ReceiptsAndExpenditure receiptsAndExpenditure) =>
            OperationData = receiptsAndExpenditure;

        public ReceiptsAndExpenditure Data => OperationData;

        public void Notify()
        {
            foreach (IReceiptsAndExpenditureOperationObserver raeo in observers)
                raeo.ReceiptsAndExpenditureOperationNotify();
        }
        public void Add(IReceiptsAndExpenditureOperationObserver operationObserver) =>
            observers.Add(operationObserver);
        public void Remove(IReceiptsAndExpenditureOperationObserver operationObserver) =>
            observers.Remove(operationObserver);
    }
}
