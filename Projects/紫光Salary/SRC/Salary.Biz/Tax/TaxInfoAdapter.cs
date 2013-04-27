using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Salary.Biz;
using Salary.Core.Data;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using Salary.Core.Utility;

namespace Salary.Biz
{
    public class TaxInfoAdapter
    {
        public TaxInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static TaxInfoAdapter _Instance = new TaxInfoAdapter();
        public static TaxInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertTaxInfo(TaxInfo taxInfo)
        {
            String sql = ORMapping.GetInsertSql(taxInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }
        public Int32 UpdateTaxInfo(TaxInfo taxInfo)
        {
            String sql = ORMapping.GetUpdateSql(taxInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }
        public Int32 DeleteTaxInfo(String yearMonth, String taxID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(TaxInfoDBConst.TaxID, taxID);
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", String.IsNullOrEmpty(yearMonth) ? TaxInfoDBConst.TableName : ORMapping.GetMappingInfo<TaxMonthInfo>().TableName , builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public TaxInfo LoadTaxInfo(String yearMonth, String taxID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(TaxInfoDBConst.TaxID, taxID);
            return GetTaxInfoList(yearMonth, builder).FirstOrDefault();
        }

        public List<TaxInfo> GetTaxInfoList(String yearMonth, WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY QUANTUM_START"
                , String.IsNullOrEmpty(yearMonth) ? TaxInfoDBConst.TableName : ORMapping.GetMappingInfo<TaxMonthInfo>().TableName 
                , builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<TaxInfo> result = new List<TaxInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                TaxInfo info = String.IsNullOrEmpty(yearMonth) ? new TaxInfo() : new TaxMonthInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }
    }
}
