using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Web.BasePage;
using Salary.Biz;
using Salary.Core.Utility;
using Salary.Biz.Eunm;
using System.Data;
using NPOI.HSSF.UserModel;
using System.Text;
using Salary.Web.Utility;

public partial class UI_Salary_SalaryEdit : BasePage
{
    public string yearMonth;
    //string feeCodeGangWei;

    protected void Page_Load(object sender, EventArgs e)
    {
        yearMonth = DecodedQueryString[PayMonthInfoConst.YearMonth];
        if (!IsPostBack)
        {//this.litTitle.Text = String.Concat(yearMonth.Substring(0, 4), "年", yearMonth.Substring(4, 2), "月薪资表");
            this.InitializeControl();
        }
        else
        {
            GridViewControl.ResetGridView(this.gvList);
        }
    }

    private void InitializeControl()
    {
        //feeCodeGangWei = FeeInfoAdapter.Instance.LoadFeeInfoByName("岗位工资").FeeCode;
        List<ReportInfo> reportList = ReportInfoAdapter.Instance.GetReportInfoList(null);
        this.ddlReport.DataTextField = ReportInfoConst.ReportName;
        this.ddlReport.DataValueField = ReportInfoConst.ReportCode;
        this.ddlReport.DataSource = reportList;
        this.ddlReport.DataBind();//this.ddlReport.Items.Insert(0, new ListItem("----请选择----", ""));
        this.GridViewDataBind();
    }

