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

public partial class UI_UserList : BasePage
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
        //UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.UserEditUrl);
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
            builder.AppendItem(UserInfoDBConst.Logout, this.ddlUse.SelectedValue);
        }
        List<UserInfo> infoList = UserInfoAdapter.Instance.GetUserInfoList(builder);
        GridViewControl.GridViewDataBind<UserInfo>(this.gvList, infoList);
        //this.gvList.DataSource = userList;
        //this.gvList.DataBind();
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        UserInfoAdapter.Instance.DeleteUserInfo((sender as ImageButton).CommandArgument);
        //base.MessageBox("用户删除成功！");
        this.GridViewDataBind();
    }

    protected void lbtnResetPassword_Click(object sender, EventArgs e)
    {
        UserInfoAdapter.Instance.ChangePassword((sender as ImageButton).CommandArgument, SalaryConst.DefaultPassword);
        //UserInfoAdapter.Instance.ChangePassword((sender as LinkButton).CommandArgument,SalaryConst.DefaultPassword);
        base.MessageBox("用户密码重置成功！");
    }

    protected void lbtnUseLogout_Click(object sender, EventArgs e)
    {
        UserInfoAdapter.Instance.ChangeStatus((sender as LinkButton).CommandArgument, (sender as LinkButton).Text == EnumHelper.GetDescription(Status.True) ? (Int32)Status.True : (Int32)Status.False);
        //base.MessageBox(String.Format(@"用户{0}成功！",(sender as LinkButton).Text));
        this.GridViewDataBind();
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        LinkButton btnView, btnResetPassword, btnUse, btnLogout;
        ImageButton ibtnEdit;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.UserEditUrl);
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value == null)))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                btnView = this.gvList.Rows[i].FindControl("btnView") as LinkButton;
                ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                btnResetPassword = this.gvList.Rows[i].FindControl("btnResetPassword") as LinkButton;
                btnUse = this.gvList.Rows[i].FindControl("btnUse") as LinkButton;
                btnLogout = this.gvList.Rows[i].FindControl("btnLogout") as LinkButton;

                urlEdit.AppendItem(UserInfoConst.UserId, this.gvList.DataKeys[i].Values[UserInfoConst.UserId]);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);   //ActionType.View
                base.ControlClientClickBindShow(btnView, 370, 200, urlEdit.ToUrlString());
                urlEdit.RemoveAt(1);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindShow(ibtnEdit, 370, 200, urlEdit.ToUrlString());
                urlEdit.Clear();

                //btnResetPassword.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要重置该用户的密码为{0}？')", SalaryConst.DefaultPassword));

                btnUse.Text = EnumHelper.GetDescription(Status.True);
                btnLogout.Text = EnumHelper.GetDescription(Status.False);
                if (this.gvList.DataKeys[i][UserInfoConst.Logout].ToString() == (Status.True.ToString("D")).ToString())
                {
                    btnUse.Enabled = false;
                    btnLogout.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该用户？')", btnLogout.Text));
                }
                else
                {
                    btnLogout.Enabled = false;
                    btnUse.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该用户？')", btnUse.Text));
                }
            }
        }
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        base.ControlClientClickBindShow(ibtnAdd, 370, 200, urlEdit.ToUrlString());
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.GridViewDataBind();
    }
}
