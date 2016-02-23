using LBOM.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LBOM.Models
{
    /// <summary>
    /// 登入檢視模組
    /// </summary>
    public class LoginViewModel : IValidatableObject
    {
        /// <summary>
        /// 驗證前的要求URL
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 顯示訊息
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DisplayMessage { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "登入帳號")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string loginuserID { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "登入密碼")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string loginuserPassword { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //將使用者輸入的字串轉成Base64String
            string base64Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(loginuserPassword));
            //todo到DB抓使用者資料
            var user = LoginUserDataAccess.GetUser(loginuserID);
            //假如抓不到系統使用者資料
#if (RELEASE)
            if (!(loginuserID == user.loginuserID && base64Password == user.loginuserPassword))
#else
            if (!(loginuserID == user.loginuserID))
#endif
            {
                yield return new ValidationResult("無此帳號或密碼錯誤", new string[] { "DisplayMessage" });
            }

            
        }
    }
}