﻿using bitup.Cmm.Enum;
using bitup.Cmm.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
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
            var buy = new OrderData(OrderType.Purchase, market, close, quantity, time);
            ReceiveOrderDataHandler?.Invoke(this, buy);

            //var makeOrder = Manager.Instance.MakeOrder(market, Manager.UpbitOrderSide.bid, Convert.ToDecimal(quantity), Convert.ToDecimal(close));
            
            //if (makeOrder == null)
            //    return;

            //var respons = JObject.Parse(makeOrder);
            //var buy = new OrderData(OrderType.Purchase, market, close, quantity, time, respons["uuid"].ToString());
            //ReceiveOrderDataHandler?.Invoke(this, buy);
        }

        public void Sell(string market, double close, double quantity, DateTime time)
        {
            var sell = new OrderData(OrderType.Sell, market, close, quantity, time);
            ReceiveOrderDataHandler?.Invoke(this, sell);
        }
    }
}
