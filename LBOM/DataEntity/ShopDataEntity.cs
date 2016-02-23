using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace LBOM.DataEntity
{
    /// <summary>
    /// 店家資訊資料物件
    /// </summary>
    public class ShopDataEntity
    {
        /// <summary>
        /// 店家代碼
        /// </summary>
        public string shopID { get; set; }

        /// <summary>
        /// 店家名稱
        /// </summary>
        public string shopName { get; set; }

        /// <summary>
        /// 店家電話
        /// </summary>
        public string shopTEL { get; set; }

        /// <summary>
        /// 店家地址
        /// </summary>
        public string shopAddress { get; set; }

        /// <summary>
        /// 備註資訊
        /// </summary>
        public string shopRemark { get; set; }
    }
}