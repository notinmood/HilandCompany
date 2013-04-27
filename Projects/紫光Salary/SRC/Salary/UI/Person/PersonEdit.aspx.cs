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
using ChinaCustoms.Framework.DeluxeWorks.Library.Core;
using System.Data;
using Salary.Web.Utility;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;

public partial class UI_Person_PersonEdit : BasePage
{
    public string yearMonth;
    protected string personID, departId, parentID=null;
    protected ActionType action;

    protected void Page_Load(object sender, EventArgs e)
    {
        yearMonth = DecodedQueryString[PersonBaseFeeMonthInfoConst.YearMonth];
        personID = DecodedQueryString[PersonInfoConst.PersonID];
        departId = DecodedQueryString[PersonInfoConst.DepartmentID];
        parentID = DecodedQueryString[PersonInfoConst.ParentID];
        action = EnumHelper.Parse<ActionType>(DecodedQueryString[SalaryConst.QueryAction]);
        if (!IsPostBack)
        {
            this.InitializeControl();
        }
        else
        {
            GridViewControl.ResetGridView(this.gvCommonList);
            GridViewControl.ResetGridView(this.gvPositionList);
            GridViewControl.ResetGridView(this.gvFloatList);
            GridViewControl.ResetGridView(this.gvCooperateList);
            GridViewControl.ResetGridView(this.gvVirtualList);
            GridViewControl.ResetGridView(this.gvServiceList);
            //GridViewControl.ResetGridView(this.gvDepartProjectList);
        }
    }

    private void InitializeControl()
    {
        Tools.DropDownListDataBindByEnum(this.ddlPersonType, typeof(PersonType), string.Empty, true, true);
        List<DepartmentInfo> departInfoList = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(null).Where(dp => !dp.UseFlag == bool.Parse(Status.True.ToString())).ToList();
        Tools.DropDownListDataBind(this.ddlDepartment, departInfoList, "", true, DepartmentInfoConst.DepartmentName, DepartmentInfoConst.DepartmentID);
        List<ProjectInfo> projectInfoList = ProjectInfoAdapter.Instance.GetProjectInfoList(null).Where(project => !project.UseFlag == Boolean.Parse(Status.True.ToString())).ToList();
        Tools.DropDownListDataBind(this.ddlProject, projectInfoList, "", true, ProjectInfoConst.ProjectName, ProjectInfoConst.ProjectID);

        if (action != ActionType.Add)
        {
            PersonInfo info = PersonInfoAdapter.Instance.LoadPersonInfo(personID);
            this.SetPersonControlInfo(info);
        }
        else
        {
            if (String.IsNullOrEmpty(this.parentID))
            {
                this.tbPersonCode.Text = PersonInfoAdapter.Instance.CreatePersonCode();
                this.ddlDepartment.SelectedValue = String.IsNullOrEmpty(departId) ? "" : departId;
            }
            else//复制
            {
                PersonInfo personInfo = PersonInfoAdapter.Instance.LoadPersonInfo(this.parentID);
                personInfo.ParentID = this.parentID;
                this.SetPersonControlInfo(personInfo);
            }
            this.tbEntryDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            List<PersonBaseFeeInfo> personBaseFeeInfoList = new List<PersonBaseFeeInfo>();//personInfoList.Add(new PersonBaseFee());
            GridViewControl.GridViewDataBind<PersonBaseFeeInfo>(this.gvCommonList, personBaseFeeInfoList);
            GridViewControl.GridViewDataBind<PersonBaseFeeInfo>(this.gvPositionList, personBaseFeeInfoList);
            GridViewControl.GridViewDataBind<PersonBaseFeeInfo>(this.gvFloatList, personBaseFeeInfoList);
            GridViewControl.GridViewDataBind<PersonBaseFeeInfo>(this.gvCooperateList, personBaseFeeInfoList);
            GridViewControl.GridViewDataBind<PersonBaseFeeInfo>(this.gvVirtualList, personBaseFeeInfoList);
        }
        if (!String.IsNullOrEmpty(yearMonth))
        {
            this.SetControlReadOnly();
        }
        else
        {
            ////this.tabs-2.Visible = false;
            ////this.tabs-3.Visible = false;
        }
    }

