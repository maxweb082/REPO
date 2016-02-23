﻿using AutoMapper;
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
    /// <summary>
    /// 訂購項目資料存取物件
    /// </summary>
    public class OrderItemDataAccess:BaseDataAccess
    {

        /// <summary>
        /// 新增訂購項目
        /// </summary>
        /// <param name="lstData"></param>
        public static void AddOrderItemData(List<OrderItemDataEntity> lstData)
        {

            var strSQL = @"

                    INSERT INTO LBOM_ORDER_ITEM
                               (orderItemID
                               ,orderID
                               ,orderItemLoginuserID
                               ,productID
                               ,orderItemQuantity)
                         VALUES
                               (:orderItemID
                               ,:orderID
                               ,:orderItemLoginuserID
                               ,:productID
                               ,:orderItemQuantity)


                        ";
            using (var tsc = new TransactionScope())
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                OracleParameter[] aryParm = {
                    new OracleParameter(":orderItemID",OracleDbType.Varchar2,50),
                    new OracleParameter(":orderID",OracleDbType.Varchar2,50),
                    new OracleParameter(":orderItemLoginuserID",OracleDbType.Varchar2,50),
                    new OracleParameter(":productID",OracleDbType.Varchar2,50),
                    new OracleParameter(":orderItemQuantity",OracleDbType.Int32),

                };
                cmd.Parameters.AddRange(aryParm);
                conn.Open();
                try
                {
                    foreach (var d in lstData)
                    {
                        cmd.Parameters[":orderItemID"].Value = d.orderItemID;
                        cmd.Parameters[":orderID"].Value = d.orderID;
                        cmd.Parameters[":orderItemLoginuserID"].Value = d.orderItemLoginuserID;
                        cmd.Parameters[":productID"].Value = d.productID;
                        cmd.Parameters[":orderItemQuantity"].Value = d.orderItemQuantity;
                        cmd.ExecuteNonQuery();
                    }
                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }

        }

        /// <summary>
        /// 取得訂購明細的匯出資料
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static List<OrderItemExportEntity> GetExportList(string orderID)
        {
            var strSQL = @"
                    SELECT P.PRODUCTNAME productName,
                            P.PRODUCTPRICE productPrice,
                            OI.ORDERITEMQUANTITY orderItemQuantity,
                            LU.LOGINUSERNAME loginuserName,
                            P.PRODUCTPRICE*OI.ORDERITEMQUANTITY amount 
                        FROM LBOM_ORDER_ITEM OI 
                        JOIN LBOM_PRODUCT P ON OI.PRODUCTID=P.PRODUCTID 
                        JOIN LBOM_LOGIN_USER LU ON OI.ORDERITEMLOGINUSERID=LU.LOGINUSERID 
                        WHERE OI.ORDERID=:ORDERID
            ";

            var lst = new List<OrderItemExportEntity>();
            orderID = string.IsNullOrEmpty(orderID) ? null : orderID;

            OracleParameter[] parms = {
                new OracleParameter(":ORDERID",(object)orderID??DBNull.Value) };

            //using (var conn = new OracleConnection(ConnectionString))
            //using (var cmd=new OracleCommand(strSQL,conn))
            //{
            //    conn.Open();
            //    cmd.Parameters.AddRange(parms);
            //    var dr = cmd.ExecuteReader();

            //    Mapper.CreateMap<IDataReader, OrderItemExportEntity>();
            //    var data = Mapper.Map<IDataReader, IList<OrderItemExportEntity>>(dr).ToList().First();
            //    lst.Add(data);
            //}

            lst = ReadData<OrderItemExportEntity>(strSQL, parms);

            

            return lst;
        }

        /// <summary>
        /// 檢查指定訂單代號是否有人訂購
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool IsOrderItemExist(string orderID)
        {
            var strSQL = @"
                    SELECT P.PRODUCTNAME productName,
                            P.PRODUCTPRICE productPrice,
                            OI.ORDERITEMQUANTITY orderItemQuantity,
                            LU.LOGINUSERNAME loginuserName,
                            P.PRODUCTPRICE*OI.ORDERITEMQUANTITY amount 
                        FROM LBOM_ORDER_ITEM OI 
                        JOIN LBOM_PRODUCT P ON OI.PRODUCTID=P.PRODUCTID 
                        JOIN LBOM_LOGIN_USER LU ON OI.ORDERITEMLOGINUSERID=LU.LOGINUSERID 
                        WHERE OI.ORDERID=:ORDERID
            ";
            var isExist = false;

            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                conn.Open();
                cmd.Parameters.Add(new OracleParameter(":ORDERID", orderID));
                var dr = cmd.ExecuteReader();
                isExist = dr.HasRows;
            }

            return isExist;
            
        }
    }
}