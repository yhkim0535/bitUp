using bitupAPI.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    public class JangoData
    {
        public OrderType Type { get; set; }
        public string Market { get; set; }
        public DateTime Time { get; set; }
        public double Price { get; set; }
        /// <summary>
        /// 수량
        /// </summary>
        public double Quantity { get; set; }
        public double 총매수금액 { get; }

        public double 총매도금액 { get; }
        /// <summary>
        /// 전일대비 등락률
        /// </summary>
        public double UpDownRatio { get; set; }

        public double UpDownPrice { get; set; }

        public JangoData(double price, double quantity, DateTime time)
        {            
            Price = price;
            Quantity = quantity;
            Time = time;
            총매수금액 = price * quantity;
        }

        public JangoData(OrderType type, string market, double price, double quantity, DateTime time)
        {
            Type = type;
            Market = market;
            Price = price;
            Quantity = quantity;
            Time = time;

            if(type == OrderType.Purchase)
                총매수금액 = price * quantity;
            else if (type == OrderType.Sell)
                총매도금액 = price * quantity;
        }

    }
}
