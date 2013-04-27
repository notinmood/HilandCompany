using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using Salary.Biz;

namespace Salary.Web.Utility
{
    public class GridViewControl
    {
        public GridViewControl()
        { 

        }

        public static void ResetGridView(GridView gridView)
        {
            if (gridView.Rows.Count == 1 && gridView.Rows[0].Cells[0].Text == SalaryConst.EmptyText)
            {
                int columnCount = gridView.Columns.Count;
                gridView.Rows[0].Cells.Clear();
                gridView.Rows[0].Cells.Add(new TableCell());
                gridView.Rows[0].Cells[0].ColumnSpan = columnCount;
                gridView.Rows[0].Cells[0].Text = SalaryConst.EmptyText;
                gridView.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
        }

        public static void GridViewDataBind(GridView gridView, DataTable table)
        {
            if (table.Rows.Count == 0)
            {
                table = table.Clone();
                table.Rows.Add(table.NewRow());
                gridView.DataSource = table;
                gridView.DataBind();
                int columnCount = gridView.HeaderRow.Cells.Count;//gridView.Columns.Count;
                gridView.Rows[0].Cells.Clear();
                gridView.Rows[0].Cells.Add(new TableCell());
                gridView.Rows[0].Cells[0].ColumnSpan = columnCount;
                gridView.Rows[0].Cells[0].Text = SalaryConst.EmptyText;
                gridView.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
            else
            {
                gridView.DataSource = table;
                gridView.DataBind();
            }

            gridView.SelectedIndex = -1;
        }
        public static void GridViewDataBind<T>(GridView gridView, List<T> list)
        {
            if (list.Count == 0)
            {
                T t = default(T);
                System.Reflection.PropertyInfo[] propertypes = null;
                t = Activator.CreateInstance<T>();
                //propertypes = t.GetType().GetProperties();
                //foreach (System.Reflection.PropertyInfo pro in propertypes)
                //{
                //    pro.SetValue(t, null, null);
                //}
                list.Add(t);
                gridView.DataSource = list;
                gridView.DataBind();
                int columnCount = gridView.Columns.Count;
                gridView.Rows[0].Cells.Clear();
                gridView.Rows[0].Cells.Add(new TableCell());
                gridView.Rows[0].Cells[0].ColumnSpan = columnCount;
                gridView.Rows[0].Cells[0].Text = SalaryConst.EmptyText;
                gridView.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
            else
            {
                gridView.DataSource = list;
                gridView.DataBind();
            }

            gridView.SelectedIndex = -1;
        }
    }
}
