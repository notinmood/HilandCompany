using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using ChinaCustoms.Framework.DeluxeWorks.Library.Core;
using System.Configuration;
using Salary.Core.Utility;

namespace Salary.Core.Data
{
    public static class BuilderEx
    {
        public static void AppendItem(this WhereSqlClauseBuilder builder, String expression)
        {
            builder.AppendItem(String.Concat(" ", expression, " "), String.Empty, String.Empty, true);
        }

        public static void IfEmptyFillDefault(this WhereSqlClauseBuilder builder)
        {
            if (builder.IsEmpty)
            {
                builder.AppendItem("1 = 1");
            }
        }

        public static void AppendLikeItem(this WhereSqlClauseBuilder builder, String dataField, String data)
        {
            builder.AppendItem(dataField, data, "LIKE");
        }

        public static String ToSqlString(this WhereSqlClauseBuilder builder)
        {
            if (builder==null) 
            {
                builder = new WhereSqlClauseBuilder();
            }
            if (builder.IsEmpty)
            {
                builder.IfEmptyFillDefault();
            }
            return builder.ToSqlString(TSqlBuilderInstance);
        }

        private static ISqlBuilder _TSqlBuilderInstance = null;
        public static ISqlBuilder TSqlBuilderInstance
        {
            get
            {
                if (_TSqlBuilderInstance == null)
                {
                    String tsqlBuilderType = ConfigurationSettings.AppSettings[SysConst.SqlBuilderType].ToString();
                    _TSqlBuilderInstance = TypeCreator.CreateInstance(tsqlBuilderType) as ISqlBuilder;
                }
                return _TSqlBuilderInstance;
            }
        }
    }
}
