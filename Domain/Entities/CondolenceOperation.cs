using System.Collections.Generic;

namespace Domain.Entities
{
    public interface ICondolenceObserver
    {
        void CondolenceNotify();
    }
    /// <summary>
    /// 操作する御布施一覧データクラスをメモリに保持するクラス
    /// </summary>
    public sealed class CondolenceOperation
    {
        private readonly static CondolenceOperation _condolenceOperation = new CondolenceOperation();

        public static CondolenceOperation GetInstance() => _condolenceOperation;

        private readonly List<ICondolenceObserver> observers = new List<ICondolenceObserver>();

        private static Condolence OperationData;

        public void Add(ICondolenceObserver observer) => observers.Add(observer);

        public void Remove(ICondolenceObserver observer) => observers.Remove(observer);

        public void SetData(Condolence condolence) => OperationData = condolence;

        public Condolence GetData() => OperationData;

        public void Notify()
        {
            foreach (ICondolenceObserver co in observers) co.CondolenceNotify();
        }
    }
}
