using AutoMapper;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;

namespace LBOM.DataAccess
{
    public class BaseDataAccess
    {
        /// <summary>
        /// 資料庫連接字串
        /// </summary>
        public static string ConnectionString = _getConnectionString();


        public BaseDataAccess()
        {

        }

        private static string _getConnectionString()
        {
            var conString = string.Empty;
            try
            {
                if (ConnectionString == null)
#if DEBUG
                    conString = WebConfigurationManager.ConnectionStrings["LBOM_TEST"].ConnectionString;
#else
                    conString = WebConfigurationManager.ConnectionStrings["LBOM"].ConnectionString;
#endif

            }
            catch (Exception ex)
            {
                throw new Exception("Connection string read error：" + ex.Message);
            }

            return conString;
        }

        /// <summary>
        /// 讀取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <param name="aryPary"></param>
        /// <returns></returns>
        protected static List<T> ReadData<T>(string queryString, SqlParameter[] aryPary = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddRange(aryPary ?? new SqlParameter[] { });

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Mapper.CreateMap<IDataReader, T>();
                        return  Mapper.Map<IDataReader, List<T>>(reader);
                    }

                }

            }

            return new List<T>();
        }
    }
}