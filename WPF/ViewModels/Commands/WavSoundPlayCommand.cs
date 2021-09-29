using System.Media;

namespace WPF.ViewModels.Commands
{
    /// <summary>
    /// wavファイルを操作するメソッド統括クラス
    /// </summary>
    public static class WavSoundPlayCommand
    {
        private static SoundPlayer Player;
        private const string FILEPATH = @"./files/";
        /// <summary>
        /// Playerを再生します
        /// </summary>
        /// <param name="fileName"></param>
        public static void Play(string fileName)
        {
            if (Player == null) { Stop(); }

            Player = new SoundPlayer($"{FILEPATH}{fileName}");
            Player.Play();
        }
        /// <summary>
        /// Playerを停止します
        /// </summary>
        public static void Stop()
        {
            if (Player == null) { return; }
            
            Player.Stop();
            Player.Dispose();
            Player = null;
        }
    }
}
