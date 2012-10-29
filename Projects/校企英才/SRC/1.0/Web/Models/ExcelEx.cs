using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using HiLand.Utility.Office;
using HiLand.Utility.Reflection;

namespace XQYC.Web.Models
{
    public class ExcelEx
    {
        /// <summary>
        /// 获取实体列表导出为Excel的流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="fieldsMap">实体的属性名称与Excel列名的映射字典</param>
        /// <returns></returns>
        public static Stream GetModelListExcelStream<T>(IList<T> entityList,Dictionary<string,string> fieldsMap)
        {
            DataTable dataTable = new DataTable();

            Type entityType= typeof(T);
            PropertyInfo[] propertyArray= entityType.GetProperties();


            //1.创建表头
            foreach (KeyValuePair<string, string> kvp in fieldsMap)
            {
                PropertyInfo  pi=propertyArray.First(s => s.Name == kvp.Key);
                if (pi != null)
                {
                    DataColumn dc = new DataColumn(kvp.Key, pi.PropertyType);
                    dc.Caption = kvp.Value;
                    dataTable.Columns.Add(dc);
                }
            }

            //2.添加表数据
            foreach (T item in entityList)
            {
                DataRow row = dataTable.NewRow();
                foreach (KeyValuePair<string, string> kvp in fieldsMap)
                {
                    row[kvp.Key] = ReflectHelper.GetPropertyValue( item,kvp.Key);
                }
                dataTable.Rows.Add(row);
            }

            Stream outStream = ExcelHelper.WriteExcel(dataTable);
            return outStream;
        }
    }
}