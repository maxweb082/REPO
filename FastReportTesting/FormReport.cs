using LBOM.DataAccess;
using LBOM.DataEntity;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FastReportTesting
{
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();
            AutoMapperWebConfiguration.Configure();

            

            
        }


        public void loadReport()
        {
            var orderID = "75e81f79-07b5-4876-acd3-4fbc06e1bf6f";
            var data = new List<OrderItemExportEntity>();
            var shop = ShopDataAccess.GetShopData(orderID);
            var ShopInfo = $"店家：{shop.shopName}    店家電話：{shop.shopTEL}";
            data = OrderItemDataAccess.GetExportList(orderID);



            report1.Report.Load(@"D:\WorkingDir\LBOM\LBOM\Report\OrderItemList.frx");
            report1.Report.Dictionary.RegisterBusinessObject(data, "OrderItems", 1, true);
            report1.Report.SetParameterValue("ShopInfo", ShopInfo);

            
            report1.Prepare(true);
            report1.ShowPrepared();
        }

        private void report1_StartReport(object sender, System.EventArgs e)
        {

        }
    }
}
