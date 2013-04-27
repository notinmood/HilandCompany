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
using System.Drawing;
using Salary.Web.Utility;
using Salary.Core.Data;


public partial class UI_Fee_FeeList : BasePage
{
    public String yearMonth;
    public FeeType enumFeeType = new FeeType();
    //public CommonFeeType[] enumCommonFeeType;
    String CommonFeeType;

    protected void Page_Load(object sender, EventArgs e)
    {//action = EnumHelper.Parse<ActionType>(DecodedQueryString[SalaryConst.QueryAction]);
        yearMonth = DecodedQueryString[FeeMonthInfoConst.YearMonth];
        String feeType = DecodedQueryString[FeeInfoConst.FeeType];
        CommonFeeType = DecodedQueryString[FeeInfoConst.CommonFeeType];
        if (!String.IsNullOrEmpty(feeType))
        {
            this.enumFeeType = EnumHelper.Parse<FeeType>(feeType);
        }
        //if (!String.IsNullOrEmpty(commonFeeType))
        //{
        //    this.enumCommonFeeType = EnumHelper.Parse<CommonFeeType>(commonFeeType);
        //}
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
    {   //顶级项目
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        builder.AppendItem(FeeInfoDBConst.FeeType, this.enumFeeType.ToString("D"));
        if (!String.IsNullOrEmpty(this.CommonFeeType))
        {
            WhereSqlClauseBuilder builderOR = new WhereSqlClauseBuilder(LogicOperatorDefine.Or);
            foreach (String str in this.CommonFeeType.Split(','))
            {
                builderOR.AppendItem(FeeInfoDBConst.CommonFeeType, EnumHelper.Parse<CommonFeeType>(str).ToString("D"));
            }
            builder.AppendItem(builderOR.ToSqlString());
        }
        
        List<FeeInfo> feeList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder).Where(parent => parent.ParentID == "" || parent.ParentID == null).ToList();
        Tools.DropDownListDataBind(this.ddlFee, feeList, "", true, FeeInfoConst.FeeName, FeeInfoConst.FeeID);
        this.GridViewDataBind();
    }

    private void GridViewDataBind()
    {
        this.gvList.Columns[1].HeaderText = this.enumFeeType.Equals(FeeType.Parameter) ? "参数名称" : this.enumFeeType.Equals(FeeType.Tax) ? "个税名称" : "项目";
        this.gvList.Columns[2].Visible = this.enumFeeType.Equals(FeeType.Parameter);//参数值
        //运算符号
        this.gvList.Columns[3].Visible = !(this.enumFeeType.Equals(FeeType.Parameter) || this.enumFeeType.Equals(FeeType.Tax));
        //启用日期
        this.gvList.Columns[6].Visible = this.enumFeeType.Equals(FeeType.Parameter) || this.enumFeeType.Equals(FeeType.Tax);
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        if (!this.enumFeeType.Equals(0))
        {
            builder.AppendItem(FeeInfoDBConst.FeeType, this.enumFeeType.ToString("D"));
        }
        if (!String.IsNullOrEmpty(this.CommonFeeType))
        {
            WhereSqlClauseBuilder builderOR = new WhereSqlClauseBuilder(LogicOperatorDefine.Or);
            foreach (String str in this.CommonFeeType.Split(','))
            {
                builderOR.AppendItem(FeeInfoDBConst.CommonFeeType, EnumHelper.Parse<CommonFeeType>(str).ToString("D"));
            }
            builder.AppendItem(builderOR.ToSqlString());
        }
        if (this.ddlFee.SelectedIndex > 0)
        {
            String strFeeID = this.ddlFee.SelectedValue;
            builder.AppendItem(FeeInfoDBConst.FeeID, strFeeID);
            //builder.AppendItem(FeeInfoDBConst.ParentID, strFeeID);
        }
        List<FeeInfo> feeInfoList = FeeInfoAdapter.Instance.GetFeeInfoList(yearMonth, builder);//.GetFeeTreeInfoList(yearMonth, builder);
        GridViewControl.GridViewDataBind<FeeInfo>(this.gvList, feeInfoList.OrderBy(ob => ob.FeeCode).ToList());
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string value = (sender as ImageButton).CommandArgument;
        ////if (FeeInfoAdapter.Instance.IsFeeUsed(yearMonth, value))
        ////{
        ////    base.MessageBox("该工资项目已参与其它工资项目计算，不能删除！");
        ////    return;
        ////}Not暂时不判断
        FeeInfoAdapter.Instance.DeleteFeeInfo(yearMonth, value);//base.MessageBox("工资项目删除成功！");
        this.GridViewDataBind();
    }

