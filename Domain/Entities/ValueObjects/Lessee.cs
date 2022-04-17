using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ValueObjects
{
    /// <summary>
    /// 名義人クラス
    /// </summary>
    public class Lessee
    {
        /// <summary>
        /// 管理番号
        /// </summary>
        public string ManagementNumber { get; set; }
        /// <summary>
        /// 名義人名
        /// </summary>
        public string LesseeName { get; set; }
        /// <summary>
        /// 送付先名
        /// </summary>
        public string ReceiverName { get; set; }
        /// <summary>
        /// 墓地番号
        /// </summary>
        public string GraveNumber { get; set; }
        /// <summary>
        /// 面積
        /// </summary>
        public double Area { get; set; }

        public Lessee(string managementNumber, string lesseeName, string receiverName,
            string graveNumber, double area)
        {
            ManagementNumber = managementNumber;
            LesseeName = lesseeName;
            ReceiverName = receiverName;
            GraveNumber = graveNumber;
            Area = area;
        }
    }
}
