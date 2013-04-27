using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Salary.Biz;
using Salary.Core.Data;
using Salary.Core.Utility;
using System.Transactions;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using Salary.Biz.Eunm;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data;


namespace Salary.Biz
{
    public class PayMonthInfoAdapter
    {
        public PayMonthInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static PayMonthInfoAdapter _Instance = new PayMonthInfoAdapter();
        public static PayMonthInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertPayMonthInfo(String yearMonth, bool rewrite)
        {
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                StringBuilder sql = new StringBuilder();
                WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
                if (rewrite)
                {
                    builder.AppendItem(PayMonthInfoDBConst.YearMonth, yearMonth);
                    this.DeletePayMonthInfo(builder);
                    //工资月表
                    foreach (PayMonthInfo payMonthInfo in PersonBaseFeeInfoAdapter.Instance.GetPersonBaseFeeInfoList(PersonBaseFeeTarget.PayMonth, yearMonth))
                    {
                        sql.Append(ORMapping.GetInsertSql(payMonthInfo, BuilderEx.TSqlBuilderInstance));
                        sql.Append(";");
                    }
                }
                else
                {
                    builder.AppendItem(String.Concat(PersonInfoDBConst.EntryDate, "<= DATEADD(Day,-1,CONVERT(CHAR(8),DATEADD(Month,1,'", yearMonth, "01'),120)+'1')"));
                    builder.AppendItem(String.Concat("ISNULL(", PersonInfoDBConst.LeftDate, ",DATEADD(Month,1,GETDATE()))"), yearMonth + "01", ">=");
                    List<PersonInfo> personList = PersonInfoAdapter.Instance.GetPersonInfoList(builder);
                    //工资项目月表
                    foreach (FeeInfo feeInfo in FeeInfoAdapter.Instance.GetFeeInfoList(null, null))
                    {
                        FeeMonthInfo feeMonthInfo = Tools.ClassDataCopy<FeeInfo, FeeMonthInfo>(feeInfo);
                        feeMonthInfo.YearMonth = yearMonth;
                        sql.Append(ORMapping.GetInsertSql(feeMonthInfo, BuilderEx.TSqlBuilderInstance));
                        sql.Append(";");
                    }//人员基本工资月表
                    foreach (PersonBaseFeeMonthInfo personBaseFeeMonthInfo in PersonBaseFeeInfoAdapter.Instance.GetPersonBaseFeeInfoList(PersonBaseFeeTarget.PersonBaseFeeMonth, null).Join(personList, baseFee => baseFee.PersonID, person => person.PersonID, (baseFeeDepart, person) => baseFeeDepart).ToList())
                    {
                        personBaseFeeMonthInfo.YearMonth = yearMonth;
                        sql.Append(ORMapping.GetInsertSql(personBaseFeeMonthInfo, BuilderEx.TSqlBuilderInstance));
                        sql.Append(";");
                        //工资月表
                        FeeInfo feeInfo = FeeInfoAdapter.Instance.LoadFeeInfo(null, personBaseFeeMonthInfo.FeeID);
                        if (String.IsNullOrEmpty(feeInfo.CalculateExp))
                        {
                            PayMonthInfo PayMonthInfo = Tools.ClassDataCopy<PersonBaseFeeMonthInfo, PayMonthInfo>(personBaseFeeMonthInfo);
                            PayMonthInfo.YearMonth = yearMonth;
                            PayMonthInfo.PayMoney = PayMonthInfo.FeeValue;
                            sql.Append(ORMapping.GetInsertSql(PayMonthInfo, BuilderEx.TSqlBuilderInstance));
                            sql.Append(";");
                        }
                    }
                    //人员基本工资项目部门分摊月表
                    WhereSqlClauseBuilder builderDepartment = new WhereSqlClauseBuilder();
                    foreach (PersonBaseFeeDepartmentProjectInfo personFeeDepartmentProjectInfo in PersonBaseFeeDepartmentProjectInfoAdapter.Instance.GetPersonBaseFeeDepartmentProjectInfoList(null, null).Join(personList, baseFeeDepart => baseFeeDepart.PersonID, person => person.PersonID, (baseFeeDepart, person) => baseFeeDepart).ToList())
                    {
                        PersonBaseFeeDepartmentProjectMonthInfo personFeeDepartmentProjectMonthInfo = Tools.ClassDataCopy<PersonBaseFeeDepartmentProjectInfo, PersonBaseFeeDepartmentProjectMonthInfo>(personFeeDepartmentProjectInfo);
                        personFeeDepartmentProjectMonthInfo.YearMonth = yearMonth;
                        sql.Append(ORMapping.GetInsertSql(personFeeDepartmentProjectMonthInfo, BuilderEx.TSqlBuilderInstance));
                        sql.Append(";");
                    }
                    //税率月表
                    builder.Clear();
                    foreach (TaxInfo taxInfo in TaxInfoAdapter.Instance.GetTaxInfoList(null, builder))
                    {
                        TaxMonthInfo taxMonthInfo = Tools.ClassDataCopy<TaxInfo, TaxMonthInfo>(taxInfo);
                        taxMonthInfo.YearMonth = yearMonth;
                        sql.Append(ORMapping.GetInsertSql(taxMonthInfo, BuilderEx.TSqlBuilderInstance));
                        sql.Append(";");
                    }
                }
                int result = _DataHelper.ExecuteSql(sql.ToString());
                scope.Complete();
                return result;
            }
        }


