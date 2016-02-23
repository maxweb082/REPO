using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LBOM.DataEntity
{
    public class OrderDataEntity
    {
        /// <summary>
        /// 訂購代碼
        /// </summary>
        public string orderID { get; set; }

        /// <summary>
        /// 店家代碼
        /// </summary>
        public string shopID { get; set; }

        /// <summary>
        /// 店家名稱
        /// </summary>
        public string shopName { get; set; }

        /// <summary>
        /// 訂購發起人代碼
        /// </summary>
        public string orderLoginuserID { get; set; }

        /// <summary>
        /// 訂購發起人名稱
        /// </summary>
        public string orderLoginuserName { get; set; }

        /// <summary>
        /// 訂購起始時間
        /// </summary>
        public DateTime orderStartDatetime { get; set; }

        /// <summary>
        /// 訂購結束時間
        /// </summary>
        public DateTime orderCloseDatetime { get; set; }

        /// <summary>
        /// 訂購描述
        /// </summary>
        public string orderDescript { get; set; }

        /// <summary>
        /// 是否已關閉
        /// </summary>
        public string isClosed { get; set; }
    }
}