using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Salary.Biz;
using Salary.Core.Data;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    public class DepartmentInfoAdapter
    {
        public DepartmentInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static DepartmentInfoAdapter _Instance = new DepartmentInfoAdapter();
        public static DepartmentInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertDepartmentInfo(DepartmentInfo departmentInfo)
        {
            String sql = ORMapping.GetInsertSql(departmentInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 UpdateDepartmentInfo(DepartmentInfo departmentInfo)
        {
            String sql = ORMapping.GetUpdateSql(departmentInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteDepartmentInfo(String deptID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(DepartmentInfoDBConst.DepartmentID, deptID);
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", DepartmentInfoDBConst.TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public DepartmentInfo LoadDepartmentInfo(String deptID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(DepartmentInfoDBConst.DepartmentID, deptID);
            return GetDepartmentInfoList(builder).FirstOrDefault();
        }

        public Int32 ChangeStatus(String deptId, Int32 userFlag)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(DepartmentInfoDBConst.DepartmentID, deptId);
            String sql = String.Format("UPDATE {0} SET USE_FLAG={1} WHERE {2} ", DepartmentInfoDBConst.TableName, userFlag, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public List<DepartmentInfo> GetDepartmentInfoList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY DEPARTMENT_CODE", DepartmentInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<DepartmentInfo> result = new List<DepartmentInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                DepartmentInfo info = new DepartmentInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public bool IsDeparmentCodeUsed(DepartmentInfo departmentInfo, bool isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(DepartmentInfoDBConst.DepartmentCode, departmentInfo.DepartmentCode);
            DepartmentInfo info = GetDepartmentInfoList(builder).FirstOrDefault();
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
                    return info.DepartmentID.Equals(departmentInfo.DepartmentID) ? false : true;
                }
            }
        }
        public bool IsDeparmentNameUsed(DepartmentInfo departmentInfo, bool isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(DepartmentInfoDBConst.DepartmentName, departmentInfo.DepartmentName);
            DepartmentInfo info = GetDepartmentInfoList(builder).FirstOrDefault();
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
                    return info.DepartmentID.Equals(departmentInfo.DepartmentID) ? false : true;
                }
            }
        }

        public String CreateDepartmentCode(String parentDeptID)
        {
            String sql;
            if(String.IsNullOrEmpty(parentDeptID))
            {
                sql = String.Format("SELECT ISNULL(MAX(DEPARTMENT_CODE) + 1,1) FROM {0} WHERE ISNULL(PARENT_ID,'')=''", DepartmentInfoDBConst.TableName);
            }
            else
            {
                DepartmentInfo departInfo = this.LoadDepartmentInfo(parentDeptID);
                WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
                builder.AppendItem(DepartmentInfoDBConst.ParentID, parentDeptID);
                sql = String.Format("SELECT ISNULL(MAX(DEPARTMENT_CODE) + 1,'{0}01') FROM {1} WHERE {2}", departInfo.DepartmentCode, DepartmentInfoDBConst.TableName, builder.ToSqlString());
            }
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt.Rows[0][0].ToString();
        }
    }
}
