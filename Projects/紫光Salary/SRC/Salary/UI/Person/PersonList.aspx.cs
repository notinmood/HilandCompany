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

public partial class UI_Person_PersonList : BasePage
{
    public String PersonType=String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PersonType = DecodedQueryString[PersonInfoConst.PersonType];
        
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
        //UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.PersonEditUrl);
        //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        //base.ControlClientClickBindOpen(this.btnAdd, 700, 500, urlEdit.ToUrlString()); 
        //urlEdit = new UrlParamBuilder(SalaryConst.DeparmentEditUrl);
        //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        //base.ControlClientClickBindShow(this.btnAddDepartment, 670, 300, urlEdit.ToUrlString());   
        this.TreeViewDataBind();
        this.GridViewDataBind();
    }

    private void GridViewDataBind()
    {
        WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        if (!String.IsNullOrEmpty(this.PersonType))
        {
            builder.AppendItem(PersonInfoDBConst.PersonType, EnumHelper.Parse<PersonType>(this.PersonType).ToString("D"));
        }
        TreeNode tn = this.trvDeparment.SelectedNode;
        if (tn != null)
        {
            String departID = tn.Value;
            if (!String.IsNullOrEmpty(departID))
            {
                builder.AppendItem(PersonInfoDBConst.DepartmentID, departID + "%", "LIKE");
            }
            else
            {
                btnModifyDepartment.Enabled = false;
            }
        }
        else
        {
            btnModifyDepartment.Enabled = false;
        }
        List<PersonInfo> infoList = PersonInfoAdapter.Instance.GetPersonInfoList(builder);
        GridViewControl.GridViewDataBind<PersonInfo>(this.gvList, infoList);
        //this.gvList.DataSource = infoList;
        //this.gvList.DataBind();
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        PersonInfoAdapter.Instance.DeletePersonInfo((sender as ImageButton).CommandArgument);
        //base.MessageBox("员工删除成功！");
        this.GridViewDataBind();
    }

    protected void lbtnDimission_Click(object sender, EventArgs e)
    {
        PersonInfoAdapter.Instance.ChangeDimission((sender as LinkButton).CommandArgument, (sender as LinkButton).Text == "在职" ? false : true);
        base.MessageBox(String.Format(@"用户{0}成功！", (sender as LinkButton).Text));
        this.GridViewDataBind();
    }

    protected void gvList_DataBound(object sender, EventArgs e)
    {
        LinkButton btnView; //// btnDisdimission, btnDimission;
        Label labDimission;
        ImageButton ibtnEdit, ibtnAddChild;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.PersonEditUrl);
        if (!(this.gvList.Rows.Count == 1 && (this.gvList.Rows[0].Cells[0].Text == SalaryConst.EmptyText || this.gvList.DataKeys[0].Value == null)))
        {
            for (Int32 i = 0; i < this.gvList.Rows.Count; i++)
            {
                btnView = this.gvList.Rows[i].FindControl("btnView") as LinkButton;
                ibtnEdit = this.gvList.Rows[i].FindControl("ibtnEdit") as ImageButton;
                ibtnAddChild = this.gvList.Rows[i].FindControl("ibtnAddChild") as ImageButton;
                labDimission = this.gvList.Rows[i].FindControl("labDimission") as Label;
                labDimission.Text = bool.Parse(this.gvList.DataKeys[i][PersonInfoConst.Dimission].ToString()) ? "离职" : "在职";
                labDimission.ForeColor = bool.Parse(this.gvList.DataKeys[i][PersonInfoConst.Dimission].ToString()) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                ////btnDisdimission = this.gvList.Rows[i].FindControl("btnDisdimission") as LinkButton;
                ////btnDimission = this.gvList.Rows[i].FindControl("btnDimission") as LinkButton;
                 
                urlEdit.AppendItem(PersonInfoConst.PersonID, this.gvList.DataKeys[i].Values[PersonInfoConst.PersonID]);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindOpen(btnView, 800, 500, urlEdit.ToUrlString());
                urlEdit.RemoveAt(1);
                urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
                base.ControlClientClickBindOpen(ibtnEdit, 800, 500, urlEdit.ToUrlString());
                urlEdit.Clear();
                if (String.IsNullOrEmpty(this.gvList.DataKeys[i].Values[PersonInfoConst.ParentID].ToString()))
                {
                    urlEdit.AppendItem(PersonInfoConst.ParentID, this.gvList.DataKeys[i].Values[PersonInfoConst.PersonID]);
                    urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
                    base.ControlClientClickBindOpen(ibtnAddChild, 800, 500, urlEdit.ToUrlString());
                    urlEdit.Clear();
                }
                else
                {
                    this.gvList.Rows[i].Cells[0].Text = "";
                    this.gvList.Rows[i].Cells[1].Text = "";
                    ibtnAddChild.Visible = false;
                }                

                ////if (bool.Parse(this.gvList.DataKeys[i][PersonInfoConst.Dimission].ToString()))
                ////{
                ////    btnDimission.Enabled = false;
                ////    btnDisdimission.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要入职该用户？')"));
                ////}
                ////else
                ////{
                ////    btnDisdimission.Enabled = false;
                ////    btnDimission.Attributes.Add("OnClick", String.Format(@"return confirm('您确认要离职该用户？')"));
                ////}
            }
        }
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit = new UrlParamBuilder(SalaryConst.PersonEditUrl);
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        base.ControlClientClickBindOpen(ibtnAdd, 600, 400, urlEdit.ToUrlString());
    }

    private void TreeViewDataBind()
    {
        trvDeparment.Nodes.Clear();
        TreeNode tnAdd = new TreeNode();
        tnAdd.Text = SalaryAppConst.CompanyName;    //"全部";
        tnAdd.Value = "";
        trvDeparment.Nodes.Add(tnAdd);
        //WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
        List<DepartmentInfo> departList = DepartmentInfoAdapter.Instance.GetDepartmentInfoList(null);
        foreach(DepartmentInfo departInfo in departList.Where(dp => String.IsNullOrEmpty(dp.ParentID)))
        {
            TreeNode tnChild = new TreeNode();
            tnChild.Text = departInfo.DepartmentName;
            tnChild.Value = departInfo.DepartmentID;
            TreeViewDataBind(tnChild, departList, departInfo.DepartmentID);
            tnAdd.ChildNodes.Add(tnChild);
        }
        trvDeparment.Nodes[0].Checked = true;
    }

    private void TreeViewDataBind(TreeNode tnParent, List<DepartmentInfo> departList, string parentId)
    {
        foreach (DepartmentInfo departInfo in departList.Where(dp => dp.ParentID == parentId))
        {
            TreeNode tnChild = new TreeNode();
            tnChild.Text = departInfo.DepartmentName;
            tnChild.Value = departInfo.DepartmentID;
            TreeViewDataBind(tnChild, departList, departInfo.DepartmentID);
            tnParent.ChildNodes.Add(tnChild);
        }
    }
    protected void trvDeparment_SelectedNodeChanged(object sender, EventArgs e)
    {
        this.GridViewDataBind();
        String value=this.trvDeparment.SelectedNode.Value;
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.PersonEditUrl);
        ImageButton ibtnAdd = this.gvList.HeaderRow.FindControl("ibtnAdd") as ImageButton;
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        urlEdit.AppendItem(PersonInfoConst.DepartmentID, value);
        base.ControlClientClickBindOpen(ibtnAdd, 670, 600, urlEdit.ToUrlString());
        //urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        //urlEdit.AppendItem(PersonInfoConst.DepartmentId, value);
        //base.ControlClientClickBindOpen(this.btnAdd, 700, 500, urlEdit.ToUrlString());

        urlEdit = new UrlParamBuilder(SalaryConst.DeparmentEditUrl);
        urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Add);
        urlEdit.AppendItem(DepartmentInfoConst.ParentID, value);
        base.ControlClientClickBindShow(this.btnAddDepartment, 670, 300, urlEdit.ToUrlString());

        if(!String.IsNullOrEmpty(value))
        {
            this.btnModifyDepartment.Enabled = true;
            urlEdit = new UrlParamBuilder(SalaryConst.DeparmentEditUrl);
            urlEdit.AppendItem(SalaryConst.QueryAction, ActionType.Edit);
            urlEdit.AppendItem(DepartmentInfoConst.DepartmentID, value);
            base.ControlClientClickBindShow(this.btnModifyDepartment, 670, 300, urlEdit.ToUrlString());
        }
    }
}
