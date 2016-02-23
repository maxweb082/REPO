using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LBOM.DataEntity
{
    /// <summary>
    /// 使用者相關資料
    /// </summary>
    public class LoginUserInfoDataEntity
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string loginuserID { get; set; }

        /// <summary>
        /// 使用者登入密碼
        /// </summary>
        public string loginuserPassword { get; set; }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        public string loginuserName { get; set; }

        /// <summary>
        /// 使用者部門代碼
        /// </summary>
        public string deptID { get; set; }

        /// <summary>
        /// 使用者部門名稱
        /// </summary>
        public string deptName { get; set; }
    }
}