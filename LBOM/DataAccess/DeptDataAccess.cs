using ClosedXML.Excel;
using LBOM.DataEntity;
using System.Data.SqlClient;
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
                    WHERE DEPTABBREVIATE LIKE '%' + ISNULL(@DEPTABBREVIATE, DEPTABBREVIATE) + '%' 
                    AND DEPTNAME LIKE '%' + ISNULL(@DEPTNAME, DEPTNAME) + '%'
            ";

            deptAbbreviate = string.IsNullOrEmpty(deptAbbreviate) ? null : deptAbbreviate;
            deptName = string.IsNullOrEmpty(deptName) ? null : deptName;


            SqlParameter[] parms = {
                new SqlParameter("@DEPTABBREVIATE", (object)deptAbbreviate ?? DBNull.Value),
                new SqlParameter("@DEPTNAME",(object)deptName??DBNull.Value)};


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
                            @deptid
                           ,@deptabbreviate
                           ,@deptname) 
                        ";
            using (var tsc = new TransactionScope())
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {
                SqlParameter[] aryParm = {

                    new SqlParameter("@deptID",SqlDbType.VarChar ,50),
                    new SqlParameter("@deptAbbreviate",SqlDbType.VarChar,20),
                    new SqlParameter("@deptName",SqlDbType.VarChar,50)
                };
                cmd.Parameters.AddRange(aryParm);
                conn.Open();
                try
                {
                    foreach (var d in lstData)
                    {
                        cmd.Parameters["@deptID"].Value = d.deptID;
                        cmd.Parameters["@deptAbbreviate"].Value = d.deptAbbreviate;
                        cmd.Parameters["@deptName"].Value = d.deptName;

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
                                   DEPTABBREVIATE = @deptabbreivate
                                  ,DEPTNAME = @deptname
                             WHERE 
                                    deptID=@deptID 
                             ";
            using (var tsc = new TransactionScope())
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {
                SqlParameter[] aryParm = {
                    new SqlParameter("@deptabbreivate",SqlDbType.VarChar,20),
                    new SqlParameter("@deptname",SqlDbType.VarChar,50),
                    new SqlParameter("@deptID",SqlDbType.VarChar,50)
                };
                cmd.Parameters.AddRange(aryParm);
                try
                {
                    conn.Open();
                    foreach (var d in lstData)
                    {
                        cmd.Parameters["@deptabbreivate"].Value = d.deptAbbreviate;
                        cmd.Parameters["@deptname"].Value = d.deptName;
                        cmd.Parameters["@deptID"].Value = d.deptID;

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
                                    deptID=@deptID 
                             ";
            using (var tsc = new TransactionScope())
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {
                SqlParameter[] aryParm = {
                    new SqlParameter("@deptID",deptID)
                };
                cmd.Parameters.AddRange(aryParm);
                try
                {
                    conn.Open();
                    cmd.Parameters["@deptID"].Value = deptID;
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
                    SELECT * FROM LBOM_DEPT D JOIN LBOM_LOGIN_USER U
                    ON D.DEPTID = U.DEPTID
                    WHERE DEPTABBREVIATE = ISNULL(@DEPTABBREVIATE, DEPTABBREVIATE)
                    AND DEPTNAME= ISNULL(@DEPTNAME, DEPTNAME)
                    AND D.DEPTID=ISNULL(@DEPTID, D.DEPTID)
            ";
            
            deptAbbreviate = string.IsNullOrEmpty(deptAbbreviate) ? null : deptAbbreviate;
            deptName = string.IsNullOrEmpty(deptName) ? null : deptName;

            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
                strSQL += string.Format("ORDER BY {0} {1} ", sort, order);

            SqlParameter[] parms = {
                new SqlParameter("@DEPTABBREVIATE", (object)deptAbbreviate ?? DBNull.Value),
                new SqlParameter("@DEPTNAME", (object)deptName ?? DBNull.Value),
                new SqlParameter("@DEPTID", (object)deptID ?? DBNull.Value)};

            //var lst = ReadData<ProductDataEntity>(strSQL, parms);
            //-----------------------------------------------------------------------------
            var lst = new List<DeptDataEntity>();

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
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
            SqlConnection con = new SqlConnection(ConnectionString);
            string strSQL = @"
                    SELECT DEPTABBREVIATE, DEPTNAME FROM LBOM_DEPT
                    WHERE DEPTABBREVIATE LIKE '%' + ISNULL(@DEPTABBREVIATE, DEPTABBREVIATE) + '%' 
                    AND DEPTNAME LIKE '%' + ISNULL(@DEPTNAME, DEPTNAME) + '%'
                     ";
            deptAbbreviate = string.IsNullOrEmpty(deptAbbreviate) ? null : deptAbbreviate;
            deptName = string.IsNullOrEmpty(deptName) ? null : deptName;

            SqlParameter[] parms = {
                new SqlParameter("@deptAbbreviate", (object)deptAbbreviate ?? DBNull.Value),
                new SqlParameter("@deptName", (object)deptName ?? DBNull.Value) };

            //------------------------------------------------------------------------------
            DataTable dt = new DataTable();
            dt.TableName = "LBOM_DEPT";
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(strSQL, con);
            da.SelectCommand.Parameters.AddRange(parms);
            da.Fill(dt);
            con.Close();
            return dt;
        }
    }
}