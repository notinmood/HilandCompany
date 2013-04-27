using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Salary.Core.Data;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using Salary.Core.Utility;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    public class UserInfoAdapter
    {
        public UserInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        public UserInfoAdapter(String dbName)
            : this()
        {
            _DBName = dbName;
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static UserInfoAdapter _Instance = new UserInfoAdapter();
        public static UserInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertUserInfo(UserInfo userInfo)
        {
            String sql = ORMapping.GetInsertSql(userInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 UpdateUserInfo(UserInfo userInfo)
        {
            String sql = ORMapping.GetUpdateSql(userInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeleteUserInfo(String userId)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(UserInfoDBConst.UserId, userId);
            String sql = String.Format("DELETE FROM {0} WHERE {1} ", UserInfoDBConst.TableName, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 ChangePassword(String userId, String password)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(UserInfoDBConst.UserId, userId);
            String sql = String.Format("UPDATE {0} SET PASSWORD='{1}' WHERE {2} ", UserInfoDBConst.TableName, CryptoHelper.Encode(password), builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }



        public Int32 ChangeStatus(String userId, Int32 status)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(UserInfoDBConst.UserId, userId);
            String sql = String.Format("UPDATE {0} SET LOGOUT={1} WHERE {2} ", UserInfoDBConst.TableName, status, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public UserInfo LoadUserInfo(String userId)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(UserInfoDBConst.UserId, userId);
            return GetUserInfoList(builder).FirstOrDefault();
        }

        public UserInfo LoadUserInfoByLogon(String logonName, String password)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(UserInfoDBConst.LogonName, logonName);
            builder.AppendItem(UserInfoDBConst.Password, CryptoHelper.Encode(password));
            builder.AppendItem(UserInfoDBConst.Logout, Status.True.ToString("D"));
            return GetUserInfoList(builder).FirstOrDefault();
        }

        public List<UserInfo> GetUserInfoList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ", UserInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<UserInfo> result = new List<UserInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                UserInfo info = new UserInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public bool IsLogonNameUsed(UserInfo userInfo, bool isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(UserInfoDBConst.LogonName, userInfo.LogonName);
            UserInfo info = GetUserInfoList(builder).FirstOrDefault();
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
                    return info.UserId.Equals(userInfo.UserId) ? false : true;
                }
            }
        }



        public Int32 InsertUserLogInfo(UserLogInfo userLogInfo)
        {
            String sql = ORMapping.GetInsertSql(userLogInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }
    }
}
