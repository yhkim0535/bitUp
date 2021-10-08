using bitup.Cmm.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitup.Cmm
{
    public class CandleStick
    {
        private static CandleStick _instance;

        public static CandleStick Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CandleStick();
                return _instance;
            }
        }

        public List<CandleData> Candles { get; set; }

        public CandleStick()
        {
            Candles = new List<CandleData>();
        }

        public void SetCandle(string candles)
        {
            Candles.Clear();

            //var candlesMinute = Manager.Instance.GetCandles_Minute("KRW-BCH", Manager.UpbitMinuteCandleType._15, to: DateTime.Now.AddMinutes(-2), count: 200);
            //candleRoot.First["candle_date_time_kst"]
            var candleRoot = JArray.Parse(candles);

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
                Candles.Add(candle);
            }
            //TechnicalAnalysis.GetRsi(candleStic.Candles, 14);
        }

        public void SetCandle2(string market2, Manager.UpbitMinuteCandleType unit, DateTime to = default(DateTime), int count = 1)
        {
            //var candlesMinute = Manager.Instance.GetCandles_Minute("KRW-BCH", Manager.UpbitMinuteCandleType._15, to: DateTime.Now.AddMinutes(-2), count: 200);
            var candlesMinute = Manager.Instance.GetCandles_Minute(market2, unit, to: DateTime.Now.AddMinutes(0), count: count);

            var candleRoot = JArray.Parse(candlesMinute);

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
                Candles.Add(candle);
            }
            //TechnicalAnalysis.GetRsi(candleStic.Candles, 14);
        }
    }
}
