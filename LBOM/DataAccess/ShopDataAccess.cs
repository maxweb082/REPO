
using LBOM.DataEntity;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data.SqlClient;
using System.Data;

namespace LBOM.DataAccess
{
    /// <summary>
    /// 店家資料存取物件
    /// </summary>
    public class ShopDataAccess : BaseDataAccess
    {
        /// <summary>
        /// 取得所有店家資料
        /// </summary>
        /// <param name="shopName"></param>
        /// <param name="shopTEL"></param>
        /// <returns></returns>
        public static List<ShopDataEntity> GetShopData(string shopName = null, string shopTEL = null)
        {
            var strStr = @"
                    SELECT	* 
                    FROM LBOM_SHOP
                    WHERE shopName LIKE '%' + ISNULL(@shopName, shopName) + '%' 
                    AND shopTEL = ISNULL(@shopTEL, shopTEL)
            ";

            shopName = string.IsNullOrEmpty(shopName) ? null : shopName;
            shopTEL = string.IsNullOrEmpty(shopTEL) ? null : shopTEL;

            SqlParameter[] parms = {
                new SqlParameter("@shopName",(object)shopName??DBNull.Value),
                new SqlParameter("@shopTEL", (object)shopTEL ?? DBNull.Value) };


            var lst = ReadData<ShopDataEntity>(strStr, parms);

            return lst;
        }

        /// <summary>
        /// 以訂單代號取得商店資料
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static ShopDataEntity GetShopData(string orderID)
        {
            var strStr = @"
                    SELECT	* 
                    FROM LBOM_SHOP S 
                    JOIN LBOM_ORDER O ON S.shopID=O.shopID  
                    WHERE 
                     O.orderID=@orderID 
            ";

            SqlParameter[] parms = {
                new SqlParameter("@orderID",(object)orderID??DBNull.Value) };


            var lst = ReadData<ShopDataEntity>(strStr, parms);


            return lst[0];
        }

        /// <summary>
        /// 新增店家資料
        /// </summary>
        /// <param name="lstData"></param>
        public static void AddShopData(List<ShopDataEntity> lstData)
        {

            var strSQL = @"

                INSERT INTO LBOM_SHOP
                           (shopID
                           ,shopName
                           ,shopTEL
                           ,shopAddress
                           ,shopRemark)
                     VALUES
                           (@shopID
                           ,@shopName
                           ,@shopTEL
                           ,@shopAddress
                           ,@shopRemark) ";
            using (var tsc = new TransactionScope())
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {
                SqlParameter[] aryParm = {
                    new SqlParameter("@shopID",SqlDbType.VarChar ,50),
                    new SqlParameter("@shopName",SqlDbType.NVarChar,50),
                    new SqlParameter("@shopTEL",SqlDbType.VarChar,20),
                    new SqlParameter("@shopAddress",SqlDbType.VarChar,50),
                    new SqlParameter("@shopRemark",SqlDbType.VarChar,120)
                };
                cmd.Parameters.AddRange(aryParm);
                conn.Open();
                try
                {
                    foreach (var d in lstData)
                    {
                        cmd.Parameters["@shopID"].Value = d.shopID;
                        cmd.Parameters["@shopName"].Value = d.shopName;
                        cmd.Parameters["@shopTEL"].Value = d.shopTEL;
                        cmd.Parameters["@shopAddress"].Value = d.shopAddress;
                        cmd.Parameters["@shopRemark"].Value = d.shopRemark;

                        cmd.ExecuteNonQuery();
                    }
                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }

        }

        /// <summary>
        /// 編輯店家資料
        /// </summary>
        /// <param name="lstData"></param>
        public static void UpdateShopData(List<ShopDataEntity> lstData)
        {

            var strSQL = @"

                            UPDATE LBOM_shop
                               SET 
                                  shopName = @shopName
                                  ,shopTEL = @shopTEL
                                  ,shopAddress = @shopAddress
                                  ,shopRemark = @shopRemark
                             WHERE 
                                    shopID=@shopID 
                             ";
            using (var tsc = new TransactionScope())
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {
                SqlParameter[] aryParm = {
                    new SqlParameter("@shopName",SqlDbType.VarChar,50),
                    new SqlParameter("@shopTEL",SqlDbType.VarChar,20),
                    new SqlParameter("@shopAddress",SqlDbType.VarChar,50),
                    new SqlParameter("@shopRemark",SqlDbType.VarChar,120),
                    new SqlParameter("@shopID",SqlDbType.VarChar,50)
                };
                cmd.Parameters.AddRange(aryParm);
                try
                {
                    conn.Open();
                    foreach (var d in lstData)
                    {
                        cmd.Parameters["@shopID"].Value = d.shopID;
                        cmd.Parameters["@shopName"].Value = d.shopName;
                        cmd.Parameters["@shopTEL"].Value = d.shopTEL;
                        cmd.Parameters["@shopAddress"].Value = d.shopAddress;
                        cmd.Parameters["@shopRemark"].Value = d.shopRemark;

                        if (cmd.ExecuteNonQuery() <= 0) throw new Exception("Data update failed");
                    }
                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        /// <summary>
        /// 刪除店家資料
        /// </summary>
        /// <param name="lstData"></param>
        public static void DeleteShopData(string shopID)
        {
            var strSQL = @"

                            DELETE FROM LBOM_shop
                             WHERE 
                                    shopID=@shopID 
                             ";
            using (var tsc = new TransactionScope())
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {
                SqlParameter[] aryParm = {
                    new SqlParameter("@shopID",shopID)
                };
                cmd.Parameters.AddRange(aryParm);
                try
                {
                    conn.Open();
                    cmd.Parameters["@shopID"].Value = shopID;
                    if (cmd.ExecuteNonQuery() <= 0) throw new Exception("Data delete failed");

                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

    }
}