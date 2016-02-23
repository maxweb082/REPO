using LBOM.DataEntity;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FastReportTesting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.IsMdiContainer = true;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var frmReport = new FormReport();
            frmReport.MdiParent = this;
            //frmReport.Show();
            frmReport.loadReport();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {

            var rpt = new FastReport.Report();
            var data = new List<ARAPDataEntity>();
            rpt.Report.Load(@"D:\WorkingDir\LBOM\LBOM\Report\ARAccount.frx");
            rpt.Report.Dictionary.RegisterBusinessObject(data, "DataItem", 1, true);

            rpt.Save(@"D:\WorkingDir\LBOM\LBOM\Report\ARAP_RawData.frx");

            rpt.Clear();
            rpt.Dispose();

        }


    }
}
