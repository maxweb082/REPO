using LBOM.DataAccess;
using LBOM.DataEntity;
using LBOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LBOM.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            
            return View();
        }

        [AllowAnonymous]
        public ActionResult Index2()
        {

            //var wr = new WebReport();
            //wr.DesignReport = true;
            //wr.ID = "WebDesigner";
            //wr.DesignScriptCode = false;
            //wr.DesignerPath = @"http://www.fast-report.com:2015/razor/Home/Designer";
            //wr.DesignerSaveCallBack = @"http://www.fast-report.com:2015/razor/Home/SaveDesignedReport";
            //wr.Report.LoadPrepared(@"C:\Users\user\Documents\OrderItemList.fpx");

            //ViewBag.WebReport = wr;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult test()
        {
            var lst = new List<ShopDataEntity>();


            return View();
        }

        public ActionResult Wrapper()
        {
            return View();
        }

        #region 登入/登出 
        /// <summary>
        /// 呈現後台使用者登入頁
        /// </summary>
        /// <param name="ReturnUrl">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            //ReturnUrl字串是使用者在未登入情況下要求的的Url
            LoginViewModel vm = new LoginViewModel() { ReturnUrl = ReturnUrl };
            return View(vm);
        }

        /// <summary>
        /// 後台使用者進行登入
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="u">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel vm)
        {

            //沒通過Model驗證(必填欄位沒填，DB無此帳密)
            if (!ModelState.IsValid)
            {
                return View(vm);
            }



            //都成功...
            //進行表單登入 ※之後User.Identity.Name的值就是vm.loginuserID帳號的值
            //導向預設Url(Web.config裡的defaultUrl定義)或使用者原先Request的Url
            //FormsAuthentication.RedirectFromLoginPage(vm.loginuserID, false);
            var user = DataAccess.LoginUserDataAccess.GetUser(vm.loginuserID);
            LoginProcess(user);


            return Redirect(FormsAuthentication.GetRedirectUrl(vm.loginuserID, false));


        }
        /// <summary>
        /// 後台使用者登出動作
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //清除Session中的資料
            Session.Abandon();
            //登出表單驗證
            FormsAuthentication.SignOut();
            //導至登入頁
            return RedirectToAction("Login", "Home");
        }

        #endregion
    }
}