    private void SetPersonControlInfo(PersonInfo personInfo)
    {
        this.tbPersonCode.Text = personInfo.PersonCode;
        this.tbPersonName.Text = personInfo.PersonName;
        this.ddlPersonType.SelectedValue = ((Int32)personInfo.PersonType).ToString();
        this.tbEntryDate.Text = personInfo.EntryDate.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "" : personInfo.EntryDate.ToString("yyyy-MM-dd");
        this.tbLeftDate.Text = personInfo.LeftDate.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "" : personInfo.LeftDate.ToString("yyyy-MM-dd");
        this.ddlDepartment.SelectedValue = personInfo.DepartmentID;
        this.ddlProject.SelectedValue = personInfo.ProjectID;
        this.BaseGridViewDataBind(this.gvCommonList, CommonFeeType.CommonSalary);
        this.BaseGridViewDataBind(this.gvPositionList, CommonFeeType.Position);
        this.BaseGridViewDataBind(this.gvFloatList, CommonFeeType.Float);
        this.BaseGridViewDataBind(this.gvCooperateList, CommonFeeType.Cooperate);
        this.BaseGridViewDataBind(this.gvVirtualList, CommonFeeType.Virtual);
        this.BaseGridViewDataBind(this.gvServiceList, CommonFeeType.Service);
        if (this.action != ActionType.Add)
        {
            //this.gvDepartProjectGridViewDataBind(false);
            switch (personInfo.PersonType)
            { 
                case PersonType.LaoWu:
                    this.liCommon.Visible = false;
                    this.liPosition.Visible = false;
                    this.liFloat.Visible = false;
                    this.liCooperate.Visible = false;
                    this.liVirtual.Visible = false;
                    this.tabs2.Visible = false;
                    this.tabs3.Visible = false;
                    this.tabs4.Visible = false;
                    this.tabs5.Visible = false;
                    this.tabs6.Visible = false;
                    break;
                case PersonType.JianZhi:
                    this.liCommon.Visible = false;
                    this.liFloat.Visible = false;
                    this.liCooperate.Visible = false;
                    this.liVirtual.Visible = false;
                    this.liService.Visible = false;
                    this.tabs2.Visible = false;
                    this.tabs4.Visible = false;
                    this.tabs5.Visible = false;
                    this.tabs6.Visible = false;
                    this.tabs7.Visible = false;
                    break;
                case PersonType.Retire:
                    this.liCommon.Visible = false;
                    this.liFloat.Visible = false;
                    this.liCooperate.Visible = false;
                    this.liVirtual.Visible = false;
                    this.liService.Visible = false;
                    this.tabs2.Visible = false;
                    this.tabs4.Visible = false;
                    this.tabs5.Visible = false;
                    this.tabs6.Visible = false;
                    this.tabs7.Visible = false;
                    break;
                case PersonType.QuanZhi:
                    this.liService.Visible = false;
                    this.tabs7.Visible = false;
                    break;
                case PersonType.WaiPin:
                    this.liService.Visible = false;
                    this.tabs7.Visible = false;
                    break;
                case PersonType.PaiChu:
                    this.liCooperate.Visible = false;
                    this.liVirtual.Visible = false;
                    this.liFloat.Visible = false;
                    this.liService.Visible = false;
                    this.tabs4.Visible = false;
                    this.tabs5.Visible = false;
                    this.tabs6.Visible = false;
                    this.tabs7.Visible = false;
                    break;
                case PersonType.Soldier:
                    this.liService.Visible = false;
                    this.tabs7.Visible = false;
                    break;
            }
        }
        this.tbPersonCode.Enabled = String.IsNullOrEmpty(personInfo.ParentID);
        this.tbPersonName.Enabled = String.IsNullOrEmpty(personInfo.ParentID);
    }

    private void SetControlReadOnly()
    {
        this.tbEntryDate.Enabled = false;
        this.tbLeftDate.Enabled = false;
        this.tbPersonCode.Enabled = false;
        this.tbPersonName.Enabled = false;
        this.ddlDepartment.Enabled = false;
        this.ddlProject.Enabled = false;
        this.btnSave.Enabled = false;
        this.ddlPersonType.Enabled = false;
    }

    private void BaseGridViewDataBind()//PersonType personType
    {
        GridViewControl.GridViewDataBind<PersonBaseFeeInfo>(this.gvCommonList
            , PersonBaseFeeInfoAdapter.Instance.GetPersonBaseFeeInfoList(PersonBaseFeeTarget.PersonBaseFee, yearMonth, personID)
            .OrderBy(fee => fee.FeeCode).ToList());
    }
    private void BaseGridViewDataBind(GridView gvList, CommonFeeType commonFeeType)
    {
        GridViewControl.GridViewDataBind<PersonBaseFeeInfo>(gvList
            , PersonBaseFeeInfoAdapter.Instance.GetPersonBaseFeeInfoList(PersonBaseFeeTarget.PersonBaseFee, yearMonth, personID, commonFeeType)
            .OrderBy(fee => fee.FeeCode).ToList());
    }

    private PersonInfo GetInfoFromPageControl()
    {
        PersonInfo info = null;
        switch (action)
        {
            case ActionType.Add:
                info = new PersonInfo();//orderno做主键
                info.PersonID = CommonTools.Instance.GetMaxOrderNo(PersonInfoDBConst.TableName, PersonInfoDBConst.PersonID).ToString();
                info.ParentID = this.parentID; 
                break;
            case ActionType.Edit:
                info = PersonInfoAdapter.Instance.LoadPersonInfo(personID);
                break;
        }
        info.PersonCode = this.tbPersonCode.Text.Trim();
        info.PersonName = this.tbPersonName.Text.Trim();
        info.PersonType = EnumHelper.Parse<PersonType>(this.ddlPersonType.SelectedValue);
        string str = this.tbEntryDate.Text.Trim();
        info.EntryDate = str.Length > 0 ? DateTime.Parse(str) : DateTime.Parse("0001-01-01");
        str = this.tbLeftDate.Text.Trim();
        info.LeftDate = str.Length > 0 ? DateTime.Parse(str) : DateTime.Parse("0001-01-01");
        info.Dimission = str.Length > 0;
        if (this.ddlDepartment.SelectedIndex > 0)
        {
            info.DepartmentID = this.ddlDepartment.SelectedValue;
            String nm = this.ddlDepartment.SelectedItem.Text.Replace("　", "").Trim();
            info.DepartmentName = nm.StartsWith("-") ? nm.Substring(1).Trim() : nm;
        }
        if (this.ddlProject.SelectedIndex > 0)
        {
            info.ProjectID = this.ddlProject.SelectedValue;
            String nm = this.ddlProject.SelectedItem.Text.Replace("　", "").Trim();
            info.ProjectName = nm.StartsWith("-") ? nm.Substring(1).Trim() : nm;
        }
        else
        {
            info.ProjectID = "";
            info.ProjectName = "";
        }
        return info;
    }

