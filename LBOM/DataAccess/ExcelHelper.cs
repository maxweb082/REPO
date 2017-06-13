using LBOM.DataEntity;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace LBOM.DataAccess
{
    public sealed class ExcelHelper
    {


        /// <summary>
        /// 匯出訂單EXCEL
        /// </summary>
        /// <param name="orderID"></param>
        public static byte[] OrderItemListToExcelContent(string orderID)
        {
            using (var ep = new ExcelPackage())
            {
                ExcelWorksheet es = ep.Workbook.Worksheets.Add("訂單明細");


                #region 產生店家資訊
                var shop = ShopDataAccess.GetShopData(orderID);
                es.Cells[1, 1].Value = $"店家名稱：{shop.shopName}    店家電話：{shop.shopTEL}";
                es.Select(new ExcelAddress("A1:E1"));
                es.SelectedRange.SetQuickStyle(Color.White, Color.Blue);
                es.SelectedRange.Merge = true;
                #endregion

                #region 產生欄位標頭

                var colIndex = 0;
                foreach (var s in "品名;單價;數量;訂購人;金額".Split(';'))
                {
                    es.Cells[2, ++colIndex].Value = s;
                }
                es.Select(new ExcelAddress(2, 1, 2, colIndex));
                es.SelectedRange.SetQuickStyle(Color.Red, Color.YellowGreen,hAlign:ExcelHorizontalAlignment.Center);
                #endregion

                #region 產訂購資訊內容


                var lst = OrderItemDataAccess.GetExportList(orderID);

                var rowIndex = 3;

                foreach (OrderItemExportEntity d in lst)
                {
                    es.Cells[rowIndex, 1].Value = d.productName;
                    es.Cells[rowIndex, 2].Value = d.productPrice;
                    es.Cells[rowIndex, 3].Value = d.orderItemQuantity;
                    es.Cells[rowIndex, 4].Value = d.loginuserName;
                    es.Cells[rowIndex, 5].Value = d.amount;

                    #region 設定資料列style
                    es.Select(new ExcelAddress(rowIndex, 2, rowIndex, 2));
                    es.SelectedRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    es.Select(new ExcelAddress(rowIndex, 3, rowIndex, 3));
                    es.SelectedRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    es.Select(new ExcelAddress(rowIndex, 4, rowIndex, 4));
                    es.SelectedRange.SetQuickStyle(Color.Blue, hAlign: ExcelHorizontalAlignment.Center);
                    es.Select(new ExcelAddress(rowIndex, 5, rowIndex, 5));
                    es.SelectedRange.SetQuickStyle(Color.Red, hAlign: ExcelHorizontalAlignment.Right);
                    #endregion

                    rowIndex++;
                }

                //設定自動欄寬
                for (int i = 1; i < colIndex; i++)
                {
                    es.Column(i).AutoFit();
                }

                es.SetValue(rowIndex, 1, "總計：");
                es.Cells[rowIndex, colIndex].Formula = $"SUM(E3:E{rowIndex - 1})";
                es.Select(new ExcelAddress(rowIndex, 1, rowIndex, colIndex));
                es.SelectedRange.SetQuickStyle(Color.White, bgColor: Color.DarkRed, hAlign: ExcelHorizontalAlignment.Right);
                #endregion
                return ep.GetAsByteArray();
            }


        }



    }




    public static class ExcelHelprExtension
    {
        //加入擴充方法: SetQuickStyle，指定前景色/背景色/水平對齊

        public static void SetQuickStyle(this ExcelRange range,

            Color foreColor,

            Color bgColor = default(Color),

            ExcelHorizontalAlignment hAlign = ExcelHorizontalAlignment.Left)

        {

            range.Style.Font.Color.SetColor(foreColor);

            if (bgColor != default(Color))

            {

                range.Style.Fill.PatternType = ExcelFillStyle.Solid;

                range.Style.Fill.BackgroundColor.SetColor(bgColor);

            }

            range.Style.HorizontalAlignment = hAlign;

        }
    }
}