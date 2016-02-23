using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LBOM.DataEntity
{
    /// <summary>
    /// 應收應付業績資料
    /// </summary>
    public class ARAPDataEntity
    {
        public string JOB_NO { get; set; }

        public string BL_NO { get; set; }

        public string VESSEL_CODE { get; set; }

        public string VESSEL_NAME { get; set; }

        public string CUSTOMER_ID { get; set; }

        public string CUSTOMER_NAME { get; set; }

        public string ON_BOARD_DATE { get; set; }

        public string DEPT_ID { get; set; }

        public string SALES_ID { get; set; }

        public string MODE_TYPE { get; set; }

        public string JOB_TYPE { get; set; }

        public string CURRENCY_TYPE { get; set; }

        public double AR { get; set; }

        public double AP { get; set; }

        public double AGENT_DUE { get; set; }

        public double AGENT_AR { get; set; }

        public double AGENT_AP { get; set; }

        public double INPUTVAT { get; set; }

        public double PROFIT { get; set; }

        public double AMEND_PROFIT { get; set; }

        public double AR_LOCAL { get; set; }

        public double AP_LOCAL { get; set; }

        public double AGENT_DUE_LOCAL { get; set; }

        public double AGENT_AR_LOCAL { get; set; }

        public double AGENT_AP_LOCAL { get; set; }

        public double INPUTVAT_LOCAL { get; set; }

        public double PROFIT_LOCAL { get; set; }

        public double AMEND_PROFIT_LOCAL { get; set; }

        public string CONSIGN_STATION { get; set; }

        public string CONSIGN_SALES { get; set; }

        public double CONSIGN_PROFIT_LOCAL { get; set; }

        public string MBL_NO { get; set; }

        public string AGENT_ID { get; set; }

        public string AGENT_NAME { get; set; }

        public string DISCHARGE_PORT { get; set; }

        public string POD { get; set; }

        public string DESTINATION { get; set; }

        public string LOADING_PORT { get; set; }

        public string ORIGIN { get; set; }

        public double GROSS_WEIGHT { get; set; }

        public string UNIT_GW { get; set; }

        public double CHARGEABLE_WEIGHT { get; set; }

        public string UNIT_CHAR { get; set; }
    }
}