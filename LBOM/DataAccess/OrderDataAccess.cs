using LBOM.DataEntity;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;

namespace LBOM.DataAccess
{
    public class OrderDataAccess : BaseDataAccess
    {

        /// <summary>
        /// 取得所有可用的訂購
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static List<OrderDataEntity> GetOrderData(string sort = null, string order = null)
        {
            var strSQL = @"
                 SELECT O.*,U.loginuserName orderLoginuserName,S.SHOPNAME FROM LBOM_ORDER O 
                JOIN LBOM_LOGIN_USER U ON o.orderLoginuserID =U.loginuserID 
                JOIN LBOM_SHOP S ON O.shopID=S.shopID
                WHERE getdate() BETWEEN O.orderStartDatetime AND O.orderCloseDatetime 
            ";

            sort = string.IsNullOrEmpty(sort) ? null : sort;
            order = string.IsNullOrEmpty(order) ? null : order;


            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
                strSQL += string.Format("ORDER BY {0} {1} ", sort, order);

            //SqlParameter[] parms = {
            //    new SqlParameter("@productName",(object)productName??DBNull.Value),
            //    new SqlParameter("@shopID", (object)shopID ?? DBNull.Value),
            //    new SqlParameter("@productTypeID", (object)productTypeID ?? DBNull.Value) };


            

            var lst = ReadData<OrderDataEntity>(strSQL);

            return lst;
        }

        /// <summary>
        /// 新增訂購資訊
        /// </summary>
        /// <param name="lstData"></param>
        public static void AddOrderData(List<OrderDataEntity> lstData)
        {

            var strSQL = @"
                           INSERT INTO LBOM_ORDER
                           (orderID
                           ,orderLoginuserID
                           ,shopID
                           ,orderStartDatetime
                           ,orderCloseDatetime
                           ,orderDescript
                           ,isClosed)
                     VALUES
                           (@orderID 
                           ,@orderLoginuserID
                           ,@shopID
                           ,@orderStartDatetime
                           ,@orderCloseDatetime
                           ,@orderDescript
                           ,@isClosed)
                           ";

            using (var tsc = new TransactionScope())
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {
                SqlParameter[] aryParm = {
                    new SqlParameter("@orderID",SqlDbType.VarChar,50),
                    new SqlParameter("@orderLoginuserID",SqlDbType.VarChar,50),
                    new SqlParameter("@shopID",SqlDbType.VarChar,50),
                    new SqlParameter("@orderStartDatetime",SqlDbType.DateTime),
                    new SqlParameter("@orderCloseDatetime",SqlDbType.DateTime),
                    new SqlParameter("@orderDescript",SqlDbType.VarChar,250),
                    new SqlParameter("@isClosed",SqlDbType.Char,1)
                };
                cmd.Parameters.AddRange(aryParm);
                conn.Open();
                try
                {
                    foreach (var d in lstData)
                    {
                        cmd.Parameters["@orderID"].Value = d.orderID;
                        cmd.Parameters["@orderLoginuserID"].Value = d.orderLoginuserID;
                        cmd.Parameters["@shopID"].Value = d.shopID;
                        cmd.Parameters["@orderStartDatetime"].Value = d.orderStartDatetime;
                        cmd.Parameters["@orderCloseDatetime"].Value = d.orderCloseDatetime;
                        cmd.Parameters["@orderDescript"].Value = d.orderDescript;
                        cmd.Parameters["@isClosed"].Value = "0";

                        cmd.ExecuteNonQuery();
                    }
                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }

        }
    }
}