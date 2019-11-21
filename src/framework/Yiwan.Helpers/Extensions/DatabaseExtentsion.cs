using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

/// <summary>
/// 扩展类
/// </summary>
namespace Yiwan.Helpers.Extentsion
{
    public static class EntityFrameworkExtentsion
    {
        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回Datatable类型的查询结果
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数。如果使用输出参数，则它们的值在完全读取结果之前不可用。这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见 http://go.microsoft.com/fwlink/?LinkID=398589。</param>
        public static DataSet SqlQueryForDataSet(this Database database, string sqlCommand, params SqlParameter[] parameters)
        {
            if (database == null) return null;

            DataSet dataSet = new DataSet();

            using (SqlConnection sqlConnection = (SqlConnection)database.Connection)
            {
#pragma warning disable CA2100 // 检查 SQL 查询是否存在安全漏洞
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand, sqlConnection);
#pragma warning restore CA2100 // 检查 SQL 查询是否存在安全漏洞
                dataAdapter.SelectCommand.Parameters.Add(parameters);
                dataAdapter.SelectCommand.Parameters.AddRange(parameters);

                dataAdapter.Fill(dataSet);

                dataAdapter.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return dataSet;
        }
        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回Datatable类型的查询结果
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数。如果使用输出参数，则它们的值在完全读取结果之前不可用。这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见 http://go.microsoft.com/fwlink/?LinkID=398589。</param>
        public static DataTable SqlQueryForDataTable(this Database database, string sql, params SqlParameter[] parameters)
        {
            using (DataSet dataSet = SqlQueryForDataSet(database, sql, parameters))
            {
                if (dataSet.Tables.Count > 0) return dataSet.Tables[0];
                else return null;
            }
        }
    }
}
