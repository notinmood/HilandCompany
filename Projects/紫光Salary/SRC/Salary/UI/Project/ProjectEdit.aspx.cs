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

public partial class UI_Project_ProjectEdit : BasePage
{
    string projectID, projectClassID;
    ActionType action;

    protected void Page_Load(object sender, EventArgs e)
    {
        projectID = DecodedQueryString[ProjectInfoConst.ProjectID];
        projectClassID = DecodedQueryString[ProjectInfoConst.ProjectClassID];
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
        ProjectInfo projectInfo = null;
        if (action != ActionType.Add)
        {
            projectInfo = ProjectInfoAdapter.Instance.LoadProjectInfo(projectID);
            this.tbProjectCode.Text = projectInfo.ProjectCode;
            this.tbProjectName.Text = projectInfo.ProjectName;
        }
        else
        {
            this.tbProjectCode.Text = ProjectInfoAdapter.Instance.CreateProjectCode(this.ddlProjectClass.SelectedValue);
        }
        List<ProjectClassInfo> infoList = ProjectClassInfoAdapter.Instance.GetProjectClassInfoList(builder);
        this.ddlProjectClass.DataSource = infoList;
        this.ddlProjectClass.DataTextField = ProjectClassInfoConst.ProjectClassName;
        this.ddlProjectClass.DataValueField = ProjectClassInfoConst.ProjectClassID;
        this.ddlProjectClass.DataBind();
        this.ddlProjectClass.Items.Insert(0, new ListItem("----请选择----", ""));
        if (action != ActionType.Add)
        {
            this.ddlProjectClass.SelectedValue = projectInfo.ProjectClassID;
        }
        else if(!string.IsNullOrEmpty(projectClassID))
        {
            this.ddlProjectClass.SelectedValue = projectClassID;
            this.ddlProjectClass_SelectedIndexChanged(null, null);
        }
    }

    private ProjectInfo GetInfoFromPageControl()
    {
        ProjectInfo info = null;
        switch (action)
        {
            case ActionType.Add:
                info = new ProjectInfo();
                info.ProjectId = CommonTools.Instance.GetMaxOrderNo(ProjectInfoDBConst.TableName, ProjectInfoDBConst.ProjectID).ToString();
                break;
            case ActionType.Edit:
                info = ProjectInfoAdapter.Instance.LoadProjectInfo(projectID);
                break;
        }
        info.ProjectCode = this.tbProjectCode.Text.Trim();
        info.ProjectName = this.tbProjectName.Text.Trim();
        if (this.ddlProjectClass.SelectedIndex > 0)
        {
            info.ProjectClassID = this.ddlProjectClass.SelectedValue;
            String nm = this.ddlProjectClass.SelectedItem.Text.Replace("　", "").Trim();
            info.ProjectClassName = nm.StartsWith("-") ? nm.Substring(1).Trim() : nm;
        }
        return info;
    }

    protected void btnSaveClick(Object sender, EventArgs e)
    {
        ProjectInfo info = this.GetInfoFromPageControl();
        if (ProjectInfoAdapter.Instance.IsProjectCodeUsed(info, action == ActionType.Add))
        {
            base.MessageBox("项目编码冲突，请重新定义！");
            this.tbProjectCode.Focus();
            return;
        }
        if (ProjectInfoAdapter.Instance.IsProjectNameUsed(info, action == ActionType.Add))
        {
            base.MessageBox("项目名称冲突，请重新定义！");
            this.tbProjectName.Focus();
            return;
        }
        if (action == ActionType.Add)
        {
            ProjectInfoAdapter.Instance.InsertProjectInfo(info);
        }
        else
        {
            ProjectInfoAdapter.Instance.UpdateProjectInfo(info);
        }
        String javascript = String.Format(@"
            window.dialogArguments.location.href = window.dialogArguments.location.href ;
            alert('项目保存成功！');
            window.close();");
        ExecuteJavascript(javascript);
    }
    protected void ddlProjectClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (action == ActionType.Add)
        {
            this.tbProjectCode.Text = ProjectInfoAdapter.Instance.CreateProjectCode(this.ddlProjectClass.SelectedValue);
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
