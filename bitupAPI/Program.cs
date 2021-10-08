using System;
using System.Globalization;
using bitup.Cmm;
using bitup.Cmm.Model;
using Newtonsoft.Json.Linq;

namespace bitupAPI
{
    public class Program
    {
        static public Jango jango;
        static public CandleStick candleStic;
        static public VR _vr;
        static public InfinityTrade _it;

        private static int _매수금액 = 6000;

        /// Access key - 1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx
        /// Secret key - OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P
        static void Main(string[] args)
        {
            //var U = new manager("accessKey 입력", "secretKey 입력");
            jango = new Jango();
            candleStic = new CandleStick();
            _vr = new VR(10000000, 250000);
            _it = new InfinityTrade(10000000, 250000);

            //jsonTest();
            //jangoTest();
            //vrTest();
            //InfinityTest();

            basic();
        }

        public static void GetCandle()
        {
            Manager.Instance.SetKeys("1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx", "OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P");
            //var U = new Manager("1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx", "OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P");
            var candlesMinute = Manager.Instance.GetCandles_Minute("KRW-BCH", Manager.UpbitMinuteCandleType._1, to: DateTime.Now.AddMinutes(0), count: 200);
            var candleRoot = JArray.Parse(candlesMinute);
            //candleRoot.First["candle_date_time_kst"]

            foreach (var item in candleRoot)
            {
                var market = item["market"];
                var time = item["candle_date_time_kst"];
                var open = item["opening_price"];
                var high = item["high_price"];
                var low = item["low_price"];
                var close = item["trade_price"];
                var timestamp = item["timestamp"];
                var volumePrice = item["candle_acc_trade_price"];
                var volume = item["candle_acc_trade_volume"];
                //var unit = item["unit"];

                var candle = new CandleData();
                candle.market = market.ToString();
                candle.candle_date_time_kst = time.ToString();
                candle.open = Convert.ToDouble(open.ToString());
                candle.high = Convert.ToDouble(high.ToString());
                candle.low = Convert.ToDouble(low.ToString());
                candle.close = Convert.ToDouble(close.ToString());

                //candle.timestamp = int.Parse(timestamp.ToString());
                candle.volumePrice = Convert.ToDouble(volumePrice.ToString());
                candle.volume = Convert.ToDouble(volume.ToString());

                candleStic.Candles.Add(candle);

                //var jangoItem = new JangoData(new DateTime(), P(price), 1);
            }

            //TechnicalAnalysis.GetRsi(candleStic.Candles, 14);
        }

