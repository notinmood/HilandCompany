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
    public class ProjectClassInfoAdapter
    {
        public ProjectClassInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static ProjectClassInfoAdapter _Instance = new ProjectClassInfoAdapter();
        public static ProjectClassInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertProjectClassInfo(ProjectClassInfo projectClassInfo)
        {
            String sql = ORMapping.GetInsertSql(projectClassInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 UpdateProjectClassInfo(ProjectClassInfo projectClassInfo)
        {
            String sql = ORMapping.GetUpdateSql(projectClassInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteProjectClassInfo(String projectClassId)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectClassInfoDBConst.ProjectClassID, projectClassId);
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", ProjectClassInfoDBConst.TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public ProjectClassInfo LoadProjectClassInfo(String projectClassId)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectClassInfoDBConst.ProjectClassID, projectClassId);
            return GetProjectClassInfoList(builder).FirstOrDefault();
        }

        public Int32 ChangeStatus(String projectClassId, Int32 userFlag)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectClassInfoDBConst.ProjectClassID, projectClassId);
            String sql = String.Format("UPDATE {0} SET USE_FLAG={1} WHERE {2} ", ProjectClassInfoDBConst.TableName, userFlag, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public List<ProjectClassInfo> GetProjectClassInfoList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY PROJECT_CLASS_ID", ProjectClassInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<ProjectClassInfo> result = new List<ProjectClassInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                ProjectClassInfo info = new ProjectClassInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public Boolean IsProjectClassIDUsed(ProjectClassInfo projectClassInfo, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectClassInfoDBConst.ProjectClassID, projectClassInfo.ProjectClassID);
            ProjectClassInfo info = GetProjectClassInfoList(builder).FirstOrDefault();
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
                    return false;
                }
            }
        }
        public Boolean IsProjectClassNameUsed(ProjectClassInfo projectClassInfo, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectClassInfoDBConst.ProjectClassName, projectClassInfo.ProjectClassName);
            ProjectClassInfo info = GetProjectClassInfoList(builder).FirstOrDefault();
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
                    return info.ProjectClassID.Equals(projectClassInfo.ProjectClassID) ? false : true;
                }
            }
        }

        public String CreateProjectClassId(String parentClassId)
        {
            String sql;
            if (String.IsNullOrEmpty(parentClassId))
            {
                sql = String.Format("SELECT ISNULL(MAX(PROJECT_CLASS_ID) + 1,1) FROM {0} WHERE ISNULL(PARENT_CLASS_ID,'')=''", ProjectClassInfoDBConst.TableName);
            }
            else
            {
                WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
                builder.AppendItem(ProjectClassInfoDBConst.ParentClassID, parentClassId);
                sql = String.Format("SELECT ISNULL(MAX(PROJECT_CLASS_ID) + 1,'{0}01') FROM {1} WHERE {2}", parentClassId, ProjectClassInfoDBConst.TableName, builder.ToSqlString());
            }
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt.Rows[0][0].ToString();
        }
    }
}
