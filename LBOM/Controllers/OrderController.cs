using LBOM.DataAccess;
using LBOM.DataEntity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
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


        /// <summary>
        /// 匯出訂單
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult ExportExcel()
        {

            //if (Session["OrderItemExcel"] == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No ordr item data can be exported.");
            //}
            

            return new ExportExcelResult() { OrderID = Request.QueryString["ord"].ToString() };
        }

        /// <summary>
        /// 產生指定訂單編號的EXCEL匯出檔案
        /// <para>本方法將會回傳狀態碼。</para>
        /// <para>200：表示該訂單編號有資料可匯出，且已產生檔案存放在server端的Session["OrderItemExcel"]，請進一步呼叫ExportExcel動作以取回。</para>
        /// <para>404：表示該訂單編號沒有資料可匯出。</para>
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult GenExportExcel(string orderID)
        {
            HttpStatusCodeResult status = null;

            if (!OrderItemDataAccess.IsOrderItemExist(orderID))
            {
                status = new HttpStatusCodeResult(HttpStatusCode.NotFound, "No ordr item data can be exported.");
            }
            else
            {
                status = new HttpStatusCodeResult(HttpStatusCode.OK);

                //Session["OrderItemExcel"] = ExcelHelper.OrderItemListToExcelContent(orderID);
            }

            return status;
        }
    }
}