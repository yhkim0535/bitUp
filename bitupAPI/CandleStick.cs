using bitupAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    public class CandleStick
    {
        public List<CandleData> Candles { get; set; }

        public CandleStick()
        {
            Candles = new List<CandleData>();
        }
    }
}
