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

        private double _적립;

        public VR(double pool, double feed)
        {
            Pool = pool;
            Feed = feed;
            _적립 = feed;
            _data = new Jango();
        }

        public void AddFeed(double feed)
        {
            Pool += feed;
        }

        public double Get평가금()
        {
            return _data.평가금액;
        }

        /// <summary>
        /// 최초매수
        /// </summary>
        public void FirstBuy(string market, double close, double quantity, DateTime time)
        {
            _data.Buy(market, close, quantity, time);
            Pool -= close * quantity;
            V = _data.평가금액;
            MinV = V * 0.8;
            MaxV = V * 1.2;
        }

        public void Buy(string market, double close, DateTime time)
        {
            var price = MinV - _data.평가금액;
            var quantity = price / close;
            
            if (close * quantity > Pool)
                quantity = Pool / close;

            _data.Buy(market, close, quantity, time);
            Pool -= close * quantity;
        }

        public void Sell(string market, double close, DateTime time)
        {
            var price = _data.평가금액 - MaxV;
            var quantity = price / close;
            _data.Sell(market, close, quantity, time);
            Pool += close * quantity;
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="close">종가</param>
        /// <param name="feed">인출 / 입금 금액</param>
        public void NewCycle(double close, double feed)
        {
            Pool += feed;
            ComputV(close, feed);
        }

        public double GetV()
        {
            //return _data.평가금액;
            return V;
        }

        private void ComputV(double close, double feed)
        {
            _data.Update(close);
            V = V * ComputeUpRatio() + feed;
            MinV = V * 0.8;
            MaxV = V * 1.2;
        }

        public void Update(double close)
        {
            _data.Update(close);
        }

        /// <summary>
        /// 상승률을 구한다
        /// </summary>
        /// <returns></returns>
        public double ComputeUpRatio()
        {
            var upRato = 0.0;
            var factor = Pool / V;

            if (factor > 0.55)
            {
                if (_data.평가금액 > V)
                    upRato = 1.06;
                else
                    upRato = 1.055;
            }
            else if (factor > 0.5)
            {
                if (_data.평가금액 > V)
                    upRato = 1.055;
                else
                    upRato = 1.05;
            }
            else if (factor > 0.45)
            {
                if (_data.평가금액 > V)
                    upRato = 1.05;
                else
                    upRato = 1.045;
            }
            else if (factor > 0.4)
            {
                if (_data.평가금액 > V)
                    upRato = 1.045;
                else
                    upRato = 1.04;
            }
            else if (factor > 0.35)
            {
                if (_data.평가금액 > V)
                    upRato = 1.04;
                else
                    upRato = 1.035;
            }
            else if (factor > 0.3)
            {
                if (_data.평가금액 > V)
                    upRato = 1.35;
                else
                    upRato = 1.03;
            }
            else if (factor > 0.25)
            {
                if (_data.평가금액 > V)
                    upRato = 1.03;
                else
                    upRato = 1.025;
            }
            else if (factor > 0.2)
            {
                if (_data.평가금액 > V)
                    upRato = 1.025;
                else
                    upRato = 1.02;
            }
            else if (factor > 0.15)
            {
                if (_data.평가금액 > V)
                    upRato = 1.02;
                else
                    upRato = 1.015;
            }
            else if (factor > 0.1)
            {
                if (_data.평가금액 > V)
                    upRato = 1.015;
                else
                    upRato = 1.01;
            }
            else if (factor > 0.05)
            {
                if (_data.평가금액 > V)
                    upRato = 1.01;
                else
                    upRato = 1.005;
            }
            else if (factor > 0.01)
            {
                if (_data.평가금액 > V)
                    upRato = 1.005;
                else
                    upRato = 1.001;
            }
            else if (factor < 0.01)
            {
                upRato = 1.001;
            }
            return upRato;
        }

    }
}
