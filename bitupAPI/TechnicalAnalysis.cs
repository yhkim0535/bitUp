using bitupAPI.Model;
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

            for (int i=1; i<data.Count; i++)
            {
                var difference = data[i].close - data[i - 1].close;
                if (difference >= 0)
                    Rsi[i].U = difference;
                else
                    Rsi[i].D = difference;
            }

            for(int i=0; i<data.Count; i++)
            {
                Rsi[0].AU = (int)Rsi.Skip(stdDay).Take(period).Average(x => x.U);
                Rsi[0].AD = (int)Rsi.Skip(stdDay).Take(period).Average(x => x.D);
            }

            Rsi.ForEach(x => x.RS = x.AU / x.AD);
            Rsi.ForEach(x => x.Value = x.RS / (1 + x.RS));
            //Rsi.ForEach(x=> x.Signal = x.Value.)

        }

        public static void GetRsi()
        {

        }
    }
}
