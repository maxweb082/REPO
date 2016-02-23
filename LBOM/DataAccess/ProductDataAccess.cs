using LBOM.DataEntity;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace LBOM.DataAccess
{
    public class ProductDataAccess : BaseDataAccess
    {
        public static object SqlOracleDbType { get; private set; }

        /// <summary>
        /// 取得所有餐點資料
        /// </summary>
        /// <param name="shopID"></param>
        /// <param name="productTypeID"></param>
        /// <param name="productName"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static List<ProductDataEntity> GetProductData(string shopID =null, string productTypeID = null, string productName = null, string sort = null, string order = null)
        {
            var strSQL = @"
                    SELECT * FROM LBOM_PRODUCT P JOIN LBOM_PRODUCT_TYPE PT 
                    ON P.PRODUCTTYPEID=PT.PRODUCTTYPEID
                    WHERE 
                    P.PRODUCTNAME LIKE '%'|| NVL(:productName,P.PRODUCTNAME)||'%' AND
                    P.PRODUCTTYPEID = NVL(:productTypeID,P.PRODUCTTYPEID) AND 
                    P.SHOPID=NVL(:shopID,P.SHOPID)
            ";

            productName = string.IsNullOrEmpty(productName) ? null : productName;
            productTypeID = string.IsNullOrEmpty(productTypeID) ? null : productTypeID;
            shopID = string.IsNullOrEmpty(shopID) ? null : shopID;

            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
                strSQL += string.Format("ORDER BY {0} {1} ", sort, order);

            OracleParameter[] parms = {
                new OracleParameter(":productName",(object)productName??DBNull.Value),
                new OracleParameter(":shopID", (object)shopID ?? DBNull.Value),
                new OracleParameter(":productTypeID", (object)productTypeID ?? DBNull.Value) };

            //var lst = ReadData<ProductDataEntity>(strSQL, parms);
            //-----------------------------------------------------------------------------
            var lst = new List<ProductDataEntity>();

            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.Parameters.AddRange(parms);
                conn.Open();
                using (var pd = cmd.ExecuteReader())
                {
                    if (pd.HasRows)
                    {
                        while (pd.Read())
                        {
                            ProductDataEntity product = new ProductDataEntity()
                            {
                                productID = pd["productID"].ToString(),
                                productName = pd["productName"].ToString(),
                                productTypeID = pd["productTypeID"].ToString(),
                                shopID = pd["shopID"].ToString(),
                                productPrice =Convert.ToInt32(pd["productPrice"]),
                                productTypeName = pd["productTypeName"].ToString()
                            };
                        lst.Add(product);
                        }                    
                        //Mapper.CreateMap<IDataReader, LoginUserInfoDataEntity>();
                        //user = Mapper.Map<IDataReader, IList<LoginUserInfoDataEntity>>(dr).ToList().First();
                    }
                }
            }
                return lst;
        }

        /// <summary>
        /// 新增餐點資料
        /// </summary>
        /// <param name="lstData"></param>
        public static void AddProductData(List<ProductDataEntity> lstData)
        {

            var strSQL = @"

                    INSERT INTO LBOM_PRODUCT
                               (productID
                               ,productName
                               ,productTypeID
                               ,shopID
                               ,productPrice)
                         VALUES
                               (:productID
                               ,:productName
                               ,:productTypeID
                               ,:shopID
                               ,:productPrice) 
                          ";
            
            using (var tsc = new TransactionScope())
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                OracleParameter[] aryParm = {
                    new OracleParameter(":productID",OracleDbType.Varchar2,50),
                    new OracleParameter(":productName",OracleDbType.Varchar2,50),
                    new OracleParameter(":productTypeID",OracleDbType.Varchar2,50),
                    new OracleParameter(":shopID",OracleDbType.Varchar2,50),
                    new OracleParameter(":productPrice",OracleDbType.Int32)
                };
                cmd.Parameters.AddRange(aryParm);
                conn.Open();
                try
                {
                    foreach (var d in lstData)
                    {
                        cmd.Parameters[":productID"].Value = d.productID;
                        cmd.Parameters[":productName"].Value = d.productName;
                        cmd.Parameters[":productTypeID"].Value = d.productTypeID;
                        cmd.Parameters[":shopID"].Value = d.shopID;
                        cmd.Parameters[":productPrice"].Value = d.productPrice;

                        cmd.ExecuteNonQuery();
                    }
                    tsc.Complete();
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        /// <summary>
        /// 修改餐點資料
        /// </summary>
        /// <param name="lstData"></param>
        public static void UpdateProductData(List<ProductDataEntity> lstData)
        {

            var strSQL = @"
                        UPDATE LBOM_PRODUCT
                           SET 
                              productName = :productName
                              ,productTypeID = :productTypeID
                              ,shopID = :shopID
                              ,productPrice = :productPrice
                         WHERE 
                                productID=:productID
                             ";
            using (var tsc = new TransactionScope())
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                OracleParameter[] aryParm = {
                    new OracleParameter(":productID",OracleDbType.Varchar2,50),
                    new OracleParameter(":productTypeID",OracleDbType.Varchar2,50),
                    new OracleParameter(":productName",OracleDbType.Varchar2,50),
                    new OracleParameter(":shopID",OracleDbType.Varchar2,50),
                    new OracleParameter(":productPrice",OracleDbType.Int32)
                };
                cmd.Parameters.AddRange(aryParm);
                try
                {
                    conn.Open();
                    foreach (var d in lstData)
                    {
                        cmd.Parameters[":productID"].Value = d.productID;
                        cmd.Parameters[":productTypeID"].Value = d.productTypeID;
                        cmd.Parameters[":productName"].Value = d.productName;
                        cmd.Parameters[":shopID"].Value = d.shopID;
                        cmd.Parameters[":productPrice"].Value = d.productPrice;

                        if (cmd.ExecuteNonQuery() <= 0) throw new Exception("Data update failed");
                    }
                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        /// <summary>
        /// 刪除餐點資料
        /// </summary>
        /// <param name="productID"></param>
        public static void DeleteProductData(string productID)
        {
            var strSQL = @"
                             DELETE FROM LBOM_PRODUCT 
                             WHERE 
                                    productID=:productID 
                             ";
            using (var tsc = new TransactionScope())
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                OracleParameter[] aryParm = {
                    new OracleParameter(":productID",productID)
                };
                cmd.Parameters.AddRange(aryParm);
                try
                {
                    conn.Open();
                    cmd.Parameters[":productID"].Value = productID;
                    if (cmd.ExecuteNonQuery() <= 0) throw new Exception("Data delete failed");

                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }
        }
    }
}