namespace LBOM.DataEntity
{
    /// <summary>
    /// 訂購項目資料物件
    /// </summary>
    public class OrderItemDataEntity
    {
        /// <summary>
        /// 訂購項目代碼
        /// </summary>
        public string orderItemID { get; set; }

        /// <summary>
        /// 訂購代碼
        /// </summary>
        public string orderID { get; set; }

        /// <summary>
        /// 訂購者代碼
        /// </summary>
        public string orderItemLoginuserID { get; set; }

        /// <summary>
        /// 餐點代碼
        /// </summary>
        public string productID { get; set; }

        /// <summary>
        /// 訂購數量
        /// </summary>
        public int orderItemQuantity { get; set; }

        /// <summary>
        /// 訂購項目建立時間
        /// </summary>
        public string orderItemCreateDatetime { get; set; }
    }
}