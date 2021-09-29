using bitupAPI.Enum;
using bitupAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    /// <summary>
    /// 이벤트 형태로 바꾸자... sell, buy 이벤트
    /// </summary>
    public class Jango
    {
        public List<OrderData> Data { get; set; }

        public double CurrentPrice { get; set; }

        /// <summary>
        /// 총 매수금액
        /// </summary>
        public double TotalBuyPrice { get; set; }
        /// <summary>
        /// 매수 평균가
        /// </summary>
        public double AvgBuyprice { get; set; }

        public double 평가금액 { get; set; }

        /// <summary>
        /// 총 보유수량
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// 평가손익
        /// </summary>
        public double Profit { get; set; }
        /// <summary>
        /// 수익률
        /// </summary>
        public double _profitRatio;

        public double 실현손익 { get; set; }

        public double Fee = 0.0;    //0.0005f

        public Jango()
        {
            Data = new List<OrderData>();
        }

        public void Add (OrderData data)
        {
            Data.Add(data);
        }

        public void Add(List<OrderData> data)
        {
            Data.AddRange(data);
        }

        public void Buy(string market, double close, double quantity, DateTime time)
        {
            var buy = new OrderData(OrderType.Purchase, market, close, quantity, time);
            Data.Add(buy);

            Update(close);
        }

        public void Sell(string market, double close, double quantity, DateTime time)
        {
            if (Quantity < quantity)
                return;
            
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

            var sell = new OrderData(OrderType.Sell, market, close, quantity, time, 수익금, 수익률);
            Data.Add(sell);

            Update(close);
        }

        public void Update(double close)
        {
            TotalBuyPrice = 0;
            Quantity = 0;
            CurrentPrice = close;
            
            foreach(var item in Data)
            {
                //var price = item.Price * item.Quantity;

                if(item.Type == OrderType.Purchase)
                {
                    //TotalBuyPrice += item.총매수금액;
                    Quantity += item.Quantity;
                }
                else if(item.Type == OrderType.Sell)
                {
                    //TotalBuyPrice += item.총매도금액;
                    Quantity -= item.Quantity;
                }
            }

            Quantity = Math.Round(Quantity, 8);
            var 총매수금 = Data.Where(x => x.Type == OrderType.Purchase).Sum(x => x.Price * x.Quantity);
            var 총매수량 = Data.Where(x => x.Type == OrderType.Purchase).Sum(x => x.Quantity);
            var 총매도금 = Data.Where(x => x.Type == OrderType.Sell).Sum(x => x.Price * x.Quantity);
            var 총매도량 = Data.Where(x => x.Type == OrderType.Sell).Sum(x => x.Quantity);

            var 총매매금 = 총매수금 - 총매도금;
            var 총매매량 = Math.Round(총매수량 - 총매도량, 8);
            //var 평단 = 총매수금 / 총매수량;
            //AvgBuyprice = (TotalBuyPrice / Quantity);
            //AvgBuyprice = Math.Round(총매수금 / 총매수량);

            if (총매매량 == 0)
            {
                AvgBuyprice = 0;
                TotalBuyPrice = 0;
                평가금액 = 0;
                Profit = 0;
                _profitRatio = 0;
                실현손익 = ProfitHistory.Profit;
                return;
            }


            AvgBuyprice = Math.Round(총매매금 / 총매매량);

            TotalBuyPrice = AvgBuyprice * Quantity;
            평가금액 = Math.Round(CurrentPrice * Quantity);

            //ProfitRatio = Math.Round((((CurrentPrice / AvgBuyprice) - 1) * 100) - Fee, 2);
            //Profit = ((AvgBuyprice - CurrentPrice) * Quantity) * 0.0005f;
            //Profit = Math.Round(TotalBuyPrice * (ProfitRatio / 100));
            Profit = (CurrentPrice - AvgBuyprice) * Quantity;
            _profitRatio = Math.Round(((Profit / TotalBuyPrice) * 100) - Fee, 2);

            실현손익 = ProfitHistory.Profit;
        }
    }
}
