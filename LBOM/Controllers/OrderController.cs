using LBOM.DataAccess;
using LBOM.DataEntity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LBOM.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得所有可用的訂購資料
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult GetOrderData(string sort = null, string order = null)
        {
            var orders = OrderDataAccess.GetOrderData(sort, order);

            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增訂購資訊
        /// </summary>
        /// <param name="uData"></param>
        /// <returns></returns>
        public string NewOrderData(OrderDataEntity uData)
        {
            //var isSuccess = true;
            var errorMsg = string.Empty;

            uData.orderID = Guid.NewGuid().ToString();
            uData.orderLoginuserID = UserInfo.loginuserID;
            var orders = new List<OrderDataEntity>() { uData };
            try
            { OrderDataAccess.AddOrderData(orders); }
            catch (Exception ex)
            {
                errorMsg = ex.Message.Replace("\n", "");
                //isSuccess = false;
            }
            return errorMsg;
            //return Json(new { isSuccess = isSuccess, errorMsg = errorMsg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得店家資訊搜尋清單
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public ActionResult GetShopList(string q)
        {
            var shops = ShopDataAccess.GetShopData(shopName: q);

            return Json(shops, JsonRequestBehavior.AllowGet);
        }
    }
}