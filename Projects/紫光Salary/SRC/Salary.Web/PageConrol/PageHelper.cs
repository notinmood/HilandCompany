using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salary.Core.PageConrol
{
    public static class PageHelper
    {
        /// <summary>
        /// SQL如果有参数，这样配置：123 --PAGE_BEGIN## 456 --PAGE_WITH_END## 789 --PAGE_END## 012
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="PageSize"></param>
        /// <param name="aSQL"></param>
        /// <returns></returns>
        /// lgy测试结果，输入：Common.GetCanPageSql("99",88,"123--PAGE_BEGIN##456--PAGE_WITH_END##789--PAGE_END##012")
        ///          执行结果：123 456 Select top(8712) identity(int,1,1) as r@o#w$i@d ,* into #tempTable from (789)as a   
        ///                    select * from (Select  * from #tempTable where r@o#w$i@d between 8625 and 8712) as b order by r@o#w$i@d  ; 
        ///                    456    SELECT COUNT(*) FROM (789) AS C    Drop table #tempTable 012
        public static string GetCanPageSql(int pageNum, int PageSize, string aSQL)
        {
            string sql_begin = "";
            string sql_with = "";
            string sql = aSQL;
            string sql_end = "";
            int pos_begin = aSQL.IndexOf("--PAGE_BEGIN##");
            int pos_end = aSQL.IndexOf("--PAGE_END##");
            if (pos_begin >= 0)
                sql_begin = aSQL.Substring(0, pos_begin);
            else
                pos_begin = -14;
            if (pos_end >= 0)
                sql_end = aSQL.Substring(pos_end + 12);
            else
                pos_end = aSQL.Length;
            sql = aSQL.Substring(pos_begin + 14, pos_end - pos_begin - 14);
            int pos_with_end = sql.IndexOf("--PAGE_WITH_END##");
            if (pos_with_end >= 0)
            {
                sql_with = sql.Substring(0, pos_with_end);
                sql = sql.Substring(pos_with_end + 17);
            }
            string strComm = " " + sql_with + " Select top " + pageNum * PageSize + " identity(int,1,1) as r@o#w$i@d ,* into #tempTable from (" + sql + ")as a    ";
            strComm += "   select * from (Select  * from #tempTable where r@o#w$i@d between " + ((pageNum - 1) * PageSize + 1) + " and " + pageNum * PageSize + ") as b order by r@o#w$i@d  ; ";
            strComm += sql_with + "    SELECT COUNT(*) FROM (" + sql + ") AS C ";
            strComm += "   Drop table #tempTable ";
            return sql_begin + strComm + sql_end;
        }

        public static string GetCanPageSql(int pageNum, int PageSize, string aSQL, string OrderByCondition)
        {
            string sql_begin = "";
            string sql_with = "";
            string sql = aSQL;
            string sql_end = "";
            int pos_begin = aSQL.IndexOf("--PAGE_BEGIN##");
            int pos_end = aSQL.IndexOf("--PAGE_END##");
            if (pos_begin >= 0)
                sql_begin = aSQL.Substring(0, pos_begin);
            else
                pos_begin = -14;
            if (pos_end >= 0)
                sql_end = aSQL.Substring(pos_end + 12);
            else
                pos_end = aSQL.Length;
            sql = aSQL.Substring(pos_begin + 14, pos_end - pos_begin - 14);
            int pos_with_end = sql.IndexOf("--PAGE_WITH_END##");
            if (pos_with_end >= 0)
            {
                sql_with = sql.Substring(0, pos_with_end);
                sql = sql.Substring(pos_with_end + 17);
            }
            string strComm = " " + sql_with + " SELECT * FROM (SELECT ROW_NUMBER() OVER( " + OrderByCondition + ") AS ROWNUM,* FROM (" +
                aSQL+") A)B WHERE ROWNUM BETWEEN " + ((pageNum - 1) * PageSize + 1) + " and " + pageNum * PageSize +";";
            strComm += sql_with + "    SELECT COUNT(*) FROM (" + sql + ") AS C ";
            return sql_begin + strComm + sql_end;
        }
    }
}