        public Int32 KelownaPayMonthInfo(String yearMonth)
        {
            DateTime dt = DateTime.Parse(yearMonth.Insert(4, "-").Insert(7, "-01")).AddMonths(1);
            String yearMonthD = String.Concat(dt.Year.ToString(), dt.Month.ToString());
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                StringBuilder sql = new StringBuilder();
                WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
                foreach (PersonBaseFeeMonthInfo PersonBaseFeeMonthInfo in PersonBaseFeeInfoAdapter.Instance.GetPersonBaseFeeInfoList(PersonBaseFeeTarget.PersonBaseFeeMonth, yearMonth))
                {
                    PersonBaseFeeMonthInfo.YearMonth = yearMonthD;
                    sql.Append(ORMapping.GetInsertSql(PersonBaseFeeMonthInfo, BuilderEx.TSqlBuilderInstance));
                    sql.Append(";");
                }
                //工资月表
                builder.AppendItem(PayMonthInfoDBConst.YearMonth, yearMonth);
                foreach (PayMonthInfo PayMonthInfo in PayMonthInfoAdapter.Instance.GetPayMonthInfoList(builder))
                {
                    PayMonthInfo.YearMonth = yearMonthD;
                    sql.Append(ORMapping.GetInsertSql(PayMonthInfo, BuilderEx.TSqlBuilderInstance));
                    sql.Append(";");
                }
                //人员基本工资项目部门分摊月表
                builder.Clear();
                builder.AppendItem(PersonBaseFeeDepartmentProjectMonthInfoDBConst.YearMonth, yearMonth);
                foreach (PersonBaseFeeDepartmentProjectMonthInfo personFeeDepartmentProjectMonthInfo in PersonBaseFeeDepartmentProjectInfoAdapter.Instance.GetPersonBaseFeeDepartmentProjectInfoList(yearMonth, builder))
                {
                    personFeeDepartmentProjectMonthInfo.YearMonth = yearMonthD;
                    sql.Append(ORMapping.GetInsertSql(personFeeDepartmentProjectMonthInfo, BuilderEx.TSqlBuilderInstance));
                    sql.Append(";");
                }
                //工资项目月表
                builder.Clear();
                builder.AppendItem(FeeMonthInfoDBConst.YearMonth, yearMonth);
                foreach (FeeMonthInfo feeMonthInfo in FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder))
                {
                    feeMonthInfo.YearMonth = yearMonthD;
                    sql.Append(ORMapping.GetInsertSql(feeMonthInfo, BuilderEx.TSqlBuilderInstance));
                    sql.Append(";");
                }
                //税率月表
                builder.Clear();
                builder.AppendItem(TaxMonthInfoDBConst.YearMonth, yearMonth);
                foreach (TaxMonthInfo taxMonthInfo in TaxInfoAdapter.Instance.GetTaxInfoList(yearMonth, builder))
                {
                    taxMonthInfo.YearMonth = yearMonthD;
                    sql.Append(ORMapping.GetInsertSql(taxMonthInfo, BuilderEx.TSqlBuilderInstance));
                    sql.Append(";");
                }
                int result = _DataHelper.ExecuteSql(sql.ToString());
                scope.Complete();
                return result;
            }
        }

        public List<PayMonthInfo> GetPayMonthInfoList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ",ORMapping.GetMappingInfo<PayMonthInfo>().TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<PayMonthInfo> result = new List<PayMonthInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                PayMonthInfo info = new PayMonthInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public bool IsCreated(String yearMonth)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PayMonthInfoDBConst.YearMonth, yearMonth);
            String sql = String.Format("SELECT DISTINCT YEAR_MONTH FROM {0} WHERE {1} ", ORMapping.GetMappingInfo<PayMonthInfo>().TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt.Rows.Count > 0;
        }

        public Int32 DeletePayMonthInfo(WhereSqlClauseBuilder builder)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(String.Format("DELETE FROM {0} WHERE {1}; ", ORMapping.GetMappingInfo<PayMonthInfo>().TableName, builder.ToSqlString()));
            sql.Append(String.Format("DELETE FROM {0} WHERE {1}; ", ORMapping.GetMappingInfo<FeeMonthInfo>().TableName, builder.ToSqlString()));
            sql.Append(String.Format("DELETE FROM {0} WHERE {1}; ", ORMapping.GetMappingInfo<TaxMonthInfo>().TableName, builder.ToSqlString()));
            sql.Append(String.Format("DELETE FROM {0} WHERE {1}; ", ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName, builder.ToSqlString()));
            sql.Append(String.Format("DELETE FROM {0} WHERE {1}; ", ORMapping.GetMappingInfo<PersonBaseFeeDepartmentProjectMonthInfo>().TableName, builder.ToSqlString()));
            int result = _DataHelper.ExecuteSql(sql.ToString());
            return result;
        }


        public DataTable GetPayMonthInfoYears()
        {
            String sql = string.Format(@"SELECT DISTINCT SUBSTRING(YEAR_MONTH,1,4) AS 'Year' FROM PAY_MONTH ORDER BY Year DESC");
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable GetPayMonthInfoListDT(WhereSqlClauseBuilder builder)
        {
            String sql = string.Format(@"SELECT DISTINCT SUBSTRING(YEAR_MONTH,1,4) + '年' + SUBSTRING(YEAR_MONTH,5,2) + '月'  AS 'YearMonth' FROM PAY_MONTH WHERE {0} ORDER BY YearMonth DESC", builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable GetPayMonthInfoListDTP(int pageIndex, int pageSize, ref int recordCount)
        {
            String sql = string.Format(@"SELECT DISTINCT SUBSTRING(YEAR_MONTH,1,4) + '年' + SUBSTRING(YEAR_MONTH,5,2) + '月'  AS 'YearMonth' FROM PAY_MONTH");
            DataSet ds = _DataHelper.GetDataSet(PageHelper.GetCanPageSql(pageIndex, pageSize, sql, "ORDER BY YearMonth DESC"));
            recordCount = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            DataTable dt = ds.Tables[0];
            return dt;
        }

        public DataTable LoadPayMonthInfo(string yearMonth, String reportId)
        {
            String sql = string.Empty;
            ReportInfo report = ReportInfoAdapter.Instance.LoadReportInfo(reportId);
            if (report.ReportName.Contains("部门"))
            {
                sql = String.Format(@"DECLARE @SQL VARCHAR(8000), @CSQL VARCHAR(4000)
                                            SELECT @SQL=ISNULL(@SQL + '],[','') + FEE_ID, @CSQL=ISNULL(@CSQL + ',','') + 'SUM(' + FEE_ID + ') AS ''' + FEE_ID + '''' FROM REPORT R INNER JOIN REPORT_FEE F ON R.REPORT_ID = F.REPORT_ID WHERE R.REPORT_ID='{0}' ORDER BY F.ORDER_NO
                                            EXEC ('SELECT '''' AS ''PersonId'', P.DEPARTMENT_NAME AS ''部门'',' + @CSQL + ' FROM (SELECT PERSON_ID AS ''PersonId'', PERSON_NAME AS ''姓名'', FEE_ID,PAY_MONEY FROM PAY_MONTH WHERE YEAR_MONTH=''{1}'')P PIVOT(MAX(PAY_MONEY) FOR FEE_ID IN ([' + @SQL + ']))B  inner join PERSON P on B.PersonId = P.PERSON_ID GROUP BY DEPARTMENT_NAME ')", reportId, yearMonth);
            }
            else
            {
                sql = String.Format(@"DECLARE @SQL VARCHAR(8000), @CSQL VARCHAR(4000)
                                            SELECT @SQL=ISNULL(@SQL + '],[','') + FEE_ID FROM REPORT R INNER JOIN REPORT_FEE F ON R.REPORT_ID = F.REPORT_ID WHERE R.REPORT_ID='{0}' ORDER BY F.ORDER_NO
                                            EXEC ('
                            SELECT * FROM (SELECT PERSON_ID AS ''PersonId'', PERSON_NAME AS ''姓名'', FEE_ID,PAY_MONEY FROM PAY_MONTH WHERE YEAR_MONTH=''{1}'')P PIVOT(MAX(PAY_MONEY) FOR FEE_ID IN ([' + @SQL + ']))B
                            
                            ') ", reportId, yearMonth);
            }
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<ReportFee> reportFeeList = ReportInfoAdapter.Instance.GetReportFeeList(reportId);
            for (int i = 2; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = reportFeeList.Where(rf => rf.FeeID == dt.Columns[i].Caption).FirstOrDefault().ReportFeeName;
            }
            return dt;
        }

        public DataSet LoadPayMonthInfoDS(string yearMonth, String reportId)
        {
            String sql = string.Empty;
            ReportInfo report = ReportInfoAdapter.Instance.LoadReportInfo(reportId);
            if (report.ReportName.Contains("部门"))
            {
                sql = String.Format(@"DECLARE @SQL VARCHAR(8000), @CSQL VARCHAR(4000)
                                            SELECT @SQL=ISNULL(@SQL + '],[','') + FEE_NAME, @CSQL=ISNULL(@CSQL + ',','') + 'SUM(' + FEE_NAME + ') AS ''' + FEE_NAME + '''' FROM REPORT R INNER JOIN REPORT_FEE F ON R.REPORT_ID = F.REPORT_ID WHERE R.REPORT_ID='{0}' ORDER BY F.ORDER_NO
                                            EXEC ('SELECT 2 AS TABLENUM;SELECT P.DEPARTMENT_NAME AS ''部门'',' + @CSQL + ' FROM (SELECT PERSON_ID AS ''PersonId'', PERSON_NAME AS ''姓名'', FEE_NAME,PAY_MONEY FROM PAY_MONTH WHERE YEAR_MONTH=''{1}'')P PIVOT(MAX(PAY_MONEY) FOR FEE_NAME IN ([' + @SQL + ']))B  inner join PERSON P on B.PersonId = P.PERSON_ID GROUP BY DEPARTMENT_NAME ')", reportId, yearMonth);
            }
            else
            {
                sql = String.Format(@"DECLARE @SQL VARCHAR(8000)
                                            SELECT @SQL=ISNULL(@SQL + '],[','') + FEE_NAME FROM REPORT R INNER JOIN REPORT_FEE F ON R.REPORT_ID = F.REPORT_ID WHERE R.REPORT_ID='{0}' ORDER BY F.ORDER_NO
                                            EXEC ('SELECT 2 AS TABLENUM;SELECT * FROM (SELECT PERSON_NAME AS ''姓名'', FEE_NAME,PAY_MONEY FROM PAY_MONTH WHERE YEAR_MONTH=''{1}'')P PIVOT(MAX(PAY_MONEY) FOR FEE_NAME IN ([' + @SQL + ']))B') ", reportId, yearMonth);
            }
            DataSet ds = _DataHelper.GetDataSet(sql);//PERSON_ID AS ''PersonId'', 
            return ds;
        }

        public Int32 CalculatePayMonthInfo(String yearMonth)//计算薪资
        {
            int result = 0;
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeMonthInfoDBConst.YearMonth, yearMonth);
            builder.AppendItem(FeeMonthInfoDBConst.UseFlag, Status.True.ToString("D"));
            builder.AppendItem(FeeMonthInfoDBConst.FeeType, FeeType.Parameter.ToString("D"), "<>");
            builder.AppendItem(FeeMonthInfoDBConst.FeeType, FeeType.Tax.ToString("D"), "<>");
            builder.AppendItem(String.Concat("ISNULL(",FeeMonthInfoDBConst.CalculateExp,",'')<>''"));
            List<FeeInfo> feeList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder);
            foreach (FeeMonthInfo feeMonthInfo in feeList.Where(fe => !String.IsNullOrEmpty(fe.CalculateExp)))
            {
                String caculateExp = FeeInfoAdapter.Instance.ConvertExp(yearMonth, feeMonthInfo.CalculateExp);
                //if (feeMonthInfo.FeeType.Equals(FeeType.Tax))
                //{
                //    caculateExp = "ISNULL(DBO.CACULATETAX(''" + yearMonth + "'',''" + FeeInfoAdapter.Instance.GetTaxFeeID(yearMonth, feeMonthInfo.FeeID) + "''," + FeeInfoAdapter.Instance.ConvertExp(yearMonth, FeeInfoAdapter.Instance.GetTaxFeeCalculateExp(yearMonth, feeMonthInfo.FeeID)) + "),0)";
                //}
                //else
                //{
                //    caculateExp = FeeInfoAdapter.Instance.ConvertExp(yearMonth, feeMonthInfo.CalculateExp);
                //}
                String sql = String.Format(@"DECLARE @SQL VARCHAR(8000)
                                            SELECT @SQL=ISNULL(@SQL + '],[','') + Convert(nvarchar(32),FEE_ID) FROM FEE_MONTH F WHERE (F.FEE_TYPE={5} OR F.FEE_TYPE={6}) AND ISNULL(CALCULATE_EXP,'')='' AND YEAR_MONTH={0}                                           
                                            EXEC ('INSERT INTO PAY_MONTH(YEAR_MONTH,PERSON_ID,PERSON_NAME,FEE_ID,FEE_NAME,FEE_VALUE,PAY_MONEY,FORMULA) 
                                            SELECT ''{0}'' AS ''YEAR_MONTH'', PERSON_ID, PERSON_NAME, ''{1}'' AS ''FEE_ID'', ''{2}'' AS ''FEE_NAME'', {3} AS ''FEE_VALUE'', {3} AS ''PAY_MONEY'',''{4}'' AS ''FORMULA'' 
                                            FROM (SELECT PERSON_ID, PERSON_NAME, FEE_ID,PAY_MONEY FROM PAY_MONTH WHERE YEAR_MONTH=''{0}'')P PIVOT(MAX(PAY_MONEY) FOR FEE_ID IN ([' + @SQL + ']))B') "
                    , yearMonth, feeMonthInfo.FeeID, feeMonthInfo.FeeName, caculateExp, caculateExp.Replace("'", ""), FeeType.Common.ToString("D"), FeeType.Parameter.ToString("D"));
                result += _DataHelper.ExecuteSql(sql);
            }
            return result;
        }
        
        public DataTable GetPayMonthYears()
        {
            String sql = string.Format(@"SELECT DISTINCT SUBSTRING(YEAR_MONTH,1,4) AS 'Year' FROM PAY_MONTH ORDER BY Year DESC");
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable GetPayMonthListDT(WhereSqlClauseBuilder builder)
        {
            String sql = string.Format(@"SELECT DISTINCT SUBSTRING(YEAR_MONTH,1,4) + '年' + SUBSTRING(YEAR_MONTH,5,2) + '月'  AS 'YearMonth' FROM PAY_MONTH WHERE {0} ORDER BY YearMonth DESC", builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt;
        }
    }
}
