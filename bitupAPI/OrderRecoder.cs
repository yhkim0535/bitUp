using bitupAPI.Enum;
using bitupAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    public static class OrderRecoder
    {
        public static List<OrderData> Data { get; set; } = new List<OrderData>();

        public static double CurrentPrice { get; set; }

        /// <summary>
        /// 총 매수금액
        /// </summary>
        public static double TotalBuyPrice { get; set; }
        /// <summary>
        /// 매수 평균가
        /// </summary>
        public static double AvgBuyprice { get; set; }

        public static double 평가금액 { get; set; }

        /// <summary>
        /// 총 보유수량
        /// </summary>
        public static double Quantity { get; set; }
        /// <summary>
        /// 평가손익
        /// </summary>
        public static double Profit { get; set; }
        /// <summary>
        /// 수익률
        /// </summary>
        public static double _profitRatio;

        public static double 실현손익 { get; set; }

        public static double Fee = 0.0;    //0.0005f

        //public static OrderRecoder()
        //{
        //    Data = new List<JangoData>();
        //}

        public static void Add(OrderData data)
        {
            Data.Add(data);
        }

        public static void Add(List<OrderData> data)
        {
            Data.AddRange(data);
        }

        public static void Buy(string market, double close, double quantity, DateTime time)
        {
            Data.Add(new OrderData(OrderType.Purchase, market, close, quantity, time));
        }

        public static void Sell(string market, double close, double quantity, DateTime time)
        {
            var 수익률 = Math.Round((((close / AvgBuyprice) - 1) * 100) - Fee, 2);
            var 수익금 = Math.Round((close * quantity) * (수익률 / 100));

            var profit = new ProfitHistoryData
            {
                Quantity = quantity,
                Market = market,
                SellPrice = close,
                Time = time,
                ProfitRatio = 수익률,
                Profit = 수익금,
                TotalSellPrice = close * quantity,
            };

            ProfitHistory.Add(profit);

            Data.Add(new OrderData(OrderType.Sell, market, close, quantity, time, 수익금, 수익률));
        }    
    }
}
