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

namespace Salary.Biz
{
    public class PersonInfoAdapter
    {
        public PersonInfoAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static PersonInfoAdapter _Instance = new PersonInfoAdapter();
        public static PersonInfoAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 InsertPersonInfo(PersonInfo personInfo)
        {
            String sql = ORMapping.GetInsertSql(personInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 UpdatePersonInfo(PersonInfo personInfo)
        {
            String sql = ORMapping.GetUpdateSql(personInfo, BuilderEx.TSqlBuilderInstance);
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public Int32 DeletePersonInfo(String personID)
        {
            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
                builder.AppendItem(PersonInfoDBConst.PersonID, personID);
                String sql = String.Format("DELETE FROM {0} WHERE {1} ", PersonInfoDBConst.TableName, builder.ToSqlString());
                int result = _DataHelper.ExecuteSql(sql);
                PersonBaseFeeDepartmentProjectInfoAdapter.Instance.DeletePersonBaseFeeDepartmentProjectInfo(null, builder);
                PersonBaseFeeInfoAdapter.Instance.DeletePersonBaseFeeInfo(null, personID);
                scope.Complete();
                return result;
            }
        }

        public PersonInfo LoadPersonInfo(String personID)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonInfoDBConst.PersonID, personID);
            return GetPersonInfoList(builder).FirstOrDefault();
        }

        public List<PersonInfo> GetPersonInfoList(WhereSqlClauseBuilder builder)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY PERSON_CODE, ENTRY_DATE", PersonInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            List<PersonInfo> result = new List<PersonInfo>();
            for (Int32 i = 0; i < dt.Rows.Count; i++)
            {
                PersonInfo info = new PersonInfo();
                ORMapping.DataRowToObject(dt.Rows[i], info);
                result.Add(info);
            }
            return result;
        }

        public Int32 ChangeDimission(String personID, bool dimission)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonInfoDBConst.PersonID, personID);
            String sql = String.Format("UPDATE {0} SET DIMISSION={1} WHERE {2} ", PersonInfoDBConst.TableName, dimission ? 1 : 0, builder.ToSqlString());
            int result = _DataHelper.ExecuteSql(sql);
            return result;
        }

        public bool IsPersonCodeUsed(PersonInfo personInfo, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonInfoDBConst.PersonCode, personInfo.PersonCode);
            builder.AppendItem(PersonInfoDBConst.ParentID, personInfo.ParentID);
            builder.AppendItem(PersonInfoDBConst.EntryDate, personInfo.EntryDate);
            PersonInfo info = GetPersonInfoList(builder).FirstOrDefault();
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
                    return info.PersonID.Equals(personInfo.PersonID) ? false : true;
                }
            }
        }

        public bool IsPersonNameUsed(PersonInfo personInfo, Boolean isAdd)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonInfoDBConst.PersonName, personInfo.PersonName);
            builder.AppendItem(PersonInfoDBConst.ParentID, personInfo.ParentID);
            builder.AppendItem(PersonInfoDBConst.EntryDate, personInfo.EntryDate);
            PersonInfo info = GetPersonInfoList(builder).FirstOrDefault();
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
                    return info.PersonID.Equals(personInfo.PersonID) ? false : true;
                }
            }
        }

        public String CreatePersonCode()
        {
            String sql = String.Format("SELECT ISNULL(MAX(PERSON_CODE) + 1,000001) FROM {0}", PersonInfoDBConst.TableName);
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt.Rows[0][0].ToString();
        }
        public String CreatePersonCode(String departID)
        {
            DepartmentInfo departInfo = DepartmentInfoAdapter.Instance.LoadDepartmentInfo(departID);
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonInfoDBConst.DepartmentID, departID);
            String sql = String.Format("SELECT ISNULL(MAX(PERSON_CODE) + 1,'{0}01') FROM {1} WHERE {2}", departInfo.DepartmentCode, PersonInfoDBConst.TableName, builder.ToSqlString());
            DataTable dt = _DataHelper.GetDataTable(sql);
            return dt.Rows[0][0].ToString();
        }
    }
}
