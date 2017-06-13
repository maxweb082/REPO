//using FastReport.Web;
using LBOM.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace LBOM.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ARAP()
        {

            return View();
        }

        public string GetArapReport(string salesID = null)
        {
            //var data = ReportsDataAccess.GetARAPReportData(salesID);
            //var wr = new WebReport();

            //wr.Report.Load(Server.MapPath(@"~/Report/ARAP.frx"));
            //wr.Width = 900;
            //wr.Report.RegisterData(data, "DataItem");
            //wr.ShowRefreshButton = false;
            //wr.ShowPrint = false;

            ////ViewBag.WebReport = wr;
            //return wr.Scripts().ToHtmlString() + wr.Styles().ToHtmlString() + wr.GetHtml().ToHtmlString();
            return "";
        }

        /// <summary>
        /// 檢查是否有資料可匯出
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckExportData()
        {
            
            var data = ReportsDataAccess.GetARAPReportData(isAll: true);
            var canExport = data.Count > 0;

            return Json(new { canExport = canExport, total = data.Count },JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 取得匯出的EXCEL檔案
        /// </summary>
        /// <param name="salesID"></param>
        /// <returns></returns>
        //public ActionResult GetArapRarExcel(string salesID = null)
        //{
        //    var stream = new MemoryStream();
        //    var data = ReportsDataAccess.GetARAPReportData(salesID, true);
        //    FastReport.Utils.Config.WebMode = true;
        //    using (var rpt = new FastReport.Report())
        //    {
        //        rpt.Load(Server.MapPath(@"~/Report/ARAP_Raw.frx"));
        //        rpt.RegisterData(data, "DataItem");
        //        if (!rpt.Prepare())
        //            return null;

        //        var excelExp = new FastReport.Export.OoXML.Excel2007Export();
        //        rpt.Export(excelExp, stream);
        //        excelExp.Dispose();
        //        stream.Position = 0;
        //    }
        //    var fileName = string.Format("ApAr_RawData{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //    return File(stream, MediaTypeNames.Application.Octet, fileName);
        //}
    }
}