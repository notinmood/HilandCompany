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
using Salary.Web.Utility;

public partial class UI_Salary_SalaryList : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitializeControl();
        }
        else
        {
            GridViewControl.ResetGridView(this.gvList);
        }
    }

    private void InitializeControl()
    {
        this.ddlYears.DataTextField = "Year";
        this.ddlYears.DataValueField = "Year";
        this.ddlYears.DataSource = PayMonthInfoAdapter.Instance.GetPayMonthYears();
        this.ddlYears.DataBind();
        this.ddlYears.Items.Insert(0, new ListItem("----请选择----", ""));
        this.GridViewDataBind(false);
        if(this.gvList.Rows.Count>0)
        {
            if (!(this.gvList.Rows.Count == 1 && this.gvList.Rows[0].Cells.Count == 1))
            {
                string yearMonth = this.gvList.DataKeys[0].Values[PayMonthInfoConst.YearMonth].ToString().Replace("年", "-").Replace("月", "-01");
                this.tbYearMonth.Text = DateTime.Parse(yearMonth).AddMonths(1).ToString("yyyy-MM");
            }
        }
        else
        {
            this.gvList.BorderWidth = 0;
            this.tbYearMonth.Text=DateTime.Now.ToString("yyyy-MM");
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.GridViewDataBind(false);
    }

    private void GridViewDataBind(bool addNew)
    {
        WhereSqlClauseBuilder builer = new WhereSqlClauseBuilder();
        if (this.ddlYears.SelectedIndex > 0)
        {
            builer.AppendItem(PayMonthInfoDBConst.YearMonth, this.ddlYears.SelectedValue + "%", " LIKE ");
        }
        DataTable dt = PayMonthInfoAdapter.Instance.GetPayMonthListDT(builer);
        if (addNew)
        {
            DataRow dr = dt.NewRow();
            dt.Rows.InsertAt(dr, 0);
            this.gvList.EditIndex = 0;
            GridViewControl.GridViewDataBind(this.gvList, dt);
            this.CurRowEdit(0);
        }
        else
        {
            GridViewControl.GridViewDataBind(this.gvList, dt);
        }
    }

    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        this.GridViewDataBind(true);
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(PayMonthInfoDBConst.YearMonth, (sender as ImageButton).CommandArgument.Replace("年", "").Replace("月", ""));
        PayMonthInfoAdapter.Instance.DeletePayMonthInfo(builder);
        //base.MessageBox("薪资表删除成功！");
        this.GridViewDataBind(false);
    }

    protected void ibtnReCreate_Click(object sender, EventArgs e)
    {
        String yearMonth=(sender as ImageButton).CommandArgument.Replace("年", "").Replace("月", "");
        if (this.IsPayMonthCreated(yearMonth))
        {
            base.MessageBox("该月薪资表已存在，若需重新生成请先删除！");
            return;
        }
        PayMonthInfoAdapter.Instance.InsertPayMonthInfo(yearMonth, true);
        PayMonthInfoAdapter.Instance.CalculatePayMonthInfo(yearMonth);
        base.MessageBox("薪资表重新生成成功！");
        this.GridViewDataBind(false);
    }

    protected void ibtnKelowna_Click(object sender, EventArgs e)
    {
       String yearMonth = (sender as ImageButton).CommandArgument.Replace("年","").Replace("月", "");
        //String yearMonth = String.Concat(dt.Year.ToString(), dt.Month.ToString());
        PayMonthInfoAdapter.Instance.KelownaPayMonthInfo(yearMonth);
        //PayMonthInfoAdapter.Instance.CalculatePayMonth(yearMonth);
        base.MessageBox("薪资表复制生成成功！");
        this.GridViewDataBind(false);
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        LinkButton btnView;
        ImageButton ibtnEdit, ibtnFeeMonth;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.SalaryEditUrl);
        UrlParamBuilder urlFeeMonthList = new UrlParamBuilder(SalaryConst.FeeListUrl);
        String yearMonth="";
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value.ToString() == "")))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                if (this.gvList.EditIndex != i)
                {
                    btnView = this.gvList.Rows[i].FindControl("btnView") as LinkButton;
                    ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                    ibtnFeeMonth = this.gvList.Rows[i].FindControl("ibtnFeeMonth") as ImageButton;
                    yearMonth = this.gvList.DataKeys[i].Values[PayMonthInfoConst.YearMonth].ToString().Replace("年", "").Replace("月", "");

                    urlFeeMonthList.AppendItem(FeeMonthInfoConst.YearMonth, yearMonth);
                    base.ControlClientClickBindOpen(ibtnFeeMonth, 480, 600, urlFeeMonthList.ToUrlString());
                    urlEdit.AppendItem(PayMonthInfoConst.YearMonth, yearMonth);
                    urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                    base.ControlClientClickBindOpen(btnView, 900, 700, urlEdit.ToUrlString());
                    urlEdit.RemoveAt(1);
                    urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                    base.ControlClientClickBindOpen(ibtnEdit, 900,700, urlEdit.ToUrlString());
                    urlEdit.Clear();
                    urlFeeMonthList.Clear();

                }
            }
        }
        //ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        //base.ControlClientClickBindOpen(ibtnAdd, 370, 300, urlEdit.ToUrlString());
    }

    protected void gvList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.gvList.EditIndex = -1;
        this.GridViewDataBind(false);
        //this.btnAdd.Enabled = true;
        this.SetGvListHeaderAddBtn(true);
    }

    protected void gvList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox tbYearMonth = (TextBox)this.gvList.Rows[e.RowIndex].FindControl("tbYearMonth");
        string yearMonth = tbYearMonth.Text.Replace("-", "");
        if (this.IsPayMonthCreated(yearMonth))
        {
            base.MessageBox("该月薪资表已存在，若需重新生成请先删除！");
            return;
        }
        PayMonthInfoAdapter.Instance.InsertPayMonthInfo(yearMonth, false);
        PayMonthInfoAdapter.Instance.CalculatePayMonthInfo(yearMonth);
        base.MessageBox("薪资表生成成功！");
        this.gvList.EditIndex = -1;
        this.GridViewDataBind(false);
    }

    private void SetGvListHeaderAddBtn(Boolean boolValue)
    {
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        ibtnAdd.Enabled = boolValue;
    }
    private bool IsPayMonthCreated(String yearMonth)
    {
        return PayMonthInfoAdapter.Instance.IsCreated(yearMonth);
    }
    private void CurRowEdit(int rowIndex)
    {
        TextBox tbYearMonth = (TextBox)this.gvList.Rows[rowIndex].FindControl("tbYearMonth");
        if (this.gvList.DataKeys[rowIndex].Values[PayMonthInfoConst.YearMonth].ToString() == "")
        {
            if (this.gvList.Rows.Count > 1)
            {
                string yearMonth = this.gvList.DataKeys[1].Values[PayMonthInfoConst.YearMonth].ToString().Replace("年", "-").Replace("月", "-01");
                tbYearMonth.Text = DateTime.Parse(yearMonth).AddMonths(1).ToString("yyyy-MM");
            }
            else
            {
                tbYearMonth.Text = DateTime.Now.ToString("yyyy-MM");
            }
        }
        //this.btnAdd.Enabled = false;
        this.SetGvListHeaderAddBtn(false);
    }

    #region 无用
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string yearMonth = this.tbYearMonth.Text.Replace("-", "");
        if (this.IsPayMonthCreated(yearMonth))
        {
            base.MessageBox("该月薪资表已存在，若需重新生成请先删除！");
            return;
        }
        PayMonthInfoAdapter.Instance.InsertPayMonthInfo(yearMonth, false);
        PayMonthInfoAdapter.Instance.CalculatePayMonthInfo(yearMonth);
        base.MessageBox("薪资表生成成功！");
        this.GridViewDataBind(false);
    }

    protected void tbYearMonth_TextChanged(object sender, EventArgs e)
    {
        //this.IsPayMonthCreated();
    }
    #endregion 无用
}
