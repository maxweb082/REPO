using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LBOM.DataEntity
{
    /// <summary>
    /// 部門相關資料
    /// </summary>
    public class DeptDataEntity
    {
        /// <summary>
        /// 部門代碼
        /// </summary>
        public string deptID { get; set; }

        /// <summary>
        /// 部門縮寫
        /// </summary>
        public string deptAbbreviate { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        public string deptName { get; set; }

        /// <summary>
        /// 部門員工名稱
        /// </summary>
        public string loginuserName { get; set; }
    }
}