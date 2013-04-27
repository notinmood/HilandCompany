using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Salary.Core.Data;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Transactions;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data;
using System.Text;

namespace Salary.Biz
{
    public class ReportInfoAdapter
    {
        public ReportInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static ReportInfoAdapter _Instance = new ReportInfoAdapter();
        public static ReportInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        #region 报表

        public Int32 InsertReportInfo(ReportInfo reportInfo)
        {
            String sql = ORMapping.GetInsertSql(reportInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 UpdateReportInfo(ReportInfo reportInfo)
        {
            String sql = ORMapping.GetUpdateSql(reportInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteReportInfo(String reportID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportInfoDBConst.ReportID, reportID);
            String sql = String.Format("DELETE FROM {0} WHERE {1};DELETE FROM {2} WHERE {1} ", ReportInfoDBConst.TableName, builder.ToSqlString(), ReportFeeDBConst.TableName);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public ReportInfo LoadReportInfo(String reportID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportInfoDBConst.ReportID, reportID);
            return GetReportInfoList(builder).FirstOrDefault();
        }

        public List<ReportInfo> GetReportInfoList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY REPORT_CODE", ReportInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<ReportInfo> result = new List<ReportInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                ReportInfo info = new ReportInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public Boolean IsReportCodeUsed(ReportInfo report, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportInfoDBConst.ReportCode, report.ReportCode);
            ReportInfo reportInfo = GetReportInfoList(builder).FirstOrDefault();
            if (reportInfo == null)
            {
                return false;
            }
            else
            {
                if (isAdd)
                {
                    return true;
                }
                else
                {
                    return reportInfo.ReportID.Equals(report.ReportID) ? false : true;
                }
            }
        }

        public Boolean IsReportNameUsed(ReportInfo report, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportInfoDBConst.ReportName, report.ReportName);
            ReportInfo reportInfo = GetReportInfoList(builder).FirstOrDefault();
            if (reportInfo == null)
            {
                return false;
            }
            else
            {
                if (isAdd)
                {
                    return true;
                }
                else
                {
                    return reportInfo.ReportID.Equals(report.ReportID) ? false : true;
                }
            }
        }

        public Int32 ChangeStatus(String reportID, Int32 userFlag)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportInfoDBConst.ReportID, reportID);
            String sql = String.Format("UPDATE {0} SET USE_FLAG={1} WHERE {2} ", ReportInfoDBConst.TableName, userFlag, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        #endregion 报表

        #region 报表款项

        public Int32 InsertReportFee(ReportFee reportFee)
        {
            String sql = ORMapping.GetInsertSql(reportFee, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 InsertReportFee(List<ReportFee> reportFeeList)
        {

            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                //ReportAdapter.Instance.DeleteReportFee(reportFeeList.FirstOrDefault().ReportID);
                StringBuilder sql = new StringBuilder();
                Int32 Count = 0;
                foreach (ReportFee reportFee in reportFeeList)
                {
                    reportFee.ReportFeeName = reportFee.FeeName;
                    reportFee.OrderNo = this.GetMaxReportFeeOrderNo(reportFee.ReportID)+Count;
                    sql.Append(ORMapping.GetInsertSql(reportFee, BuilderEx.TSqlBuilderInstance));
                    sql.Append(";");
                    Count += 1;
                }
                int result = _DataHelper.ExecuteSql(sql.ToString());
                scope.Complete();
                return result;
            }
        }

        private Int32 GetMaxReportFeeOrderNo(String ReportID)
        {
            String sql = String.Format("SELECT MAX({0}) + 1 FROM {1} WHERE {2}='{3}'", ReportFeeDBConst.OrderNo, ReportFeeDBConst.TableName, ReportFeeDBConst.ReportID, ReportID);
            DataTable dt = _DataHelper.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                Int32 count = 0;
                return Int32.TryParse(dt.Rows[0][0].ToString(), out count) ? count : 1;
            }
            else
            {
                return 10;
            }
        }

        public Int32 UpdateReportFee(ReportFee reportFee)
        {
            String sql = ORMapping.GetUpdateSql(reportFee, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteReportFee(String reportID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportFeeDBConst.ReportID, reportID);
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", ReportFeeDBConst.TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteReportFee(String reportID, String feeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportFeeDBConst.ReportID, reportID);
            builder.AppendItem(ReportFeeDBConst.FeeID, feeID);
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", ReportFeeDBConst.TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteReportFee(List<ReportFee> reportFeeList)
        {
            StringBuilder sbFeeID = new StringBuilder();
            foreach (ReportFee reportFee in reportFeeList)
            {
                sbFeeID.Append("'");
                sbFeeID.Append(reportFee.FeeID);
                sbFeeID.Append("',");
            }
            String strWhere= sbFeeID.ToString();
            strWhere=strWhere.Substring(0,strWhere.Length - 1);
            String sql = String.Format("DELETE FROM {0} WHERE {1}='{2}' AND {3} IN ({4})", ReportFeeDBConst.TableName, ReportFeeDBConst.ReportID, reportFeeList.FirstOrDefault().ReportID, ReportFeeDBConst.FeeID, strWhere);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        //public ReportFee LoadReportFee(String reportID, String feeCode)
        //{
        //    WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        //    builder.AppendItem(ReportFeeDBConst.ReportID, reportID);
        //    builder.AppendItem(ReportFeeDBConst.FeeCode, feeCode);
        //    return GetReportFeeList(builder).FirstOrDefault();
        //}

        public List<ReportFee> GetReportFeeList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format(@"SELECT RF.*, F.FEE_TYPE, F.DEFAULT_VALUE FROM {0} RF LEFT OUTER JOIN {1} F ON RF.FEE_ID=F.FEE_ID WHERE {2} ORDER BY ORDER_NO", ReportFeeDBConst.TableName, FeeInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<ReportFee> result = new List<ReportFee>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                ReportFee info = new ReportFee();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        /*
        public List<ReportFee> GetReportFeeList(WhereSqlClauseBuilder reportFeeBuilder,WhereSqlClauseBuilder builder)
        {
            builder = builder == null ? new WhereSqlClauseBuilder() : builder;
            builder.AppendItem("F." + FeeInfoDBConst.UseFlag, Status.True.ToString("D"));//ISNULL(R.ORDER_NO,F.ORDER_NO)
            String sql= String.Format(@"SELECT R.REPORT_ID, F.FEE_NAME, F.FEE_ID, F.FEE_TYPE, '' AS 'ORDER_NO'
                                        ,F.DEFAULT_VALUE, CASE WHEN R.REPORT_ID IS NULL THEN 'false' ELSE 'true' END AS 'CHECKED'
                                        FROM {0} F 
                                        LEFT OUTER JOIN (SELECT REPORT_ID, FEE_NAME, FEE_ID, ORDER_NO FROM {1} WHERE {2})R 
                                        ON F.FEE_ID=R.FEE_ID WHERE {3} ORDER BY CHECKED DESC,ORDER_NO", FeeInfoDBConst.TableName, ReportFeeDBConst.TableName, reportFeeBuilder.ToSqlString(), builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<ReportFee> result = new List<ReportFee>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                ReportFee info = new ReportFee();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }*/

        public ReportFee LoadReportFee(string reportID, string feeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(String.Concat("RF.",ReportFeeDBConst.ReportID), reportID);
            builder.AppendItem(String.Concat("RF.",ReportFeeDBConst.FeeID), feeID);
            return GetReportFeeList(builder).FirstOrDefault();
        }

        public List<ReportFee> GetReportFeeList(string reportID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportFeeDBConst.ReportID, reportID);
            return GetReportFeeList(builder);
        }

        public List<FeeInfo> GetReportCanSelectFeeList(string reportID)
        {

            String sql = String.Format(@"SELECT * FROM {0} WHERE ISNULL(PARENT_ID,'')='' AND use_flag=0 AND {3} NOT IN (SELECT {3} FROM {1} WHERE {4} ='{2}') ORDER BY {5}", FeeInfoDBConst.TableName, ReportFeeDBConst.TableName, reportID, FeeInfoDBConst.FeeID, ReportInfoDBConst.ReportID, FeeInfoDBConst.FeeCode);
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<FeeInfo> resultFeeInfoList = new List<FeeInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                FeeInfo info = new FeeInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                resultFeeInfoList.Add(info);
            }
            List<FeeInfo> childrenFeeInfoList = new List<FeeInfo>();
            foreach (FeeInfo feeInfo in resultFeeInfoList)
            {
                childrenFeeInfoList.AddRange(GetReportFeeChildrenInfoList(reportID, feeInfo));
            }
            resultFeeInfoList.AddRange(childrenFeeInfoList);

            //String sql = String.Format(@"SELECT * FROM {0} WHERE use_flag=0 AND {3} NOT IN (SELECT {3} FROM {1} WHERE {4} ='{2}') ORDER BY {5}", FeeInfoDBConst.TableName, ReportFeeDBConst.TableName, reportID, FeeInfoDBConst.FeeID, ReportInfoDBConst.ReportID, FeeInfoDBConst.FeeCode);
            //DataTable dt = _DataHelper.GetDataTable(sql);
            //List<FeeInfo> result = new List<FeeInfo>();
            //for (Int32 i = 0; i < dt.Rows.Count; i++)
            //{
            //    FeeInfo info = new FeeInfo();
            //    ORMapping.DataRowToObject(dt.Rows[i], info);
            //    result.Add(info);
            //}
            return resultFeeInfoList;
        }

        public List<FeeInfo> GetReportFeeChildrenInfoList(String reportID, FeeInfo feeInfo)
        {
            WhereSqlClauseBuilder builderChilder = new WhereSqlClauseBuilder();
            builderChilder.AppendItem(FeeInfoDBConst.ParentID, feeInfo.FeeID);
            builderChilder.AppendItem(String.Concat(FeeInfoDBConst.FeeID, " NOT IN (SELECT FEE_ID FROM REPORT_FEE WHERE REPORT_ID='", reportID, "')"));
            List<FeeInfo> resultList = FeeInfoAdapter.Instance.GetFeeInfoList(null, builderChilder);
            List<FeeInfo> childrenList = new List<FeeInfo>();
            foreach (FeeInfo info in resultList)
            {
                childrenList.AddRange(this.GetReportFeeChildrenInfoList(reportID, info));
            }
            resultList.AddRange(childrenList);
            return resultList;
        }

        #endregion 报表款项


        #region 报表人员

        public Int32 InsertReportPerson(ReportPerson reportPerson)
        {
            String sql = ORMapping.GetInsertSql(reportPerson, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 UpdateReportPerson(ReportPerson reportPerson)
        {
            String sql = ORMapping.GetUpdateSql(reportPerson, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteReportPerson(String reportID, String personId)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportPersonDBConst.ReportID, reportID);
            builder.AppendItem(ReportPersonDBConst.PersonID, personId);
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", ReportPersonDBConst.TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public ReportPerson LoadReportPerson(String reportID, String personId)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportPersonDBConst.ReportID, reportID);
            builder.AppendItem(ReportPersonDBConst.PersonID, personId);
            return GetReportPersonList(builder).FirstOrDefault();
        }

        public List<ReportPerson> GetReportPersonList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ", ReportPersonDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<ReportPerson> result = new List<ReportPerson>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                ReportPerson info = new ReportPerson();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public List<ReportPerson> GetReportPersonList(string reportID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ReportPersonDBConst.ReportID, reportID);
            return GetReportPersonList(builder);
        }

        #endregion 报表人员
    }
}
