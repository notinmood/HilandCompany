using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salary.Core.Data;
using System.Data;
using System.Web;

using System.Globalization;
using System.Data.Common;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;


namespace Salary.Biz
{
    public class CommonTools
    {
        public CommonTools()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static CommonTools _Instance = new CommonTools();
        public static CommonTools Instance
        {
            get
            {
                return _Instance;
            }
        }

        public Int32 GetMaxOrderNo(String tableName, String orderNo)
        {
            String sql = String.Format("SELECT MAX({0}) + 10 FROM {1}", orderNo, tableName);
            DataTable dt = _DataHelper.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                Int32 count = 0;
                return Int32.TryParse(dt.Rows[0][0].ToString(), out count) ? count : 10;
            }
            else
            {
                return 10;
            }
        }


        public static DataSet ConvertDsGuidToName(DataSet dataset)
        {
            DataSet ds = dataset;
            DataTable dt;
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                dt = ds.Tables[i];
                //MyHeBingSameColumn(dt);
                int sumTitleIndex = -1;
                int guidNum = 0;
                int hebingNum = 0;
                int sumNum = 0;
                int countNum = 0;
                int avgNum = 0;
                int chnDateNum = 0;
                int j = 0;
                int index1 = 0;
                int index2 = 0;
                int index3 = 0;
                int index4 = 0;
                int index5 = 0;
                int index6 = 0;


                for (j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.Length > 4 && dt.Columns[j].ColumnName.Substring(0, 5).ToUpper() == "GUID_")
                        guidNum += 1;
                    if (dt.Columns[j].ColumnName.Length > 6 && dt.Columns[j].ColumnName.Substring(0, 7).ToUpper() == "HEBING_")
                        hebingNum += 1;
                    if (dt.Columns[j].ColumnName.Length > 3 && dt.Columns[j].ColumnName.Substring(0, 4).ToUpper() == "SUM_")
                        sumNum += 1;
                    if (dt.Columns[j].ColumnName.Length > 3 && dt.Columns[j].ColumnName.Substring(0, 4).ToUpper() == "AVG_")
                        avgNum += 1;
                    if (dt.Columns[j].ColumnName.Length > 5 && dt.Columns[j].ColumnName.ToUpper().IndexOf("COUNT_") >= 0)
                        countNum += 1;
                    //if (dt.Columns[j].ColumnName.Length > 9 && dt.Columns[j].ColumnName.Substring(0, 9).ToUpper() == "SUMTITLE_")
                    if (dt.Columns[j].ColumnName.Length > 9 && dt.Columns[j].ColumnName.ToUpper().IndexOf("SUMTITLE_") >= 0)
                        sumTitleIndex = j;
                    if (dt.Columns[j].ColumnName.Length > 6 && dt.Columns[j].ColumnName.Substring(0, 7).ToUpper() == "CHNDATE")
                        chnDateNum += 1;

                }
                int[] guid_column = new int[guidNum];
                int[] hebing_column = new int[hebingNum];
                string[] previous_col = new string[hebingNum];
                int[] sum_column = new int[sumNum];
                int[] avg_column = new int[avgNum];
                int[] count_column = new int[countNum];
                int[] count_total = new int[countNum];
                int[] chnDate_column = new int[chnDateNum];
                double[] sum_total = new double[sumNum];
                double[] avg_total = new double[sumNum];

