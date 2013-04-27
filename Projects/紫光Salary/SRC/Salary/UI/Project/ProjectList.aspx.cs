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

public partial class UI_Project_ProjectList : BasePage
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
        //UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.ProjectEditUrl);
        //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        //base.ControlClientClickBindShow(this.btnAdd, 370, 300, urlEdit.ToUrlString());
        //urlEdit = new UrlParamBuilder(SalaryConst.ProjectClassEditUrl);
        //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        //base.ControlClientClickBindShow(this.btnAddProjectClass, 370, 300, urlEdit.ToUrlString());
        this.TreeViewDataBind();
        this.GridViewDataBind();
    }

    private void GridViewDataBind()
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        TreeNode tn = this.trvProjectClass.SelectedNode;
        if (tn != null)
        {
            String projectClassId = tn.Value;
            if (!String.IsNullOrEmpty(projectClassId))
            {
                builder.AppendItem(ProjectInfoDBConst.ProjectClassID, projectClassId + "%", "LIKE");
            }
            else
            {
                btnEditProjectClass.Enabled = false;
            }
        }
        else
        {
            btnEditProjectClass.Enabled = false;
        }
        List<ProjectInfo> infoList = ProjectInfoAdapter.Instance.GetProjectInfoList(builder);
        GridViewControl.GridViewDataBind<ProjectInfo>(this.gvList, infoList);
        //this.gvList.DataSource = projectList;
        //this.gvList.DataBind();
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        //if (ProjectAdapter.Instance.IsHaveChildProject((sender as LinkButton).CommandArgument))
        //{
        //    base.MessageBox("该项目下尚有子项目，请先将子项目删除！");
        //}
        //else
        //{
        //    ProjectAdapter.Instance.DeleteProjectInfo((sender as LinkButton).CommandArgument);
        //    //base.MessageBox("项目删除成功！");
        //    this.GridViewDataBind();
        //}
        ProjectInfoAdapter.Instance.DeleteProjectInfo((sender as ImageButton).CommandArgument);
        this.GridViewDataBind();
    }

    protected void lbtnUseLogout_Click(object sender, EventArgs e)
    {
        string text = (sender as LinkButton).Text;
        string value = (sender as LinkButton).CommandArgument;
        //if (text == EnumHelper.GetDescription(Status.Logout) && ProjectAdapter.Instance.IsHaveChildProject(value))
        //{
        //    base.MessageBox(String.Format("该项目下尚有子项目，不能{0}！", text));
        //    return;
        //}
        ProjectInfoAdapter.Instance.ChangeStatus(value, text == EnumHelper.GetDescription(Status.True) ? (Int32)Status.True : (Int32)Status.False);
        //base.MessageBox(String.Format(@"项目{0}成功！", (sender as LinkButton).Text));
        this.GridViewDataBind();
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        LinkButton btnView, btnUse, btnLogout;
        ImageButton ibtnEdit;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.ProjectEditUrl);
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value == null)))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                btnView = this.gvList.Rows[i].FindControl("btnView") as LinkButton;
                ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                btnUse = this.gvList.Rows[i].FindControl("btnUse") as LinkButton;
                btnLogout = this.gvList.Rows[i].FindControl("btnLogout") as LinkButton;

                urlEdit.AppendItem(ProjectInfoConst.ProjectID, this.gvList.DataKeys[i].Values[ProjectInfoConst.ProjectID]);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindShow(btnView, 370, 300, urlEdit.ToUrlString());
                urlEdit.RemoveAt(1);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindShow(ibtnEdit, 370, 300, urlEdit.ToUrlString());
                urlEdit.Clear();

                btnUse.Text = EnumHelper.GetDescription(Status.True);
                btnLogout.Text = EnumHelper.GetDescription(Status.False);
                if (bool.Parse(this.gvList.DataKeys[i][ProjectInfoConst.UseFlag].ToString()))
                {
                    btnLogout.Enabled = false;
                    btnUse.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该项目？')", btnUse.Text));
                }
                else
                {
                    btnUse.Enabled = false;
                    btnLogout.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该项目？')", btnLogout.Text));
                }
            }
        }
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit = new UrlParamBuilder(SalaryConst.ProjectEditUrl);
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        base.ControlClientClickBindShow(ibtnAdd, 370, 300, urlEdit.ToUrlString());
    }

    private void TreeViewDataBind()
    {
        trvProjectClass.Nodes.Clear();
        TreeNode tnAdd = new TreeNode();
        tnAdd.Text = SalaryAppConst.CompanyName;    //"全部";
        tnAdd.Value = "";
        trvProjectClass.Nodes.Add(tnAdd);
        //WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        List<ProjectClassInfo> projectClassList = ProjectClassInfoAdapter.Instance.GetProjectClassInfoList(null);
        foreach (ProjectClassInfo projectClassInfo in projectClassList.Where(pc => String.IsNullOrEmpty(pc.ParentClassID)))
        {
            TreeNode tnChild = new TreeNode();
            tnChild.Text = projectClassInfo.ProjectClassName;
            tnChild.Value = projectClassInfo.ProjectClassID;
            TreeViewDataBind(tnChild, projectClassList, projectClassInfo.ProjectClassID);
            tnAdd.ChildNodes.Add(tnChild);
        }
        trvProjectClass.Nodes[0].Checked = true;
    }

    private void TreeViewDataBind(TreeNode tnParent, List<ProjectClassInfo> projectClassList, string parentID)
    {
        foreach (ProjectClassInfo projectClassInfo in projectClassList.Where(pc => pc.ParentClassID == parentID))
        {
            TreeNode tnChild = new TreeNode();
            tnChild.Text = projectClassInfo.ProjectClassName;
            tnChild.Value = projectClassInfo.ProjectClassID;
            TreeViewDataBind(tnChild, projectClassList, projectClassInfo.ProjectClassID);
            tnParent.ChildNodes.Add(tnChild);
        }
    }
    protected void trvProjectClass_SelectedNodeChanged(object sender, EventArgs e)
    {
        this.GridViewDataBind();
        String value = this.trvProjectClass.SelectedNode.Value;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.ProjectEditUrl);
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        urlEdit.AppendItem(ProjectInfoConst.ProjectClassID, value);
        base.ControlClientClickBindShow(ibtnAdd, 370, 300, urlEdit.ToUrlString());

        urlEdit = new UrlParamBuilder(SalaryConst.ProjectClassEditUrl);
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        urlEdit.AppendItem(ProjectClassInfoConst.ParentClassID, value);
        base.ControlClientClickBindShow(this.btnAddProjectClass, 370, 300, urlEdit.ToUrlString());

        if (!String.IsNullOrEmpty(value))
        {
            this.btnEditProjectClass.Enabled = true;
            urlEdit = new UrlParamBuilder(SalaryConst.ProjectClassEditUrl);
            urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
            urlEdit.AppendItem(ProjectClassInfoConst.ProjectClassID, value);
            base.ControlClientClickBindShow(this.btnEditProjectClass, 370, 300, urlEdit.ToUrlString());
        }
    }
}
