using LBOM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LBOM.Controllers
{
    [Authorize]
    public class ProductTypeController : BaseController
    {
        // GET: ProductType
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得產品類別資料
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="productTypeID"></param>
        /// <param name="productTypeName"></param>
        /// <returns></returns>
        public ActionResult GetProductTypeData(string productTypeID = null, string productTypeName = null)
        {

            var productTypes = ProductTypeDataAccess.GetProductTypeData(productTypeID, productTypeName);

            //var offset = rows * (page - 1);
            //var q = productTypes.Skip(offset).Take(rows).ToList();
            //var result = new { total = productTypes.Count, rows = q };

            return Json(productTypes, JsonRequestBehavior.AllowGet);

        }
    }
}