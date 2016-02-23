using LBOM.DataAccess;
using LBOM.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebApplication1.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        [AllowAnonymous]
        public ActionResult Index()
        {

            var orderID = "75e81f79-07b5-4876-acd3-4fbc06e1bf6f";
            var data = new List<OrderItemExportEntity>();
            var shop = ShopDataAccess.GetShopData(orderID);
            var ShopInfo = $"店家：{shop.shopName}    店家電話：{shop.shopTEL}";
            data = OrderItemDataAccess.GetExportList(orderID);

            var rpt = new XtraReport1();

            rpt.Parameters["ShopInfo"].Value = ShopInfo;
            rpt.DataSource = data;

            return View(rpt);
        }


    }
}