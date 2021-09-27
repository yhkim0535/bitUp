using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    public class CandlesMinute
    {
        public string market;
        public string candle_date_time_utc;
        public string candle_date_time_kst;
        public float opening_price;
        public float high_price;
        public float low_price;
        public float trade_price;
        public int timestamp;
        public float candle_acc_trade_price;
        public float candle_acc_trade_volume;
        public int unit;
    }
}
