using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupTrade
{
    /// <summary>
    /// 무한 백테스트
    /// </summary>
    public class InfinityTrade
    {
        public JangoBoard _jango;

        public double Pool { get; private set; }
        public double Feed { get; private set; }
        public double count { get; set; }

        public InfinityTrade(double pool, double feed)
        {
            Pool = pool;
            Feed = feed;
            _jango = new JangoBoard();
        }
        public void Update(string market, double close)
        {
            _jango.Update(market, close);
        }

        public double Get평단()
        {
            return _jango.AvgBuyprice;
        }

        public double Get수익률()
        {
            return _jango._profitRatio;
        }

        public double Get수량()
        {
            return _jango.Quantity;
        }

        /// <summary>
        /// 큰수매수. 평단보다 +5% 이면 매수하지 않음
        /// </summary>
        public void BuyUp(string market, double close, DateTime time)
        {
            var quantity = Math.Round((Feed / 2) / close, 8);

            if (close * quantity > Pool)
                return; // quantity = Pool / close;

            _jango.Buy(market, close, quantity, time);
            Pool -= close * quantity;
            count += 0.5;

            OrderManager.Instance.Buy(market, close, quantity, time);

            Console.WriteLine("[{0}] - {1} : 절반매수 주문 [{2}.  Profit: {3}({4})]", count, time, close, _jango.Profit, Get수익률());
        }

        /// <summary>
        /// 평단매수. 평단보다 아래에서만 매수함
        /// </summary>
        public void BuyDown(string market, double close, DateTime time)
        {
            var quantity = Math.Round(Feed / close, 8);

            if (close * quantity > Pool)
                return; // quantity = Pool / close;

            _jango.Buy(market, close, quantity, time);
            Pool -= close * quantity;
            count += 1;

            OrderManager.Instance.Buy(market, close, quantity, time);

            Console.WriteLine("[{0}] - {1} : 1회매수 주문 [{2}.  Profit: {3}({4})]", count, time, close, _jango.Profit, Get수익률());
        }

        public void SellHalf(string market, double close, DateTime time)
        {
            var quantity = Math.Round(_jango.Quantity / 2, 8);
            Pool += (_jango.평가금액 / 2);
            _jango.Sell(market, close, quantity, time);
            count /= 2;
            OrderManager.Instance.Sell(market, close, quantity, time);

            Console.WriteLine("[{0}] - {1} : Half매도 주문 [{2}.  Profit: {3}({4})]", count, time, close, _jango.Profit, Get수익률());
        }

        public void SellAll(string market, double close, DateTime time)
        {
            var quantity = Math.Round(_jango.Quantity, 8);
            Pool += _jango.평가금액;
            _jango.Sell(market, close, quantity, time);
            count = 0;
            OrderManager.Instance.Sell(market, close, quantity, time);

            Console.WriteLine("[{0}] - {1} : All매도 주문 [{2}.  Profit: {3}({4})]", count, time, close, _jango.Profit, Get수익률());
        }
    }
}
