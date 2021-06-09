namespace WPF.Views.Datas
{
    /// <summary>
    /// ウィンドウを閉じる際のキャンセル動作を制御するクラス
    /// </summary>
    public interface IClosing
    {
        bool OnClosing();
    }
}
