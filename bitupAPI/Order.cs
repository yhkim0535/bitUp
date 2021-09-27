using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI
{
    public class Order
    {
        /// <summary>
        /// 주문번호
        /// </summary>
        public string Uuid { get; set; }
        /// <summary>
        /// 주문 종류
        /// </summary>
        public string Side { get; set; }
        /// <summary>
        /// 주문 방식
        /// </summary>
        public string Ord_type { get; set; }
        /// <summary>
        /// 주문 당시 화폐 가격. NumberString
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 주문 상태
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 마켓 고유키
        /// </summary>
        public string Market { get; set; }
        /// <summary>
        /// 주문 시간
        /// </summary>
        public string Created { get; set; }
        /// <summary>
        /// 주문 양
        /// </summary>
        public string Volume { get; set; }
        /// <summary>
        /// 체결 후 남은 주문 양
        /// </summary>
        public string RemainingVolume { get; set; }
        /// <summary>
        /// 수수료로 예약된 비용
        /// </summary>
        public string ReservedFee { get; set; }
        /// <summary>
        /// 남은 수수료
        /// </summary>
        public string RemainingFee { get; set; }
        /// <summary>
        /// 사용된 수수료
        /// </summary>
        public string PaidFee { get; set; }
        /// <summary>
        /// 거래에 사용중인 비용
        /// </summary>
        public string Locked { get; set; }
        /// <summary>
        /// 체결양
        /// </summary>
        public string ExecutedVolume { get; set; }
        /// <summary>
        /// 주문에 걸린 체결 수
        /// </summary>
        public string TradesCount { get; set; }
        /// <summary>
        /// 체결 시간
        /// </summary>
        public string TradeCreated { get; set; }
    }
}
