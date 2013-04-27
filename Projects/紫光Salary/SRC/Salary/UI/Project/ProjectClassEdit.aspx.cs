using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Salary.Web.BasePage;
using Salary.Biz.Eunm;
using Salary.Biz;
using Salary.Core.Utility;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;

public partial class UI_Project_ProjectClassEdit : BasePage
{
    string projectClassId, parentClassId;
    ActionType action;

    protected void Page_Load(object sender, EventArgs e)
    {
        projectClassId = DecodedQueryString[ProjectClassInfoConst.ProjectClassID];
        parentClassId = DecodedQueryString[ProjectClassInfoConst.ParentClassID];
        action = EnumHelper.Parse<ActionType>(DecodedQueryString[SalaryConst.QueryAction]);
        if (!IsPostBack)
        {
            this.InitializeControl();
        }
    }

    private void InitializeControl()
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(ProjectClassInfoDBConst.UseFlag, Status.True.ToString("D"));
        ProjectClassInfo projectClassInfo = null;
        if (action != ActionType.Add)
        {
            projectClassInfo = ProjectClassInfoAdapter.Instance.LoadProjectClassInfo(projectClassId);
            this.tbProjectClassCode.Enabled = false;
            this.tbProjectClassCode.Text = projectClassInfo.ProjectClassID;
            this.tbProjectClassName.Text = projectClassInfo.ProjectClassName;
            builder.AppendItem(ProjectClassInfoDBConst.ProjectClassID, projectClassInfo.ProjectClassID + "%", "NOT LIKE");
            builder.AppendItem("LEN(" + ProjectClassInfoDBConst.ProjectClassID + ")", projectClassInfo.ProjectClassID.Length, "<=");
        }
        else
        {
            this.tbProjectClassCode.Text = ProjectClassInfoAdapter.Instance.CreateProjectClassId(this.ddlProjectClass.SelectedValue);
        }
        List<ProjectClassInfo> projectClassList = ProjectClassInfoAdapter.Instance.GetProjectClassInfoList(builder);
        this.ddlProjectClass.DataSource = projectClassList;
        this.ddlProjectClass.DataTextField = ProjectClassInfoConst.ProjectClassName;
        this.ddlProjectClass.DataValueField = ProjectClassInfoConst.ProjectClassID;
        this.ddlProjectClass.DataBind();
        this.ddlProjectClass.Items.Insert(0, new ListItem("----请选择----", ""));

        if (action != ActionType.Add)
        {
            this.ddlProjectClass.SelectedValue = projectClassInfo.ParentClassID;
        }
        else if (!String.IsNullOrEmpty(parentClassId))
        {
            this.ddlProjectClass.SelectedValue = parentClassId;
            this.ddlProjectClass_SelectedIndexChanged(null,null);
        }
    }

    private ProjectClassInfo GetInfoFromPageControl()
    {
        ProjectClassInfo info = null;
        switch (action)
        {
            case ActionType.Add:
                info = new ProjectClassInfo();
                info.ProjectClassID = this.tbProjectClassCode.Text;
                break;
            case ActionType.Edit:
                info = ProjectClassInfoAdapter.Instance.LoadProjectClassInfo(projectClassId);
                //info.ProjectClassId = this.tbProjectClassCode.Text;//CZ
                break;
        }
        info.ProjectClassName = this.tbProjectClassName.Text.Trim();
        if (this.ddlProjectClass.SelectedIndex > 0)
        {
            info.ParentClassID = this.ddlProjectClass.SelectedValue;
            String nm = this.ddlProjectClass.SelectedItem.Text.Replace("　", "").Trim();
            info.ParentClassName = nm.StartsWith("-") ? nm.Substring(1).Trim() : nm;
        }
        return info;
    }

    protected void btnSaveClick(Object sender, EventArgs e)
    {
        ProjectClassInfo info = this.GetInfoFromPageControl();
        if (ProjectClassInfoAdapter.Instance.IsProjectClassNameUsed(info, action == ActionType.Add))
        {
            base.MessageBox("项目分类名称冲突，请重新定义！");
            this.tbProjectClassName.Focus();
            return;
        }
        if (action == ActionType.Add)
        {
            ProjectClassInfoAdapter.Instance.InsertProjectClassInfo(info);
        }
        else
        {
            ProjectClassInfoAdapter.Instance.UpdateProjectClassInfo(info);
        }
        String javascript = String.Format(@"
            window.dialogArguments.location.href = window.dialogArguments.location.href ;
            alert('项目分类保存成功！');
            window.close();");
        ExecuteJavascript(javascript);
    }
    protected void ddlProjectClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (action == ActionType.Add)
        {
            this.tbProjectClassCode.Text = ProjectClassInfoAdapter.Instance.CreateProjectClassId(this.ddlProjectClass.SelectedValue);
        }
    }
    protected void ddlProjectClass_DataBound(object sender, EventArgs e)
    {
        foreach (ListItem li in this.ddlProjectClass.Items)
        {
            li.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfProjectName(li.Value) + li.Text;
        }
    }
}