                for (j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.Length > 4 && dt.Columns[j].ColumnName.Substring(0, 5).ToUpper() == "GUID_")
                    {
                        guid_column[index1] = j;
                        index1 += 1;
                    }
                    if (dt.Columns[j].ColumnName.Length > 6 && dt.Columns[j].ColumnName.Substring(0, 7).ToUpper() == "HEBING_")
                    {
                        hebing_column[index5] = j;
                        index5 += 1;
                    }
                    if (dt.Columns[j].ColumnName.Length > 3 && dt.Columns[j].ColumnName.Substring(0, 4).ToUpper() == "SUM_")
                    {
                        sum_column[index2] = j;
                        sum_total[index2] = 0;
                        index2 += 1;
                    }
                    if (dt.Columns[j].ColumnName.Length > 3 && dt.Columns[j].ColumnName.Substring(0, 4).ToUpper() == "AVG_")
                    {
                        avg_column[index4] = j;
                        avg_total[index4] = 0;
                        index4 += 1;
                    }
                    if (dt.Columns[j].ColumnName.Length > 5 && dt.Columns[j].ColumnName.ToUpper().IndexOf("COUNT_") >= 0)
                    {
                        count_column[index3] = j;
                        count_total[index3] = 0;
                        index3 += 1;
                    }
                    if (dt.Columns[j].ColumnName.Length > 6 && dt.Columns[j].ColumnName.ToUpper().IndexOf("CHNDATE") >= 0)
                    {
                        chnDate_column[index6] = j;
                        index6 += 1;
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    for (j = 0; j < guid_column.Length; j++)
                    {
                        if (row[guid_column[j]].ToString().IndexOf(',') > 0)
                        {
                            string[] array1 = row[guid_column[j]].ToString().Split(',');
                            string string2 = "";
                            for (int k = 0; k < array1.Length; k++)
                            {
                                array1[k] = "";//GetDisplayNameByGUID(array1[k]);
                                if (k < array1.Length - 1)
                                    string2 = string2 + array1[k] + ",";
                                else
                                    string2 = string2 + array1[k];
                            }
                            row[guid_column[j]] = string2;
                        }
                        else
                            row[guid_column[j]] = "";//GetDisplayNameByGUID(row[guid_column[j]].ToString());
                    }

                    for (j = 0; j < sum_column.Length; j++)
                    {
                        sum_total[j] += double.Parse(row[sum_column[j]].ToString());
                    }

                    for (j = 0; j < hebing_column.Length; j++)
                    {
                        if (previous_col[j] != row[hebing_column[j]].ToString())
                            previous_col[j] = row[hebing_column[j]].ToString();
                        else
                            row[hebing_column[j]] = "";
                    }

                    for (j = 0; j < chnDate_column.Length; j++)
                    {
                        row[chnDate_column[j]] = NumToCHNStr(row[chnDate_column[j]].ToString());
                    }

                }

                if (sumTitleIndex >= 0)
                {
                    DataRow dr = dt.NewRow();
                    //for (j = 0; j < dt.Columns.Count; j++)
                    //    dr[j] = "";
                    dr[sumTitleIndex] = "合计";
                    for (j = 0; j < sum_column.Length; j++)
                    {
                        dr[sum_column[j]] = sum_total[j].ToString();
                    }
                    for (j = 0; j < count_column.Length; j++)
                    {
                        count_total[j] = dt.Rows.Count;
                        dr[count_column[j]] = count_total[j];
                    }
                    for (j = 0; j < avg_column.Length; j++)
                    {
                        avg_total[j] = double.Parse(dr[avg_column[j] - 1].ToString()) / double.Parse(dr[avg_column[j] - 2].ToString());
                        dr[avg_column[j]] = avg_total[j];
                    }
                    dt.Rows.Add(dr);
                }
            }

            return ds;
        }


        /// <summary>
        /// 将数字转换为制定的中文数字
        /// </summary>
        /// <param name="intLong"></param>
        /// <returns></returns>
        public static string NumToCHNStr(string intLong)
        {
            string strResult = "";
            int num = 0;
            string[] arrayCHN = { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            char[] strArray = intLong.ToCharArray();
            for (int i = 0; i < strArray.Length; i++)
            {
                num = int.Parse(strArray[i].ToString());
                strResult += arrayCHN[num].ToString();
            }
            if (strResult.Length > 4)
                strResult = strResult.Substring(0, 4).ToString() + "年" + strResult.Substring(4, strResult.Length - 4) + "月";
            return strResult;
        }

        /// <summary>
        /// 匹配数字串
        /// </summary>
        /// <param name="strExp"></param>
        /// <returns></returns>
        public String[] MatchNumber(String strExp)
        {
            String parameterClausePattern = @"\d*$";
            MatchCollection parameterClauseMatchs = Regex.Matches(strExp, parameterClausePattern, RegexOptions.Singleline);
            Dictionary<String, String> para = parameterClauseMatchs.OfType<Match>().Select(i => i.Value).Distinct().ToDictionary(i => i, i => string.Empty);
            if (parameterClauseMatchs.Count > 0)
            {
                String[] rtn = new String[parameterClauseMatchs.Count];
                Int32 i = 0;
                foreach (String dt in para.Keys)
                {
                    rtn[i] = dt;
                    i++;
                }
                return rtn;
            }
            return new String[] { strExp };
        }
    }
}