    private void GridViewDataBind()
    {
        DataTable dt = PayMonthInfoAdapter.Instance.LoadPayMonthInfo(yearMonth, this.ddlReport.SelectedValue);
        this.gvList.ShowFooter = dt.Rows.Count > 0;
        GridViewControl.GridViewDataBind(this.gvList, dt);
        //this.gvList.DataSource = PayMonthInfoAdapter.Instance.LoadPayMonth(yearMonth, this.ddlReport.SelectedValue);
        //this.gvList.DataBind();               
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value.ToString() == "")))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                for (Int32 j = 2; j < this.gvList.HeaderRow.Cells.Count - 1; j++)
                {
                    this.gvList.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Right;//金额列居右
                }
            }
        }
        Int32 gvWidth = 0;//计算Grid的宽度
        for (Int32 i = 0; i < this.gvList.HeaderRow.Cells.Count; i++)
        {
            gvWidth += this.gvList.HeaderRow.Cells[i].Text.Length;
        }
        this.gvList.Width = gvWidth * 15;
        this.gvList.Columns[this.gvList.Columns.Count - 1].Visible = !this.ddlReport.SelectedItem.Text.Contains("部门");//部门合计报表 隐藏操作列
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(PayMonthInfoDBConst.YearMonth, this.yearMonth.Replace("年", "").Replace("月", ""));
        builder.AppendItem(PayMonthInfoDBConst.PersonID, (sender as ImageButton).CommandArgument);
        PayMonthInfoAdapter.Instance.DeletePayMonthInfo(builder);
        //base.MessageBox("删除成功！");
        this.GridViewDataBind();
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        ImageButton ibtnEdit;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.PersonEditUrl);
        Double[] dobSum = new Double[this.gvList.FooterRow.Cells.Count];
        //Array.Clear(dobSum, 0, dobSum.Length);
        //for (Int32 i = 0; i < dobSum.Length; i++)
        //{
        //    dobSum[i] = 0;
        //}        
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value.ToString() == "")))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                for (Int32 j = 0; j < this.gvList.HeaderRow.Cells.Count; j++)
                {
                    if (this.gvList.HeaderRow.Cells[j].Text != "姓名" && this.gvList.HeaderRow.Cells[j].Text != "部门" && this.gvList.HeaderRow.Cells[j].Text != "操作" && this.gvList.HeaderRow.Cells[j].Visible)
                    {
                        String text = Server.HtmlDecode(this.gvList.Rows[i].Cells[j].Text.Trim());
                        dobSum[j] += text.Trim().Length > 0 ? Double.Parse(text) : 0;
                    }
                }
                ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                if (!String.IsNullOrEmpty(yearMonth))
                {
                    urlEdit.AppendItem(PayMonthInfoConst.YearMonth, yearMonth);
                }
                urlEdit.AppendItem(PayMonthInfoConst.PersonID, this.gvList.DataKeys[i].Values[PersonInfoConst.PersonID]);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindOpen(ibtnEdit, 460, 800, urlEdit.ToUrlString());
                urlEdit.Clear();
            }
            for (Int32 i = 0; i < this.gvList.FooterRow.Cells.Count; i++)
            {
                if (this.gvList.HeaderRow.Cells[i].Text == "姓名" || this.gvList.HeaderRow.Cells[i].Text == "部门")
                {
                    this.gvList.FooterRow.Cells[i].Text = "合计：";
                }
                if (this.gvList.HeaderRow.Cells[i].Text != "姓名" && this.gvList.HeaderRow.Cells[i].Text != "部门" && this.gvList.HeaderRow.Cells[i].Text != "操作" && this.gvList.HeaderRow.Cells[i].Visible)
                {
                    this.gvList.FooterRow.Cells[i].Text = dobSum[i].ToString("#,##0.00");
                }
            }
        }
    }
    protected void gvList_RowCreated(object sender, GridViewRowEventArgs e)
    {   //将操作模板列移至最后
        TableCell cell = e.Row.Cells[0];
        e.Row.Cells.RemoveAt(0);
        e.Row.Cells.Add(cell);
        //隐藏自动生成的用户ID列
        cell = e.Row.Cells[0];  
        cell.Visible = false;
        #region ///
        //排列款项列-表头列
        //Int32 targetCol = 2;
        //foreach (ReportFee reportFee in ReportAdapter.Instance.GetReportFeeListByName("薪资表"))
        //{
        //    Int32 currentCol = -1;
        //    for (Int32 i = 0; i < e.Row.Cells.Count; i++)
        //    {
        //        if (e.Row.RowType == DataControlRowType.Header)
        //        {
        //            if (e.Row.Cells[i].Text == reportFee.FeeName)
        //            {
        //                currentCol = i;
        //                break;
        //            }
        //        }
        //        //else if (e.Row.RowType == DataControlRowType.DataRow)
        //        //{
        //        //    if (this.gvList.HeaderRow.Cells[i].ToolTip == reportFee.FeeName)
        //        //    {
        //        //        currentCol = i;
        //        //        break;
        //        //    }
        //        //}
        //    }
        //    if (currentCol > 0)
        //    {
        //        TableCell currentCell = e.Row.Cells[currentCol];
        //        //String targetText = e.Row.Cells[targetCol].Text;
        //        e.Row.Cells.RemoveAt(currentCol);
        //        e.Row.Cells.AddAt(targetCol, currentCell);
        //        //if (e.Row.RowType == DataControlRowType.Header)
        //        //{
        //        //    e.Row.Cells[currentCol].ToolTip = reportFee.FeeName;
        //        //    e.Row.Cells[targetCol].ToolTip = targetText;
        //        //}
        //        targetCol++;
        //    }
        //}
        #endregion
    }
    protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow && !this.ddlReport.SelectedItem.Text.Contains("部门"))
        {
            Int32 gwgz = -1;
            for (Int32 i = 0; i < this.gvList.HeaderRow.Cells.Count; i++)
            {
                if (this.gvList.HeaderRow.Cells[i].Text == "岗位工资")
                {
                    gwgz = i;//break;
                }
                if (this.gvList.HeaderRow.Cells[i].Text != "姓名" && this.gvList.HeaderRow.Cells[i].Text != "操作" && e.Row.Cells[i].Visible )
                {
                    String text = Server.HtmlDecode(e.Row.Cells[i].Text.Trim());
                    e.Row.Cells[i].Text = text.Trim().Length > 0 ? Double.Parse(text).ToString("#,##0.00") : "";
                }
            }
            #region 不用了
            /*
            //为岗位工资加链接
            if (gwgz >= 0)
            {
                LinkButton btnGwgz = new LinkButton();
                btnGwgz.Text = e.Row.Cells[gwgz].Text;
                UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.StaionSalaryEditUrl);
                urlEdit.AppendItem(PayMonthConst.YearMonth, yearMonth);
                urlEdit.AppendItem(PayMonthConst.FeeCode, feeCodeGangWei);
                urlEdit.AppendItem(PayMonthConst.PersonId, this.gvList.DataKeys[e.Row.RowIndex].Values[PersonInfoConst.PersonId]);
                urlEdit.AppendItem(PayMonthConst.PayMoney, e.Row.Cells[gwgz].Text);
                base.ControlClientClickBindShow(btnGwgz, 500, 500, urlEdit.ToUrlString());
                e.Row.Cells[gwgz].Controls.Add(btnGwgz);
            }
            //for (Int32 i = 2; i < e.Row.Cells.Count-1; i++)
            //{
            //    if (this.gvList.HeaderRow.Cells[i].Text.IndexOf("(") < 0)
            //    {
            //        e.Row.Cells[i].Text = DataBinder.Eval(e.Row.DataItem, this.gvList.HeaderRow.Cells[i].Text).ToString();
            //    }
            //}
             */
            #endregion 不用了
        }
    }

    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GridViewDataBind();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        #region ///
        //Response.ClearContent();
        //Response.AddHeader("content-disposition", "attachment;filename=" + this.ddlReport.SelectedItem.Text + ".xls");
        //Response.Charset = "GB2312";
        //Response.ContentEncoding = System.Text.Encoding.UTF8;
        //Response.ContentType = "application/vnd.xls";
        //this.EnableViewState = false;
        //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        //this.gvList.AllowPaging = false;
        //this.GridViewDataBind();
        //this.gvList.RenderControl(htmlWrite);

        //Response.Write(@"<style> td{ mso-number-format:\@; } </style>");
        //Response.Write(stringWrite.ToString());
        //Response.Flush();
        //Response.End();
        //this.gvList.AllowPaging = true;
        //this.GridViewDataBind();

        //DataTable dt=PayMonthInfoAdapter.Instance.LoadPayMonth(yearMonth, this.ddlReport.SelectedValue);
        //dt.Columns.RemoveAt(0);
        //Salary.Core.Utility.DataTableRenderToExcel.RenderDataTableToExcel(dt, String.Format( @"C:\Documents and Settings\Administrator\Desktop\{0}.xls",this.ddlReport.SelectedItem.Text + yearMonth));
        //base.MessageBox("导出成功！");

        //this.GetExcel(PayMonthInfoAdapter.Instance.GetPayMonthListDT());
        //this.ExportExcel(PayMonthInfoAdapter.Instance.LoadPayMonth(yearMonth, this.ddlReport.SelectedValue));
        #endregion
        DataTable dt = PayMonthInfoAdapter.Instance.LoadPayMonthInfo(yearMonth, this.ddlReport.SelectedValue);
        //dt.Columns.RemoveAt(0);
        //this.ExportExcel(dt);
        //ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "updateScript", "ExportToFile('Salary.xls','" + yearMonth + "','" + this.ddlReport.SelectedValue + "');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "updateScript", "ExportToFile('Salary.xls','" + yearMonth + "','" + this.ddlReport.SelectedValue + "');", true);
    }

    protected void GetExcel(DataTable dt)
    {
        NPOI.HSSF.UserModel.HSSFWorkbook book = new HSSFWorkbook();
        HSSFSheet sheet = book.CreateSheet("test_01");// (HSSFSheet)book.GetSheetAt(0);
        HSSFRow row = (HSSFRow)sheet.CreateRow(0);
        for (Int32 i = 0; i < dt.Columns.Count; i++)
        {
            row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
        }
        for (Int32 i = 0; i < dt.Rows.Count; i++)
        {
            HSSFRow row2 = sheet.CreateRow(i = 1);
            for (Int32 j = 0; j < dt.Columns.Count; j++)
            {
                row2.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
            }
        }
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        book.Write(ms);
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=EmptyWorkbook.xls"));
        Response.BinaryWrite(ms.ToArray());
        book=null;
        ms.Close();
        ms.Dispose();
    }

    #region ///
    /*
    private void ExportExcel(DataTable dt)
    {
        HSSFWorkbook hssfworkbook = new HSSFWorkbook();
        HSSFSheet hssfSheet = hssfworkbook.CreateSheet("Sheet1");
        //标题行
        HSSFRow titleRow = hssfSheet.CreateRow(0);

        for (int j = 0; j < dt.Columns.Count; j++)
        {
            titleRow.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
        }

        HSSFRow contentRow = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            contentRow = hssfSheet.CreateRow(i + 1);
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string drValue = dt.Rows[i][j].ToString();

                switch (dt.Rows[i][j].GetType().ToString())
                {
                    case "System.String"://字符串类型
                        contentRow.CreateCell(j).SetCellValue(drValue);
                        break;
                    case "System.DateTime"://日期类型
                        DateTime dateV;
                        DateTime.TryParse(drValue, out dateV);
                        contentRow.CreateCell(j).SetCellValue(dateV.ToString("yyyy-MM-dd HH:mm:ss"));
                        break;
                    case "System.Boolean"://布尔型
                        bool boolV = false;
                        bool.TryParse(drValue, out boolV);
                        contentRow.CreateCell(j).SetCellValue(boolV);
                        break;
                    case "System.Int16"://整型
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                        int intV = 0;
                        int.TryParse(drValue, out intV);
                        contentRow.CreateCell(j).SetCellValue(intV);
                        break;
                    case "System.Decimal"://浮点型
                    case "System.Double":
                        double doubV = 0;
                        double.TryParse(drValue, out doubV);
                        contentRow.CreateCell(j).SetCellValue(doubV.ToString());
                        break;
                    case "System.DBNull"://空值处理
                        contentRow.CreateCell(j).SetCellValue("");
                        break;
                    default:
                        contentRow.CreateCell(i).SetCellValue("");
                        break;
                }

            }
        }

        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        hssfworkbook.Write(stream);

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + this.ddlReport.SelectedItem.Text + ".xls");
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        byte[] bytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(bytes, 0, (int)stream.Length);
        HttpContext.Current.Response.BinaryWrite(bytes);
        stream.Close();

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.Close();
    }
     */
    #endregion

    private void ExportExcel(DataTable dt)
    {
        StringBuilder sb = new StringBuilder("	try {\n");
        sb.Append("		var fs = new ActiveXObject(\"Excel.Application\");\n");
        sb.Append("		fs.Visible = false;\n");
        sb.Append("		var fswb = fs.Workbooks.Add();\n");
        int defaultRow = 60000;
        int iSheet = dt.Rows.Count / defaultRow;
        int iSheetRes = dt.Rows.Count % defaultRow;
        iSheet += (iSheetRes == 0 ? 0 : 1);
        for (int f = 0; f < iSheet; f++)
        {
            if (f < 3)
            {
                sb.AppendFormat("		var fsws = fs.WorkSheets({0});\n", f + 1);
            }
            else
            {
                sb.Append("		var fsws = fs.WorkSheets.Add();\n");
            }
            int count = 0;
            if (f + 1 == iSheet)
            {
                count = dt.Rows.Count;
            }
            else
            {
                count = defaultRow * (f + 1);
            }
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                bool isValueType = dt.Columns[j].DataType.IsValueType;
                sb.AppendFormat("		fsws.Cells(1, {0}).Value = \"{1}\";\n", j + 1, dt.Columns[j].ColumnName);
                for (int i = 0; i < count - defaultRow * f; i++)
                {
                    if (!isValueType) sb.AppendFormat("		fsws.Cells({0}, {1}).NumberFormat = \"@\";\n", i + 2, j + 1);
                    string sTemp = dt.Rows[i + defaultRow * f][j].ToString();
                    sTemp = sTemp.Replace("\n", "").Replace("\r", "").Replace("\"", "'");
                    sb.AppendFormat("		fsws.Cells({0}, {1}).Value = \"{2}\";\n", i + 2, j + 1, sTemp);
                }
            }
        }
        sb.Append("			fs.Visible = true;\n");
        #region ///
        //sb.Append("			fs.Quit();\n");

        //sb.Append("		var WshShell = new ActiveXObject(\"WScript.Shell\");\n");
        //sb.AppendFormat("		var pathName = WshShell.SpecialFolders(\"MyDocuments\") + \"\\\\{0}\";\n", "aa");
        //sb.Append("		var fso = new ActiveXObject(\"Scripting.FileSystemObject\");\n");
        //sb.Append("		if (!fso.FolderExists(pathName)) fso.CreateFolder(pathName);\n");
        //sb.AppendFormat("		var fileName = pathName + \"\\\\{0}.xls\";\n", "abc");

        //sb.Append("		var version = fs.Application.version;\n");
        //sb.AppendFormat("		if (version == \"12.0\") fileName = pathName + \"\\\\{0}.xlsx\";\n", "abc");

        //sb.Append("		if (fso.FileExists(fileName)) fso.DeleteFile(fileName);\n");

        //sb.Append("			fswb.SaveAs(fileName);\n");
        //sb.Append("			window.alert(\"文件保存成功，请查看文件：\" + fileName + \"！\");\n");
        //sb.Append("			fs.Quit();\n");


        //sb.Append("		if (window.confirm(\"请选择是否打开，确定则打开，取消则保存！\"))\n");
        //sb.Append("		{\n");
        //sb.Append("			fs.Visible = true;\n");
        //sb.Append("		}\n");
        //sb.Append("		else\n");
        //sb.Append("		{\n");
        //sb.Append("			fswb.SaveAs(fileName);\n");
        //sb.Append("			window.alert(\"文件保存成功，请查看文件：\" + fileName + \"！\");\n");
        //sb.Append("			fs.Quit();\n");
        //sb.Append("		}\n");
        #endregion
        sb.Append("	}\n");
        sb.Append("	catch(e) {\n");
        sb.Append("		window.alert(\"错误信息：\" + e.message + \"错误描述：\" + e.description);\n");
        sb.Append("	}\n");
        System.Web.UI.Page page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;
        page.ClientScript.RegisterClientScriptBlock(page.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);
    }
}
