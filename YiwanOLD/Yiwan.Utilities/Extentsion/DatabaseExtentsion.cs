using System.Data;
using System.Data.SqlClient;
using System;
using System.Text.RegularExpressions;

namespace Yiwan.Utilities.Extentsion
{
    public static class Entity111FrameworkExtentsion
    {
        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回Datatable类型的查询结果
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数。如果使用输出参数，则它们的值在完全读取结果之前不可用。这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见 http://go.microsoft.com/fwlink/?LinkID=398589。</param>
        public static DataSet SqlQueryForDataSet(this System.Data.Entity.Database database, string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection sqlConnection = (SqlConnection)database.Connection)
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.SelectCommand.Parameters.AddRange(parameters);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                dataAdapter.Dispose();
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return dataSet;
            }
        }
        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回Datatable类型的查询结果
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数。如果使用输出参数，则它们的值在完全读取结果之前不可用。这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见 http://go.microsoft.com/fwlink/?LinkID=398589。</param>
        public static DataTable SqlQueryForDataTable(this System.Data.Entity.Database database, string sql, params SqlParameter[] parameters)
        {
            return SqlQueryForDataSet(database, sql, parameters).Tables[0];
        }
        //扩展方法必须是静态的，第一个参数必须加上this
        public static bool IsEmail(this string _input)
        {
            return Regex.IsMatch(_input, @"^\w+@\w+\.\w+$");
        }

    }
}
