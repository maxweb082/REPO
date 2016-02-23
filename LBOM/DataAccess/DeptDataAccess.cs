using ClosedXML.Excel;
using LBOM.DataEntity;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace LBOM.DataAccess
{
    public class DeptDataAccess : BaseDataAccess
    {
        /// <summary>
        /// 取得所有部門資料
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptName"></param>
        /// <returns></returns>
        public static List<DeptDataEntity> GetDeptData(string deptAbbreviate = null, string deptName = null)
        {
            var strStr = @"
                    SELECT * FROM LBOM_DEPT
                    WHERE DEPTABBREVIATE LIKE '%' || NVL(:DEPTABBREVIATE, DEPTABBREVIATE) || '%' 
                    AND DEPTNAME LIKE '%' || NVL(:DEPTNAME, DEPTNAME) || '%'
            ";

            deptAbbreviate = string.IsNullOrEmpty(deptAbbreviate) ? null : deptAbbreviate;
            deptName = string.IsNullOrEmpty(deptName) ? null : deptName;


            OracleParameter[] parms = {
                new OracleParameter(":deptAbbreviate", (object)deptAbbreviate ?? DBNull.Value),
                new OracleParameter(":deptName",(object)deptName??DBNull.Value)};


            var lst = ReadData<DeptDataEntity>(strStr, parms);

            return lst;
        }

        /// <summary>
        /// 新增部門資料
        /// </summary>
        /// <param name="lstData"></param>
        public static void AddDeptData(List<DeptDataEntity> lstData)
        {

            var strSQL = @"
                    INSERT INTO LBOM_DEPT
                           (
                            DEPTID
                           ,DEPTABBREVIATE
                           ,DEPTNAME)
                     VALUES
                           (
                            :deptid
                           ,:deptabbreviate
                           ,:deptname) 
                        ";
            using (var tsc = new TransactionScope())
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                OracleParameter[] aryParm = {

                    new OracleParameter(":deptID",OracleDbType.Varchar2 ,50),
                    new OracleParameter(":deptAbbreviate",OracleDbType.Varchar2,20),
                    new OracleParameter(":deptName",OracleDbType.Varchar2,50)
                };
                cmd.Parameters.AddRange(aryParm);
                conn.Open();
                try
                {
                    foreach (var d in lstData)
                    {
                        cmd.Parameters[":deptID"].Value = d.deptID;
                        cmd.Parameters[":deptAbbreviate"].Value = d.deptAbbreviate;
                        cmd.Parameters[":deptName"].Value = d.deptName;

                        cmd.ExecuteNonQuery();
                    }
                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }

        }

        /// <summary>
        /// 編輯部門資料
        /// </summary>
        /// <param name="lstData"></param>
        public static void UpdateDeptData(List<DeptDataEntity> lstData)
        {

            var strSQL = @"
                            UPDATE  LBOM_DEPT
                               SET 
                                   DEPTABBREVIATE = :deptabbreivate
                                  ,DEPTNAME = :deptname
                             WHERE 
                                    deptID=:deptID 
                             ";
            using (var tsc = new TransactionScope())
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                OracleParameter[] aryParm = {
                    new OracleParameter(":deptAbbreviate",OracleDbType.Varchar2,20),
                    new OracleParameter(":deptName",OracleDbType.Varchar2,50),
                    new OracleParameter(":deptID",OracleDbType.Varchar2,50)
                };
                cmd.Parameters.AddRange(aryParm);
                try
                {
                    conn.Open();
                    foreach (var d in lstData)
                    {
                        cmd.Parameters[":deptAbbreviate"].Value = d.deptAbbreviate;
                        cmd.Parameters[":deptName"].Value = d.deptName;
                        cmd.Parameters[":deptID"].Value = d.deptID;

                        if (cmd.ExecuteNonQuery() <= 0) throw new Exception("Data update failed");
                    }
                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        /// <summary>
        /// 刪除部門資料
        /// </summary>
        /// <param name="lstData"></param>
        public static void DeleteDeptData(string deptID)
        {
            var strSQL = @"

                            DELETE FROM  LBOM_DEPT
                             WHERE 
                                    deptID=:deptID 
                             ";
            using (var tsc = new TransactionScope())
            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                OracleParameter[] aryParm = {
                    new OracleParameter(":deptID",deptID)
                };
                cmd.Parameters.AddRange(aryParm);
                try
                {
                    conn.Open();
                    cmd.Parameters[":deptID"].Value = deptID;
                    if (cmd.ExecuteNonQuery() <= 0) throw new Exception("Data delete failed");

                    tsc.Complete();
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        /// <summary>
        /// 取得部門員工資料
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptAbbreviate"></param>
        /// <param name="deptName"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static List<DeptDataEntity> GetUserNameData(string deptID = null, string deptAbbreviate = null, string deptName = null, string sort = null, string order = null)
        {
            var strSQL = @"
                    SELECT* FROM LBOM_DEPT D JOIN LBOM_LOGIN_USER U
                    ON D.DEPTID = U.DEPTID
                    WHERE DEPTABBREVIATE = NVL(:DEPTABBREVIATEE, DEPTABBREVIATE)
                    AND DEPTNAME= NVL(:DEPTNAME, DEPTNAME)
                    AND D.DEPTID=NVL(:DEPTID, D.DEPTID)
            ";
            
            deptAbbreviate = string.IsNullOrEmpty(deptAbbreviate) ? null : deptAbbreviate;
            deptName = string.IsNullOrEmpty(deptName) ? null : deptName;

            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
                strSQL += string.Format("ORDER BY {0} {1} ", sort, order);

            OracleParameter[] parms = {
                new OracleParameter(":deptAbbreviate)", (object)deptAbbreviate ?? DBNull.Value),
                new OracleParameter(":deptName", (object)deptName ?? DBNull.Value),
                new OracleParameter(":deptid)", (object)deptID ?? DBNull.Value)};

            //var lst = ReadData<ProductDataEntity>(strSQL, parms);
            //-----------------------------------------------------------------------------
            var lst = new List<DeptDataEntity>();

            using (var conn = new OracleConnection(ConnectionString))
            using (var cmd = new OracleCommand(strSQL, conn))
            {
                cmd.Parameters.AddRange(parms);
                conn.Open();
                using (var dp = cmd.ExecuteReader())
                {
                    if (dp.HasRows)
                    {
                        while (dp.Read())
                        {
                            DeptDataEntity dept = new DeptDataEntity()
                            {
                                deptID = dp["deptID"].ToString(),
                                deptAbbreviate = dp["deptAbbreviate"].ToString(),
                                deptName = dp["deptName"].ToString(),
                                loginuserName = dp["loginuserName"].ToString()
                            };
                            lst.Add(dept);
                        }
                        //Mapper.CreateMap<IDataReader, LoginUserInfoDataEntity>();
                        //user = Mapper.Map<IDataReader, IList<LoginUserInfoDataEntity>>(dr).ToList().First();
                    }
                }
            }
            return lst;
        }
        /// <summary>
        /// 匯出EXCEL資料
        /// </summary>
        public static DataTable GetExportData(string deptAbbreviate=null,string deptName=null)
        {
            OracleConnection con = new OracleConnection(ConnectionString);
            string strSQL = @"
                    SELECT DEPTABBREVIATE, DEPTNAME FROM LBOM_DEPT
                    WHERE DEPTABBREVIATE LIKE '%' || NVL(:DEPTABBREVIATE, DEPTABBREVIATE) || '%' 
                    AND DEPTNAME LIKE '%' || NVL(:DEPTNAME, DEPTNAME) || '%'
                     ";
            deptAbbreviate = string.IsNullOrEmpty(deptAbbreviate) ? null : deptAbbreviate;
            deptName = string.IsNullOrEmpty(deptName) ? null : deptName;

            OracleParameter[] parms = {
                new OracleParameter(":deptAbbreviate)", (object)deptAbbreviate ?? DBNull.Value),
                new OracleParameter(":deptName", (object)deptName ?? DBNull.Value) };

            //------------------------------------------------------------------------------
            DataTable dt = new DataTable();
            dt.TableName = "LBOM_DEPT";
            con.Open();
            OracleDataAdapter da = new OracleDataAdapter(strSQL, con);
            da.SelectCommand.Parameters.AddRange(parms);
            da.Fill(dt);
            con.Close();
            return dt;
        }
    }
}