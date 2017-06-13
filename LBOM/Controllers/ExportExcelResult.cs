using LBOM.DataAccess;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LBOM.Controllers
{
    public class ExportExcelResult:ActionResult
    {
        public string OrderID { get; set; }

        public ExportExcelResult()
        { }

        public override void ExecuteResult(ControllerContext context)
        {

            exportOrderExcel(context);
        }


        private void exportOrderExcel(ControllerContext context)
        {
            context.HttpContext.Response.ContentEncoding = Encoding.UTF8;
            context.HttpContext.Response.ContentType = 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";


            var fileName = string.Format("Order{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var exportFileName=context.HttpContext.Request.Browser.Browser.Equals("FireFox", StringComparison.OrdinalIgnoreCase)?
                fileName: 
                HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            using (var memS = new MemoryStream(ExcelHelper.OrderItemListToExcelContent(OrderID)))
            {
                memS.WriteTo(context.HttpContext.Response.OutputStream);
                memS.Close();
            }

                


        }
    }
}