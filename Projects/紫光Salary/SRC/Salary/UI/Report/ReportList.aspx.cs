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
using Salary.Web.Utility;

public partial class UI_Report_ReportList : BasePage
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
        //UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.ReportEditUrl);
        //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        //base.ControlClientClickBindOpen(this.btnAdd, 370, 300, urlEdit.ToUrlString());
        Tools.DropDownListDataBindByEnum(this.ddlUse, typeof(Status), string.Empty, true, true);
        this.GridViewDataBind();
    }

    private void GridViewDataBind()
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        if(this.ddlUse.SelectedIndex>0)
        {
            builder.AppendItem(ReportInfoDBConst.UserFlag, this.ddlUse.SelectedValue);
        }
        List<ReportInfo> infoList = ReportInfoAdapter.Instance.GetReportInfoList(builder);
        GridViewControl.GridViewDataBind<ReportInfo>(this.gvList, infoList);
        //this.gvList.DataSource = infoList;
        //this.gvList.DataBind();
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        ReportInfoAdapter.Instance.DeleteReportInfo((sender as ImageButton).CommandArgument);
        //base.MessageBox("报表删除成功！");
        this.GridViewDataBind();
    }

    protected void lbtnUseLogout_Click(object sender, EventArgs e)
    {
        string text = (sender as LinkButton).Text;
        string value = (sender as LinkButton).CommandArgument;
        ReportInfoAdapter.Instance.ChangeStatus(value, text == EnumHelper.GetDescription(Status.True) ? (Int32)Status.True : (Int32)Status.False);
        //base.MessageBox(String.Format(@"报表{0}成功！", (sender as LinkButton).Text));
        this.GridViewDataBind();
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        LinkButton btnView, btnUse, btnLogout;
        ImageButton ibtnEdit;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.ReportEditUrl);
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value == null)))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                btnView = this.gvList.Rows[i].FindControl("btnView") as LinkButton;
                //btnEdit = this.gvList.Rows[i].FindControl("btnEdit") as LinkButton;
                btnUse = this.gvList.Rows[i].FindControl("btnUse") as LinkButton;
                btnLogout = this.gvList.Rows[i].FindControl("btnLogout") as LinkButton;
                ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                ////if (btnView.Text == "薪资表")
                ////{
                ////    LinkButton btnDelete = this.gvList.Rows[i].FindControl("btnDelete") as LinkButton;
                ////    btnDelete.Style.Add("display", "none");
                ////    btnEdit.Style.Add("display", "none");
                ////    continue;
                ////}
                urlEdit.AppendItem(ReportInfoConst.ReportID, this.gvList.DataKeys[i].Values[ReportInfoConst.ReportID]);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindOpen(btnView, 800, 500, urlEdit.ToUrlString());
                urlEdit.RemoveAt(1);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                //base.ControlClientClickBindOpen(btnEdit, 370, 300, urlEdit.ToUrlString());
                base.ControlClientClickBindOpen(ibtnEdit, 800, 500, urlEdit.ToUrlString());
                urlEdit.Clear();

                btnUse.Text = EnumHelper.GetDescription(Status.True);
                btnLogout.Text = EnumHelper.GetDescription(Status.False);
                if (bool.Parse(this.gvList.DataKeys[i][ProjectInfoConst.UseFlag].ToString()))
                {
                    btnLogout.Enabled = false;
                    btnUse.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该报表？')", btnUse.Text));
                }
                else
                {
                    btnUse.Enabled = false;
                    btnLogout.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该报表？')", btnLogout.Text));
                }
            }
        }
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        base.ControlClientClickBindOpen(ibtnAdd, 800, 500, urlEdit.ToUrlString());
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.GridViewDataBind();
    }
}