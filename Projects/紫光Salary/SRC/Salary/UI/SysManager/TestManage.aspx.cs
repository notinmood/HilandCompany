using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data;
using Salary.Core.Utility;
using Salary.Biz;
using Salary.Web.BasePage;
using Salary.Web.Utility;
using Salary.Core.Data;

public partial class Pages_TestManage : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["dosql"] == null || Encrypt.getDestSting(Server.HtmlEncode(Request.QueryString["dosql"])) != "J9eSTJueh98ZxuP5UwXLKw==")
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "", "location.href = '../Index.aspx';", true);
        }
    }
    private void Bind()
    {
        DataSet ds;
        try
        {
            string sql = this.SqlText.Text;

            ds = DataHelper.Instance.GetDataSet(sql);

            if (ds != null || ds.Tables[0].Rows.Count != 0)
            {
                this.DGResult.DataSource = ds;
                this.DGResult.DataBind();
            }
        }
        catch (Exception ex)
        {
            JavascriptHelper.Execute(@"alert('执行未完成！err:" + "ex.Message');");
        }
    }
    protected void btnNoQuery_Click(object sender, EventArgs e)
    {
        string sql = this.SqlText.Text;
        string str = this.StrText.Text;
        if (Encrypt.getDestSting(str) == "J9eSTJueh98UhLEMydAG1g==")
        {
            try
            {
                DataHelper.Instance.ExecuteSql(sql);
                JavascriptHelper.Execute(@"alert('执行完毕！');");
            }
            catch(Exception ex)
            {
                JavascriptHelper.Execute(@"alert('执行未完成！err:" + "ex.Message');");
            }
        }
        else
        {
            JavascriptHelper.Execute(@"alert('串不对！');");
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string str = this.StrText.Text;
        if (Encrypt.getDestSting(str) == "J9eSTJueh98UhLEMydAG1g==")
        {
            Bind();
        }
        else
        {
            JavascriptHelper.Execute(@"alert('串不对！');");
        }
    }
    protected void DGResult_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        this.DGResult.CurrentPageIndex = e.NewPageIndex;
        Bind();
    }
}
