using LBOM.DataAccess;
using LBOM.DataEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LBOM.Controllers
{
    /// <summary>
    /// 店家資訊 Controller
    /// </summary>
    [Authorize]
    public class ShopController : BaseController
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();

        }

        /// <summary>
        /// 取得所有使用者資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetShopData(string sort, string order, int page, int rows, string shopName = null, string shopTEL = null)
        {

            var shops = ShopDataAccess.GetShopData(shopName, shopTEL);

            var q = (order.ToUpper() == "ASC") ?
                shops.OrderBy(x => x.shopName).ToList() :
                shops.OrderByDescending(x => x.shopName).ToList();

            var offset = rows * (page - 1);
            q = q.Skip(offset).Take(rows).ToList();
            var result = new { total = shops.Count, rows = q };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 新增店家資料
        /// </summary>
        /// <param name="uData"></param>
        /// <returns></returns>
        public string NewShopData(ShopDataEntity uData)
        {
            uData.shopID = Guid.NewGuid().ToString();
            var shops = new List<ShopDataEntity>() { uData };
            ShopDataAccess.AddShopData(shops);

            return JsonConvert.SerializeObject(new { success = true, errorMsg = "" });
        }

        /// <summary>
        /// 刪除店家資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteShopData(string shopID)
        {
            var errorMsg = string.Empty;
            var isSuccess = true;
            try { ShopDataAccess.DeleteShopData(shopID); }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMsg = ex.Message.Replace("\n", "");
            }

            return Json(new { success = isSuccess, errorMsg = errorMsg });
        }

        /// <summary>
        /// 修改店家資料
        /// </summary>
        /// <param name="uData"></param>
        /// <returns></returns>
        public string EditShopData(ShopDataEntity uData)
        {
            var shops = new List<ShopDataEntity>() { uData };

            var errorMsg = string.Empty;
            var isSuccess = true;
            try { ShopDataAccess.UpdateShopData(shops); }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMsg = ex.Message.Replace("\n", "");
            }


            return JsonConvert.SerializeObject(new { success = isSuccess, errorMsg = errorMsg });


        }


        public ActionResult GetProductData(int page, int rows, string shopID = null, string productTypeID = null, string productName = null, string sort = null, string order = null)
        {
            var shops = ProductDataAccess.GetProductData(shopID, productTypeID, productName, sort, order);


            var offset = rows * (page - 1);
            var q = shops.Skip(offset).Take(rows).ToList();
            var result = new { total = shops.Count, rows = q };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}