        public static void InfinityTest()
        {
            GetCandle();

            var count = 96;
            var preClose = 80000000.0;// candleStic.Candles[count - 1].close;
            //_it.BuyDown(candleStic.Candles[count].market, candleStic.Candles[count].close, DateTime.Parse(candleStic.Candles[count].candle_date_time_kst));
            var market = candleStic.Candles[0].market;


            for (int i = 1; i < count; i++)
            {
                var close = candleStic.Candles[count - i].close;
                var updown = (candleStic.Candles[count - i].close - preClose) / preClose * 100;

                _it.Update(market, close);
                                
                
                if(_it.count == 0)
                    _it.BuyDown(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
                else if (_it.Get평단() < close && close < _it.Get평단() * 1.025 && _it.count < 20)
                    _it.BuyUp(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
                else if (close < _it.Get평단() * 1 && _it.count < 39.5)
                    _it.BuyDown(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));

                if(_it.count > 20)
                {
                    if(_it.Get수익률() > 2)
                        _it.SellAll(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
                    else if (_it.Get수익률() > 1)
                        _it.SellHalf(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
                }
                else if(_it.count <= 20)
                {
                    if (_it.Get수익률() > 3)
                        _it.SellAll(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
                }
                
                preClose = candleStic.Candles[count - i].close;
            }

            var span = new TimeSpan(_it._jango.Data[0].Time.Hour, _it._jango.Data[0].Time.Minute, _it._jango.Data[0].Time.Second - 3);
            var term = new TimeSpan(0, 0, 45);
            var sub = span.Minutes % 15;
            var result = (sub == 14 && span.Seconds > term.Seconds);
            

            var aa = ProfitHistory.Data;
        }

        public static void vrTest()
        {
            GetCandle();

            //var count = candleStic.Candles.Count;
            var count = 96;
            var preClose = 80000000.0;// candleStic.Candles[count - 1].close;
            var quantity = 5000000 / candleStic.Candles[count - 1].close;
            //_vr.FirstBuy(candleStic.Candles[count - 1].market, candleStic.Candles[count - 1].close, quantity, DateTime.Parse(candleStic.Candles[count - 1].candle_date_time_kst));
            _vr.FirstBuy(candleStic.Candles[count - 1].market, candleStic.Candles[count - 1].close, quantity, DateTime.Parse(candleStic.Candles[count - 1].candle_date_time_kst));


            for (int i = 2; i < count; i++)
            {
                var close = candleStic.Candles[count - i].close;
                var updown = (candleStic.Candles[count - i].close - preClose) / preClose * 100;

                _vr.Update(close);

                if( i % 10 == 0)
                    _vr.NewCycle(close, 250000);

                //현재봉이 2%이상 상승시 매도주문
                if (_vr.Get평가금() > _vr.MaxV)
                    _vr.Sell(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
                else if (_vr.Get평가금() < _vr.MinV)
                    _vr.Buy(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));

                preClose = candleStic.Candles[count - i].close;
            }
            
        }

        public static void jangoTest()
        {
            jango.Buy("btc", 10000, 10f, DateTime.Now);
            jango.Buy("btc", 9800, 10f, DateTime.Now);
            jango.Buy("btc", 9200, 10f, DateTime.Now);
            jango.Buy("btc", 9300, 10f, DateTime.Now);
            
            jango.Sell("btc", 9400, 10f, DateTime.Now);
            jango.Buy("btc", 9200, 10f, DateTime.Now);
            jango.Buy("btc", 8800, 10f, DateTime.Now);
            jango.Sell("btc", 9200, 10f, DateTime.Now);
            jango.Buy("btc", 9000, 10f, DateTime.Now);


            jango.Update(9500);

            Console.ReadLine();
        }

        public static void jsonTest()
        {
            Manager.Instance.SetKeys("1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx", "OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P");
            //var U = new Manager("1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx", "OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P");

            #region 자산
            //// 자산 조회
            //var account = U.GetAccount();
            //var root = JArray.Parse(account);
            //foreach (var item in root)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(account);
            #endregion


            #region 시세 정보
            // 마켓 코드 조회
            //Console.WriteLine(U.GetMarkets());

            // 캔들(분, 일, 주, 월) 조회
            //Console.WriteLine(U.GetCandles_Week("KRW-BTC", to: DateTime.Now.AddDays(-14), count: 2));
            //Console.WriteLine(U.GetCandles_Month("KRW-BTC", to: DateTime.Now.AddMonths(-2), count: 2));

            //var candlesDay = U.GetCandles_Day("KRW-BTC", to: DateTime.Now.AddDays(-2), count: 200);
            //Console.WriteLine(candlesDay);


            var candlesMinute = Manager.Instance.GetCandles_Minute("KRW-QTUM", Manager.UpbitMinuteCandleType._30, to: DateTime.Now.AddMinutes(-2), count: 200);
            //var candlesMinute = U.GetCandles_Day("KRW-BTC", to: DateTime.Now.AddDays(0), count: 200);

            //var test = JObject.Parse(candlesMinute);
            //var rootObject = JsonConvert.DeserializeObject<CandlesMinute>(candlesMinute);



            var candleRoot = JArray.Parse(candlesMinute);


            //foreach(JObject item in candleRoot)
            //{
            //    var name = item.GetValue("trade_price").ToString();
            //}
            
            foreach (var item in candleRoot)
            {
                var market = item["market"];
                var time = item["candle_date_time_kst"];
                var open = item["opening_price"];
                var high = item["high_price"];
                var low = item["low_price"];
                var close = item["trade_price"];
                var timestamp = item["timestamp"];
                var volumePrice = item["candle_acc_trade_price"];
                var volume = item["candle_acc_trade_volume"];
                //var unit = item["unit"];

                var candle = new CandleData();
                candle.market = market.ToString();
                candle.candle_date_time_kst = time.ToString();
                candle.open = Convert.ToDouble(open.ToString());
                candle.high = Convert.ToDouble(high.ToString());
                candle.low = Convert.ToDouble(low.ToString());
                candle.close = Convert.ToDouble(close.ToString());
                

                //candle.timestamp = int.Parse(timestamp.ToString());
                candle.volumePrice = Convert.ToDouble(volumePrice.ToString());
                candle.volume = Convert.ToDouble(volume.ToString());
                //candle.unit = int.Parse(unit.ToString());
                
                candleStic.Candles.Add(candle);
                

                //var jangoItem = new JangoData(new DateTime(), P(price), 1);
            }


            //var count = candleStic.Candles.Count;
            //for(int i = 1; i < 11; i++)
            //{
            //    jango.Buy(candleStic.Candles[count - i].market, (int)candleStic.Candles[count - i].close, 0.05f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
            //}
            ////jango.Buy(candleStic.Candles[count-1].market, (int)candleStic.Candles[0].close, 0.05f, DateTime.Parse(candleStic.Candles[0].candle_date_time_kst));
            ////jango.Buy(candleStic.Candles[count-2].market, (int)candleStic.Candles[1].close, 0.05f, DateTime.Parse(candleStic.Candles[1].candle_date_time_kst));
            //jango.Update(candleStic.Candles[count - 10].close);

            //jango.Buy(candleStic.Candles[0].market, 62949000, -0.05f, DateTime.Parse(candleStic.Candles[1].candle_date_time_kst));
            //jango.Update(candleStic.Candles[count - 10].close);


            var count = candleStic.Candles.Count;
            var preClose = 80000000.0;// candleStic.Candles[count - 1].close;
            
            for (int i = 1; i <200; i++)
            {
                //if(i==0)
                //    jango.Buy(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, 0.05f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));

                //var 매수수량 = _매수금액 / candleStic.Candles[count - i].close;
                var 매수수량 = 38;

                var updown = (candleStic.Candles[count - i].close - preClose) / preClose * 100;

                //현재봉이 2%이상 상승시 매도주문
                if (updown > 0.9f)
                    jango.Sell(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, 매수수량, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
                else if(updown < -1.5f)
                    jango.Buy(candleStic.Candles[count - i].market, candleStic.Candles[count - i].close, 매수수량, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));

                preClose = candleStic.Candles[count - i].close;
            }
            jango.Update(preClose);


            //jango.Buy("btc", candleStic.Candles[count - i].close, 0.01f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
            //jango.Buy("btc", candleStic.Candles[count - i].close, 0.01f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
            //jango.Buy("btc", candleStic.Candles[count - i].close, 0.01f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
            //jango.Buy("btc", candleStic.Candles[count - i].close, 0.01f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
            //jango.Buy("btc", candleStic.Candles[count - i].close, 0.01f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
            //jango.Sell("btc", candleStic.Candles[count - i].close, 0.01f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
            //jango.Buy("btc", candleStic.Candles[count - i].close, 0.01f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));
            //jango.Sell("btc", candleStic.Candles[count - i].close, 0.01f, DateTime.Parse(candleStic.Candles[count - i].candle_date_time_kst));




            #endregion

            Console.ReadLine();
        }

        public static void basic()
        {
            Manager.Instance.SetKeys("1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx", "OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P");
            //var U = new Manager("1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx", "OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P");
            
            #region 자산
            // 자산 조회
            var account = Manager.Instance.GetAccount();
            var root = JArray.Parse(account);
            foreach (var item in root)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(account);
            #endregion

            #region 주문
            // 주문 가능 정보
            var orderChance = Manager.Instance.GetOrderChance("KRW-BTC");
            //var root = JObject.Parse(orderChance);
            Console.WriteLine(orderChance);

            // 개별 주문 조회
            var order = Manager.Instance.GetOrder("주문 uuid");
            Console.WriteLine(order);

            // 주문 리스트 조회
            var allOrder = Manager.Instance.GetAllOrder();
            Console.WriteLine(allOrder);

            // 주문하기
            //var makeOrder = U.MakeOrder("KRW-BTC", manager.UpbitOrderSide.bid, 0.0001m, 5780000);
            //var makeOrder = Manager.Instance.MakeOrder("KRW-BTC", Manager.UpbitOrderSide.bid, 0.0001m, 57800000);
            //Console.WriteLine(makeOrder);
            OrderManager.Instance.Buy("KRW-BTC", 52800000, 0.0001, DateTime.Now);

            // 주문 취소
            var cancelOrder = Manager.Instance.CancelOrder("주문 uuid");
            Console.WriteLine(cancelOrder);
            #endregion

            #region 시세 정보
            // 마켓 코드 조회
            //Console.WriteLine(U.GetMarkets());

            // 캔들(분, 일, 주, 월) 조회
            var candlesMinute = Manager.Instance.GetCandles_Minute("KRW-BTC", Manager.UpbitMinuteCandleType._1, to: DateTime.Now.AddMinutes(-2), count: 2);
            var candlesDay = Manager.Instance.GetCandles_Day("KRW-BTC", to: DateTime.Now.AddDays(-2), count: 2);
            Console.WriteLine(candlesMinute);
            Console.WriteLine(candlesDay);
            Console.WriteLine(Manager.Instance.GetCandles_Week("KRW-BTC", to: DateTime.Now.AddDays(-14), count: 2));
            Console.WriteLine(Manager.Instance.GetCandles_Month("KRW-BTC", to: DateTime.Now.AddMonths(-2), count: 2));
            
            // 당일 체결 내역 조회
            var ticks = Manager.Instance.GetTicks("KRW-BTC", count: 5);
            Console.WriteLine(ticks);

            // 현재가 정보 조회
            //var ticker = Manager.Instance.GetTicker("KRW-BTC,KRW-ETH");
            var ticker = Manager.Instance.GetTicker("KRW-BTC");
            Console.WriteLine(ticker);
            //var tickerChar = JArray.Parse(ticker)[0]["trade_time_kst"].ToString();//.Substring(4, 2)
            //var currentTime2 = DateTime.ParseExact(tickerChar, "HHmmss", CultureInfo.InvariantCulture);

            // 시세 호가 정보(Orderbook) 조회
            var orderBook = Manager.Instance.GetOrderbook("KRW-BTC,KRW-ETH");
            Console.WriteLine(orderBook);
            #endregion

            Console.ReadLine();
        }
    }
}
