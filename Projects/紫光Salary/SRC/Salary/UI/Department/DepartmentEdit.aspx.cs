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

public partial class UI_Department_DepartmentEdit : BasePage
{
    string departID, parentID;
    ActionType action;

    protected void Page_Load(object sender, EventArgs e)
    {
        departID = DecodedQueryString[DepartmentInfoConst.DepartmentID];
        parentID = DecodedQueryString[DepartmentInfoConst.ParentID];
        action = EnumHelper.Parse<ActionType>(DecodedQueryString[SalaryConst.QueryAction]);
        if (!IsPostBack)
        {
            this.InitializeControl();
        }
    }

    private void InitializeControl()
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(DepartmentInfoDBConst.UseFlag, Status.True.ToString("D"));
        DepartmentInfo departInfo = null;

        if (action != ActionType.Add)
        {
            departInfo = DepartmentInfoAdapter.Instance.LoadDepartmentInfo(departID);
            this.tbDepartmentCode.Text = departInfo.DepartmentCode;
            this.tbDepartmentName.Text = departInfo.DepartmentName;
            //this.ddlDepartment.Items.Remove(this.ddlDepartment.Items.FindByValue(departInfo.DepartmentId));
            builder.AppendItem(DepartmentInfoDBConst.DepartmentCode, departInfo.DepartmentCode + "%", "NOT LIKE");
            builder.AppendItem("LEN(" + DepartmentInfoDBConst.DepartmentCode + ")", departInfo.DepartmentCode.Length, "<");
        }
        else
        {
            this.tbDepartmentCode.Text = DepartmentInfoAdapter.Instance.CreateDepartmentCode(this.ddlDepartment.SelectedValue);
        }
        List<DepartmentInfo> departList = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(builder);
        this.ddlDepartment.DataSource = departList;
        this.ddlDepartment.DataTextField = DepartmentInfoConst.DepartmentName;
        this.ddlDepartment.DataValueField = DepartmentInfoConst.DepartmentID;
        this.ddlDepartment.DataBind();
        this.ddlDepartment.Items.Insert(0, new ListItem("----请选择----", ""));

        if (action != ActionType.Add)
        {
            this.ddlDepartment.SelectedValue = departInfo.ParentID;
        }
        else if(!String.IsNullOrEmpty(parentID))
        {
            this.ddlDepartment.SelectedValue = parentID;
            this.ddlDepartment_SelectedIndexChanged(null, null);
        }
    }

    private DepartmentInfo GetInfoFromPageControl()
    {
        DepartmentInfo departInfo = null;
        switch (action)
        {
            case ActionType.Add:
                departInfo = new DepartmentInfo();
                departInfo.DepartmentID = CommonTools.Instance.GetMaxOrderNo(DepartmentInfoDBConst.TableName, DepartmentInfoDBConst.DepartmentID).ToString();
                break;
            case ActionType.Edit:
                departInfo = DepartmentInfoAdapter.Instance.LoadDepartmentInfo(departID);
                break;
        }
        departInfo.DepartmentCode = this.tbDepartmentCode.Text.Trim();
        departInfo.DepartmentName = this.tbDepartmentName.Text.Trim();
        if (this.ddlDepartment.SelectedIndex > 0)
        {
            departInfo.ParentID = this.ddlDepartment.SelectedValue;
            String nm = this.ddlDepartment.SelectedItem.Text.Replace("　", "").Trim();
            departInfo.ParentName = nm.StartsWith("-") ? nm.Substring(1).Trim() : nm;
        }
        return departInfo;
    }

    protected void btnSaveClick(Object sender, EventArgs e)
    {
        DepartmentInfo departInfo = this.GetInfoFromPageControl();
        if (DepartmentInfoAdapter.Instance.IsDeparmentCodeUsed(departInfo, action == ActionType.Add))
        {
            base.MessageBox("部门编码已使用，请重新定义！");
            this.tbDepartmentCode.Focus();
            return;
        }
        if (DepartmentInfoAdapter.Instance.IsDeparmentNameUsed(departInfo, action == ActionType.Add))
        {
            base.MessageBox("部门名称冲突，请重新定义！");
            this.tbDepartmentName.Focus();
            return;
        }
        if (action == ActionType.Add)
        {
            DepartmentInfoAdapter.Instance.InsertDepartmentInfo(departInfo);
            //departId = departInfo.DepartmentId;
            //UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.DeparmentEditUrl);
            //urlEdit.AppendItem(DepartmentInfoConst.DepartmentId, departId);
            //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
            //Redirect(urlEdit.ToUrlString());
        }
        else
        {
            DepartmentInfoAdapter.Instance.UpdateDepartmentInfo(departInfo);
        }
        //base.MessageAndRefreshParentByCurrHref("部门保存成功！");
        String javascript = String.Format(@"
            window.dialogArguments.location.href = window.dialogArguments.location.href ;
            alert('部门保存成功！');
            window.close();");
        ExecuteJavascript(javascript);
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (action == ActionType.Add)
        {
            this.tbDepartmentCode.Text = DepartmentInfoAdapter.Instance.CreateDepartmentCode(this.ddlDepartment.SelectedValue);
        }
    }
    protected void ddlDepartment_DataBound(object sender, EventArgs e)
    {
        DepartmentInfo departmentInfo = null;
        foreach (ListItem li in this.ddlDepartment.Items)
        {
            departmentInfo = DepartmentInfoAdapter.Instance.LoadDepartmentInfo(li.Value);
            li.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfDeptName(departmentInfo.DepartmentCode) + li.Text;
        }
    }
}
