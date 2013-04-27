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

public partial class UI_Department_DepartmentList : BasePage
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
        //UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.DeparmentEditUrl);
        //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        //base.ControlClientClickBindShow(this.btnAdd, 370, 300, urlEdit.ToUrlString());
        Tools.DropDownListDataBindByEnum(this.ddlUse, typeof(Status), string.Empty, true, true);
        this.GridViewDataBind();
    }

    private void GridViewDataBind()
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        if (this.ddlUse.SelectedIndex > 0)
        {
            builder.AppendItem(DepartmentInfoDBConst.UseFlag, this.ddlUse.SelectedValue);
        }
        List<DepartmentInfo> infoList = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(builder);
        GridViewControl.GridViewDataBind<DepartmentInfo>(this.gvList, infoList);
        //this.gvList.DataSource = departList;
        //this.gvList.DataBind();
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        String departID = (sender as ImageButton).CommandArgument;
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder(), builderP = new WhereSqlClauseBuilder();
        builder.AppendItem(DepartmentInfoDBConst.ParentID, departID);
        builderP.AppendItem(PersonInfoDBConst.DepartmentID, departID);
        if (DepartmentInfoAdapter.Instance.GetDepartmentInfoList(builder).Any())
        {
            base.MessageBox("该部门下尚有子部门，请先将子部门删除！");
        }
        else if (PersonInfoAdapter.Instance.GetPersonInfoList(builder).Any())
        {
            base.MessageBox("该部门下尚有人员，请先将人员移出！");
        }
        else
        {
            DepartmentInfoAdapter.Instance.DeleteDepartmentInfo(departID);
            //base.MessageBox("部门删除成功！");
            this.GridViewDataBind();
        }
    }

    protected void lbtnUseLogout_Click(object sender, EventArgs e)
    {
        string text=(sender as LinkButton).Text, departID = (sender as LinkButton).CommandArgument;
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder(), builderP = new WhereSqlClauseBuilder();
        builder.AppendItem(DepartmentInfoDBConst.ParentID, departID);
        builderP.AppendItem(PersonInfoDBConst.DepartmentID, departID);
        if (text == EnumHelper.GetDescription(Status.False))
        {
            if (DepartmentInfoAdapter.Instance.GetDepartmentInfoList(builder).Any())
            {
                base.MessageBox(String.Format("该部门下尚有子部门，不能{0}！", text));
                return;
            }
            else if (PersonInfoAdapter.Instance.GetPersonInfoList(builder).Any())
            {
                base.MessageBox(String.Format("该部门下尚有人员，不能{0}！", text));
                return;
            }
        }
        DepartmentInfoAdapter.Instance.ChangeStatus(departID, text == EnumHelper.GetDescription(Status.True) ? (Int32)Status.True : (Int32)Status.False);
        //base.MessageBox(String.Format(@"部门{0}成功！", text));
        this.GridViewDataBind();
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        LinkButton btnView, btnUse, btnLogout;
        ImageButton ibtnEdit, ibtnAddChild;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.DeparmentEditUrl);
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value == null)))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                String strDeptID=this.gvList.DataKeys[i].Values[DepartmentInfoConst.DepartmentID].ToString();
                String strDeptCode = this.gvList.DataKeys[i].Values[DepartmentInfoConst.DepartmentCode].ToString();
                btnView = this.gvList.Rows[i].FindControl("btnView") as LinkButton;
                ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                ibtnAddChild = this.gvList.Rows[i].FindControl("ibtnAddChild") as ImageButton;
                btnUse = this.gvList.Rows[i].FindControl("btnUse") as LinkButton;
                btnLogout = this.gvList.Rows[i].FindControl("btnLogout") as LinkButton;
                //缩进
                btnView.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfDeptName(strDeptCode) + btnView.Text;

                urlEdit.AppendItem(DepartmentInfoConst.DepartmentID, strDeptID);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindShow(btnView, 380, 310, urlEdit.ToUrlString());
                urlEdit.RemoveAt(1);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindShow(ibtnEdit, 380, 310, urlEdit.ToUrlString());
                urlEdit.Clear();
                urlEdit.AppendItem(DepartmentInfoConst.ParentID, strDeptID);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
                base.ControlClientClickBindShow(ibtnAddChild, 380, 310, urlEdit.ToUrlString());
                urlEdit.Clear();

                btnUse.Text = EnumHelper.GetDescription(Status.True);
                btnLogout.Text = EnumHelper.GetDescription(Status.False);
                if (bool.Parse(this.gvList.DataKeys[i][DepartmentInfoConst.UseFlag].ToString()))
                {
                    btnLogout.Enabled = false;
                    btnUse.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该部门？')", btnUse.Text));
                }
                else
                {
                    btnUse.Enabled = false;
                    btnLogout.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该部门？')", btnLogout.Text));
                }
            }
        }
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        base.ControlClientClickBindShow(ibtnAdd, 380, 310, urlEdit.ToUrlString());
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.GridViewDataBind();
    }
}