    protected void btnSaveClick(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(yearMonth))
        {
            return;
        }
        PersonInfo info = this.GetInfoFromPageControl();
        if (PersonInfoAdapter.Instance.IsPersonCodeUsed(info, action == ActionType.Add))
        {
            base.MessageBox("人员编码冲突，请重新定义！");
            this.tbPersonCode.Focus();
            return;
        }
        if (PersonInfoAdapter.Instance.IsPersonNameUsed(info, action == ActionType.Add))
        {
            base.MessageBox("姓名冲突，请重新定义！");
            this.tbPersonName.Focus();
            return;
        }
        if (action == ActionType.Add)
        {
            PersonInfoAdapter.Instance.InsertPersonInfo(info);
            personID = info.PersonID;
            if (!String.IsNullOrEmpty(this.parentID))//复制工资、分摊信息
            {//not
                List<PersonBaseFeeInfo> PersonBaseFeeInfoList = PersonBaseFeeInfoAdapter.Instance.GetPersonBaseFeeInfoList(PersonBaseFeeTarget.PersonBaseFee, this.yearMonth);
                foreach (PersonBaseFeeInfo personBaseFee in PersonBaseFeeInfoList.Where(baseFee => baseFee.FeeID == this.parentID).ToList())
                {
                    if (personBaseFee.FeeValue > 0)
                    {
                        personBaseFee.PersonID = this.personID;
                        PersonBaseFeeInfoAdapter.Instance.InsertPersonBaseFeeInfo(personBaseFee);
                    }
                }
                WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
                builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.PersonID, this.parentID);
                List<PersonBaseFeeDepartmentProjectInfo> personBaseFeeDPList = PersonBaseFeeDepartmentProjectInfoAdapter.Instance.GetPersonBaseFeeDepartmentProjectInfoList(null, builder);
                foreach (PersonBaseFeeDepartmentProjectInfo personBaseFeeDP in personBaseFeeDPList)
                {
                    personBaseFeeDP.PersonID = this.personID;//personBaseFeeDP.PbfdpId = Guid.NewGuid().ToString();
                    PersonBaseFeeDepartmentProjectInfoAdapter.Instance.InsertPersonBaseFeeDepartmentProjectInfo(personBaseFeeDP);
                }
            }
            
            UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.PersonEditUrl);
            urlEdit.AppendItem(PersonInfoConst.PersonID, personID);
            urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
            Redirect(urlEdit.ToUrlString());
        }
        else
        {
            PersonInfoAdapter.Instance.UpdatePersonInfo(info);
        }
        base.MessageAndRefreshParentByCurrHrefAndClose("员工保存成功！");
    }

    protected void gvBaseList_DataBound(object sender, EventArgs e)
    {
        GridView gvList = (GridView)sender;
        if (!(gvList.Rows.Count == 1 && (gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || gvList.DataKeys[0].Value == null)))
        {
            ImageButton ibtnEdit;
            Label lblFeeName;
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            String strAddSpace = String.Empty;
            Double sumValue = 0.00;
            for (Int32 i = 0; i < gvList.Rows.Count; i++)
            {
                String strFeeCode = gvList.DataKeys[i].Values[FeeInfoConst.FeeCode].ToString();
                String strFeeID = gvList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                lblFeeName = gvList.Rows[i].FindControl("lblFeeName") as Label;
                //缩进
                strAddSpace = SalaryAppAdapter.Instance.AddSpaceInFrontOfFeeName(strFeeCode);
                lblFeeName.Text = strAddSpace + gvList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                //gvList.Rows[i].Cells[0].Text = strAddSpace + gvList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                builder.AppendItem(FeeInfoDBConst.ParentID, strFeeID);
                if (FeeInfoAdapter.Instance.GetFeeInfoList(this.yearMonth, builder).Any())
                {
                    //gvList.Rows[i].Cells[1].Text = "";
                    ibtnEdit = gvList.Rows[i].FindControl("btnCommonEdit") as ImageButton;
                    ibtnEdit.Visible = false;
                    //ibtnAddChild = gvList.Rows[i].FindControl("ibtnSave") as ImageButton;
                    //ibtnAddChild.Visible = false;
                }
                builder.Clear();
                if (strAddSpace.Length == 0)
                {
                    sumValue += Double.Parse(gvList.DataKeys[i].Values[PersonBaseFeeInfoConst.FeeValue].ToString());
                }
            }
            gvList.FooterRow.Cells[1].Text = sumValue.ToString("n");//"#,##0.00"
        }
    }

    protected void gvBaseList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView gvList = (GridView)sender;
        gvList.EditIndex = -1;
        switch (gvList.ID)
        {
            case "gvCommonList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.CommonSalary);
                break;
            case "gvPositionList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Position);
                break;
            case "gvFloatList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Float);
                break;
            case "gvCooperateList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Cooperate);
                break;
            case "gvVirtualList" :
                this.BaseGridViewDataBind(gvList, CommonFeeType.Virtual);
                break;
            case "gvServiceList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Service);
                break;
        }
        //switch (gvList.DataKeyNames.Length)
        //{
        //    case 6:
        //        this.BaseGridViewDataBind(gvList, CommonFeeType.Cooperate);
        //        break;
        //    case 4:
        //        this.BaseGridViewDataBind(gvList, CommonFeeType.CommonSalary);
        //        break;
        //    case 5:
        //        this.BaseGridViewDataBind(gvList, CommonFeeType.Welfare);
        //        break;
        //    case 7:
        //        this.BaseGridViewDataBind(gvList, CommonFeeType.Provide);
        //        break;
        //    case 8:
        //        this.BaseGridViewDataBind(gvList,CommonFeeType.Service);
        //        break;
        //}
    }

    protected void gvBaseList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //tabSelected.Value = "1";
        GridView gvList = (GridView)sender;
        gvList.EditIndex = e.NewEditIndex;
        switch (gvList.ID)
        {
            case "gvCommonList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.CommonSalary);
                break;
            case "gvPositionList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Position);
                break;
            case "gvFloatList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Float);
                break;
            case "gvCooperateList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Cooperate);
                DropDownList ddlDepartment = (DropDownList)gvList.Rows[e.NewEditIndex].FindControl("ddlDepartment");
                DropDownList ddlProject = (DropDownList)gvList.Rows[e.NewEditIndex].FindControl("ddlProject");
                ddlDepartment.DataBound += new EventHandler(this.ddlDepartment_DataBound);
                List<DepartmentInfo> departList = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(null).Where(dp => !dp.UseFlag == bool.Parse(Status.True.ToString())).ToList();
                Tools.DropDownListDataBind(ddlDepartment, departList, "", true, DepartmentInfoConst.DepartmentName, DepartmentInfoConst.DepartmentID);
                //ddlDepartment.Items.Remove(ddlDepartment.Items.FindByValue(this.ddlDepartment.SelectedValue));
                List<ProjectInfo> projectList = ProjectInfoAdapter.Instance.GetProjectInfoList(null).Where(project => !project.UseFlag == Boolean.Parse(Status.True.ToString())).ToList();
                Tools.DropDownListDataBind(ddlProject, projectList, "", true, ProjectInfoConst.ProjectName, ProjectInfoConst.ProjectID);
                ddlDepartment.SelectedValue = gvList.DataKeys[e.NewEditIndex][PersonBaseFeeInfoConst.DepartmentID].ToString();
                ddlProject.SelectedValue = gvList.DataKeys[e.NewEditIndex][PersonBaseFeeInfoConst.ProjectID].ToString();
                break;
            case "gvVirtualList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Virtual);
                DropDownList ddlDepartmentV = (DropDownList)gvList.Rows[e.NewEditIndex].FindControl("ddlDepartment");
                DropDownList ddlProjectV = (DropDownList)gvList.Rows[e.NewEditIndex].FindControl("ddlProject");
                ddlDepartmentV.DataBound += new EventHandler(this.ddlDepartment_DataBound);
                List<DepartmentInfo> departListV = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(null).Where(dp => !dp.UseFlag == bool.Parse(Status.True.ToString())).ToList();
                Tools.DropDownListDataBind(ddlDepartmentV, departListV, "", true, DepartmentInfoConst.DepartmentName, DepartmentInfoConst.DepartmentID);
                //ddlDepartment.Items.Remove(ddlDepartment.Items.FindByValue(this.ddlDepartment.SelectedValue));
                List<ProjectInfo> projectListV = ProjectInfoAdapter.Instance.GetProjectInfoList(null).Where(project => !project.UseFlag == Boolean.Parse(Status.True.ToString())).ToList();
                Tools.DropDownListDataBind(ddlProjectV, projectListV, "", true, ProjectInfoConst.ProjectName, ProjectInfoConst.ProjectID);
                ddlDepartmentV.SelectedValue = gvList.DataKeys[e.NewEditIndex][PersonBaseFeeInfoConst.DepartmentID].ToString();
                ddlProjectV.SelectedValue = gvList.DataKeys[e.NewEditIndex][PersonBaseFeeInfoConst.ProjectID].ToString();
                break;
            case "gvServiceList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Service);
                break;
        }
        ////switch (gvList.DataKeyNames.Length)
        ////{
        ////    case 6:
        ////        this.BaseGridViewDataBind(gvList, CommonFeeType.Cooperate);
        ////        DropDownList ddlDepartment = (DropDownList)gvList.Rows[e.NewEditIndex].FindControl("ddlDepartment");
        ////        DropDownList ddlProject = (DropDownList)gvList.Rows[e.NewEditIndex].FindControl("ddlProject");
        ////        ddlDepartment.DataBound += new EventHandler(this.ddlDepartment_DataBound);
        ////        List<DepartmentInfo> departList = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(null).Where(dp => !dp.UseFlag == bool.Parse(Status.True.ToString())).ToList();
        ////        Tools.DropDownListDataBind(ddlDepartment, departList, "", true, DepartmentInfoConst.DepartmentName, DepartmentInfoConst.DepartmentID);
        ////        //ddlDepartment.Items.Remove(ddlDepartment.Items.FindByValue(this.ddlDepartment.SelectedValue));
        ////        List<ProjectInfo> projectList = ProjectInfoAdapter.Instance.GetProjectInfoList(null).Where(project => !project.UseFlag == Boolean.Parse(Status.True.ToString())).ToList();
        ////        Tools.DropDownListDataBind(ddlProject, projectList, "", true, ProjectInfoConst.ProjectName, ProjectInfoConst.ProjectID);
        ////        ddlDepartment.SelectedValue = gvList.DataKeys[e.NewEditIndex][PersonBaseFeeInfoConst.DepartmentID].ToString();
        ////        ddlProject.SelectedValue = gvList.DataKeys[e.NewEditIndex][PersonBaseFeeInfoConst.ProjectID].ToString();
        ////        break;
        ////    case 4:
        ////        this.BaseGridViewDataBind(gvList, CommonFeeType.CommonSalary);
        ////        break;
        ////    case 5:
        ////        this.BaseGridViewDataBind(gvList, CommonFeeType.Welfare);
        ////        break;
        ////    case 7:
        ////        this.BaseGridViewDataBind(gvList,CommonFeeType.Provide);
        ////        break;
        ////    case 8:
        ////        this.BaseGridViewDataBind(gvList, CommonFeeType.Service);
        ////        break;
        ////}
        //DropDownList ddl = (DropDownList)gv.Rows[e.NewEditIndex].FindControl("ddlJitiDaitan");
        //ddl.SelectedValue = gv.DataKeys[e.NewEditIndex]["JitiDaitan"].ToString();
    }

    protected void gvBaseList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView gvList = (GridView)sender;
        TextBox tbFeeValue = (TextBox)gvList.Rows[e.RowIndex].FindControl("tbFeeValue");

        string feeValue = tbFeeValue.Text.Trim().Equals(String.Empty) || tbFeeValue.Text.Trim().Equals(".00") ? "0" : tbFeeValue.Text.Trim();
        if (!StrHelper.IsValidDecimal(feeValue))
        {
            base.MessageBox("工资项目值必须为数字！");
            tbFeeValue.Focus();
            return;
        }
        ////Decimal oldFeeValue = Decimal.Parse(gv.DataKeys[e.RowIndex]["FeeValue"].ToString());
        Decimal newFeeValue = Decimal.Parse(feeValue.Trim());
        PersonBaseFeeInfo personBaseFeeInfo = String.IsNullOrEmpty(this.yearMonth) ? new PersonBaseFeeInfo() : new PersonBaseFeeMonthInfo(this.yearMonth);
        personBaseFeeInfo.PersonID = personID;
        personBaseFeeInfo.PersonName = this.tbPersonName.Text;
        personBaseFeeInfo.FeeValue = newFeeValue;
        personBaseFeeInfo.FeeID = gvList.DataKeys[e.RowIndex][PersonBaseFeeInfoConst.FeeID].ToString();
        personBaseFeeInfo.FeeName = gvList.DataKeys[e.RowIndex][PersonBaseFeeInfoConst.FeeName].ToString();

        if (gvList.ID == "gvCooperateList")
        {
            DropDownList ddlDepartment = (DropDownList)gvList.Rows[e.RowIndex].FindControl("ddlDepartment");
            DropDownList ddlProject = (DropDownList)gvList.Rows[e.RowIndex].FindControl("ddlProject");
            if (ddlDepartment.SelectedIndex == 0 && newFeeValue>0)
            {
                base.MessageBox("请选择相关部门！");
                ddlDepartment.Focus();
                return;
            }
            if (ddlProject.SelectedIndex == 0 && newFeeValue>0)
            {
                base.MessageBox("请选择相关项目！");
                ddlProject.Focus();
                return;
            }
            personBaseFeeInfo.DepartmentID = ddlDepartment.SelectedValue;
            personBaseFeeInfo.DepartmentName = ddlDepartment.SelectedIndex>0?ddlDepartment.SelectedItem.Text:String.Empty;
            personBaseFeeInfo.ProjectID = ddlProject.SelectedValue;
            personBaseFeeInfo.ProjectName = ddlProject.SelectedIndex > 0 ? ddlProject.SelectedItem.Text : String.Empty;
        }
        PersonBaseFeeInfoAdapter.Instance.UpdatePersonBaseFeeInfo(personBaseFeeInfo);
        //base.MessageBox("工资项目保存成功！");
        gvList.EditIndex = -1;
        switch (gvList.ID)
        {
            case "gvCommonList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.CommonSalary);
                break;
            case "gvPositionList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Position);
                break;
            case "gvFloatList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Float);
                break;
            case "gvCooperateList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Cooperate);
                break;
            case "gvVirtualList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Virtual);
                break;
            case "gvServiceList":
                this.BaseGridViewDataBind(gvList, CommonFeeType.Service);
                break;
        }
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (action == ActionType.Add)
        {
            this.tbPersonCode.Text = PersonInfoAdapter.Instance.CreatePersonCode(this.ddlDepartment.SelectedValue);
        }
    }

    protected void ddlBaseFee_DataBound(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        FeeInfo feeInfo = null;
        foreach (ListItem li in ddl.Items)
        {
            feeInfo = FeeInfoAdapter.Instance.LoadFeeInfo(this.yearMonth, li.Value);
            li.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfFeeName(feeInfo.FeeCode) + li.Text;
        }
    }

    protected void ddlDepartment_DataBound(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        DepartmentInfo departmentInfo = null;
        foreach (ListItem li in ddl.Items)
        {
            departmentInfo = DepartmentInfoAdapter.Instance.LoadDepartmentInfo(li.Value);
            li.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfDeptName(departmentInfo.DepartmentCode) + li.Text;
        }
    }

    protected void ddlProject_DataBound(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        ProjectInfo projectInfo = null;
        foreach (ListItem li in ddl.Items)
        {
            projectInfo = ProjectInfoAdapter.Instance.LoadProjectInfo(li.Value);
            li.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfProjectName(projectInfo.ProjectCode) + li.Text;
        }
    }

    #region 部门项目分摊
    /*
    private void gvDepartProjectGridViewDataBind(bool addNew)
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.PersonID, this.personID);
        List<PersonBaseFeeDepartmentProjectInfo> personBaseFeeDepartmentProjectInfoList = PersonBaseFeeDepartmentProjectInfoAdapter.Instance.GetPersonBaseFeeDepartmentProjectInfoList(this.yearMonth, builder);
        if (addNew)
        {
            PersonBaseFeeDepartmentProjectInfo personBaseFeeDepartmentProjectInfo = String.IsNullOrEmpty(this.yearMonth) ? new PersonBaseFeeDepartmentProjectInfo() : new PersonBaseFeeDepartmentProjectMonthInfo(this.yearMonth);
            personBaseFeeDepartmentProjectInfoList.Insert(0, personBaseFeeDepartmentProjectInfo);
            this.gvDepartProjectList.EditIndex = 0;
            GridViewControl.GridViewDataBind(this.gvDepartProjectList, personBaseFeeDepartmentProjectInfoList);
            this.CurRowEdit(0);
        }
        else
        {
            GridViewControl.GridViewDataBind(this.gvDepartProjectList, personBaseFeeDepartmentProjectInfoList);
        }

        //DataTable dt = PersonInfoAdapter.Instance.GetPersonBaseFeeDepartmentProjectListDT(yearMonth, personID);
        //if (addNew)
        //{
        //    DataRow dr = dt.NewRow();
        //    dt.Rows.InsertAt(dr, 0);
        //    this.gvDepartProjectList.EditIndex = 0;
        //    GridViewControl.GridViewDataBind(this.gvDepartProjectList, dt);
        //    this.CurRowEdit(0);
        //}
        //else
        //{
        //    GridViewControl.GridViewDataBind(this.gvDepartProjectList, dt);
        //}
    }


    protected void gvDepartProjectList_DataBound(object sender, EventArgs e)
    {
        if (!(this.gvDepartProjectList.Rows.Count == 1 && (this.gvDepartProjectList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvDepartProjectList.DataKeys[0].Value == null)))
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            String strAddSpace = String.Empty;
            Double sumValue = 0.00;
            for (Int32 i = 0; i < this.gvDepartProjectList.Rows.Count; i++)
            {
                sumValue += Double.Parse(this.gvDepartProjectList.DataKeys[i].Values[PersonBaseFeeDepartmentProjectInfoConst.StationMoney].ToString());
            }
            this.gvDepartProjectList.FooterRow.Cells[3].Text = sumValue.ToString("n");//"#,##0.00"
        }
    }

    protected void gvDepartProjectList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.gvDepartProjectList.EditIndex = -1;
        this.gvDepartProjectGridViewDataBind(false);
        ImageButton ibtnAdd = this.gvDepartProjectList.HeaderRow.FindControl("ibtnAddDepart") as ImageButton;
        ibtnAdd.Enabled = true;
    }

    protected void gvDepartProjectList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.gvDepartProjectList.EditIndex = e.NewEditIndex;
        this.gvDepartProjectGridViewDataBind(false);
        DropDownList ddlFee = (DropDownList)this.gvDepartProjectList.Rows[e.NewEditIndex].FindControl("ddlFee");
        DropDownList ddlDepartment = (DropDownList)this.gvDepartProjectList.Rows[e.NewEditIndex].FindControl("ddlDepartment");
        DropDownList ddlProject = (DropDownList)this.gvDepartProjectList.Rows[e.NewEditIndex].FindControl("ddlProject");
        ddlFee.DataBound += new EventHandler(this.ddlBaseFee_DataBound);
        ddlDepartment.DataBound += new EventHandler(this.ddlDepartment_DataBound);
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(FeeInfoDBConst.UseFlag, Status.True.ToString("D"));
        builder.AppendItem(FeeInfoDBConst.FeeType, (Int32)FeeType.Common);
        List<FeeInfo> feeList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder);
        Tools.DropDownListDataBind(ddlFee, feeList, "", true, FeeInfoConst.FeeName, FeeInfoConst.FeeID);
        List<DepartmentInfo> departList = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(null).Where(dp => !dp.UseFlag == bool.Parse(Status.True.ToString())).ToList();
        Tools.DropDownListDataBind(ddlDepartment, departList, "", true, DepartmentInfoConst.DepartmentName, DepartmentInfoConst.DepartmentID);
        //ddlDepartment.Items.Remove(ddlDepartment.Items.FindByValue(this.ddlDepartment.SelectedValue));
        List<ProjectInfo> projectList = ProjectInfoAdapter.Instance.GetProjectInfoList(null).Where(project => !project.UseFlag == Boolean.Parse(Status.True.ToString())).ToList();
        Tools.DropDownListDataBind(ddlProject, projectList, "", true, ProjectInfoConst.ProjectName, ProjectInfoConst.ProjectID);
        //ddlProject.Items.Remove(ddlProject.Items.FindByValue(this.ddlProject.SelectedValue));

        TextBox tbStationMoney = (TextBox)this.gvDepartProjectList.Rows[e.NewEditIndex].FindControl("tbStationMoney");

        String strPersonID = this.gvDepartProjectList.DataKeys[e.NewEditIndex][PersonBaseFeeDepartmentProjectInfoConst.PersonID].ToString();
        String strFeeID = this.gvDepartProjectList.DataKeys[e.NewEditIndex][PersonBaseFeeDepartmentProjectInfoConst.FeeID].ToString();
        String strDepartID = this.gvDepartProjectList.DataKeys[e.NewEditIndex][PersonBaseFeeDepartmentProjectInfoConst.DepartmentID].ToString();
        String strProjectID = this.gvDepartProjectList.DataKeys[e.NewEditIndex][PersonBaseFeeDepartmentProjectInfoConst.ProjectID].ToString();
        PersonBaseFeeDepartmentProjectInfo personBaseFeeDepartmentProjectInfo = PersonBaseFeeDepartmentProjectInfoAdapter.Instance.LoadPersonBaseFeeDepartmentProjectInfo(yearMonth, strFeeID, strPersonID, strDepartID, strProjectID);
        ddlDepartment.SelectedValue = personBaseFeeDepartmentProjectInfo.DepartmentID;
        ddlProject.SelectedValue = personBaseFeeDepartmentProjectInfo.ProjectID;
        ddlFee.SelectedValue = personBaseFeeDepartmentProjectInfo.FeeID;
        tbStationMoney.Text = personBaseFeeDepartmentProjectInfo.StationMoney.ToString();

        ImageButton ibtnAdd = this.gvDepartProjectList.HeaderRow.FindControl("ibtnAddDepart") as ImageButton;
        ibtnAdd.Enabled = false;
    }

    protected void gvDepartProjectList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DropDownList ddlFee = (DropDownList)this.gvDepartProjectList.Rows[e.RowIndex].FindControl("ddlFee");
        DropDownList ddlDepartment = (DropDownList)this.gvDepartProjectList.Rows[e.RowIndex].FindControl("ddlDepartment");
        DropDownList ddlProject = (DropDownList)this.gvDepartProjectList.Rows[e.RowIndex].FindControl("ddlProject");
        TextBox tbStationMoney = (TextBox)this.gvDepartProjectList.Rows[e.RowIndex].FindControl("tbStationMoney");
        if (ddlFee.SelectedIndex <= 0)
        {
            base.MessageBox("请选择分摊的工资项目！");
            ddlFee.Focus();
            return;
        }
        if (ddlDepartment.SelectedIndex <= 0)
        {
            base.MessageBox("请选择分摊的部门！");
            ddlDepartment.Focus();
            return;
        }
        if (ddlProject.SelectedIndex <= 0)
        {
            base.MessageBox("请选择分摊的项目！");
            ddlProject.Focus();
            return;
        }
        String strStationMoney = tbStationMoney.Text.Trim();
        if (strStationMoney.Length <= 0)
        {
            base.MessageBox("请输入分摊的金额！");
            tbStationMoney.Focus();
            return;
        }
        Func<PersonBaseFeeInfo, Boolean> filter = delegate(PersonBaseFeeInfo pbf) { return pbf.FeeID == ddlFee.SelectedValue; };//pbf.PersonID == this.personID && pbf.FeeID == ddlFee.SelectedValue;
        PersonBaseFeeInfo personBaseFeeInfo = PersonBaseFeeInfoAdapter.Instance.GetPersonBaseFeeInfoList(PersonBaseFeeTarget.PersonBaseFee, this.yearMonth, this.personID)
            .Where(filter).FirstOrDefault();
        DataKey strKeyValue = this.gvDepartProjectList.DataKeys[e.RowIndex].Value == null ? null : this.gvDepartProjectList.DataKeys[e.RowIndex];
        Decimal calculateValue = PersonBaseFeeInfoAdapter.Instance.CalculatePersonBaseFeeDepartmentProjectInfo(yearMonth, personID, ddlFee.SelectedValue);
        PersonBaseFeeDepartmentProjectInfo personBaseFeeDepartmentProjectInfo 
            = PersonBaseFeeDepartmentProjectInfoAdapter.Instance.LoadPersonBaseFeeDepartmentProjectInfo(
            yearMonth, ddlFee.SelectedValue, personID, ddlDepartment.SelectedValue, ddlProject.SelectedValue);
        if ((strKeyValue == null && personBaseFeeDepartmentProjectInfo != null) 
                || (strKeyValue != null && personBaseFeeDepartmentProjectInfo != null
                    && personBaseFeeDepartmentProjectInfo.PersonID != strKeyValue[0] && personBaseFeeDepartmentProjectInfo.FeeID != strKeyValue[1] 
                    && personBaseFeeDepartmentProjectInfo.DepartmentID != strKeyValue[2] && personBaseFeeDepartmentProjectInfo.ProjectID != strKeyValue[3]))
        {
            base.MessageBox("该部门和项目已分摊该项工资项目，请选择其它部门或项目！");
            ddlDepartment.Focus();
            return;
        }//Not
        if (personBaseFeeInfo.FeeValue < (Decimal.Parse(strStationMoney) + calculateValue))
        {
            base.MessageBox("该工资项目的分摊金额已大于工资项目金额，请调整分摊金额！");
            tbStationMoney.Focus();
            return;
        }
        personBaseFeeDepartmentProjectInfo = personBaseFeeDepartmentProjectInfo 
            == null ? String.IsNullOrEmpty(this.yearMonth) ? new PersonBaseFeeDepartmentProjectInfo() : new PersonBaseFeeDepartmentProjectMonthInfo(this.yearMonth) : personBaseFeeDepartmentProjectInfo;
        personBaseFeeDepartmentProjectInfo.DepartmentID = ddlDepartment.SelectedValue;
        String nm = ddlDepartment.SelectedItem.Text.Replace("　", "").Trim();
        personBaseFeeDepartmentProjectInfo.DepartmentName = nm.StartsWith("-") ? nm.Substring(1).Trim() : nm;
        personBaseFeeDepartmentProjectInfo.ProjectID = ddlProject.SelectedValue;
        nm = ddlProject.SelectedItem.Text.Replace("　", "").Trim();
        personBaseFeeDepartmentProjectInfo.ProjectName = nm.StartsWith("-") ? nm.Substring(1).Trim() : nm;

        personBaseFeeDepartmentProjectInfo.FeeID = ddlFee.SelectedValue;
        personBaseFeeDepartmentProjectInfo.FeeName = ddlFee.SelectedItem.Text;
        personBaseFeeDepartmentProjectInfo.PersonID = personID;
        personBaseFeeDepartmentProjectInfo.PersonName = tbPersonName.Text.Trim();
        personBaseFeeDepartmentProjectInfo.StationMoney = Decimal.Parse(strStationMoney);
        if (strKeyValue != null)
        {
            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.PersonID, strKeyValue[0]);
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.FeeID, strKeyValue[1]);
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.DepartmentID, strKeyValue[2]);
            builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.ProjectID, strKeyValue[3]);
            PersonBaseFeeDepartmentProjectInfoAdapter.Instance.DeletePersonBaseFeeDepartmentProjectInfo(yearMonth, builder);
            //personBaseFeeDepartmentProjectInfo.PbfdpId = Guid.NewGuid().ToString();//String.Concat(personBaseFeeDepartment.PersonId, personBaseFeeDepartment.FeeCode, personBaseFeeDepartment.DepartmentId);
        }
        PersonBaseFeeDepartmentProjectInfoAdapter.Instance.InsertPersonBaseFeeDepartmentProjectInfo(personBaseFeeDepartmentProjectInfo);
        //base.MessageBox("部门分摊成功！");
        this.gvDepartProjectList.EditIndex = -1;
        this.gvDepartProjectGridViewDataBind(false);
    }

    private bool IsSharedDepartment(String feeID, String departId, String personId)
    {
        return false;
    }

    protected void ibtnAddDepart_Click(object sender, EventArgs e)
    {
        this.gvDepartProjectGridViewDataBind(true);//this.btnAdd.Enabled = false;
        ImageButton ibtnAddDepart = this.gvDepartProjectList.HeaderRow.FindControl("ibtnAddDepart") as ImageButton;
        ibtnAddDepart.Enabled = false;
    }
    protected void lbtnDeleteDepart_Click(object sender, EventArgs e)
    {
        String[] strIDs = (sender as ImageButton).CommandArgument.Split('|');
        WhereSqlClauseBuilder builder=new WhereSqlClauseBuilder();
        builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.PersonID, strIDs[0]);
        builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.FeeID, strIDs[1]);
        builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.DepartmentID, strIDs[2]);
        builder.AppendItem(PersonBaseFeeDepartmentProjectInfoDBConst.ProjectID, strIDs[3]);
        PersonBaseFeeDepartmentProjectInfoAdapter.Instance.DeletePersonBaseFeeDepartmentProjectInfo(yearMonth, builder);
        this.gvDepartProjectGridViewDataBind(false);
    }

    private void CurRowEdit(int rowIndex)
    {
        DropDownList ddlFee = (DropDownList)this.gvDepartProjectList.Rows[rowIndex].FindControl("ddlFee");
        DropDownList ddlDepartment = (DropDownList)this.gvDepartProjectList.Rows[rowIndex].FindControl("ddlDepartment");
        DropDownList ddlProject = (DropDownList)this.gvDepartProjectList.Rows[rowIndex].FindControl("ddlProject");
        ddlFee.DataBound += new EventHandler(this.ddlBaseFee_DataBound);
        ddlDepartment.DataBound += new EventHandler(this.ddlDepartment_DataBound);
        //ddlProject.DataBound += new EventHandler(this.ddlProject_DataBound);
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(FeeInfoDBConst.UseFlag, Status.True.ToString("D"));
        builder.AppendItem(FeeInfoDBConst.FeeType, (Int32)FeeType.Common);
        List<FeeInfo> feeList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder);
        Tools.DropDownListDataBind(ddlFee, feeList, "", true, FeeInfoConst.FeeName, FeeInfoConst.FeeID);
        List<DepartmentInfo> departList = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(null).Where(dp => !dp.UseFlag == bool.Parse(Status.True.ToString())).ToList();
        Tools.DropDownListDataBind(ddlDepartment, departList, "", true, DepartmentInfoConst.DepartmentName, DepartmentInfoConst.DepartmentID);
        //ddlDepartment.Items.Remove(ddlDepartment.Items.FindByValue(this.ddlDepartment.SelectedValue));
        List<ProjectInfo> projectList = ProjectInfoAdapter.Instance.GetProjectInfoList(null).Where(project => !project.UseFlag == Boolean.Parse(Status.True.ToString())).ToList();
        Tools.DropDownListDataBind(ddlProject, projectList, "", true, ProjectInfoConst.ProjectName, ProjectInfoConst.ProjectID);
        //ddlProject.Items.Remove(ddlProject.Items.FindByValue(this.ddlProject.SelectedValue));

        //TextBox tbStationMoney = (TextBox)this.gvDepartProjectList.Rows[rowIndex].FindControl("tbStationMoney");
    }
     * */
    #endregion 部门项目分摊

    #region 合作工资

    #endregion  合作工资
}