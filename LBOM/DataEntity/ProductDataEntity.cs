namespace LBOM.DataEntity
{
    /// <summary>
    /// 店家產品資料物件
    /// </summary>
    public class ProductDataEntity
    {
        /// <summary>
        /// 產品代碼
        /// </summary>
        public string productID { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string productName { get; set; }
        
        /// <summary>
        /// 產品類型代碼
        /// </summary>
        public string productTypeID { get; set; }

        /// <summary>
        /// 所屬店家代碼
        /// </summary>
        public string shopID { get; set; }

        /// <summary>
        /// 產品價格
        /// </summary>
        public int productPrice { get; set; }

        /// <summary>
        /// 產品類型名稱
        /// </summary>
        public string productTypeName { get; set; }
    }
}