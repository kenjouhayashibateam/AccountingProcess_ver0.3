using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Datas
{
    public class ReceiptsAndExpenditureBase
    {
        /// <summary>
        /// 出納ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// データ登録日
        /// </summary>
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// 登録担当者
        /// </summary>
        public Rep RegistrationRep { get; set; }
        /// <summary>
        /// 担当場所
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 貸方部門
        /// </summary>
        public CreditDept CreditDept { get; set; }
        /// <summary>
        /// 内容詳細
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 単位付き金額
        /// </summary>
        public string PriceWithUnit => TextHelper.AmountWithUnit(Price);
        /// <summary>
        /// 有効性
        /// </summary>
        public bool IsValidity { get; set; }
        /// <summary>
        /// 入出金日
        /// </summary>
        public DateTime AccountActivityDate { get; set; }
        /// <summary>
        /// 出力日
        /// </summary>
        public DateTime OutputDate { get; set; }
        /// <summary>
        /// 未出力かのチェック
        /// </summary>
        public bool IsUnprinted { get; set; }
        /// <summary>
        /// 軽減税率かのチェック
        /// </summary>
        public bool IsReducedTaxRate { get; set; }
    }
}
