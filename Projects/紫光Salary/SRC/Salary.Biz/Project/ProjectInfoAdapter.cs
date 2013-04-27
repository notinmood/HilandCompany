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
    public class ProjectInfoAdapter
    {
        public ProjectInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static ProjectInfoAdapter _Instance = new ProjectInfoAdapter();
        public static ProjectInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertProjectInfo(ProjectInfo projectInfo)
        {
            String sql = ORMapping.GetInsertSql(projectInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 UpdateProjectInfo(ProjectInfo projectInfo)
        {
            String sql = ORMapping.GetUpdateSql(projectInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteProjectInfo(String projectID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectInfoDBConst.ProjectID, projectID);
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", ProjectInfoDBConst.TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public ProjectInfo LoadProjectInfo(String projectId)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectInfoDBConst.ProjectID, projectId);
            return GetProjectInfoList(builder).FirstOrDefault();
        }

        public Int32 ChangeStatus(String projectID, Int32 userFlag)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectInfoDBConst.ProjectID, projectID);
            String sql = String.Format("UPDATE {0} SET USE_FLAG={1} WHERE {2} ", ProjectInfoDBConst.TableName, userFlag, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public List<ProjectInfo> GetProjectInfoList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY PROJECT_CODE", ProjectInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<ProjectInfo> result = new List<ProjectInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                ProjectInfo info = new ProjectInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public Boolean IsProjectCodeUsed(ProjectInfo projectInfo, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectInfoDBConst.ProjectCode, projectInfo.ProjectCode);
            ProjectInfo info = GetProjectInfoList(builder).FirstOrDefault();
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
                    return info.ProjectId.Equals(projectInfo.ProjectId) ? false : true;
                }
            }
        }
        public Boolean IsProjectNameUsed(ProjectInfo projectInfo, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectInfoDBConst.ProjectName, projectInfo.ProjectName);
            ProjectInfo info = GetProjectInfoList(builder).FirstOrDefault();
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
                    return info.ProjectId.Equals(projectInfo.ProjectId) ? false : true;
                }
            }
        }

        public String CreateProjectCode(String projectClassID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectInfoDBConst.ProjectClassID, projectClassID);
            String sql = String.Format("SELECT ISNULL(MAX(PROJECT_CODE) + 1,'{0}0001') FROM {1} WHERE {2}", projectClassID, ProjectInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt.Rows[0][0].ToString();
        }
    }
}
