using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    public class JangoData
    {
        public string Market { get; set; }        
        public double AvgBuyprice { get; set; }
        public double Profit { get; set; }
        public double ProfitRatio { get; set; }
        public double Quantity { get; set; }
        public double TotalBuyPrice { get; set; }
        public double 평가금액 { get; set; }
        
        public DateTime Time { get; set; }
        public double Close { get; set; }

        public double Fee { get; set; }

        public JangoData(string market, double close, double quantity, DateTime time)
        {
            Market = market;
            Close = close;
            Quantity = quantity;
            Time = time;

            AvgBuyprice = close;
            TotalBuyPrice = close * quantity;
            평가금액 = close * quantity;
            Fee = 0.0;
        }
        public void Buy()
        {
            //Data.Add(new OrderData(OrderType.Purchase, market, close, quantity, time));

        }

        public void UpdateBuy(double price, double quantity)
        {
            var buyPrice = price * quantity;
            var avg = Math.Round((buyPrice + TotalBuyPrice) / (quantity + Quantity), 2);

            AvgBuyprice = avg;
            TotalBuyPrice += buyPrice;
            Quantity = Math.Round(Quantity + quantity, 8);

            Update(price);
        }

        public void UpdateSell(double price, double quantity)
        {
            var sellPrice = price * quantity;
            //var avg = (TotalBuyPrice - sellPrice) / (Quantity - quantity);

            //AvgBuyprice = avg;
            var 수익금 = sellPrice - TotalBuyPrice;
            Quantity = Math.Round(Quantity - quantity, 8);
            TotalBuyPrice = AvgBuyprice * Quantity;
            
            Update(price);

            //if (Quantity < quantity)
            //    return;

            //var 수익률 = Math.Round((((close / AvgBuyprice) - 1) * 100) - Fee, 2);
            //var 수익금 = Math.Round((close * quantity) * (수익률 / 100));

            //var profit = new ProfitHistoryData
            //{
            //    Quantity = quantity,
            //    Market = market,
            //    SellPrice = close,
            //    Time = time,
            //    ProfitRatio = 수익률,
            //    Profit = 수익금,
            //    TotalSellPrice = close * quantity,
            //};

            //ProfitHistory.Add(profit);
        }

        public void Update(double close)
        {
            Close = close;
            평가금액 = Math.Round(Close * Quantity, 2);
            Profit = Math.Round((Close - AvgBuyprice) * Quantity, 2);
            ProfitRatio = Math.Round(((Profit / TotalBuyPrice) * 100) - Fee, 2);
        }

        //public void Update()
        //{
        //    TotalBuyPrice = 0;
        //    Quantity = 0;
        //    Close = close;

        //    Quantity = Math.Round(Quantity, 8);
        //    var 총매수금 = Data.Where(x => x.Type == OrderType.Purchase).Sum(x => x.Price * x.Quantity);
        //    var 총매수량 = Data.Where(x => x.Type == OrderType.Purchase).Sum(x => x.Quantity);

        //    var 총매매금 = 총매수금 - 총매도금;
        //    var 총매매량 = Math.Round(총매수량 - 총매도량, 8);
        //    //var 평단 = 총매수금 / 총매수량;
        //    //AvgBuyprice = (TotalBuyPrice / Quantity);
        //    //AvgBuyprice = Math.Round(총매수금 / 총매수량);

        //    //if (총매매량 == 0)
        //    //{
        //    //    AvgBuyprice = 0;
        //    //    TotalBuyPrice = 0;
        //    //    평가금액 = 0;
        //    //    Profit = 0;
        //    //    _profitRatio = 0;
        //    //    실현손익 = ProfitHistory.Profit;
        //    //    return;
        //    //}


        //    AvgBuyprice = Math.Round(총매매금 / 총매매량);

        //    TotalBuyPrice = AvgBuyprice * Quantity;
        //    평가금액 = Math.Round(Close * Quantity);

        //    //ProfitRatio = Math.Round((((CurrentPrice / AvgBuyprice) - 1) * 100) - Fee, 2);
        //    //Profit = ((AvgBuyprice - CurrentPrice) * Quantity) * 0.0005f;
        //    //Profit = Math.Round(TotalBuyPrice * (ProfitRatio / 100));
        //    Profit = (Close - AvgBuyprice) * Quantity;
        //    _profitRatio = Math.Round(((Profit / TotalBuyPrice) * 100) - Fee, 2);

        //    실현손익 = ProfitHistory.Profit;
        //}
    }
}
