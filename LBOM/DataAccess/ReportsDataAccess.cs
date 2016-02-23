using LBOM.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LBOM.DataAccess
{
    public class ReportsDataAccess : BaseDataAccess
    {
        public static List<ARAPDataEntity> GetARAPReportData(string salesID = null, bool isAll = false)
        {
            var strSQL = @"SELECT * FROM SALE_STA ";
            var lst = new List<ARAPDataEntity>();
            if (!string.IsNullOrEmpty(salesID) || isAll)
            {
                if (!isAll) strSQL += $"WHERE SALES_ID='{salesID}'";
                lst = ReadData<ARAPDataEntity>(strSQL);
            }

            return lst;
        }
    }
}