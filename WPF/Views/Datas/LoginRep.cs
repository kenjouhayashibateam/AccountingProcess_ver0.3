using Domain.Entities.ValueObjects;
using System.Collections.Generic;

namespace WPF.Views.Datas
{
    public interface ILoginRepObserver
    {
        public void SetRep(Rep rep);
    }

    public sealed class LoginRep
    {
        private readonly List<ILoginRepObserver> observers = new List<ILoginRepObserver>();
        private readonly static LoginRep loginRep = new LoginRep();
        private Rep _rep;

        public Rep Rep => _rep;

        public static LoginRep GetInstance()
        {
            return loginRep;
        }

        public void SetRep(Rep rep)
        {
            _rep = rep;
            foreach (ILoginRepObserver observer in observers)
            {
                observer.SetRep(rep);
            }
        }

        public void Add(ILoginRepObserver observer)
        {
            observers.Add(observer);
        }

        public void Remove(ILoginRepObserver observer)
        {
            observers.Remove(observer);
        }
    }
}
