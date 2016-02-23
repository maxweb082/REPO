namespace LBOM.DataEntity
{
    /// <summary>
    /// 產品類別資料物件
    /// </summary>
    public class ProductTypeDataEntity
    {
        /// <summary>
        /// 產品類別代碼
        /// </summary>
        public string productTypeID { get; set; }

        /// <summary>
        /// 產品類別名稱
        /// </summary>
        public string productTypeName { get; set; }

        /// <summary>
        /// 產品類別顯示順序
        /// </summary>
        public int productTypeDisplayOrder { get; set; }
    }
}