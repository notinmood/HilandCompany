using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Salary.Web.BasePage;
using Salary.Biz;
using Salary.Core.Utility;

public partial class UI_Portal_ChangeUserPwd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnModifyClick(object sender, EventArgs e)
    {
        if(CurrentUserInfo.Password!=CryptoHelper.Encode(txtOldPwd.Text))
        {
            MessageBox("旧密码输入不正确！");
            return;
        }

        if(txtNewPwd.Text!=txtConfirmPwd.Text)
        {
            MessageBox("新密码和确认密码输入不一致！");
            return;
        }
        UserInfoAdapter.Instance.ChangePassword(CurrentUserInfo.UserId, this.txtNewPwd.Text);
        CurrentUserInfo.Password = UserInfoAdapter.Instance.LoadUserInfo(CurrentUserInfo.UserId).Password;
        MessageAndClose("新密码修改成功！");
    }
}
