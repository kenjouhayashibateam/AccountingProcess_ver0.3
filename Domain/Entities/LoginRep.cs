using Domain.Entities.ValueObjects;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// ログインしている職員の変更を通知します
    /// </summary>
    public interface ILoginRepObserver { public void SetRep(Rep rep); }
    /// <summary>
    /// ログインしている経理担当者クラス
    /// </summary>
    public sealed class LoginRep
    {
        //オブザーバーを格納するList
        private readonly List<ILoginRepObserver> observers = new List<ILoginRepObserver>();
        private static readonly LoginRep loginRep = new LoginRep();

        public Rep Rep { get; private set; }

        public static LoginRep GetInstance() { return loginRep; }

        public void SetRep(Rep rep)
        {
            Rep = rep;
            foreach (ILoginRepObserver observer in observers) { observer.SetRep(rep); }
        }

        public void Add(ILoginRepObserver observer) { observers.Add(observer); }

        public void Remove(ILoginRepObserver observer) { _ = observers.Remove(observer); }
    }
}
