using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Salary.Web.BasePage;
using Salary.Biz;

public partial class UI_Portal_TurnToLogin : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentUserInfo = null;
        RefreahParentPage(SalaryConst.LoginPageUrl);
    }
}
