using System;
using Salary.Web.BasePage;
using Salary.Biz.Eunm;

public partial class UI_Portal_Menu : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUserInfo.UserType != UserType.AdminUser)
        {
            ////this.menuSalary.Visible = false;
            ////this.menuReport.Visible = false;
            ////this.menuFee.Visible = false;
            ////this.menuSysManager.Visible = false;
            ////this.menuPersonList.Visible = false;
            ////this.menuProject.Visible = false;
        }
    }

    /// <summary>
    /// 重新登录
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnReLogin_Click(object sender, EventArgs e)
    {
        ReLogonSystem();
    }
}
