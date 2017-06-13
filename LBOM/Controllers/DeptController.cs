using ClosedXML.Excel;
using LBOM.DataAccess;
using LBOM.DataEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;



namespace LBOM.Controllers
{
    /// <summary>
    /// 部門資訊 Controller
    /// </summary>
    [Authorize]
    public class DeptController : BaseController
    {
        // GET: Dept
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得所有部門資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDeptData(string sort, string order, int page, int rows, string deptAbbreviate = null, string deptName = null)
        {

            var depts = DeptDataAccess.GetDeptData(deptAbbreviate,deptName);

            var q = (order.ToUpper() == "ASC") ?
                depts.OrderBy(x => x.deptAbbreviate).ToList() :
                depts.OrderByDescending(x => x.deptAbbreviate).ToList();

            var offset = rows * (page - 1);
            q = q.Skip(offset).Take(rows).ToList();
            var result = new { total = depts.Count, rows = q };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 新增部門資料
        /// </summary>
        /// <param name="uData"></param>
        /// <returns></returns>
        public string NewDeptData(DeptDataEntity uData)
        {
            uData.deptID = Guid.NewGuid().ToString();
            var depts = new List<DeptDataEntity>() { uData };
            DeptDataAccess.AddDeptData(depts);

            return JsonConvert.SerializeObject(new { success = true, errorMsg = "" });
        }

        /// <summary>
        /// 刪除店家資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteDeptData(string deptID)
        {
            var errorMsg = string.Empty;
            var isSuccess = true;
            try { DeptDataAccess.DeleteDeptData(deptID); }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMsg = ex.Message.Replace("\n", "");
            }

            return Json(new { success = isSuccess, errorMsg = errorMsg });
        }

        /// <summary>
        /// 修改部門資料
        /// </summary>
        /// <param name="uData"></param>
        /// <returns></returns>
        public string EditDeptData(DeptDataEntity uData)
        {
            var depts = new List<DeptDataEntity>() { uData };

            var errorMsg = string.Empty;
            var isSuccess = true;
            try { DeptDataAccess.UpdateDeptData(depts); }
            catch (Exception ex)
            {
                isSuccess = false;
                errorMsg = ex.Message.Replace("\n", "");
            }
            
            return JsonConvert.SerializeObject(new { success = isSuccess, errorMsg = errorMsg });
            
        }

        /// <summary>
        /// 取得部門員工資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserNameData(string deptID = null)
        {

            var depts = DeptDataAccess.GetUserNameData(deptID);

            //var q = (order.ToUpper() == "ASC") ?
            //    depts.OrderBy(x => x.deptAbbreviate).ToList() :
            //    depts.OrderByDescending(x => x.deptAbbreviate).ToList();

            //var offset = rows * (page - 1);
            //q = q.Skip(offset).Take(rows).ToList();
            //var result = new { total = depts.Count, rows = q };

            return Json(depts, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 匯出EXCEL
        /// </summary>
        //------------------------------------------------------------------------------
        public ActionResult GetExportData(string deptAbbreviate=null , string deptName=null)
        {
            var depts = DeptDataAccess.GetExportData(deptAbbreviate, deptName);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(depts);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= EmployeeReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            //return RedirectToAction("Index", "ExportData");
            return View();

        }
    }    
}