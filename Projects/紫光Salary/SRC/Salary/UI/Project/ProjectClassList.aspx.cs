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

public partial class UI_Project_ProjectClassList : BasePage
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
        //UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.ProjectClassEditUrl);
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
            builder.AppendItem(ProjectClassInfoDBConst.UseFlag, this.ddlUse.SelectedValue);
        }
        List<ProjectClassInfo> infoList = ProjectClassInfoAdapter.Instance.GetProjectClassInfoList(builder);
        GridViewControl.GridViewDataBind<ProjectClassInfo>(this.gvList, infoList);
        //this.gvList.DataSource = projectClassList;
        //this.gvList.DataBind();
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string value = (sender as ImageButton).CommandArgument;
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder(), builderP=new WhereSqlClauseBuilder();
        builder.AppendItem(ProjectClassInfoDBConst.ParentClassID, value);
        builderP.AppendItem(ProjectInfoDBConst.ProjectClassID, value);
        if (ProjectClassInfoAdapter.Instance.GetProjectClassInfoList(builder).Any())
        {
            base.MessageBox("该项目分类下尚有子分类，请先将子分类删除！");
        }
        else if (ProjectInfoAdapter.Instance.GetProjectInfoList(builderP).Any())
        {
            base.MessageBox("该项目分类已有项目使用，不能删除！");
        }
        else
        {
            ProjectClassInfoAdapter.Instance.DeleteProjectClassInfo(value);
            //base.MessageBox("项目分类删除成功！");
            this.GridViewDataBind();
        }
    }

    protected void lbtnUseLogout_Click(object sender, EventArgs e)
    {
        string text = (sender as LinkButton).Text;
        string value = (sender as LinkButton).CommandArgument;
        if (text == EnumHelper.GetDescription(Status.False))
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder(), builderP = new WhereSqlClauseBuilder();
            builder.AppendItem(ProjectClassInfoDBConst.ParentClassID, value);
            builderP.AppendItem(ProjectInfoDBConst.ProjectClassID, value);
            if (ProjectClassInfoAdapter.Instance.GetProjectClassInfoList(builder).Any())
            {
                base.MessageBox(String.Format("该项目分类下尚有子分类，不能{0}！", text));
                return;
            }
            else if (ProjectInfoAdapter.Instance.GetProjectInfoList(builderP).Any())
            {
                base.MessageBox(String.Format("该项目分类已有项目使用，不能{0}！", text));
                return;
            }
        }
        ProjectClassInfoAdapter.Instance.ChangeStatus(value, text == EnumHelper.GetDescription(Status.True) ? (Int32)Status.True : (Int32)Status.False);
        //base.MessageBox(String.Format(@"项目分类{0}成功！", (sender as LinkButton).Text));
        this.GridViewDataBind();
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        LinkButton btnView, btnUse, btnLogout;
        ImageButton ibtnEdit, ibtnAddChild;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.ProjectClassEditUrl);
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value == null)))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                String strProjectClassId = this.gvList.DataKeys[i].Values[ProjectClassInfoConst.ProjectClassID].ToString();
                btnView = this.gvList.Rows[i].FindControl("btnView") as LinkButton;
                ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                ibtnAddChild = this.gvList.Rows[i].FindControl("ibtnAddChild") as ImageButton;
                btnUse = this.gvList.Rows[i].FindControl("btnUse") as LinkButton;
                btnLogout = this.gvList.Rows[i].FindControl("btnLogout") as LinkButton;
                //缩进
                btnView.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfProjectName(strProjectClassId) + btnView.Text;

                urlEdit.AppendItem(ProjectClassInfoConst.ProjectClassID, strProjectClassId);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindShow(btnView, 370, 300, urlEdit.ToUrlString());
                urlEdit.RemoveAt(1);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindShow(ibtnEdit, 370, 300, urlEdit.ToUrlString());
                urlEdit.Clear();
                urlEdit.AppendItem(ProjectClassInfoConst.ParentClassID, strProjectClassId);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
                base.ControlClientClickBindShow(ibtnAddChild, 370, 300, urlEdit.ToUrlString());
                urlEdit.Clear();

                btnUse.Text = EnumHelper.GetDescription(Status.True);
                btnLogout.Text = EnumHelper.GetDescription(Status.False);
                if (bool.Parse(this.gvList.DataKeys[i][ProjectClassInfoConst.UseFlag].ToString()))
                {
                    btnLogout.Enabled = false;
                    btnUse.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该项目分类？')", btnUse.Text));
                }
                else
                {
                    btnUse.Enabled = false;
                    btnLogout.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该项目分类？')", btnLogout.Text));
                }
            }
        }
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        base.ControlClientClickBindShow(ibtnAdd, 370, 300, urlEdit.ToUrlString());
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.GridViewDataBind();
    }
}
