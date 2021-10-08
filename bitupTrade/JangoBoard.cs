using bitup.Cmm.Model;
using bitup.Cmm.Enum;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bitup.Cmm;

namespace bitupTrade
{
    public class JangoBoard
    {
        public ConcurrentDictionary<string, JangoData> TradingList = new ConcurrentDictionary<string, JangoData>();

        public List<JangoData> Data { get; set; }

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

        public JangoBoard()
        {
            Data = new List<JangoData>();
            TradingList = new ConcurrentDictionary<string, JangoData>();
            OrderManager.Instance.ReceiveOrderDataHandler += ReceiveOrderData;
        }

        private void ReceiveOrderData(object sender, OrderData e)
        {
            if(e.Type == OrderType.Purchase)
            {
                var a = 1.0;
            }
            else if(e.Type == OrderType.Sell)
            {
                if (Quantity < e.Quantity)
                    return;
            }

            //실제 체결시 기록
            OrderRecoder.Data.Add(e);
        }

        public void TryAdd(JangoData data)
        {
            //TradingList.ContainsKey(data.Market);

            if (!TradingList.ContainsKey(data.Market))
                TradingList.TryAdd(data.Market, data);
        }

        public void Add(JangoData data)
        {
            Data.Add(data);
        }

        public void Add(List<JangoData> data)
        {
            Data.AddRange(data);
        }

        public void Buy(string market, double close, double quantity, DateTime time)
        {
            Data.Add(new JangoData(market, close, quantity, time));

            Update(market, close);


            //version 2
            if(TradingList.ContainsKey(market))
            {
                if (TradingList.TryGetValue(market, out var tradingItem))
                    tradingItem.UpdateBuy(close, quantity);
            }
            else
            {
                TradingList.TryAdd(market, new JangoData(market, close, quantity, time));
            }
            
        }

        public void Sell(string market, double close, double quantity, DateTime time)
        {
            if (Quantity < quantity)
                return;

            //1. 수익률 계산후 수익금 계산
            //2. 총 매수금액 - 총매도금액 으로 수익금 계산.
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
            
            //Update(close);
            
            var name = Data[0].Market;
            Data.RemoveAll(x => x.Market == name);

            if (Quantity > quantity)
                Data.Add(new JangoData(market, AvgBuyprice, Quantity - quantity, time));

            //Update(close);

            //version 2
            if (TradingList.ContainsKey(market))
            {
                if (TradingList.TryGetValue(market, out var tradingItem))
                {
                    if (tradingItem.Quantity - quantity == 0)
                        TradingList.TryRemove(market, out tradingItem);
                    else
                        tradingItem.UpdateSell(close, quantity);
                }
                    
            }
            else
            {
                TradingList.TryAdd(market, new JangoData(market, close, quantity, time));
            }
        }
        
        public void Update(string market, double close)
        {
            TotalBuyPrice = 0;
            Quantity = Data.Sum(x => x.Quantity);
            CurrentPrice = close;
            
            Quantity = Math.Round(Quantity, 8);
            var 총매수금 = Data.Sum(x => x.Close * x.Quantity);
            var 총매수량 = Data.Sum(x => x.Quantity);
                        
            AvgBuyprice = Math.Round(총매수금 / 총매수량, 2);

            TotalBuyPrice = AvgBuyprice * Quantity;
            평가금액 = Math.Round(CurrentPrice * Quantity);

            //ProfitRatio = Math.Round((((CurrentPrice / AvgBuyprice) - 1) * 100) - Fee, 2);
            //Profit = ((AvgBuyprice - CurrentPrice) * Quantity) * 0.0005f;
            //Profit = Math.Round(TotalBuyPrice * (ProfitRatio / 100));
            Profit = (CurrentPrice - AvgBuyprice) * Quantity;
            _profitRatio = Math.Round(((Profit / TotalBuyPrice) * 100) - Fee, 2);

            실현손익 = ProfitHistory.Profit;


            if (TradingList.ContainsKey(market))
            {
                if (TradingList.TryGetValue(market, out var tradingItem))
                    tradingItem.Update(close);
            }
        }

    }
}
