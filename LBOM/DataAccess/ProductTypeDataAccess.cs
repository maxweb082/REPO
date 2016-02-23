﻿using LBOM.DataEntity;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LBOM.DataAccess
{
    /// <summary>
    /// 產品類別資料存取物件
    /// </summary>
    public class ProductTypeDataAccess : BaseDataAccess
    {

        /// <summary>
        /// 取得產品類別資料
        /// </summary>
        /// <param name="productTypeID"></param>
        /// <param name="productTypeName"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static List<ProductTypeDataEntity> GetProductTypeData(string productTypeID = null, string productTypeName = null, string sort = null, string order = null)
        {
            var strSQL = @"
                    SELECT * FROM LBOM_PRODUCT_TYPE
                    WHERE PRODUCTTYPENAME LIKE '%'|| NVL(:productTypeName,productTypeName)||'%' 
                    AND PRODUCTTYPEID = NVL(:productTypeID,productTypeID) 
            ";

            productTypeName = string.IsNullOrEmpty(productTypeName) ? null : productTypeName;
            productTypeID = string.IsNullOrEmpty(productTypeID) ? null : productTypeID;

            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
                strSQL += string.Format("ORDER BY {0} {1} ", sort, order);

            OracleParameter[] parms = {
                new OracleParameter(":productTypeName",(object)productTypeName??DBNull.Value),
                new OracleParameter(":productTypeID", (object)productTypeID ?? DBNull.Value) };

            //var lst = ReadData<ProductTypeDataEntity>(strSQL, parms);
            var lst = new List<ProductTypeDataEntity>();

            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                cmd.Parameters.AddRange(parms);
                conn.Open();
                using (var pt = cmd.ExecuteReader())
                {
                    if (pt.HasRows)
                    {
                        while (pt.Read())
                        {
                            ProductTypeDataEntity producttype = new ProductTypeDataEntity()
                            {
                                productTypeID = pt["productTypeID"].ToString(),
                                productTypeName = pt["productTypeName"].ToString(),
                                productTypeDisplayOrder = Convert.ToInt32(pt["productTypeDisplayOrder"]),
                            };
                            lst.Add(producttype);
                        }
                    }
                }
            }
            return lst;
        }
    }
}