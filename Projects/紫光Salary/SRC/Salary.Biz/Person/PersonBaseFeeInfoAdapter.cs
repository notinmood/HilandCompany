using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Salary.Biz;
using Salary.Core.Data;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using Salary.Biz.Eunm;
using Salary.Core.Utility;
using System.Transactions;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data;
using System.Text.RegularExpressions;

namespace Salary.Biz
{
    public class PersonBaseFeeInfoAdapter
    {
        public PersonBaseFeeInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static PersonBaseFeeInfoAdapter _Instance = new PersonBaseFeeInfoAdapter();
        public static PersonBaseFeeInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertPersonBaseFeeInfo(PersonBaseFeeInfo PersonBaseFeeInfo)
        {
            String sql = ORMapping.GetInsertSql(PersonBaseFeeInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }
        public Int32 UpdatePersonBaseFeeInfo(PersonBaseFeeInfo personBaseFeeInfo)
        {
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                int result = this.DeletePersonBaseFeeInfo((personBaseFeeInfo is PersonBaseFeeMonthInfo) ? ((PersonBaseFeeMonthInfo)personBaseFeeInfo).YearMonth : null, personBaseFeeInfo.PersonID, personBaseFeeInfo.FeeID);
                result = InsertPersonBaseFeeInfo(personBaseFeeInfo);
                scope.Complete();
                return result;
            }
        }
        public Int32 DeletePersonBaseFeeInfo(String yearMonth, String personID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonBaseFeeInfoDBConst.PersonID, personID);
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(PersonBaseFeeMonthInfoDBConst.YearMonth, yearMonth);
            }
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }
        public Int32 DeletePersonBaseFeeInfo(String yearMonth, String personID, String feeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonBaseFeeInfoDBConst.PersonID, personID);
            builder.AppendItem(PersonBaseFeeInfoDBConst.FeeID, feeID);
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(PersonBaseFeeMonthInfoDBConst.YearMonth, yearMonth);
            }
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public PersonBaseFeeInfo LoadPersonBaseFeeInfo(PersonBaseFeeTarget feeTarget, String yearMonth, String personID, String feeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(String.Concat("F.", FeeInfoDBConst.FeeType), FeeType.Common.ToString("D"));
            builder.AppendItem(String.Concat("F.", FeeInfoDBConst.FeeID), feeID);
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(String.Concat("F.", FeeMonthInfoDBConst.YearMonth), yearMonth);
                builder.AppendItem(String.Concat("B.", PersonBaseFeeMonthInfoDBConst.YearMonth), yearMonth);
                builder.AppendItem(String.Concat("P.", PersonInfoDBConst.PersonID), personID);
                builder.AppendItem(String.Concat("P.", PersonInfoDBConst.EntryDate, "<= DATEADD(Day,-1,CONVERT(CHAR(8),DATEADD(Month,1,'", yearMonth, "01'),120)+'1')"));
                builder.AppendItem(String.Concat("ISNULL(P.", PersonInfoDBConst.LeftDate, ",DATEADD(Month,1,GETDATE()))"), yearMonth + "01", ">=");
            }
            String sql = string.Format(@"SELECT '{4}' AS 'YEAR_MONTH',F.FEE_ID,F.FEE_CODE,F.FEE_NAME,F.FEE_TYPE
                                            ,P.PERSON_ID,P.PERSON_NAME
                                            ,ISNULL(B.FEE_VALUE,F.DEFAULT_VALUE) AS 'FEE_VALUE'
                                            ,ISNULL(B.FEE_VALUE,F.DEFAULT_VALUE) AS 'PAY_MONEY'
                                            ,ISNULL(B.JITI_DAITAN,{2}) AS 'JITI_DAITAN',B.MEMO,F.CALCULATE_EXP 
                                            FROM PERSON P CROSS JOIN {0} F
                                            LEFT OUTER JOIN {1} B ON P.PERSON_ID=B.PERSON_ID AND F.FEE_ID = B.FEE_ID 
                                            WHERE {3}
                                            ORDER BY F.FEE_CODE"
                                            , String.IsNullOrEmpty(yearMonth) ? FeeInfoDBConst.TableName : ORMapping.GetMappingInfo<FeeMonthInfo>().TableName
                                            , String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName
                                            , (int)JitiDaitan.JitiDaitan, builder.ToSqlString(), yearMonth);
            DataTable dt = _DataHelper.GetDataTable(sql);
            if(dt.Rows.Count>0)
            {
                PersonBaseFeeInfo info = feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFeeMonth) ? new PersonBaseFeeMonthInfo() : feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFee) ? new PersonBaseFeeInfo() : new PayMonthInfo();
                ORMapping.DataRowToObject(dt.Rows[0], info);//计算有子项的组成
                if (!String.IsNullOrEmpty(dt.Rows[0]["CALCULATE_EXP"].ToString()))
                {
                    info.FeeValue = this.CalculateCommonFeeValue(yearMonth, info.PersonID, info.FeeID);
                }
                return info;
            }
            return new PersonBaseFeeInfo();
        }

        public List<PersonBaseFeeInfo> GetNoCalculatePersonBaseFeeInfoList(PersonBaseFeeTarget feeTarget, String yearMonth)
        {
            WhereSqlClauseBuilder builer=new WhereSqlClauseBuilder();
            if(!String.IsNullOrEmpty(yearMonth))
            {
                builer.AppendItem(PersonBaseFeeMonthInfoDBConst.YearMonth, yearMonth);
            }
            String sql = String.Format("SELECT * FROM {0} WHERE {1}", String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName, builer.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<PersonBaseFeeInfo> result = new List<PersonBaseFeeInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                PersonBaseFeeInfo info = feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFee) ? new PersonBaseFeeInfo() : new PersonBaseFeeMonthInfo(yearMonth);
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }//not

        public List<PersonBaseFeeInfo> GetPersonBaseFeeInfoList(PersonBaseFeeTarget feeTarget, String yearMonth)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(String.Concat("F.", FeeInfoDBConst.UseFlag), Status.True.ToString("D"));
            builder.AppendItem(String.Concat("F.", FeeInfoDBConst.FeeType), FeeType.Common.ToString("D"));
            //builder.AppendItem(String.Concat("ISNULL(F.", FeeInfoDBConst.CalculateExp, ",'')=''"));
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(String.Concat("F.", FeeMonthInfoDBConst.YearMonth), yearMonth);
                builder.AppendItem(String.Concat("B.", PersonBaseFeeMonthInfoDBConst.YearMonth), yearMonth);
                builder.AppendItem(String.Concat("P.", PersonInfoDBConst.EntryDate, "<= DATEADD(Day,-1,CONVERT(CHAR(8),DATEADD(Month,1,'", yearMonth, "01'),120)+'1')"));
                builder.AppendItem(String.Concat("ISNULL(P.", PersonInfoDBConst.LeftDate, ",DATEADD(Month,1,GETDATE()))"), yearMonth + "01", ">=");
            }
            String sql = string.Format(@"SELECT '{4}' AS 'YEAR_MONTH',F.FEE_ID,F.FEE_CODE,F.FEE_NAME,F.FEE_TYPE
                                            ,P.PERSON_ID,P.PERSON_NAME
                                            ,ISNULL(B.FEE_VALUE,F.DEFAULT_VALUE) AS 'FEE_VALUE'
                                            ,ISNULL(B.FEE_VALUE,F.DEFAULT_VALUE) AS 'PAY_MONEY'
                                            ,ISNULL(B.JITI_DAITAN,{2}) AS 'JITI_DAITAN',B.MEMO,F.CALCULATE_EXP 
                                            FROM PERSON P CROSS JOIN {0} F
                                            LEFT OUTER JOIN {1} B ON P.PERSON_ID=B.PERSON_ID AND F.FEE_ID = B.FEE_ID 
                                            WHERE {3}
                                            ORDER BY F.FEE_CODE"
                                            , String.IsNullOrEmpty(yearMonth) ? FeeInfoDBConst.TableName:ORMapping.GetMappingInfo<FeeMonthInfo>().TableName
                                            ,String.IsNullOrEmpty(yearMonth)?PersonBaseFeeInfoDBConst.TableName:ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName
                                            , (int)JitiDaitan.JitiDaitan, builder.ToSqlString(), yearMonth);
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<PersonBaseFeeInfo> result = new List<PersonBaseFeeInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                PersonBaseFeeInfo info = feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFeeMonth) ? new PersonBaseFeeMonthInfo() : feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFee) ? new PersonBaseFeeInfo() : new PayMonthInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                //计算有子项的组成
                //if (!String.IsNullOrEmpty(dt.Rows[i]["CALCULATE_EXP"].ToString()))
                //{
                //    info.FeeValue = this.CalculateCommonFeeValue(yearMonth, info.PersonID, info.FeeID);
                //}
                result.Add(info);
            }
            return result;
        }

        public List<PersonBaseFeeInfo> GetPersonBaseFeeInfoList(PersonBaseFeeTarget feeTarget, String yearMonth, String personID)
        {
            String columns = String.Empty, columnCals = String.Empty;
            //取得需计算的列表            
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(FeeMonthInfoDBConst.YearMonth, yearMonth);
            }
            builder.AppendItem(FeeInfoDBConst.UseFlag, Status.True.ToString("D"));
            builder.AppendItem(FeeInfoDBConst.FeeType, FeeType.Common.ToString("D"));
            //builder.AppendItem(String.Concat("ISNULL(",FeeMonthInfoDBConst.CalculateExp,",'')<>''"));
            List<FeeInfo> feeList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder);
            foreach (FeeInfo feeInfo in feeList)
            {
                columns += String.Concat(",[", feeInfo.FeeID, "]");
                columnCals += String.IsNullOrEmpty(feeInfo.CalculateExp) ? String.Concat(",Convert(decimal(18,3),[", feeInfo.FeeID, "]) AS [", feeInfo.FeeID, "]")
                    : String.Concat(",Convert(decimal(18,3),", FeeInfoAdapter.Instance.ConvertExp(yearMonth, feeInfo.CalculateExp).Replace(" ", ""), ") AS [", feeInfo.FeeID, "]");
            }
            WhereSqlClauseBuilder builderF=new WhereSqlClauseBuilder();
            builderF.AppendItem(String.Concat("F.", FeeInfoDBConst.FeeType), FeeType.Common.ToString("D"));
            builderF.AppendItem(String.Concat("ISNULL(F.", FeeInfoDBConst.CalculateExp, ",'')=''"));
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builderF.AppendItem(String.Concat("F.", FeeMonthInfoDBConst.YearMonth), yearMonth);
            }

            WhereSqlClauseBuilder builderP = new WhereSqlClauseBuilder();
            if (!String.IsNullOrEmpty(personID))
            {
                builderP.AppendItem(String.Concat("P.", PersonInfoDBConst.PersonID), personID);
            }

            String sql = String.Format(@"DECLARE @FSQL VARCHAR(8000), @CSQL VARCHAR(8000), @NSQL  VARCHAR(8000)
                                        SELECT @FSQL=ISNULL(@FSQL + '],[','') + Convert(nvarchar(32),FEE_ID) FROM {0} F 
                                        WHERE {2}
                                        exec ('SELECT PERSON_ID,PERSON_NAME, [FEE_ID], [FEE_VALUE] FROM (
                                        SELECT PERSON_ID, PERSON_NAME, {4} FROM (
                                        SELECT F.FEE_ID,P.PERSON_ID,P.PERSON_NAME,ISNULL(B.FEE_VALUE,F.DEFAULT_VALUE) AS ''PAY_MONEY''
                                        FROM PERSON P CROSS JOIN {0} F
                                        LEFT OUTER JOIN {1} B ON P.PERSON_ID=B.PERSON_ID AND F.FEE_ID = B.FEE_ID
                                        WHERE {3} AND {6})P 
                                        PIVOT(MAX(PAY_MONEY) FOR FEE_ID IN ([' + @FSQL + ']))B
                                        )T UNPIVOT ([FEE_VALUE] FOR [FEE_ID] IN ({5}))D')"
                ,String.IsNullOrEmpty(yearMonth) ? FeeInfoDBConst.TableName : ORMapping.GetMappingInfo<FeeMonthInfo>().TableName
                ,String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName
                ,builderF.ToSqlString()
                ,builderF.ToSqlString().Replace("'","''")
                , columnCals.Remove(0, 1), columns.Remove(0, 1)
                , builderP.ToSqlString().Replace("'", "''"));
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<PersonBaseFeeInfo> result = new List<PersonBaseFeeInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                PersonBaseFeeInfo info = feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFeeMonth) ? new PersonBaseFeeMonthInfo() : feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFee) ? new PersonBaseFeeInfo() : new PayMonthInfo(yearMonth);
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            //List<PersonBaseFeeInfo> resultNew = new List<PersonBaseFeeInfo>();
            List<PersonBaseFeeInfo> resultNew = result.Join(feeList, baseFee => baseFee.FeeID, fee => fee.FeeID, (baseFee, fee) =>
            {
                PersonBaseFeeInfo info = String.IsNullOrEmpty(yearMonth) ? new PersonBaseFeeInfo() : new PersonBaseFeeMonthInfo(yearMonth);
                info.FeeCode = fee.FeeCode;
                info.FeeID = baseFee.FeeID;
                info.FeeName = fee.FeeName;
                info.FeeValue = baseFee.FeeValue;
                info.PersonID = baseFee.PersonID;
                info.PersonName = baseFee.PersonName;
                return info;
                //return String.IsNullOrEmpty(yearMonth) ? new PersonBaseFeeInfo() : new PersonBaseFeeMonthInfo(yearMonth);
                //{
                //    FeeCode = fee.FeeCode,
                //    FeeID = baseFee.FeeID,
                //    FeeName = fee.FeeName,
                //    FeeValue = baseFee.FeeValue,
                //    PersonID = baseFee.PersonID,
                //    PersonName = baseFee.PersonName
                //};
            }).ToList();
            return resultNew;
        }

        public List<PersonBaseFeeInfo> GetPersonBaseFeeInfoList(PersonBaseFeeTarget feeTarget, String yearMonth, String personID, CommonFeeType commonFeeType)
        {
            String columns = String.Empty, columnCals = String.Empty;
            //取得需计算的列表            
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(FeeMonthInfoDBConst.YearMonth, yearMonth);
            }
            if (commonFeeType.ToString("D") != "0")
            {
                builder.AppendItem(FeeInfoDBConst.CommonFeeType, commonFeeType.ToString("d"));
            }
            builder.AppendItem(FeeInfoDBConst.UseFlag, Status.True.ToString("D"));
            builder.AppendItem(FeeInfoDBConst.FeeType, FeeType.Common.ToString("D"));
            //builder.AppendItem(String.Concat("ISNULL(",FeeMonthInfoDBConst.CalculateExp,",'')<>''"));
            List<FeeInfo> feeList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder);
            foreach (FeeInfo feeInfo in feeList)
            {
                columns += String.Concat(",[", feeInfo.FeeID, "]");
                columnCals += String.IsNullOrEmpty(feeInfo.CalculateExp) ? String.Concat(",Convert(decimal(18,3),[", feeInfo.FeeID, "]) AS [", feeInfo.FeeID, "]")
                    : String.Concat(",Convert(decimal(18,3),", FeeInfoAdapter.Instance.ConvertExp(yearMonth, feeInfo.CalculateExp).Replace(" ", ""), ") AS [", feeInfo.FeeID, "]");
            }
            WhereSqlClauseBuilder builderF = new WhereSqlClauseBuilder();
            builderF.AppendItem(String.Concat("F.", FeeInfoDBConst.FeeType), FeeType.Common.ToString("D"));
            builderF.AppendItem(String.Concat("ISNULL(F.", FeeInfoDBConst.CalculateExp, ",'')=''"));
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builderF.AppendItem(String.Concat("F.", FeeMonthInfoDBConst.YearMonth), yearMonth);
            }

            WhereSqlClauseBuilder builderP = new WhereSqlClauseBuilder();
            if (!String.IsNullOrEmpty(personID))
            {
                builderP.AppendItem(String.Concat("P.", PersonInfoDBConst.PersonID), personID);
            }

            String sql = String.Format(@"DECLARE @FSQL VARCHAR(8000), @CSQL VARCHAR(8000), @NSQL  VARCHAR(8000)
                                        SELECT @FSQL=ISNULL(@FSQL + '],[','') + Convert(nvarchar(32),FEE_ID) FROM {0} F 
                                        WHERE {2}
                                        exec ('SELECT D.PERSON_ID,D.PERSON_NAME,D.[FEE_ID],D.[FEE_VALUE],BF.DEPARTMENT_NAME,BF.PROJECT_NAME,BF.DEPARTMENT_ID,BF.PROJECT_ID FROM (
                                        SELECT PERSON_ID, PERSON_NAME, {4} FROM (
                                        SELECT F.FEE_ID,P.PERSON_ID,P.PERSON_NAME,ISNULL(B.FEE_VALUE,F.DEFAULT_VALUE) AS ''PAY_MONEY''
                                        FROM PERSON P CROSS JOIN {0} F
                                        LEFT OUTER JOIN {1} B ON P.PERSON_ID=B.PERSON_ID AND F.FEE_ID = B.FEE_ID
                                        WHERE {3} AND {6})P 
                                        PIVOT(MAX(PAY_MONEY) FOR FEE_ID IN ([' + @FSQL + ']))B
                                        )T UNPIVOT ([FEE_VALUE] FOR [FEE_ID] IN ({5}))D
                                        LEFT OUTER JOIN PERSON_BASE_FEE BF ON D.FEE_ID=BF.FEE_ID AND D.PERSON_ID=BF.PERSON_ID
                                        ')"
                , String.IsNullOrEmpty(yearMonth) ? FeeInfoDBConst.TableName : ORMapping.GetMappingInfo<FeeMonthInfo>().TableName
                , String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName
                , builderF.ToSqlString()
                , builderF.ToSqlString().Replace("'", "''")
                , columnCals.Remove(0, 1), columns.Remove(0, 1)
                , builderP.ToSqlString().Replace("'", "''"));
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<PersonBaseFeeInfo> result = new List<PersonBaseFeeInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                PersonBaseFeeInfo info = feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFeeMonth) ? new PersonBaseFeeMonthInfo() : feeTarget.Equals(PersonBaseFeeTarget.PersonBaseFee) ? new PersonBaseFeeInfo() : new PayMonthInfo(yearMonth);
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            //List<PersonBaseFeeInfo> resultNew = new List<PersonBaseFeeInfo>();
            List<PersonBaseFeeInfo> resultNew = result.Join(feeList.Where(f =>f.CommonFeeType==commonFeeType).ToList(), baseFee => baseFee.FeeID, fee => fee.FeeID, (baseFee, fee) =>
            {
                PersonBaseFeeInfo info = String.IsNullOrEmpty(yearMonth) ? new PersonBaseFeeInfo() : new PersonBaseFeeMonthInfo(yearMonth);
                info.FeeCode = fee.FeeCode;
                info.FeeID = baseFee.FeeID;
                info.FeeName = fee.FeeName;
                info.FeeValue = baseFee.FeeValue;
                info.PersonID = baseFee.PersonID;
                info.PersonName = baseFee.PersonName;
                info.DepartmentID = baseFee.DepartmentID;
                info.DepartmentName = baseFee.DepartmentName;
                info.ProjectID = baseFee.ProjectID;
                info.ProjectName = baseFee.ProjectName;
                return info;
            }).ToList();
            return resultNew;
        }

        public List<PersonBaseFeeInfo> GetPersonBaseFeeInfoList(String yearMonth, String personID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(PersonBaseFeeMonthInfoDBConst.YearMonth, yearMonth);
            }
            if (!String.IsNullOrEmpty(personID))
            {
                builder.AppendItem(PersonBaseFeeInfoDBConst.PersonID, personID);
            }
            String sql = String.Format("SELECT * FROM {0} WHERE {1}", String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeMonthInfoDBConst>().TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<PersonBaseFeeInfo> result = new List<PersonBaseFeeInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                PersonBaseFeeInfo info = String.IsNullOrEmpty(yearMonth) ? new PersonBaseFeeInfo() : new PersonBaseFeeMonthInfo(yearMonth);
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        private Decimal CalculateCommonFeeValue(String yearMonth, String personID, String feeID)
        {
            FeeInfo feeInfo = FeeInfoAdapter.Instance.LoadFeeInfo(yearMonth, feeID);
            String resultExp = this.ConvertCommonExp(yearMonth, feeInfo.CalculateExp);
            String parameterClausePattern = @"\[(.*?)\]";
            MatchCollection parameterClauseMatchs = Regex.Matches(resultExp, parameterClausePattern, RegexOptions.Singleline);
            Dictionary<String, String> para = parameterClauseMatchs.OfType<Match>().Select(i => i.Value).Distinct().ToDictionary(i => i, i => string.Empty);
            if (parameterClauseMatchs.Count == 0)
            {
                return 0;
            }
            foreach (String dt in para.Keys)
            {
                String strFeeID = dt.Replace("[", "").Replace("]", "");
                PersonBaseFeeInfo personBaseFeeInfo = this.LoadPersonBaseFeeInfo(PersonBaseFeeTarget.PersonBaseFee, yearMonth, personID, strFeeID);
                resultExp = resultExp.Replace(dt, personBaseFeeInfo.FeeValue.ToString());
            }
            return Decimal.Parse(resultExp);
        }
        private String ConvertCommonExp(String yearMonth, String caculateExp)
        {
            String rtn = FeeInfoAdapter.Instance.ConvertFunctionNameWithFunctionCode(yearMonth, caculateExp);
            String parameterClausePattern = @"\[(.*?)\]";
            MatchCollection parameterClauseMatchs = Regex.Matches(caculateExp, parameterClausePattern, RegexOptions.Singleline);
            Dictionary<String, String> para = parameterClauseMatchs.OfType<Match>().Select(i => i.Value).Distinct().ToDictionary(i => i, i => string.Empty);
            if (parameterClauseMatchs.Count == 0)
            {
                return "";
            }
            foreach (String dt in para.Keys)
            {
                String str = dt.Replace("[", "").Replace("]", "");
                rtn = rtn.Replace(dt, ConvertCommonExpToCommon(yearMonth, str));
            }
            return rtn;
        }
        private String ConvertCommonExpToCommon(String yearMonth, String feeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeMonthInfoDBConst.FeeID, feeID);
            FeeInfo feeInfo = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder).FirstOrDefault();
            if (feeInfo.FeeType.Equals(FeeType.Common))
            {
                if (!String.IsNullOrEmpty(feeInfo.CalculateExp))
                {
                    return String.Concat("(", this.ConvertCommonExp(yearMonth, feeInfo.CalculateExp), ")");
                }
                return "[" + feeID + "]";
            }
            return feeInfo.CalculateExp;
        }
        
        public Decimal CalculatePersonBaseFeeDepartmentProjectInfo(String yearMonth, String personID, String feeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(PersonBaseFeeDepartmentProjectMonthInfoDBConst.YearMonth, yearMonth);
            }
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.PersonID, personID);
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.FeeID, feeID);
            String sql = String.Format("SELECT ISNULL(SUM(STATION_MONEY),0) FROM {0} WHERE {1} "
                , String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeDepartmentProjectInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeDepartmentProjectMonthInfo>().TableName
                , builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            return Decimal.Parse(dt.Rows[0][0].ToString());
        }

    }
}
