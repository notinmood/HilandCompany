using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Salary.Web.BasePage;
using Salary.Biz.Eunm;
using Salary.Biz;
using Salary.Core.Data;
using Salary.Core.Utility;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Web.Utility;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;


public partial class UI_Fee_FeeEdit : BasePage
{
    public String yearMonth, name = "工资";
    protected String feeID, parentID;
    protected FeeType enumFeeType = new FeeType();
    protected CommonFeeType enumCommonFeeType = new CommonFeeType();
    protected ActionType action;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.yearMonth = DecodedQueryString[FeeMonthInfoConst.YearMonth];
        this.feeID = DecodedQueryString[FeeInfoConst.FeeID];
        this.parentID = DecodedQueryString[FeeInfoConst.ParentID];
        this.action = EnumHelper.Parse<ActionType>(DecodedQueryString[SalaryConst.QueryAction]);
        String feeType = DecodedQueryString[FeeInfoConst.FeeType], commonFeeType = DecodedQueryString[FeeInfoConst.CommonFeeType];
        if (!String.IsNullOrEmpty(feeType))
        {
            this.enumFeeType = EnumHelper.Parse<FeeType>(feeType);
            this.name = this.enumFeeType.Equals(FeeType.Common) ? "工资组成" : this.enumFeeType.Equals(FeeType.Sum) 
                ? "工资计算" : this.enumFeeType.Equals(FeeType.Parameter) ? "参数" : this.enumFeeType.Equals(FeeType.Tax) ? "个税" : this.name;
        }
        if (!String.IsNullOrEmpty(commonFeeType))
        {
            this.enumCommonFeeType = EnumHelper.Parse<CommonFeeType>(commonFeeType);
        }
        if (!IsPostBack)
        {
            this.InitializeControl();
        }
        else
        {
            GridViewControl.ResetGridView(this.gvTaxList);
        }
    }

    private void InitializeControl()
    {
        FeeInfo feeInfo = null;////Tools.DropDownListDataBindByEnum(this.ddlFeeType, typeof(FeeType), string.Empty, true, true);
        Tools.DropDownListDataBindByEnum(this.ddlCalculateSign, typeof(CalculateSign), string.Empty, true, true);
        this.InitializeFeeDropDownList();
        this.InitializeTaxTargetFeeDropDownList();
        if (action != ActionType.Add)
        {
            feeInfo = FeeInfoAdapter.Instance.LoadFeeInfo(yearMonth, feeID);
            this.parentID = feeInfo.ParentID;
            this.enumFeeType = feeInfo.FeeType;
            this.tbFeeCode.Text = feeInfo.FeeCode;
            this.tbFeeName.Text = feeInfo.FeeName;
            this.tbDefaultValue.Text = feeInfo.DefaultValue.ToString("#.00");
            this.tbStartDate.Text = base.ShowCorrectDateTime(feeInfo.StartDate.ToString());
            this.tbTaxBaseValue.Text = feeInfo.TaxBaseValue.ToString();
            this.tbCalculateExp.Text = FeeInfoAdapter.Instance.ConvertFeeCodeWithFeeNameInExp(yearMonth, feeInfo.CalculateExp);
            this.ddlFee.SelectedValue = String.IsNullOrEmpty(feeInfo.ParentID) ? "" : FeeInfoAdapter.Instance.LoadFeeInfo(this.yearMonth, feeInfo.ParentID).FeeCode;
            this.ddlCalculateSign.SelectedValue = feeInfo.CalculateSign;////this.ddlFeeType.SelectedValue = ((Int32)feeInfo.FeeType).ToString();
            switch (feeInfo.FeeType)
            {
                case FeeType.Tax:
                    //this.ddlTaxTargetFee.SelectedValue = FeeInfoAdapter.Instance.LoadFeeInfo(this.yearMonth, feeInfo.TaxTargetFeeID).FeeCode;
                    this.TaxGridViewDataBind(false);
                    break;
                case FeeType.Sum:
                    this.InitializeCalculateExpFeeListBox();
                    break;
            }
        }
        else
        {
            if ((this.enumFeeType.Equals(FeeType.Parameter) || this.enumFeeType.Equals(FeeType.Tax)) && !String.IsNullOrEmpty(this.parentID))
            {
                this.tbFeeName.Text = FeeInfoAdapter.Instance.LoadFeeInfo(this.yearMonth, this.parentID).FeeName;
            }////this.ddlFeeType.SelectedValue = this.enumFeeType.Equals(0) ? "" : this.enumFeeType.ToString("D");
            this.ddlFee.SelectedValue = String.IsNullOrEmpty(parentID) ? "" : FeeInfoAdapter.Instance.LoadFeeInfo(this.yearMonth, this.parentID).FeeCode;
            this.tbFeeCode.Text = FeeInfoAdapter.Instance.CreateFeeCode(this.enumFeeType.ToString("D"), parentID);
        }
        this.ddlFeeType_SelectedIndexChanged(null, null);
    }

    protected void ddlFeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ddlFeeType.Visible = false;
        this.tbFeeName.Enabled = !((this.enumFeeType.Equals(FeeType.Parameter) || this.enumFeeType.Equals(FeeType.Tax)) && !String.IsNullOrEmpty(this.parentID));
        if (String.IsNullOrEmpty(this.parentID))
        {
            this.lblParent.Visible = false;
            this.ddlFee.Visible = false;
            this.trCalculateSign.Style.Add("display", "none");
        }
        switch (this.enumFeeType)//EnumHelper.Parse<FeeType>(this.ddlFeeType.SelectedValue)
        {
            case FeeType.Common:
                //this.trDefaultValue.Style.Add("display", "none");
                this.trTax.Style.Add("display", "none");
                this.trTaxRate.Style.Add("display", "none");
                this.trCalculateExp.Style.Add("display", "none");
                this.trlitbFee.Style.Add("display", "none");
                break;
            case FeeType.Sum:
                this.InitializeCalculateExpFeeListBox();
                this.trDefaultValue.Style.Add("display", "none");
                this.trTax.Style.Add("display", "none");
                this.trTaxRate.Style.Add("display", "none");
                break;
            case FeeType.Tax:
                this.tbFeeName.Enabled = String.IsNullOrEmpty(parentID) && !(this.ddlFee.SelectedIndex>0);
                this.lblParent.Visible = false;
                this.ddlFee.Visible = false;
                this.trDefaultValue.Style.Add("display", "none");
                this.trCalculateSign.Style.Add("display", "none");
                this.trCalculateExp.Style.Add("display", "none");
                this.trlitbFee.Style.Add("display", "none");
                this.trTax.Style.Add("display", "block");
                this.trTaxRate.Style.Add("display", "block");
                this.TaxGridViewDataBind(false);
                if (this.feeID == SalaryConst.PersonalIncomeTaxID || this.parentID == SalaryConst.PersonalIncomeTaxID)
                {
                    this.gvTaxList.Columns[3].Visible = false;
                    this.gvTaxList.Columns[4].Visible = false;
                }
                if (this.feeID == SalaryConst.ServiceFeeTaxID || this.parentID == SalaryConst.ServiceFeeTaxID)
                {
                    this.lblTaxBaseValue.Style.Add("display", "none");
                    this.lblTaxBaseValueEnd.Style.Add("display", "none");
                    this.tbTaxBaseValue.Style.Add("display", "none");
                    this.tbTaxBaseValue.Text = decimal.Zero.ToString();
                }
                break;
            case FeeType.Parameter:
                this.tbFeeName.Enabled = String.IsNullOrEmpty(parentID) && !(this.ddlFee.SelectedIndex > 0);
                this.lblParent.Visible = false;
                this.ddlFee.Visible = false;
                this.trCalculateSign.Style.Add("display", "none");
                this.trCalculateExp.Style.Add("display", "none");
                this.trlitbFee.Style.Add("display", "none");
                this.trTax.Style.Add("display", "none");
                this.trTaxRate.Style.Add("display", "none");
                break;
            default:
                this.trDefaultValue.Style.Add("display", "block");
                this.trCalculateExp.Style.Add("display", "none");
                this.trlitbFee.Style.Add("display", "none");
                this.trTax.Style.Add("display", "none");
                this.trTaxRate.Style.Add("display", "none");
                break;
        }
    }

    protected void ddlFee_DataBound(object sender, EventArgs e)
    {
        foreach (ListItem li in this.ddlFee.Items)
        {//FeeInfo feeInfo = FeeInfoAdapter.Instance.LoadFeeInfo(yearMonth,li.Value);//feeInfo.FeeCode
            li.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfFeeName(li.Value) + li.Text;
        }
    }

    protected void ddlTaxTargetFee_DataBound(object sender, EventArgs e)
    {//FeeInfo feeInfo = null;
        foreach (ListItem li in this.ddlTaxTargetFee.Items)
        {//feeInfo = FeeInfoAdapter.Instance.LoadFeeInfo(this.yearMonth, li.Value);//feeInfo.FeeCode
            li.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfFeeName(li.Value) + li.Text;
        }
    }

    private void InitializeFeeDropDownList()
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(FeeInfoDBConst.FeeType, this.enumFeeType.ToString("D"));
        List<FeeInfo> feeList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder);
        this.ddlFee.DataSource = feeList;
        this.ddlFee.DataTextField = FeeInfoConst.FeeName;
        this.ddlFee.DataValueField = FeeInfoConst.FeeCode;
        this.ddlFee.DataBind();
        this.ddlFee.Items.Insert(0, new ListItem("----请选择----", ""));
    }

    private void InitializeTaxTargetFeeDropDownList()
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(FeeInfoDBConst.FeeType, FeeType.Sum.ToString("D"));
        builder.AppendItem(FeeInfoDBConst.UseFlag, false);
        this.ddlTaxTargetFee.DataTextField = FeeInfoConst.FeeName;
        this.ddlTaxTargetFee.DataValueField = FeeInfoConst.FeeCode;
        List<FeeInfo> feeList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder);//.GetFeeTreeInfoList(yearMonth, builder);
        this.ddlTaxTargetFee.DataSource = feeList.OrderBy(feeInfo => feeInfo.FeeCode);
        this.ddlTaxTargetFee.DataBind();
        this.ddlTaxTargetFee.Items.Insert(0, new ListItem("----请选择----", string.Empty));
    }

    private void InitializeCalculateExpFeeListBox()
    {
        List<FeeInfo> feeList = new List<FeeInfo>(), feeListNew = new List<FeeInfo>();
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        if (action != ActionType.Add)
        {//if (this.ddlFee.SelectedIndex > 0){} //为完成：如果打开后又选了重新选择了父项目或改了项目编码怎么办？
            builder.AppendItem(FeeInfoDBConst.FeeID, feeID, "<>");
            builder.AppendItem(FeeInfoDBConst.ParentID, feeID, "<>");
            builder.AppendItem(FeeInfoDBConst.CalculateExp, String.Concat(@"%\[", feeID, @"\]%##ESCAPE##"), " NOT LIKE ");
        }
        builder.AppendItem(FeeInfoDBConst.FeeType, FeeType.Sum.ToString("D"));//计算型
        feeList = this.ReCreateFeeID(FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder).Where(fee => !fee.UseFlag == bool.Parse(Status.True.ToString())).ToList());
        Tools.ListBoxDataBind(this.litbCalculateFee, feeList, FeeInfoConst.FeeName, FeeInfoConst.FeeID);
        builder.Clear();
        builder.AppendItem(FeeInfoDBConst.FeeType, FeeType.Common.ToString("D"));//组成型
        feeList = this.ReCreateFeeID(FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder).Where(fee => !fee.UseFlag == bool.Parse(Status.True.ToString())).ToList());
        Tools.ListBoxDataBind(this.litbCommonFee, feeList, FeeInfoConst.FeeName, FeeInfoConst.FeeID);
        builder.Clear();
        builder.AppendItem(FeeInfoDBConst.FeeType, FeeType.Parameter.ToString("D"));//参数型
        feeList = this.ReCreateFeeID(FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder).Where(parent => (!parent.UseFlag == bool.Parse(Status.True.ToString())) && (parent.ParentID == "" || parent.ParentID == null)).ToList());
        Tools.ListBoxDataBind(this.litbParameterFee, feeList, FeeInfoConst.FeeName, FeeInfoConst.FeeID);
        builder.Clear();
        builder.AppendItem(FeeInfoDBConst.FeeType, FeeType.Tax.ToString("D"));//个税型
        feeList = this.ReCreateFeeID(FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder).Where(parent => (!parent.UseFlag == bool.Parse(Status.True.ToString())) && (parent.ParentID == "" || parent.ParentID == null)).ToList());
        Tools.ListBoxDataBind(this.litbTaxFee, feeList, FeeInfoConst.FeeName, FeeInfoConst.FeeID);

        Tools.ListBoxDataBindByEnum(this.litbFunction, typeof(FeeFunction), false);//函数
    }

    private List<FeeInfo> ReCreateFeeID(List<FeeInfo> feeInfoList)
    {
        List<FeeInfo> feeListNew = new List<FeeInfo>();
        String strFeeNameOld = String.Empty;
        foreach (FeeInfo feeInfo in feeInfoList)
        {
            strFeeNameOld = feeInfo.FeeName;
            feeInfo.FeeName = SalaryAppAdapter.Instance.AddSpaceInFrontOfFeeName(feeInfo.FeeCode) + feeInfo.FeeName;
            feeInfo.FeeID = feeInfo.ParentID.Length > 0 ? String.Concat("{", feeInfo.ParentName, "}", strFeeNameOld) : strFeeNameOld;
            feeListNew.Add(feeInfo);
        }
        return feeListNew;
    }

    #region 项目

    private FeeInfo GetInfoFromPageControl()
    {
        FeeInfo info = null;
        switch (action)
        {
            case ActionType.Add:
                info = String.IsNullOrEmpty(yearMonth) ? new FeeInfo() : new FeeMonthInfo(yearMonth);
                info.FeeID = CommonTools.Instance.GetMaxOrderNo(String.IsNullOrEmpty(yearMonth) ? FeeInfoDBConst.TableName : ORMapping.GetMappingInfo<FeeMonthInfo>().TableName, FeeInfoDBConst.FeeID).ToString();
                info.CommonFeeType = this.enumCommonFeeType;
                break;
            case ActionType.Edit:
                info = FeeInfoAdapter.Instance.LoadFeeInfo(yearMonth, feeID);
                break;
        }
        info.FeeCode = this.tbFeeCode.Text.Trim();
        info.FeeName = this.tbFeeName.Text.Trim();
        if (this.ddlFee.SelectedIndex > 0)
        {
            info.ParentID = FeeInfoAdapter.Instance.LoadFeeInfoByCode(this.yearMonth, this.ddlFee.SelectedValue).FeeID;
            String nm = this.ddlFee.SelectedItem.Text.Replace("　", "").Trim();
            info.ParentName = nm.StartsWith("-") ? nm.Substring(1).Trim() : nm;
        }
        info.FeeType = this.enumFeeType;
        info.CalculateSign = this.ddlCalculateSign.SelectedIndex > 0 ? this.ddlCalculateSign.SelectedValue : null;
        String txt = String.Empty;
        if (info.FeeType.Equals(FeeType.Tax))
        {
            //info.CalculateExp = String.Concat("[", FeeInfoAdapter.Instance.LoadFeeInfoByCode(this.yearMonth, this.ddlTaxTargetFee.SelectedValue).FeeID, "]");
            txt = this.tbTaxBaseValue.Text;
            info.TaxBaseValue = String.IsNullOrEmpty(txt) ? 0 : Decimal.Parse(txt);
            //info.TaxTargetFeeID = this.ddlTaxTargetFee.SelectedIndex > 0 ? FeeInfoAdapter.Instance.LoadFeeInfoByCode(this.yearMonth, this.ddlTaxTargetFee.SelectedValue).FeeID : "";
            info.StartDate = DateTime.Parse(this.tbStartDate.Text);
        }
        else if (info.FeeType.Equals(FeeType.Sum))
        {
            info.CalculateExp = FeeInfoAdapter.Instance.ConvertFeeNameWithFeeCodeInExp(yearMonth, this.tbCalculateExp.Text);
        }
        else if (info.FeeType.Equals(FeeType.Common))
        {
            info.CalculateExp = FeeInfoAdapter.Instance.ConvertFeeNameWithFeeCodeInExp(yearMonth, this.tbCalculateExp.Text);//string.Empty;
            ////info.TaxTargetFeeID = this.ddlTargetParameter.SelectedIndex > 0 ? this.ddlTargetParameter.SelectedValue : "";
        }
        else if(info.FeeType.Equals(FeeType.Parameter))
        {
            info.StartDate = DateTime.Parse(this.tbStartDate.Text);
        }
        txt =this.tbDefaultValue.Text;
        info.DefaultValue = String.IsNullOrEmpty(txt) ? 0 : Decimal.Parse(txt);
        return info;
    }

    protected void btnSaveClick(Object sender, EventArgs e)
    {
        FeeInfo info = this.GetInfoFromPageControl();
        if (FeeInfoAdapter.Instance.IsFeeCodeUsed(info, action == ActionType.Add))
        {
            base.MessageBox(this.name + "编码冲突，请重新定义！");
            this.tbFeeCode.Focus();
            return;
        }
        if (action == ActionType.Add)
        {
            FeeInfoAdapter.Instance.InsertFeeInfo(info);
            UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.FeeEditUrl);
            if (!String.IsNullOrEmpty(yearMonth))
            {
                urlEdit.AppendItem(FeeMonthInfoConst.YearMonth, yearMonth);
            }
            urlEdit.AppendItem(FeeInfoConst.FeeID, info.FeeID);
            urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
            Redirect(urlEdit.ToUrlString());
        }
        else
        {
            FeeInfoAdapter.Instance.UpdateFeeInfo(info);
        }
        if (info.FeeType.Equals(FeeType.Tax))
        {
            base.MessageAndRefreshParentByCurrHref(this.name + "保存成功！");
        }
        else
        {
            base.MessageAndRefreshParentByCurrHrefAndClose(this.name + "保存成功！");
        }
    }

    #endregion 项目

    #region  税率

    private void TaxGridViewDataBind(bool addNew)
    {
        List<TaxInfo> taxInfoList = null;
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(TaxInfoDBConst.FeeID, feeID);
        taxInfoList = TaxInfoAdapter.Instance.GetTaxInfoList(yearMonth, builder);
        if (addNew)
        {
            TaxInfo taxInfo = String.IsNullOrEmpty(yearMonth) ? new TaxInfo() : new TaxMonthInfo(yearMonth);
            taxInfo.TaxID = "";
            taxInfo.FeeID = feeID;
            taxInfoList.Insert(0, taxInfo);
            this.gvTaxList.EditIndex = 0;
        }
        GridViewControl.GridViewDataBind<TaxInfo>(this.gvTaxList, taxInfoList);
        if (action == ActionType.Add)
        {//ImageButton ibtnAdd = this.gvTaxList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
            //ibtnAdd.Enabled = false;
            this.gvTaxList.Enabled = false;
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        TaxInfoAdapter.Instance.DeleteTaxInfo(yearMonth, (sender as ImageButton).CommandArgument);//base.MessageBox("个税税率删除成功！");
        this.TaxGridViewDataBind(false);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.TaxGridViewDataBind(true);//this.btnAdd.Enabled = false;
        ImageButton ibtnAdd = this.gvTaxList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        ibtnAdd.Enabled = false;
    }

    protected void gvTaxList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.gvTaxList.EditIndex = -1;
        this.TaxGridViewDataBind(false);//this.btnAdd.Enabled = true;
        ImageButton ibtnAdd = this.gvTaxList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        ibtnAdd.Enabled = true;
    }

    protected void gvTaxList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.gvTaxList.EditIndex = e.NewEditIndex;
        this.TaxGridViewDataBind(false);//this.btnAdd.Enabled = false;
        ImageButton ibtnAdd = this.gvTaxList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        ibtnAdd.Enabled = false;
    }

    protected void gvTaxList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox tbQuantumStart = (TextBox)this.gvTaxList.Rows[e.RowIndex].FindControl("tbQuantumStart");
        TextBox tbQuantumEnd = (TextBox)this.gvTaxList.Rows[e.RowIndex].FindControl("tbQuantumEnd");
        TextBox tbRate = (TextBox)this.gvTaxList.Rows[e.RowIndex].FindControl("tbRate");
        TextBox tbSubtract = (TextBox)this.gvTaxList.Rows[e.RowIndex].FindControl("tbSubtract");
        TextBox tbSubtractMultiple = (TextBox)this.gvTaxList.Rows[e.RowIndex].FindControl("tbSubtractMultiple");
        TextBox tbSubtractMoney = (TextBox)this.gvTaxList.Rows[e.RowIndex].FindControl("tbSubtractMoney");

        string quantumStart = tbQuantumStart.Text, quantumEnd = tbQuantumEnd.Text, rate = tbRate.Text
            , subtract = tbSubtract.Text, subtractMultiple = tbSubtractMultiple.Text, subtractMoney = tbSubtractMoney.Text;
        if (string.IsNullOrEmpty(quantumStart.Trim()) || !StrHelper.IsValidDecimal(quantumStart))
        {
            base.MessageBox("应税额必须为数字！");
            tbQuantumStart.Focus();
            return;
        }
        if (string.IsNullOrEmpty(quantumEnd.Trim()) || !StrHelper.IsValidDecimal(quantumEnd))
        {
            base.MessageBox("应税额必须为数字！");
            tbQuantumEnd.Focus();
            return;
        }
        if (string.IsNullOrEmpty(rate.Trim()) || !StrHelper.IsValidDecimal(rate))
        {
            base.MessageBox("税率必须为数字！");
            tbRate.Focus();
            return;
        }
        if (string.IsNullOrEmpty(subtract.Trim()) || !StrHelper.IsValidDecimal(subtract))
        {
            base.MessageBox("速算扣除数必须为数字！");
            tbSubtract.Focus();
            return;
        }
        if (string.IsNullOrEmpty(subtractMultiple.Trim()) || !StrHelper.IsValidDecimal(subtractMultiple))
        {
            base.MessageBox("速算扣除数倍数必须为数字！");
            tbSubtractMultiple.Focus();
            return;
        }
        if (string.IsNullOrEmpty(subtractMoney.Trim()) || !StrHelper.IsValidDecimal(subtractMoney))
        {
            base.MessageBox("扣除金额必须为数字！");
            tbSubtractMoney.Focus();
            return;
        }
        TaxInfo taxInfo;
        string taxID = this.gvTaxList.DataKeys[e.RowIndex][TaxInfoConst.TaxID].ToString();
        if (taxID == "")
        {
            taxInfo = String.IsNullOrEmpty(yearMonth) ? new TaxInfo() : new TaxMonthInfo(yearMonth);
            taxInfo.TaxID = Guid.NewGuid().ToString();
            taxInfo.FeeID = feeID;
            taxInfo.FeeName = this.tbFeeName.Text;
        }
        else
        {
            taxInfo = TaxInfoAdapter.Instance.LoadTaxInfo(yearMonth, taxID);
        }
        taxInfo.QuantumStart = Decimal.Parse(quantumStart);
        taxInfo.QuantumEnd = Decimal.Parse(quantumEnd);
        taxInfo.Rate =  Decimal.Parse(rate);
        taxInfo.Subtract = Decimal.Parse(subtract);
        taxInfo.SubtractMultiple = Decimal.Parse(subtractMultiple);
        taxInfo.SubtractMoney = Decimal.Parse(subtractMoney);
        if (taxID == "")
        {
            TaxInfoAdapter.Instance.InsertTaxInfo(taxInfo);
        }
        else
        {
            TaxInfoAdapter.Instance.UpdateTaxInfo(taxInfo);
        }
        base.MessageBox("税率保存成功！");
        this.gvTaxList.EditIndex = -1;
        this.TaxGridViewDataBind(false);//this.btnAdd.Enabled = true;
        ImageButton ibtnAdd = this.gvTaxList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        ibtnAdd.Enabled = true;
    }

    #endregion 税率

    protected void btnCalculateUp_Click(object sender, EventArgs e)
    {
        if (this.litbCalculateFee.SelectedIndex >= 0)
        {
            this.tbCalculateExp.Text += String.Concat(" [", this.litbCalculateFee.SelectedValue, "]");
        }
        else
        {
            base.MessageBox("请先选择要添加的“可参与运算的工资项目！”");
        }
    }
    protected void btnCommonUp_Click(object sender, EventArgs e)
    {
        if (this.litbCommonFee.SelectedIndex >= 0)
        {
            this.tbCalculateExp.Text += String.Concat(" [", this.litbCommonFee.SelectedValue, "]");
        }
        else
        {
            base.MessageBox("请先选择要添加的“可参与运算的工资项目！”");
        }
    }
    protected void btnParameterUp_Click(object sender, EventArgs e)
    {
        if (this.litbParameterFee.SelectedIndex >= 0)
        {
            this.tbCalculateExp.Text += String.Concat(" [", this.litbParameterFee.SelectedValue, "]");
        }
        else
        {
            base.MessageBox("请先选择要添加的“可参与运算的工资项目！”");
        }
    }
    protected void btnTaxUp_Click(object sender, EventArgs e)
    {
        if (this.litbTaxFee.SelectedIndex >= 0)
        {
            this.tbCalculateExp.Text += String.Concat(" [", this.litbTaxFee.SelectedValue, "]");
        }
        else
        {
            base.MessageBox("请先选择要添加的“可参与运算的工资项目！”");
        }
    }

    protected void btnCheck_Click(object sender, System.EventArgs e){ }

    /*
     
    protected void ddlFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (action == ActionType.Add)
        {
            this.tbFeeCode.Text = FeeInfoAdapter.Instance.CreateFeeCode(this.enumFeeType.ToString("D"), this.ddlFee.SelectedValue);
        }
    }
    */

    protected void gvTaxList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////if ((e.Row.RowState & DataControlRowState.Edit) != 0 && e.Row.RowType == DataControlRowType.DataRow)
        ////{
        ////    for (int i = 0; i < e.Row.Controls.Count; i++)
        ////    {
        ////       SetControlTextRTL(e.Row.Controls[i]);
        ////    }
        ////}//CZ
    }

}