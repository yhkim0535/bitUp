using bitup.Cmm;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace bitupTrade
{
    class Program
    {
        
        public static CandleStick candleStic;

        public static InfinityTrade _it;

        public static string Ticker { get; set; }

        private static Timer _timer;
        private static DateTime _preTime;

        private static bool _firstOrdered;
        static void Main(string[] args)
        {
            Ticker = "KRW-STRAX";
            _preTime = new DateTime(2000, 1, 1, 9, 0, 0);
            

            Init();

        }

        public static void Init()
        {
            //_it = new InfinityTrade(1000000, 25000);
            _it = new InfinityTrade(400000, 12000);
            Manager.Instance.SetKeys("1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx", "OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P");
            candleStic = new CandleStick();


            //_timer = new Timer { Interval = 3000 };
            //_timer.Elapsed += _timer_Elapsed;
            //_timer.Start();
            
            while(true)
            {
                _timer_Elapsed();
                System.Threading.Thread.Sleep(10000);
            }
            

            //_timer.Stop();
        }


        private static void _timer_Elapsed()
        //private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {


            //var candleCount = 10;
            //var candlesMinute = Manager.Instance.GetCandles_Minute(Ticker, Manager.UpbitMinuteCandleType._3, to: DateTime.Now.AddMinutes(0), count: candleCount);
            //candleStic.SetCandle(candlesMinute);

            //CandleStick.Instance.SetCandle(candlesMinute);
            //candleStic.SetCandle2(Ticker, Manager.UpbitMinuteCandleType._15, to: DateTime.Now.AddMinutes(0), count: candleCount);

            ////idea 1.
            //// 당일 체결 내역 조회하여 시간 얻어옴(거래량이 적은것은 현재 시간과 안맞을수 잇음)
            //var ticks = Manager.Instance.GetTicks(Ticker, count: 2);
            //var tickData = JArray.Parse(ticks);
            //var currentTime = Convert.ToDateTime(tickData[0]["trade_time_utc"]).AddHours(9);

            //idea 2.
            var ticker = Manager.Instance.GetTicker(Ticker);
            var tickerChar = JArray.Parse(ticker)[0]["trade_time_kst"].ToString();//.Substring(4, 2)
            var currentTime = DateTime.ParseExact(tickerChar, "HHmmss", CultureInfo.InvariantCulture);

            var barInterval = 15;
            var subMinute = currentTime.Minute % barInterval;
            var result = (subMinute == barInterval - 1 && currentTime.Second > 45);//15분봉기준 현재시간이 14분 45초가 넘을경우 매수 진행

            //var market = candleStic.Candles[0].market;
            //var close = candleStic.Candles[0].close;
            //var candleTime = DateTime.Parse(candleStic.Candles[0].candle_date_time_kst);

            // 시세 호가 정보(Orderbook) 조회 - 1호가 위 가격 주문
            var orderBook = Manager.Instance.GetOrderbook(Ticker);
            var hoga = Convert.ToDouble(JArray.Parse(orderBook)[0]["orderbook_units"][1]["ask_price"].ToString());
            var market = Ticker;
            var close = hoga;
            var candleTime = currentTime;


            _it.Update(market, close);

            //if (_preTime >= candleTime)
            //    return;


            if (!result)
            {
                _firstOrdered = false;
                return;
            }
            

            if (_firstOrdered) return;

            

            //close = hoga;

            if (_it.count == 0)
                _it.BuyDown(market, close, candleTime);
            else if (_it.Get평단() < close && close < _it.Get평단() * 1.025 && _it.count < 20)
                _it.BuyUp(market, close, candleTime);
            else if (close < _it.Get평단() * 1 && _it.count < 39.5)
                _it.BuyDown(market, close, candleTime);
            else
                Console.WriteLine("[{0}] - {1} 매수조건 없음", _it.count, currentTime);

            if (_it.count > 20)
            {
                if (_it.Get수익률() > 2)
                    _it.SellAll(market, close, candleTime);
                else if (_it.Get수익률() > 1)
                    _it.SellHalf(market, close, candleTime);
            }
            else if (_it.count <= 20)
            {
                if (_it.Get수익률() > 3)
                    _it.SellAll(market, close, candleTime);
            }

            //Console.WriteLine("[{0}] - {1} : 매수 주문 [{2}.  Profit: {3}({4})]", _it.count, currentTime, hoga, _it._jango.Profit, _it.Get수익률());



            //_preTime = candleTime;
            _firstOrdered = true;
        }

        
    }
}
