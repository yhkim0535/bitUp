using bitup.Cmm.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitup.Cmm.Model
{
    public class ProfitHistoryData
    {
        public ProfitType Type { get; set; }

        public string Market { get; set; }
        public DateTime Time { get; set; }
        /// <summary>
        /// 판매시 가격
        /// </summary>
        public double SellPrice { get; set; }
        /// <summary>
        /// 총 판매금액
        /// </summary>
        public double TotalSellPrice { get; set; }
        /// <summary>
        /// 수량
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// 수익/손실 액
        /// </summary>
        public double Profit { get; set; }
        /// <summary>
        /// 수익/손실 비율
        /// </summary>
        public double ProfitRatio { get; set; }

    }
}