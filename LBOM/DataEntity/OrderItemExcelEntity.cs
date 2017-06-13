using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LBOM.DataEntity
{
    /// <summary>
    /// 訂購項目匯出資料物件
    /// </summary>
    public class OrderItemExportEntity
    {
        /// <summary>
        /// 餐點名稱
        /// </summary>
        public string productName { get; set; }

        /// <summary>
        /// 餐點價格
        /// </summary>
        public int productPrice { get; set; }

        /// <summary>
        /// 訂購數量
        /// </summary>
        public int orderItemQuantity { get; set; }

        /// <summary>
        /// 訂購者
        /// </summary>
        public string loginuserName { get; set; }

        /// <summary>
        /// 訂購金額
        /// </summary>
        public int amount { get; set; }


    }
}