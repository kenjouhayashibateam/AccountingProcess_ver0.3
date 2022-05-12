using System.Collections.Generic;

namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 前日決算、預り金をメモリに保持して監視するインターフェイス
    /// </summary>
    public interface IOriginalTotalAmountObserver
    {
        void OriginalTotalAmoutNotify();
    }
    /// <summary>
    /// 経理担当場所リスト
    /// </summary>
    public enum Locations
    {
        管理事務所,
        青蓮堂
    }
    /// <summary>
    /// 経理担当場所（シングルトン）
    /// </summary>
    public sealed class AccountingProcessLocation
    {
        private static readonly AccountingProcessLocation accountingLocation =
            new AccountingProcessLocation();
        private readonly List<IOriginalTotalAmountObserver> Observers =
            new List<IOriginalTotalAmountObserver>();
        private static int originalTotalAmount;

        public void Add(IOriginalTotalAmountObserver originalTotalAmountObserver)
        { Observers.Add(originalTotalAmountObserver); }
        public void Remove(IOriginalTotalAmountObserver originalTotalAmountObserver)
        { _ = Observers.Remove(originalTotalAmountObserver); }
        private void Notify()
        { foreach (IOriginalTotalAmountObserver ota in Observers) { ota.OriginalTotalAmoutNotify(); } }
        /// <summary>
        /// 担当場所
        /// </summary>
        public static Locations Location { get; set; }
        /// <summary>
        /// 金庫計算、出納登録前の金額
        /// </summary>
        public static int OriginalTotalAmount
        {
            get => originalTotalAmount;
            set
            {
                if (originalTotalAmount == value) { return; }
                originalTotalAmount = value;
                accountingLocation.Notify();
            }
        }
        /// <summary>
        /// 担当場所インスタンス
        /// </summary>
        /// <returns></returns>
        public static AccountingProcessLocation GetInstance() { return accountingLocation; }
        /// <summary>
        /// 担当場所を設定します
        /// </summary>
        /// <param name="location">場所</param>
        public static void SetLocation(Locations location) { Location = location; }
        /// <summary>
        /// 春秋苑の会計かﾜｲｽﾞｺｱの会計か
        /// </summary>
        public static bool IsAccountingGenreShunjuen { get; set; }
        /// <summary>
        /// 会計のジャンル文字列を返す
        /// </summary>
        public static string GetAccountingGenreString =>
            IsAccountingGenreShunjuen ? "春秋苑会計" : "ワイズコア会計";
        /// <summary>
        /// 金種表を出力したか
        /// </summary>
        public static bool IsCashBoxOutputed { get; set; }
        /// <summary>
        /// 収支日報を出力したか
        /// </summary>
        public static bool IsBalanceAccountOutputed { get; set; }
    }
}
