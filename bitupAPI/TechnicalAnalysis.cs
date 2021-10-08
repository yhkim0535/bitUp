using bitup.Cmm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    public static class TechnicalAnalysis
    {
        public static List<RsiData> Rsi;// = new List<RsiData>();
        //public static double CalculateRsi(IEnumerable<double> closePrices)
        //{
        //    var prices = closePrices as double[] ?? closePrices.ToArray();

        //    double sumGain = 0;
        //    double sumLoss = 0;
        //    for (int i = 1; i < prices.Length; i++)
        //    {
        //        var difference = prices[i] - prices[i - 1];
        //        if (difference >= 0)
        //        {
        //            sumGain += difference;
        //        }
        //        else
        //        {
        //            sumLoss -= difference;
        //        }
        //    }

        //    if (sumGain == 0) return 0;
        //    if (Math.Abs(sumLoss) < double.Tolerance) return 100;

        //    var relativeStrength = sumGain / sumLoss;

        //    return 100.0 - (100.0 / (1 + relativeStrength));
        //}

        private static void ComputeRsiParam(List<CandleData> data, int period, int stdDay = 0)
        {
            Rsi = new List<RsiData>();

            for (int i=0; i<data.Count - 1; i++)
            {
                var rsi = new RsiData();
                var difference = data[i].close - data[i + 1].close;

                if (difference >= 0)
                    rsi.U = difference;
                else
                    rsi.D = difference;

                rsi.Date = DateTime.Parse(data[i].candle_date_time_kst);
                Rsi.Add(rsi);
            }

            for (int i = 0; i < data.Count; i++)
            {
                if (data.Count < period + i)
                    break;

                Rsi[i].AU = Math.Abs(Rsi.Skip(i).Take(period).Average(x => x.U));
                Rsi[i].AD = Math.Abs(Rsi.Skip(i).Take(period).Average(x => x.D));
            }

            Rsi.ForEach(x => x.RS = x.AU / x.AD);
            Rsi.ForEach(x => x.Value = 100.0 - (100.0 / (1 + x.RS)));
            //Rsi.ForEach(x => x.Value = x.RS / (1 + x.RS));

            var rs1 = Rsi[0].AU / Rsi[0].AD;
            var value1 = 100.0 - (100.0 / (1 + rs1));

            var value2 = rs1 / (1 + rs1);
            var value3 = Rsi[0].AU / (Rsi[0].AU + Rsi[0].AD);


        }

        private static double RsiAu(List<RsiData> data, int period, int stdDay = 0)
        {
            if (data == null)
                return 0;

            if (data.Count < period + stdDay)
                return 0;

            return data.Skip(stdDay).Take(period).Average(x => x.U);
        }

        private static double RsiAd(List<RsiData> data, int period, int stdDay = 0)
        {
            if (data == null)
                return 0;

            if (data.Count < period + stdDay)
                return 0;

            return data.Skip(stdDay).Take(period).Average(x => x.D);
        }

        public static void RsiTest(List<CandleData> data, int period)
        {
            var gain = 0.0;
            var loss = 0.0;

            for (int i = data.Count - 1; data.Count >= period && (i > data.Count - 1 - period); i--)

            {
                var current = data[i].close;
                var prevday = data[i - 1].close;
                
                if (current < prevday)
                    gain += prevday - current;
                else if (current > prevday)
                    loss += current - prevday;

            }

            var avggain = gain / period;
            var avgloss = loss / period;
            var rs = avggain / avgloss;
            var rsi = 100 - (100 / (1 + rs));
        }

        public static void GetRsi(List<CandleData> data, int period, int stdDay = 0)
        {
            ComputeRsiParam(data, period, stdDay);
        }
    }
}
