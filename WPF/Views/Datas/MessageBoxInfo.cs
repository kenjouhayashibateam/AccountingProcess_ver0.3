using System.Windows;

namespace WPF.Views.Datas
{
    /// <summary>
    /// メッセージボックス呼び出しクラス
    /// </summary>
    public class MessageBoxInfo
    {
        /// <summary>
        /// 表示するテキスト
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 表示するタイトルバー
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 表示するボタンを指定する値
        /// </summary>
        public MessageBoxButton Button { get; set; } = MessageBoxButton.OK;
        /// <summary>
        /// 表示するアイコンを指定する値
        /// </summary>
        public MessageBoxImage Image { get; set; } = MessageBoxImage.None;
        /// <summary>
        /// 規定の結果を指定する値
        /// </summary>
        public MessageBoxResult DefaultResult { get; set; } = MessageBoxResult.None;
        /// <summary>
        /// オプションを指定する値オブジェクト
        /// </summary>
        public MessageBoxOptions Options { get; set; } = MessageBoxOptions.None;
        /// <summary>
        /// 結果
        /// </summary>
        public MessageBoxResult Result { get; set; } = MessageBoxResult.None;
    }
}
