using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{

    /// <summary>
    /// Value Rebalancing
    /// </summary>
    public class VR
    {
        public double Pool { get; private set; }
        public double MinV { get; set; }
        public double MaxV { get; set; }
        public double CurrentV { get; set; }
        public double V { get; set; }

        public double Feed { get; private set; }

        private Jango _data;

        public VR(double pool, double feed)
        {
            Pool = pool;
            Feed = feed;
            _data = new Jango();
        }

        public void BuyUp(string market, double close, DateTime time)
        {
            var quantity = (Feed / 2) / close;
            _data.Buy(market, close, quantity, time);
            ComputV(close);
        }

        public void BuyDown(string market, double close, DateTime time)
        {
            var quantity = Feed / close;
            _data.Buy(market, close, quantity, time);
            ComputV(close);
        }

        public double GetV()
        {
            return _data.평가금액;
        }

        public void ComputV(double close)
        {
            _data.Update(close);
            V = GetV();
            MinV = V * 0.8;
            MaxV = V * 1.2;
        }

    }
}
