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
using Salary.Web.Utility;

public partial class UI_Report_ReportEdit : BasePage
{
    protected string reportID;
    protected ActionType action;

    protected void Page_Load(object sender, EventArgs e)
    {
        reportID = DecodedQueryString[ReportInfoConst.ReportID];
        action = EnumHelper.Parse<ActionType>(DecodedQueryString[SalaryConst.QueryAction]);
        if (!IsPostBack)
        {
            this.InitializeControl();
        }
        else
        {
            GridViewControl.ResetGridView(this.gvFeeList);
            GridViewControl.ResetGridView(this.gvWelfareList);
            GridViewControl.ResetGridView(this.gvSelectedFeeList);
        }
    }

    private void InitializeControl()
    {
        this.GridViewDataBind();
        if (action != ActionType.Add)
        {
            ReportInfo info = ReportInfoAdapter.Instance.LoadReportInfo(reportID);
            this.tbReportCode.Text = info.ReportCode;
            this.tbReportName.Text = info.ReportName;
        }
        this.gvSelectedFeeList.Enabled = action != ActionType.Add;
        this.gvFeeList.Enabled = action != ActionType.Add;
        this.gvWelfareList.Enabled = action != ActionType.Add;
        this.btnAdd.Enabled = action != ActionType.Add;
        //this.btnAddAll.Enabled = action != ActionType.Add;
        this.btnMinus.Enabled = action != ActionType.Add;
        //this.btnMinusAll.Enabled = action != ActionType.Add;
    }

    private void GridViewDataBind()
    {
        List<ReportFee> reportFeeList = ReportInfoAdapter.Instance.GetReportFeeList(reportID);
        GridViewControl.GridViewDataBind<ReportFee>(this.gvSelectedFeeList, reportFeeList);
        //this.gvSelectedFeeList.DataSource = reportFeeList;
        //this.gvSelectedFeeList.DataBind();

        List<FeeInfo> feeList = ReportInfoAdapter.Instance.GetReportCanSelectFeeList(reportID);
        //GridViewControl.GridViewDataBind<FeeInfo>(this.gvFeeList, feeList.OrderBy(ob => ob.FeeCode).ToList());
        GridViewControl.GridViewDataBind<FeeInfo>(this.gvFeeList, feeList.Where(fee => fee.FeeType == FeeType.Common && fee.CommonFeeType != CommonFeeType.Welfare).OrderBy(ob => ob.FeeCode).ToList());
        GridViewControl.GridViewDataBind<FeeInfo>(this.gvWelfareList, feeList.Where(fee => fee.FeeType == FeeType.Common && fee.CommonFeeType == CommonFeeType.Welfare).OrderBy(ob => ob.FeeCode).ToList());
        GridViewControl.GridViewDataBind<FeeInfo>(this.gvCalculateFeeList, feeList.Where(fee => fee.FeeType == FeeType.Sum).ToList().OrderBy(ob => ob.FeeCode).ToList());
        //this.gvFeeList.DataSource = feeList;
        //this.gvFeeList.DataBind();

    }

    private ReportInfo GetInfoFromPageControl()
    {
        ReportInfo info = null;
        switch (action)
        {
            case ActionType.Add:
                info = new ReportInfo();
                info.ReportID = CommonTools.Instance.GetMaxOrderNo(ReportInfoDBConst.TableName, ReportInfoDBConst.ReportID).ToString();
                break;
            case ActionType.Edit:
                info = ReportInfoAdapter.Instance.LoadReportInfo(reportID);
                break;
        }
        info.ReportCode = this.tbReportCode.Text.Trim();
        info.ReportName = this.tbReportName.Text.Trim();
        return info;
    }

