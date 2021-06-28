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
        private readonly static LoginRep loginRep = new LoginRep();

        private Rep _rep;

        public Rep Rep => _rep;

        public static LoginRep GetInstance() => loginRep;

        public void SetRep(Rep rep)
        {
            _rep = rep;
            foreach (ILoginRepObserver observer in observers) observer.SetRep(rep);
        }

        public void Add(ILoginRepObserver observer) => observers.Add(observer);

        public void Remove(ILoginRepObserver observer) => observers.Remove(observer);
    }
}
