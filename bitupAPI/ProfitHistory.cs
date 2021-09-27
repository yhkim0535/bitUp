using bitupAPI.Enum;
using bitupAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    public static class ProfitHistory
    {
        public static List<ProfitHistoryData> Data { get; set; }

        /// <summary>
        /// 수익/손실 액
        /// </summary>
        public static double Profit { get; private set; }
        /// <summary>
        /// 수익/손실 비율
        /// </summary>
        public static double ProfitRatio { get; private set; }

        public static double 매도한비용 { get; private set; }

        public static void Add(ProfitHistoryData item)
        {
            if (Data == null)
                Data = new List<ProfitHistoryData>();

            Data.Add(item);
            Update();
        }

        private static void Update()
        {
            Profit = 0;
            매도한비용 = 0;
            ProfitRatio = 0;

            foreach (var item in Data)
            {
                Profit += Math.Round(item.Profit);
                매도한비용 += Math.Round(item.TotalSellPrice);
                //ProfitRatio += Math.Round(item.ProfitRatio, 2);
                ProfitRatio += item.ProfitRatio;
            }

            ProfitRatio = Math.Round(ProfitRatio / Data.Count, 2);
        }

    }
}
