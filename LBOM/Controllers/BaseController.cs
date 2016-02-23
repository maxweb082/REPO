using LBOM.DataEntity;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LBOM.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        /// 取得目前使用者資訊物件
        /// </summary>
        public LoginUserInfoDataEntity UserInfo
        {
            get
            {
                if (HttpContext.User == null || HttpContext.User.Identity.AuthenticationType != "Forms")
                    FormsAuthentication.RedirectToLoginPage();

                //倘若使用者Session已經逾時了
                if (Session["userInfo"] == null)
                {
                    //如果還沒登入驗證成功過，就重新登入
                    if (User.Identity == null || !User.Identity.IsAuthenticated)
                    {
                        //var returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);
                        //RedirectToAction("Login", "Home", returnUrl);
                        FormsAuthentication.RedirectToLoginPage();
                    }

                    //否則就幫他renew一下驗證票券
                    LoginProcess(new LoginUserInfoDataEntity() { loginuserID = User.Identity.Name });
                }

                return Session["userInfo"] as LoginUserInfoDataEntity;
            }
            set { Session["userInfo"] = value; }
        }

        /// <summary>
        /// 登入處理
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isPersisten"></param>
        protected void LoginProcess(LoginUserInfoDataEntity user, bool isPersisten = false)
        {
            //建立票證
            var ticket = new FormsAuthenticationTicket(
                    version: 1,
                    name: user.loginuserID,
                    issueDate: DateTime.Now,
                    expiration: DateTime.Now.AddMinutes(30),
                    isPersistent: isPersisten,
                    userData: "",
                    cookiePath: FormsAuthentication.FormsCookiePath
                );

            var encTicket = FormsAuthentication.Encrypt(ticket);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Session["userInfo"] = user;//將使用者資料物件存入SESSION            

            Response.Cookies.Add(cookie);

        }
    }
}