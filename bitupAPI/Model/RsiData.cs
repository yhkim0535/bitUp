using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI.Model
{
    public class RsiData
    {
        public DateTime Date { get; set; }

        public double U { get; set; }

        public double D { get; set; }
        public double AU { get; set; }
        public double AD { get; set; }
        public double RS { get; set; }

        /// <summary>
        /// 상승 돌파 여부.
        /// true : 이전값이 음수(-) 이고 현재 값이 양수(+) 인 경우.
        /// </summary>
        public bool Surpass { get; set; }


        public double Value { get; set; }


        public double Signal { get; set; }
    }
    
}
