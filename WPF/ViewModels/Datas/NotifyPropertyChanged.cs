using System.ComponentModel;
using System.Diagnostics;

namespace WPF.ViewModels.Datas
{
    /// <summary>
    /// プロパティ変更通知統括クラス  
    /// </summary>
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ変更通知イベントを呼び出します
        /// </summary>
        protected void CallPropertyChanged()
        {
            StackFrame caller = new StackFrame(1);
            string[] methodNames = caller.GetMethod().Name.Split('_');
            int i = methodNames.Length - 1;
            string propertyName = methodNames[i];

            CallPropertyChanged(propertyName);
        }
        /// <summary>
        /// 引数のプロパティの変更を通知します
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
