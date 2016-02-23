using FastReport;
using FastReport.Export;
using FastReport.Web;
using LBOM.DataAccess;
using LBOM.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace LBOM.Controllers
{
    [Authorize]
    public class OrderItemController : BaseController
    {
        // GET: OrderItem
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增訂購項目資料
        /// </summary>
        /// <param name="uData"></param>
        /// <returns></returns>
        public ActionResult NewOrderItemData(OrderItemDataEntity uData)
        {
            var isSuccess = true;
            var errorMsg = string.Empty;

            uData.orderID = Guid.NewGuid().ToString();
            uData.orderItemLoginuserID = UserInfo.loginuserID;
            var orderItems = new List<OrderItemDataEntity>() { uData };
            try
            { OrderItemDataAccess.AddOrderItemData(orderItems); }
            catch (Exception ex)
            {
                errorMsg = ex.Message.Replace("\n", "");
                isSuccess = false;
            }

            return Json(new { isSuccess = isSuccess, errorMsg = errorMsg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增訂購項目資料
        /// </summary>
        /// <param name="orderItems"></param>
        /// <returns></returns>
        public ActionResult NewOrderItemDatas(List<OrderItemDataEntity> orderItems)
        {
            var isSuccess = true;
            var errorMsg = string.Empty;

            orderItems.ForEach(x =>
            {
                x.orderItemID = Guid.NewGuid().ToString();
                x.orderItemLoginuserID = UserInfo.loginuserID;
            });


            try
            { OrderItemDataAccess.AddOrderItemData(orderItems); }
            catch (Exception ex)
            {
                errorMsg = ex.Message.Replace("\n", "");
                isSuccess = false;
            }

            return Json(new { isSuccess = isSuccess, errorMsg = errorMsg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 匯出訂單
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult ExportExcel()
        {

            if (Session["OrderItemExcel"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "沒有訂單資料可供下載");
            }
            var fileBytes = Session["OrderItemExcel"] as byte[];
            Session.Remove("OrderItemExcel");
            var fileName = string.Format("Order{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
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
                status = new HttpStatusCodeResult(HttpStatusCode.NotFound, "該訂單尚無訂購明細可匯出");
            }
            else
            {
                status = new HttpStatusCodeResult(HttpStatusCode.OK);
                Session["OrderItemExcel"] = ExcelHelper.OrderItemListToExcelContent(orderID);
            }

            return status;
        }

        /// <summary>
        /// 開啟報表檢視器
        /// <para>若有明細就開啟，無資料則傳回錯誤</para>
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult OpenReport(string orderID)
        {
            HttpStatusCodeResult status = null;
            if (OrderItemDataAccess.IsOrderItemExist(orderID))
                status = new HttpStatusCodeResult(HttpStatusCode.OK);
            else
                status = new HttpStatusCodeResult(HttpStatusCode.NotFound, "該訂單尚無訂購明細可匯出");
            return status;
        }

        /// <summary>
        /// FastReport報表
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult FRPT_OrderItem(string orderID)
        {

            var data = new List<OrderItemExportEntity>();
            var shop = ShopDataAccess.GetShopData(orderID);
            var ShopInfo = $"店家：{shop.shopName}    店家電話：{shop.shopTEL}";
            data = OrderItemDataAccess.GetExportList(orderID);

            var wr = new WebReport();
            //wr.DesignReport = true;
            //wr.DesignScriptCode = false;
            wr.Report.Load(Server.MapPath("~/Report/OrderItemList.frx"));
            wr.Width = 900;

            wr.Report.Dictionary.RegisterBusinessObject(data, "OrderItems", 1, true);
            //wr.Report.Save(Server.MapPath("~/Report/OrderItemList.frx"));
            //wr.Height = 800;
            //wr.Report.RegisterData(data, "OrderItems");
            //wr.Report.GetDataSource("OrderItems").Enabled = true;
            //var db = wr.Report.FindObject("Data1") as DataBand;
            //db.DataSource = wr.Report.GetDataSource("OrderItems");
            //wr.Report.Dictionary.RegisterData(data, "OrderItems", true);
            //(wr.Report.FindObject("Data1") as DataBand).DataSource = wr.Report.GetDataSource("OrderItems");
            wr.Report.SetParameterValue("ShopInfo", ShopInfo);
            ViewBag.WebReport = wr;

            return View();
        }


        public ActionResult XtraReport_OrderItem(string orderID)
        {
            var data = new List<OrderItemExportEntity>();
            var shop = ShopDataAccess.GetShopData(orderID);
            var ShopInfo = $"店家：{shop.shopName}    店家電話：{shop.shopTEL}";
            data = OrderItemDataAccess.GetExportList(orderID);

            var xr = new XtraReport1();
            xr.Parameters["ShopInfo"].Value = ShopInfo;
            xr.RequestParameters = false;
            xr.DataSource = data;

            return View(xr);
        }
    }
}