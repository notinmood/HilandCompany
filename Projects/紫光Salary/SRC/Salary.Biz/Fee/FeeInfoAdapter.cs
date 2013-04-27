using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Salary.Biz;
using Salary.Core.Data;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using Salary.Biz.Eunm;
using System.Text.RegularExpressions;
using System.Text;
using Salary.Core.Utility;
using System.Transactions;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data;

namespace Salary.Biz
{
    public class FeeInfoAdapter
    {
        public FeeInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static FeeInfoAdapter _Instance = new FeeInfoAdapter();
        public static FeeInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertFeeInfo(FeeInfo feeInfo)
        {
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                String sql = ORMapping.GetInsertSql(feeInfo, BuilderEx.TSqlBuilderInstance);
                int result = _DataHelper.ExecuteSql(sql);
                if (!String.IsNullOrEmpty(feeInfo.ParentID))
                {
                    this.UpdateCalculateExp((feeInfo is FeeMonthInfo) ? ((FeeMonthInfo)feeInfo).YearMonth : null, feeInfo.ParentID);
                }
                scope.Complete();
                return result;
            }
        }

        public Int32 UpdateFeeInfo(FeeInfo feeInfo)
        {
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                String sql = sql = ORMapping.GetUpdateSql(feeInfo, BuilderEx.TSqlBuilderInstance);
                int result = _DataHelper.ExecuteSql(sql);
                if (!String.IsNullOrEmpty(feeInfo.ParentID))
                {
                    this.UpdateCalculateExp((feeInfo is FeeMonthInfo) ? ((FeeMonthInfo)feeInfo).YearMonth : null, feeInfo.ParentID);
                }
                scope.Complete();
                return result;
            }
        }
        public Int32 ChangeStatus(String yearMonth, String feeID, Int32 userFlag)
        {
            FeeInfo feeInfo = this.LoadFeeInfo(yearMonth, feeID);
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeInfoDBConst.FeeID, feeID);
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(FeeMonthInfoDBConst.YearMonth, yearMonth);
            }
            String sql = String.Format("UPDATE {0} SET USE_FLAG={1} WHERE {2} ", String.IsNullOrEmpty(yearMonth) ? FeeInfoDBConst.TableName : ORMapping.GetMappingInfo<FeeMonthInfo>().TableName, userFlag, builder.ToSqlString());
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                int result = _DataHelper.ExecuteSql(sql);
                if (!String.IsNullOrEmpty(feeInfo.ParentID))
                {
                    this.UpdateCalculateExp(yearMonth, feeInfo.ParentID);
                }
                scope.Complete();
                return result;
            }
        }
        private void UpdateCalculateExp(String yearMonth, String feeID)
        {
            FeeInfo feeInfo = this.LoadFeeInfo(yearMonth, feeID);
            if (feeInfo != null)
            {
                WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
                builder.AppendItem(FeeInfoDBConst.UseFlag, Status.True.ToString("D"));
                builder.AppendItem(FeeInfoDBConst.ParentID, feeID);
                String exp = String.Empty;
                foreach (FeeInfo fee in this.GetFeeInfoList(yearMonth, builder))
                {
                    if (!String.IsNullOrEmpty(fee.CalculateSign.Trim()))
                    {
                        exp += String.Concat(EnumHelper.GetDescription<CalculateSign>(fee.CalculateSign), " [", fee.FeeID, "] ");//Not第一个运算符不是+怎么办
                    }
                }
                feeInfo.CalculateExp = exp.Length > 1 ? exp.StartsWith("+") ? exp.Remove(0, 2) : exp : "";
                this.UpdateFeeInfo(feeInfo);
            }
        }

        public Int32 DeleteFeeInfo(String yearMonth, String feeID)
        {
            FeeInfo feeInfo = this.LoadFeeInfo(yearMonth, feeID);
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeInfoDBConst.FeeID, feeID);
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(FeeMonthInfoDBConst.YearMonth, yearMonth);
            }//Not 需改进 
            String sql = String.Format(@"DELETE FROM {0} WHERE {3};
                                         DELETE FROM {1} WHERE {3};
                                         DELETE FROM {2} WHERE {3}",
                    String.IsNullOrEmpty(yearMonth) ? FeeInfoDBConst.TableName : ORMapping.GetMappingInfo<FeeMonthInfo>().TableName,
                    String.IsNullOrEmpty(yearMonth) ? TaxInfoDBConst.TableName : ORMapping.GetMappingInfo<TaxMonthInfo>().TableName,
                    String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeMonthInfo>().TableName,
                    builder.ToSqlString());
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                int result = _DataHelper.ExecuteSql(sql);
                if (!String.IsNullOrEmpty(feeInfo.ParentID))
                {
                    this.UpdateCalculateExp(yearMonth, feeInfo.ParentID);
                }
                scope.Complete();
                return result;
            }
        }
        
        public FeeInfo LoadFeeInfo(String yearMonth, String feeID)
        {
            feeID = feeID.Replace("[", "").Replace("]", "");
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeInfoDBConst.FeeID, feeID);
            return GetFeeInfoList(yearMonth, builder).FirstOrDefault();
        }
        public FeeInfo LoadFeeInfoByCode(String yearMonth, String feeCode)
        {
            feeCode = feeCode.Replace("[", "").Replace("]", "");
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeInfoDBConst.FeeCode, feeCode);
            return GetFeeInfoList(yearMonth, builder).FirstOrDefault();
        }
        public FeeInfo LoadFeeInfoByName(String yearMonth, String feeName)
        {
            String strParentExp = " ";//必须带一个空格
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            if (feeName.Contains("{"))//如果带父名
            {
                String parameterClausePattern = @"\{(.*?)\}";
                MatchCollection parameterClauseMatchs = Regex.Matches(feeName, parameterClausePattern, RegexOptions.Singleline);
                Dictionary<String, String> para = parameterClauseMatchs.OfType<Match>().Select(i => i.Value).Distinct().ToDictionary(i => i, i => string.Empty);
                strParentExp = para.Keys.FirstOrDefault();
                builder.AppendItem(FeeInfoDBConst.ParentName, strParentExp.Replace("{", "").Replace("}", ""));
            }
            builder.AppendItem(FeeInfoDBConst.FeeName, feeName.Replace(strParentExp, "").Replace("[", "").Replace("]", ""));
            return this.GetFeeInfoList(yearMonth, builder).FirstOrDefault();
        }

        public List<FeeInfo> GetFeeInfoList(String yearMonth, WhereSqlClauseBuilder builder)
        {
            if (!String.IsNullOrEmpty(yearMonth))
            {
                builder.AppendItem(FeeMonthInfoDBConst.YearMonth, yearMonth);
            }
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY FEE_CODE", String.IsNullOrEmpty(yearMonth) ? FeeInfoDBConst.TableName : ORMapping.GetMappingInfo<FeeMonthInfo>().TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<FeeInfo> result = new List<FeeInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                FeeInfo info = String.IsNullOrEmpty(yearMonth) ? new FeeInfo() : new FeeMonthInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public String CreateFeeCode()
        {
            String sql = String.Format("SELECT ISNULL(MAX({1}) + 1,101) FROM {0}", FeeInfoDBConst.TableName, FeeInfoDBConst.FeeCode);
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt.Rows[0][0].ToString();
        }
        public String CreateFeeCode(String feeTypeValue, String parentFeeID)
        {
            String sql;
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            if (String.IsNullOrEmpty(parentFeeID))
            {
                builder.AppendItem(String.Concat("ISNULL(", FeeInfoDBConst.ParentID, ", '')"), "");
                if (!String.IsNullOrEmpty(feeTypeValue))
                {
                    builder.AppendItem(FeeInfoDBConst.FeeType, feeTypeValue);
                }
                sql = String.Format("SELECT ISNULL(MAX({1}) + 1,100) FROM {0} WHERE {2}", FeeInfoDBConst.TableName, FeeInfoDBConst.FeeCode, builder.ToSqlString());
            }
            else
            {
                FeeInfo parentFeeInfo = this.LoadFeeInfo(null, parentFeeID);
                builder.AppendItem(FeeInfoDBConst.ParentID, parentFeeID);
                sql = String.Format("SELECT ISNULL(MAX({1}) + 1,'{2}01') FROM {0} WHERE {3}", FeeInfoDBConst.TableName, FeeInfoDBConst.FeeCode, parentFeeInfo.FeeCode, builder.ToSqlString());
            }
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt.Rows[0][0].ToString();
        }

        public bool IsFeeCodeUsed(FeeInfo feeInfo, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeInfoDBConst.FeeCode, feeInfo.FeeCode);
            FeeInfo info = GetFeeInfoList((feeInfo is FeeMonthInfo) ? ((FeeMonthInfo)feeInfo).YearMonth : null, builder).FirstOrDefault();
            if (info == null)
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
                    return info.FeeID.Equals(feeInfo.FeeID) ? false : true;
                }
            }
        }
        public bool IsFeeNameUsed(FeeInfo feeInfo, bool isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeMonthInfoDBConst.FeeName, feeInfo.FeeName);
            FeeInfo info = GetFeeInfoList((feeInfo is FeeMonthInfo) ? ((FeeMonthInfo)feeInfo).YearMonth : null, builder).FirstOrDefault();
            if (info == null)
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
                    return info.FeeID.Equals(feeInfo.FeeID) ? false : true;
                }
            }
        }

        public String ConvertExp(String yearMonth, String caculateExp)
        {
            String rtn = this.ConvertFunctionNameWithFunctionCode(yearMonth, caculateExp);
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
                rtn = rtn.Replace(dt, ConvertCaculateToCommon(yearMonth, str));//String.Concat("ISNULL(", rtn.Replace(dt, ConvertCaculateToCommon(yearMonth, str)), ", 0)");
            }
            return rtn;// rtn.Replace("[", "ISNULL([").Replace("]", "],0)");
        }
        private String ConvertCaculateToCommon(String yearMonth, String feeID)
        {//将计算型的转换为基本型的
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeMonthInfoDBConst.FeeID, feeID);
            FeeInfo feeInfo = GetFeeInfoList(yearMonth, builder).FirstOrDefault();
            if (feeInfo.FeeType.Equals(FeeType.Common))
            {
                if (!String.IsNullOrEmpty(feeInfo.CalculateExp))
                {
                    return String.Concat("(", this.ConvertExp(yearMonth, feeInfo.CalculateExp), ")");
                }
                return "(ISNULL([" + feeID + "],0))";
            }
            else if (feeInfo.FeeType.Equals(FeeType.Tax))
            {
                return String.Concat("ISNULL(DBO.CACULATETAX(''" + yearMonth + "'',''", feeInfo.FeeID
                    , "'',", ConvertExp(yearMonth, this.GetTaxFeeCalculateExp(yearMonth, feeInfo.FeeID)), "),0)");
            }
            else if (feeInfo.FeeType.Equals(FeeType.Parameter))
            {
                return this.GetParameterFeeValue(yearMonth, feeID).ToString();
            }
            else
            {
                return "(ISNULL(" + ConvertExp(yearMonth, feeInfo.CalculateExp) + ",0))";
            }
        }

        private Decimal GetParameterFeeValue(String yearMonth, String parameterFeeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeMonthInfoDBConst.FeeType, FeeType.Parameter.ToString("D"));
            builder.AppendItem(String.Concat(FeeMonthInfoDBConst.StartDate, "<= DATEADD(Day,-1,CONVERT(CHAR(8),DATEADD(Month,1,'", yearMonth, "01'),120)+'1')"));

            WhereSqlClauseBuilder builderOr = new WhereSqlClauseBuilder(LogicOperatorDefine.Or);
            builderOr.AppendItem(FeeMonthInfoDBConst.FeeID, parameterFeeID);
            builderOr.AppendItem(FeeMonthInfoDBConst.ParentID, parameterFeeID);
            builder.AppendItem(builderOr.ToSqlString());
            FeeInfo feeInfo = GetFeeInfoList(yearMonth, builder).OrderBy(fee => fee.StartDate).FirstOrDefault();
            return feeInfo.DefaultValue;
        }

        private String GetParameterFeeID(String yearMonth, String parameterFeeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeMonthInfoDBConst.FeeType, FeeType.Parameter.ToString("D"));
            builder.AppendItem(String.Concat(FeeMonthInfoDBConst.StartDate, "<= DATEADD(Day,-1,CONVERT(CHAR(8),DATEADD(Month,1,'", yearMonth, "01'),120)+'1')"));

            WhereSqlClauseBuilder builderOr = new WhereSqlClauseBuilder(LogicOperatorDefine.Or);
            builderOr.AppendItem(FeeMonthInfoDBConst.FeeID, parameterFeeID);
            builderOr.AppendItem(FeeMonthInfoDBConst.ParentID, parameterFeeID);
            builder.AppendItem(builderOr.ToSqlString());
            FeeInfo feeInfo = GetFeeInfoList(yearMonth, builder).OrderBy(fee => fee.StartDate).FirstOrDefault();
            return feeInfo.FeeID;
        }

        private String GetTaxFeeID(String yearMonth, String taxFeeID)
        {
            if (String.IsNullOrEmpty(yearMonth))
            { 
                yearMonth = DateTime.Now.ToString("yyyyMM");
            }
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeMonthInfoDBConst.FeeType, FeeType.Tax.ToString("D"));
            builder.AppendItem(String.Concat(FeeMonthInfoDBConst.StartDate, "<= DATEADD(Day,-1,CONVERT(CHAR(8),DATEADD(Month,1,'", yearMonth, "01'),120)+'1')"));
            WhereSqlClauseBuilder builderOr = new WhereSqlClauseBuilder(LogicOperatorDefine.Or);
            builderOr.AppendItem(FeeMonthInfoDBConst.FeeID, taxFeeID);
            builderOr.AppendItem(FeeMonthInfoDBConst.ParentID, taxFeeID);
            builder.AppendItem(builderOr.ToSqlString());
            FeeInfo feeInfo = GetFeeInfoList(yearMonth, builder).OrderBy(fee => fee.StartDate).FirstOrDefault();
            return feeInfo.FeeID;
        }

        public String GetTaxFeeCalculateExp(String yearMonth, String taxFeeID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(FeeMonthInfoDBConst.FeeType, FeeType.Tax.ToString("D"));
            builder.AppendItem(String.Concat(FeeMonthInfoDBConst.StartDate, "<= DATEADD(Day,-1,CONVERT(CHAR(8),DATEADD(Month,1,'", yearMonth, "01'),120)+'1')"));

            WhereSqlClauseBuilder builderOr = new WhereSqlClauseBuilder(LogicOperatorDefine.Or);
            builderOr.AppendItem(FeeMonthInfoDBConst.FeeID, taxFeeID);
            builderOr.AppendItem(FeeMonthInfoDBConst.ParentID, taxFeeID);
            builder.AppendItem(builderOr.ToSqlString());
            FeeInfo feeInfo = GetFeeInfoList(yearMonth, builder).OrderBy(fee => fee.StartDate).FirstOrDefault();
            return feeInfo.CalculateExp;
        }

        public String ConvertFunctionNameWithFunctionCode(String yearMonth, String caculateExp)
        {
            foreach (var funcName in (FeeFunction[])Enum.GetValues(typeof(FeeFunction)))
            {
                switch (funcName)
                {
                    case FeeFunction.PersonalIncomeTax:
                        caculateExp = caculateExp.Replace(funcName.Description() + "(", 
                            String.Concat("DBO.CACULATETAX(''", yearMonth, "'',''", SalaryConst.PersonalIncomeTaxID, "'',"));
                        break;
                    case FeeFunction.ServiceFeeTax:
                        caculateExp = caculateExp.Replace(funcName.Description() + "(",
                            String.Concat("DBO.CaculateServiceTax(''", yearMonth, "'',''", SalaryConst.ServiceFeeTaxID, "'',"));
                        break;
                    case FeeFunction.GJJ:
                        caculateExp = caculateExp.Replace(funcName.Description() + "(", 
                            String.Concat("DBO.CACULATEGJJ(''", yearMonth, "'',''", SalaryConst.GJJMINFeeID, "'',''", SalaryConst.GJJMAXFeeID, "'',"));
                        break;
                    case FeeFunction.SHBX:
                        caculateExp = caculateExp.Replace(funcName.Description() + "(",
                            String.Concat("DBO.CACULATESHBX(''", yearMonth, "'',''", SalaryConst.SPGZMINFeeID, "'',''", SalaryConst.SPGZMAXFeeID, "'',"));
                        break;
                    default:
                        caculateExp = caculateExp.Replace(funcName.Description() + "(", "");
                        break;
                }
            }
            return caculateExp;
        }

        //将费用名称替换为编码
        public String ConvertFeeNameWithFeeCodeInExp(String yearMonth, String caculateExp)
        {
            String rtn = caculateExp;//this.ConvertFunctionNameWithFunctionCode(yearMonth, caculateExp);
            String parameterClausePattern = @"\[(.*?)\]";
            MatchCollection parameterClauseMatchs = Regex.Matches(rtn, parameterClausePattern, RegexOptions.Singleline);
            Dictionary<String, String> para = parameterClauseMatchs.OfType<Match>().Select(i => i.Value).Distinct().ToDictionary(i => i, i => string.Empty);
            if (parameterClauseMatchs.Count > 0)
            {
                foreach (String dt in para.Keys)
                {
                    rtn = rtn.Replace(dt, String.Concat("[", this.LoadFeeInfoByName(yearMonth, dt).FeeID, "]"));
                }
            }
            return rtn;
        }
        //将费用编码替换为名称
        public String ConvertFeeCodeWithFeeNameInExp(String yearMonth, String caculateExp)
        {
            String rtn = caculateExp;
            String parameterClausePattern = @"\[(.*?)\]";
            MatchCollection parameterClauseMatchs = Regex.Matches(rtn, parameterClausePattern, RegexOptions.Singleline);
            Dictionary<String, String> para = parameterClauseMatchs.OfType<Match>().Select(i => i.Value).Distinct().ToDictionary(i => i, i => string.Empty);
            if (parameterClauseMatchs.Count > 0)
            {
                foreach (String dt in para.Keys)
                {
                    String strReplace = "";
                    FeeInfo feeInfo = this.LoadFeeInfo(yearMonth, dt);
                    if (!String.IsNullOrEmpty(feeInfo.ParentID))
                    {
                        strReplace = String.Concat("{", feeInfo.ParentName, "}");
                    }
                    strReplace += feeInfo.FeeName;
                    rtn = rtn.Replace(dt, String.Concat("[", strReplace, "]"));
                }
            }
            return rtn;
        }
    }
}
