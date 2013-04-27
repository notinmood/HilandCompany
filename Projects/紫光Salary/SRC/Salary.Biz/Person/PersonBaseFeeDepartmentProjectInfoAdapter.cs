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

namespace Salary.Biz
{
    public class PersonBaseFeeDepartmentProjectInfoAdapter
    {
        public PersonBaseFeeDepartmentProjectInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static PersonBaseFeeDepartmentProjectInfoAdapter _Instance = new PersonBaseFeeDepartmentProjectInfoAdapter();
        public static PersonBaseFeeDepartmentProjectInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }
        #region 人员工资项目部门项目分摊

        public Int32 InsertPersonBaseFeeDepartmentProjectInfo(PersonBaseFeeDepartmentProjectInfo PersonBaseFeeDepartmentProjectInfo)
        {
            String sql = ORMapping.GetInsertSql(PersonBaseFeeDepartmentProjectInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 UpdatePersonBaseFeeDepartmentProjectInfo(PersonBaseFeeDepartmentProjectInfo PersonBaseFeeDepartmentProjectInfo)
        {
            String sql = ORMapping.GetUpdateSql(PersonBaseFeeDepartmentProjectInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }
        public Int32 DeletePersonBaseFeeDepartmentProjectInfo(String yearMonth, WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("DELETE FROM {0} WHERE {1} "
                , String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeDepartmentProjectInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeDepartmentProjectMonthInfo>().TableName
                , builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }
        public PersonBaseFeeDepartmentProjectInfo LoadPersonBaseFeeDepartmentProjectInfo(String yearMonth, String feeID, String personID, String departmentID, String projectID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.FeeID, feeID);
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.PersonID, personID);
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.DepartmentID, departmentID);
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.ProjectID, projectID);
            return GetPersonBaseFeeDepartmentProjectInfoList(yearMonth, builder).FirstOrDefault();
        }

        public List<PersonBaseFeeDepartmentProjectInfo> GetPersonBaseFeeDepartmentProjectInfoList(String yearMonth, WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} "
                , String.IsNullOrEmpty(yearMonth) ? PersonBaseFeeDepartmentProjectInfoDBConst.TableName : ORMapping.GetMappingInfo<PersonBaseFeeDepartmentProjectMonthInfo>().TableName
                , builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<PersonBaseFeeDepartmentProjectInfo> result = new List<PersonBaseFeeDepartmentProjectInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                PersonBaseFeeDepartmentProjectInfo info = String.IsNullOrEmpty(yearMonth) ? new PersonBaseFeeDepartmentProjectInfo() : new PersonBaseFeeDepartmentProjectMonthInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }
        #endregion 人员工资项目部门项目分摊
    }
}
