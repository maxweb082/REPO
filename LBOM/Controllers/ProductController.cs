using LBOM.DataAccess;
using LBOM.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LBOM.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 取得產品資料
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="shopID"></param>
        /// <param name="productTypeID"></param>
        /// <param name="productName"></param>
        /// <returns></returns>
        public ActionResult GetProductData(string shopID = null, string productTypeID = null, string productName = null, string sort = null, string order = null)
        {
            var products = ProductDataAccess.GetProductData(shopID, productTypeID, productName, sort, order);

            //var offset = rows * (page - 1);
            //var q = shops.Skip(offset).Take(rows).ToList();
            //var result = new { total = shops.Count, rows = q };

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增餐點資料
        /// </summary>
        /// <param name="uData"></param>
        /// <returns></returns>
        public ActionResult NewProductData(ProductDataEntity uData)
        {
            uData.productID = Guid.NewGuid().ToString();
            var lstData = new List<ProductDataEntity>() { uData };
            var errorMsg = string.Empty;
            var isSuccess = true;
            try { DataAccess.ProductDataAccess.AddProductData(lstData); }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMsg = ex.Message.Replace("\n", "");
            }

            return Json(new { success = isSuccess, errorMsg = errorMsg });
        }

        /// <summary>
        /// 修改餐點資料
        /// </summary>
        /// <param name="uData"></param>
        /// <returns></returns>
        public ActionResult EditProductData(ProductDataEntity uData)
        {
            var lstData = new List<ProductDataEntity>() { uData };
            var errorMsg = string.Empty;
            var isSuccess = true;
            try { DataAccess.ProductDataAccess.UpdateProductData(lstData); }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMsg = ex.Message.Replace("\n", "");
            }

            return Json(new { success = isSuccess, errorMsg = errorMsg });
        }

        /// <summary>
        /// 刪除餐點資料
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public ActionResult DeleteProductData(string productID)
        {
            var errorMsg = string.Empty;
            var isSuccess = true;
            try { ProductDataAccess.DeleteProductData(productID); }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMsg = ex.Message.Replace("\n", "");
            }

            return Json(new { success = isSuccess, errorMsg = errorMsg });
        }
    }
}