using bitup.Cmm;
using bitup.Cmm.Enum;
using bitup.Cmm.Model;
using Newtonsoft.Json.Linq;
using System;

namespace bitupTrade
{
    public class OrderManager
    {
        private static OrderManager _instance;

        public static OrderManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrderManager();
                return _instance;
            }
        }

        public EventHandler<OrderData> ReceiveOrderDataHandler;
                

        public void Buy(string market, double close, double quantity, DateTime time)
        {
            //var buy = new OrderData(OrderType.Purchase, market, close, quantity, time);
            //ReceiveOrderDataHandler?.Invoke(this, buy);

            var makeOrder = Manager.Instance.MakeOrder(market, Manager.UpbitOrderSide.bid, Convert.ToDecimal(quantity), Convert.ToDecimal(close));
            
            var respons = JObject.Parse(makeOrder);
            if (respons["uuid"] == null)
                return;

            var buy = new OrderData(OrderType.Purchase, market, close, quantity, time, respons["uuid"].ToString());
            ReceiveOrderDataHandler?.Invoke(this, buy);
        }

        public void Sell(string market, double close, double quantity, DateTime time)
        {
            //var sell = new OrderData(OrderType.Sell, market, close, quantity, time);
            //ReceiveOrderDataHandler?.Invoke(this, sell);

            var makeOrder = Manager.Instance.MakeOrder(market, Manager.UpbitOrderSide.ask, Convert.ToDecimal(quantity), Convert.ToDecimal(close));

            if (makeOrder == null)
                return;

            var respons = JObject.Parse(makeOrder);
            var sell = new OrderData(OrderType.Purchase, market, close, quantity, time, respons["uuid"].ToString());
            ReceiveOrderDataHandler?.Invoke(this, sell);
        }
    }
}