    protected void lbtnUseLogout_Click(object sender, EventArgs e)
    {
        string text = (sender as LinkButton).Text;
        string value = (sender as LinkButton).CommandArgument;
        if (text == EnumHelper.GetDescription(Status.False))
        {
            if (!FeeInfoAdapter.Instance.LoadFeeInfo(yearMonth, value).Equals(null))
            {
                base.MessageBox(String.Format("该工资项目已被其它工资项目使用不能{0}！", text));
                return;
            }
        }
        Int32 used = (sender as LinkButton).Text == EnumHelper.GetDescription(Status.True) ? (Int32)Status.True : (Int32)Status.False;
        String feeID = (sender as LinkButton).CommandArgument;
        FeeInfoAdapter.Instance.ChangeStatus(yearMonth, feeID, used);//base.MessageBox(String.Format(@"工资项目{0}成功！", (sender as LinkButton).Text));
        this.GridViewDataBind();
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        LinkButton btnView, btnUse, btnLogout;
        ImageButton ibtnEdit, ibtnAddChild;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.FeeEditUrl);
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value == null)))
        {
            String strAddSpace = String.Empty;
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                String strFeeCode = this.gvList.DataKeys[i].Values[FeeInfoConst.FeeCode].ToString();
                String strFeeID = this.gvList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                btnView = this.gvList.Rows[i].FindControl("btnView") as LinkButton;
                ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                ibtnAddChild = this.gvList.Rows[i].FindControl("ibtnAddChild") as ImageButton;
                btnUse = this.gvList.Rows[i].FindControl("btnUse") as LinkButton;
                btnLogout = this.gvList.Rows[i].FindControl("btnLogout") as LinkButton;
                //缩进
                strAddSpace = SalaryAppAdapter.Instance.AddSpaceInFrontOfFeeName(strFeeCode);
                btnView.Text = strAddSpace + btnView.Text;
                if (!String.IsNullOrEmpty(yearMonth))
                {
                    urlEdit.AppendItem(FeeMonthInfoConst.YearMonth, yearMonth);
                }
                urlEdit.AppendItem(FeeInfoConst.FeeID, strFeeID);
                urlEdit.AppendItem(FeeInfoConst.FeeType, this.enumFeeType.ToString());
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindOpen(btnView, 620, 510, urlEdit.ToUrlString());
                urlEdit.RemoveAt(2);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindOpen(ibtnEdit, 620, 510, urlEdit.ToUrlString());
                urlEdit.Clear();
                if ((this.enumFeeType.Equals(FeeType.Tax) || this.enumFeeType.Equals(FeeType.Parameter)) && strAddSpace.Length > 0)
                {
                    btnView.Visible = false;
                    ibtnAddChild.Visible = false;
                    this.gvList.Rows[i].Cells[0].Text = "";
                }
                else
                {
                    urlEdit.AppendItem(FeeInfoConst.FeeType, this.enumFeeType.ToString());
                    //urlEdit.AppendItem(FeeInfoConst.CommonFeeType, this.enumCommonFeeType.ToString());//NOT
                    urlEdit.AppendItem(FeeInfoConst.ParentID, strFeeID);
                    urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
                    base.ControlClientClickBindOpen(ibtnAddChild, 620, 510, urlEdit.ToUrlString());
                    urlEdit.Clear();
                }

                btnUse.Text = EnumHelper.GetDescription(Status.True);
                btnLogout.Text = EnumHelper.GetDescription(Status.False);
                if (bool.Parse(this.gvList.DataKeys[i][FeeInfoConst.UseFlag].ToString()))
                {
                    btnLogout.Enabled = false;
                    btnUse.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该工资项目？')", btnUse.Text));
                }
                else
                {
                    btnUse.Enabled = false;
                    btnLogout.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要{0}该工资项目？')", btnLogout.Text));
                }
                #region 未完成
                /**
                //验证工资项目的完整性
                FeeType feeType = EnumHelper.Parse<FeeType>(this.gvList.DataKeys[i].Values[FeeInfoConst.FeeType].ToString());
                switch (feeType)
                { 
                    case FeeType.BaseCoefficient:
                        if (FeeInfoAdapter.Instance.GetFeeDetailList(this.gvList.DataKeys[i].Values[FeeInfoConst.FeeCode].ToString()).Count < 2)
                        {
                            btnView.ForeColor = Color.Red;
                        }
                        break;
                    case FeeType.Sum:
                        if (FeeInfoAdapter.Instance.GetFeeDetailList(this.gvList.DataKeys[i].Values[FeeInfoConst.FeeCode].ToString()).Count <= 0)
                        {
                            btnView.ForeColor = Color.Red;
                        }
                        break;
                    case FeeType.Tax:
                        if (FeeInfoAdapter.Instance.GetFeeDetailList(this.gvList.DataKeys[i].Values[FeeInfoConst.FeeCode].ToString()).Count <= 0 || TaxInfoAdapter.Instance.GetTaxInfoList(this.gvList.DataKeys[i].Values[FeeInfoConst.FeeCode].ToString()).Count <= 0)
                        {
                            btnView.ForeColor = Color.Red;
                        }
                        break;
                }
                 **/
                #endregion
            }
        }
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        if (!this.enumFeeType.Equals(0))
        {
            urlEdit.AppendItem(FeeInfoConst.FeeType, this.enumFeeType.ToString("D"));
        }
        base.ControlClientClickBindOpen(ibtnAdd, 700, 500, urlEdit.ToUrlString());
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.GridViewDataBind();
    }
    protected void ddlFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GridViewDataBind();
    }
}
