using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using Salary.Core.Utility;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System;

namespace Salary.Core.Data
{
    public class DataHelper
    {
        public DataHelper()
        {
        }
        public DataHelper(String dbName)
            : this()
        {
            _DBName = dbName;
        }
        public static String _DBName = SysConst.DefaultDBServer;
        private static DataHelper _Instance = new DataHelper();
        public static DataHelper Instance
        {
            get
            {
                return _Instance;
            }
        }

        #region 基本

        public DataSet GetDataSet(String sql)
        {
            DataSet ds = null;
            using (DbContext dbi = DbContext.GetContext(_DBName))
            {
                Database db = DatabaseFactory.Create(dbi);
                DbCommand command = db.GetSqlStringCommand(sql);
                command.CommandTimeout = 0;
                ds = db.ExecuteDataSet(command);
            }
            return ds;
        }

        public DataSet GetDataSet(List<String> sqlList)
        {
            if (sqlList == null || sqlList.Count == 0)
            {
                return null;
            }
            return GetDataSet(sqlList.Aggregate((s1, s2) => s1 + ";" + s2));
        }

        public DataTable GetDataTable(String sql)
        {
            return GetDataSet(sql).Tables[0];
        }

        public DataRow GetDataRow(String sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return dt.Rows[0];
        }

        public Int32 ExecuteSql(String sql)
        {
            if (String.IsNullOrEmpty(sql))
            {
                return 0;
            }

            Int32 result = 0;
            using (DbContext context = DbContext.GetContext(_DBName))
            {
                Database db = DatabaseFactory.Create(context);
                DbCommand command = db.GetSqlStringCommand(sql);
                command.CommandTimeout = 0;
                result = db.ExecuteNonQuery(command);
            }
            return result;
        }

        public Int32 ExecuteSql(String sql, params DbParameter[] dbParas)
        {
            Int32 result = 0;
            using (DbContext dbi = DbContext.GetContext(_DBName))
            {
                Database database = DatabaseFactory.Create(dbi);
                DbCommand command = database.GetSqlStringCommand(sql);
                command.CommandTimeout = 0;
                command.Parameters.AddRange(dbParas);
                result = database.ExecuteNonQuery(command);
            }
            return result;
        }

        public bool ExecuteSQLB(string sql)
        {
            bool ReValue = false;
            using (DbContext dbi = DbContext.GetContext(_DBName))
            {
                try
                {
                    Database db = DatabaseFactory.Create(dbi);
                    DbCommand command = db.GetSqlStringCommand(sql);
                    command.CommandTimeout = 0;
                    db.ExecuteNonQuery(command);
                    ReValue = true;
                }
                catch (Exception ex)
                {
                    ReValue = false;
                }
            }
            return ReValue;
        }

        #endregion

        #region 批量执行查询语句

        /// <summary>
        /// 批量执行查询语句，按照个数和maxLength分组
        /// </summary>
        /// <param name="stmtCount">每次执行查询语句的数量</param>
        /// <param name="sqlList">查询语句集合</param>
        /// <param name="maxLength">每次提交的查询语句的最大文本长度，字节</param>
        /// <param name="dbName">数据库连接串Key</param>
        public void ExecuteSqlList(int stmtCount, List<string> sqlList, int maxLength)
        {
            if (sqlList == null || sqlList.Count == 0) return;

            ListStringLengthGroup(ListStringCountGroup(sqlList, stmtCount), maxLength).ForEach(sql => ExecuteSql(sql));
        }

        /// <summary>
        /// 批量执行查询语句，按照maxLength分组
        /// </summary>
        /// <param name="sqlList">查询语句集合</param>
        /// <param name="dbName">数据库连接串Key</param>
        /// <param name="maxLength">每次提交的查询语句的最大文本长度，字节</param>
        public void ExecuteSqlList(List<string> sqlList, int maxLength)
        {
            if (sqlList == null || sqlList.Count == 0) return;

            ListStringLengthGroup(sqlList, maxLength).ForEach(sql => ExecuteSql(sql));
        }

