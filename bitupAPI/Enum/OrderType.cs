using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitupAPI.Enum
{
    public enum OrderType : int
    {
        Purchase = 1,       // 매수
        Sell = 2,           // 매도
        PurchaseCancel = 3, // 매수 취소
        SellCancel = 4,     // 매도 취소
        PurchaseModify = 5, // 매수 정정
        SellModify = 6      // 매도 정정
    }
}
