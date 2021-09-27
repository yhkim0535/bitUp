using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI.Model
{
    public class CandleData
    {
        public string market;
        public string candle_date_time_utc;
        public string candle_date_time_kst;
        public double open;
        public double high;
        public double low;
        public double close;
        public int timestamp;
        public double volumePrice;
        public double volume;
        public int unit;

        public CandleData()
        {

        }
    }
}
