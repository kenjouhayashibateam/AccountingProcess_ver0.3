﻿using Domain.Entities;
using System.Collections.Generic;

namespace WPF.ViewModels.Datas
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
        private static readonly CondolenceOperation _condolenceOperation = new CondolenceOperation();

        public static CondolenceOperation GetInstance() { return _condolenceOperation; }

        private readonly List<ICondolenceObserver> observers = new List<ICondolenceObserver>();

        private static Condolence OperationData;

        public void Add(ICondolenceObserver observer) { observers.Add(observer); }

        public void Remove(ICondolenceObserver observer) { _ = observers.Remove(observer); }

        public void SetData(Condolence condolence) { OperationData = condolence; }

        public Condolence GetData() { return OperationData; }

        public void Notify()
        {
            foreach (ICondolenceObserver co in observers) { co.CondolenceNotify(); }
        }
    }
}
