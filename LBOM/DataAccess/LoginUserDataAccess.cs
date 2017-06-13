using AutoMapper;
using LBOM.DataEntity;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LBOM.DataAccess
{
    public class LoginUserDataAccess : BaseDataAccess
    {
        /// <summary>
        /// 取得用者相關資料
        /// </summary>
        /// <param name="userID">使用者帳號</param>
        /// <returns></returns>
        public static LoginUserInfoDataEntity GetUser(string userID)
        {
            LoginUserInfoDataEntity user = new LoginUserInfoDataEntity()
            {
                loginuserID = string.Empty,
                loginuserName = string.Empty,
                loginuserPassword = string.Empty,
                deptID = string.Empty,
                deptName = string.Empty
            };

            var strSQL = @"
                            SELECT
	                            u.loginuserID 
	                            ,u.loginuserPassword 
	                            ,u.loginuserName 
	                            ,u.deptID 
	                            ,d.deptName  
                            FROM LBOM_LOGIN_USER U
                            JOIN LBOM_dept D
	                            ON U.DEPTID = d.DEPTID
                            WHERE u.loginuserID = @loginuserID
                    ";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@loginuserID", userID));
                conn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        //Mapper.CreateMap<IDataReader, LoginUserInfoDataEntity>();
                        //user = Mapper.Map<IDataReader, IList<LoginUserInfoDataEntity>>(dr).ToList().First();
                        user.deptID = dr["deptID"].ToString();
                        user.loginuserID= dr["loginuserID"].ToString();
                        user.loginuserName= dr["loginuserName"].ToString(); 
                        user.loginuserPassword= dr["loginuserPassword"].ToString();
                        user.deptName=dr["deptName"].ToString();
                    }
                }
            }
            return user;
        }
    }
}