        //public  void ExecuteSqlList<T>(DataTable dt,  int maxLength) where T : new()
        //{
        //    List<string> listSql = new List<string>();
        //    T info;
        //    dt.AsEnumerable().ToList().ForEach(row =>
        //    {
        //        info = new T();
        //        ORMapping.DataRowToObject<T>(row, info);
        //        listSql.Add(ORMapping.GetInsertSql<T>(info, TSqlBuilder.Instance));
        //    });
        //    ExecuteSqlList(listSql, maxLength);
        //}

        //public  void ExecuteSqlList<T>(DataTable dt, int maxLength) where T : new()
        //{
        //    ExecuteSqlList<T>(dt, maxLength);
        //}

        /// <summary>
        /// 批量执行查询语句，按照个数分组
        /// </summary>
        /// <param name="sqlList">查询语句集合</param>
        /// <param name="execCount">提交数量</param>
        /// <param name="dbName">数据库连接串</param>
        public void ExecuteSqlList(List<string> sqlList, int execCount, string dbName)
        {
            if (sqlList == null || sqlList.Count == 0) return;

            ListStringCountGroup(sqlList, execCount).ForEach(sql => ExecuteSql(sql));
        }

        /// <summary>
        /// 按照大小分组
        /// </summary>
        /// <param name="sqlList">列表</param>
        /// <param name="everyGroupMaxLength">组的最大长度</param>
        /// <returns>列表</returns>
        public List<string> ListStringLengthGroup(List<string> sqlList, int everyGroupMaxLength)
        {
            StringBuilder builder = new StringBuilder();
            List<string> resultList = new List<string>();
            sqlList.ForEach(groupSql =>
            {
                if (builder.Length > 0 && builder.Length + groupSql.Length + 1 > everyGroupMaxLength)
                {
                    resultList.Add(builder.ToString());
                    builder = new StringBuilder();
                }
                builder.Append(groupSql);
                builder.Append(";");
            }
            );
            resultList.Add(builder.ToString());
            return resultList;
        }

        /// <summary>
        ///  按照个数分组
        ///  如果everyGroupCount为3，则把"00","11","222","333","444","555","666" 变为 "00;11;222","333;444;555","666" 
        /// </summary>
        /// <param name="sqlList">列表</param>
        /// <param name="everyGroupCount">每组的个数</param>
        /// <returns>结果列表</returns>
        public List<string> ListStringCountGroup(List<string> sqlList, int everyGroupCount)
        {
            StringBuilder builder = null;
            var resultList = sqlList.Select((sqlStr, i) => new { GroupID = i / everyGroupCount, Sql = sqlStr }).ToList();
            return resultList.GroupBy(info => info.GroupID, (groupID, sqls) =>
            {
                builder = new StringBuilder();
                sqls.ToList().ForEach(i =>
                {
                    builder.Append(i.Sql);
                    builder.Append(";");
                }
                );
                return builder.ToString();
            }
            ).ToList();
        }

        #endregion

        #region 测试指定数据库是否可用

        /// <summary>
        /// 测试指定数据库是否可用
        /// </summary>
        /// <param name="dbName">数据库</param>
        /// <param name="repeatCount">重复次数</param>
        /// <param name="timeout">如果连接失败，重复连接间隔</param>
        /// <returns>是否可用</returns>
        public bool TestConnerctDB(int repeatCount, int timeout)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                try
                {
                    using (DbContext context = DbContext.GetContext(_DBName))
                    {
                        Database db = DatabaseFactory.Create(context);
                        string sql = "SELECT 1";
                        DataSet ds = db.ExecuteDataSet(CommandType.Text, sql);
                    }
                    return true;
                }
                catch
                {
                    Thread.Sleep(timeout);
                    continue;
                }
            }
            return false;
        }

        /// <summary>
        /// 测试指定数据库是否可用
        /// </summary>
        /// <param name="dbName">数据库</param>
        /// <returns>是否可用</returns>
        public bool TestConnerctDB()
        {
            return TestConnerctDB(5, 10000);
        }

        #endregion
    }
}