    protected void btnSaveClick(Object sender, EventArgs e)
    {
        ReportInfo info = this.GetInfoFromPageControl();
        if (ReportInfoAdapter.Instance.IsReportCodeUsed(info, action == ActionType.Add))
        {
            base.MessageBox("报表编码冲突，请重新定义！");
            this.tbReportCode.Focus();
            return;
        }
        if (ReportInfoAdapter.Instance.IsReportNameUsed(info, action == ActionType.Add))
        {
            base.MessageBox("报表名称冲突，请重新定义！");
            this.tbReportName.Focus();
            return;
        }
        if (action == ActionType.Add)
        {
            ReportInfoAdapter.Instance.InsertReportInfo(info);
            UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.ReportEditUrl);
            urlEdit.AppendItem(ReportInfoConst.ReportID, info.ReportID);
            urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
            Redirect(urlEdit.ToUrlString());
        }
        else
        {
            ReportInfoAdapter.Instance.UpdateReportInfo(info);
        }
        base.MessageAndRefreshParentByCurrHref("报表保存成功！");
    }

    /*
    protected void btnSaveFeeClick(Object sender, EventArgs e)
    {
        List<ReportFee> select = new List<ReportFee>();
        for (Int32 i = 0; i < this.gvFeeList.Rows.Count; i++)
        {
            CheckBox ckb = (CheckBox)this.gvFeeList.Rows[i].FindControl("ckbSelect");
            TextBox tbOrderNo = (TextBox)this.gvFeeList.Rows[i].FindControl("tbOrderNo");
            String orderNo = tbOrderNo.Text.Trim();
            if (String.IsNullOrEmpty(orderNo) || !StrHelper.IsValidInt(orderNo))
            {
                base.MessageBox("排序字段必须为整数！");
                tbOrderNo.Focus();
                return;
            }

            if (ckb.Checked)
            {
                ReportFee info = new ReportFee();
                info.ReportID = reportID;
                info.FeeID = this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                info.FeeName = this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                info.OrderNo = Int32.Parse(orderNo);
                select.Add(info);
            }
        }
        ReportAdapter.Instance.InsertReportFee(select);
        this.GridViewDataBind();
        base.MessageBox("报表工资项目保存成功！");
    }
    */
    protected void gvFeeList_DataBound(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        LinkButton btnView;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.FeeEditUrl);
        if (!(gv.Rows.Count == 1 && (gv.Rows[0].Cells[0].Text == SalaryConst.EmptyText || gv.DataKeys[0].Value == null)))
        {
            for (Int32 i = 0; i < gv.Rows.Count; i++)
            {
                String strFeeID = gv.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                String strFeeCode = gv.DataKeys[i].Values[FeeInfoConst.FeeCode].ToString();
                TextBox tb = gv.Rows[i].FindControl("tbOrderNo") as TextBox;
                tb.Style.Add("direction", "rtl");

                btnView = gv.Rows[i].FindControl("btnView") as LinkButton;
                btnView.Text = SalaryAppAdapter.Instance.AddSpaceInFrontOfFeeName(strFeeCode) + btnView.Text;//缩进
                urlEdit.AppendItem(FeeInfoConst.FeeID, strFeeID);
                urlEdit.AppendItem(FeeInfoConst.FeeType, gv.DataKeys[i].Values[FeeInfoConst.FeeType]);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.View);
                base.ControlClientClickBindOpen(btnView, 700, 500, urlEdit.ToUrlString());
                urlEdit.Clear();
            }
        }
    }
    protected void gvSelectedFeeList_DataBound(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        LinkButton btnView;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.FeeEditUrl);
        if (!(gv.Rows.Count == 1 && (gv.Rows[0].Cells[0].Text == SalaryConst.EmptyText || gv.DataKeys[0].Value == null)))
        {
            for (Int32 i = 0; i < gv.Rows.Count; i++)
            {
                String strFeeID = gv.DataKeys[i].Values[ReportFeeConst.FeeID].ToString();
                //TextBox tb = this.gvSelectedFeeList.Rows[i].FindControl("tbOrderNo") as TextBox;
                //tb.Style.Add("direction", "rtl");

                btnView = gv.Rows[i].FindControl("btnView") as LinkButton;
                String ParentFeeName = FeeInfoAdapter.Instance.LoadFeeInfo(null, strFeeID).ParentName;
                btnView.Text =(String.IsNullOrEmpty(ParentFeeName) ? String.Empty : String.Concat("{", ParentFeeName, "}")) + btnView.Text;//缩进
                urlEdit.AppendItem(FeeInfoConst.FeeID, strFeeID);
                urlEdit.AppendItem(FeeInfoConst.FeeType, this.gvSelectedFeeList.DataKeys[i].Values[FeeInfoConst.FeeType]);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.View);
                base.ControlClientClickBindOpen(btnView, 700, 500, urlEdit.ToUrlString());
                urlEdit.Clear();
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        List<ReportFee> select = new List<ReportFee>();
        if (!(this.gvFeeList.Rows.Count == 1 && this.gvFeeList.Rows[0].Cells[0].Text == SalaryConst.EmptyText))
        {
            for (Int32 i = 0; i < this.gvFeeList.Rows.Count; i++)
            {
                CheckBox ckb = (CheckBox)this.gvFeeList.Rows[i].FindControl("ckbSelect");
                if (ckb.Checked)
                {
                    ReportFee info = new ReportFee();
                    info.ReportID = reportID;
                    info.FeeID = this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                    info.FeeName = this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                    //info.OrderNo = Int32.Parse(this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString());
                    select.Add(info);
                }
            }
        }
        if (!(this.gvWelfareList.Rows.Count == 1 && this.gvWelfareList.Rows[0].Cells[0].Text == SalaryConst.EmptyText))
        {
            for (Int32 i = 0; i < this.gvWelfareList.Rows.Count; i++)
            {
                CheckBox ckb = (CheckBox)this.gvWelfareList.Rows[i].FindControl("ckbSelect");
                if (ckb.Checked)
                {
                    ReportFee info = new ReportFee();
                    info.ReportID = reportID;
                    info.FeeID = this.gvWelfareList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                    info.FeeName = this.gvWelfareList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                    //info.OrderNo = Int32.Parse(this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString());
                    select.Add(info);
                }
            }
        }
        if (!(this.gvCalculateFeeList.Rows.Count == 1 && this.gvCalculateFeeList.Rows[0].Cells[0].Text == SalaryConst.EmptyText))
        {
            for (Int32 i = 0; i < this.gvCalculateFeeList.Rows.Count; i++)
            {
                CheckBox ckb = (CheckBox)this.gvCalculateFeeList.Rows[i].FindControl("ckbSelect");
                if (ckb.Checked)
                {
                    ReportFee info = new ReportFee();
                    info.ReportID = reportID;
                    info.FeeID = this.gvCalculateFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                    info.FeeName = this.gvCalculateFeeList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                    //info.OrderNo = Int32.Parse(this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString());
                    select.Add(info);
                }
            }
        }
        if (select.Any())
        {
            ReportInfoAdapter.Instance.InsertReportFee(select);
            this.GridViewDataBind();
        }
    }
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        List<ReportFee> select = new List<ReportFee>();
        if (!(this.gvFeeList.Rows.Count == 1 && this.gvFeeList.Rows[0].Cells[0].Text == SalaryConst.EmptyText))
        {
            for (Int32 i = 0; i < this.gvFeeList.Rows.Count; i++)
            {
                ReportFee info = new ReportFee();
                info.ReportID = reportID;
                info.FeeID = this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                info.FeeName = this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                //info.OrderNo = Int32.Parse(this.gvFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString());
                select.Add(info);
            }
        }
        if (select.Any())
        {
            ReportInfoAdapter.Instance.InsertReportFee(select);
            this.GridViewDataBind();
        }
    }
    protected void btnMinus_Click(object sender, EventArgs e)
    {
        List<ReportFee> select = new List<ReportFee>();
        if (!(this.gvSelectedFeeList.Rows.Count == 1 && this.gvSelectedFeeList.Rows[0].Cells[0].Text == SalaryConst.EmptyText))
        {
            for (Int32 i = 0; i < this.gvSelectedFeeList.Rows.Count; i++)
            {
                CheckBox ckb = (CheckBox)this.gvSelectedFeeList.Rows[i].FindControl("ckbSelect");
                if (ckb.Checked)
                {
                    ReportFee info = new ReportFee();
                    info.ReportID = reportID;
                    info.FeeID = this.gvSelectedFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                    info.FeeName = this.gvSelectedFeeList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                    select.Add(info);
                }
            }
        }
        if (select.Any())
        {
            ReportInfoAdapter.Instance.DeleteReportFee(select);
            this.GridViewDataBind();
        }
    }
    protected void btnMinusAll_Click(object sender, EventArgs e)
    {
        List<ReportFee> select = new List<ReportFee>();
        if (!(this.gvSelectedFeeList.Rows.Count == 1 && this.gvSelectedFeeList.Rows[0].Cells[0].Text == SalaryConst.EmptyText))
        {
            for (Int32 i = 0; i < this.gvSelectedFeeList.Rows.Count; i++)
            {
                ReportFee info = new ReportFee();
                info.ReportID = reportID;
                info.FeeID = this.gvSelectedFeeList.DataKeys[i].Values[FeeInfoConst.FeeID].ToString();
                info.FeeName = this.gvSelectedFeeList.DataKeys[i].Values[FeeInfoConst.FeeName].ToString();
                select.Add(info);
            }
        }
        if (select.Any())
        {
            ReportInfoAdapter.Instance.DeleteReportFee(select);
            this.GridViewDataBind();
        }
    }


    protected void gvSelectedFeeList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.gvSelectedFeeList.EditIndex = -1;
        this.GridViewDataBind();
    }

    protected void gvSelectedFeeList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.gvSelectedFeeList.EditIndex = e.NewEditIndex;
        this.GridViewDataBind();
    }

    protected void gvSelectedFeeList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox tbOrderNO = (TextBox)this.gvSelectedFeeList.Rows[e.RowIndex].FindControl("tbOrderNO");
        TextBox tbReportFeeName = (TextBox)this.gvSelectedFeeList.Rows[e.RowIndex].FindControl("tbReportFeeName");
        String OrderNO = tbOrderNO.Text.Trim();
        String ReportFeeName = tbReportFeeName.Text;

        if (string.IsNullOrEmpty(OrderNO) || !StrHelper.IsValidDecimal(OrderNO))
        {
            base.MessageBox("排序必须为数字！");
            tbOrderNO.Focus();
            return;
        }
        if (string.IsNullOrEmpty(ReportFeeName))
        {
            base.MessageBox("请填写报表列名！");
            tbReportFeeName.Focus();
            return;
        }
        ReportFee reportFeeInfo = ReportInfoAdapter.Instance.LoadReportFee(this.reportID, this.gvSelectedFeeList.DataKeys[e.RowIndex][ReportFeeConst.FeeID].ToString());
        reportFeeInfo.OrderNo = Int32.Parse(OrderNO);
        reportFeeInfo.ReportFeeName = ReportFeeName;
        ReportInfoAdapter.Instance.UpdateReportFee(reportFeeInfo);
        this.gvSelectedFeeList.EditIndex = -1;
        this.GridViewDataBind();
    }
}
