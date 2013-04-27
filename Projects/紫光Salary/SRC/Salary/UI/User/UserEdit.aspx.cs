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

public partial class UI_User_UserEdit : BasePage
{
    string userId;
    ActionType action;

    protected void Page_Load(object sender, EventArgs e)
    {
        userId = DecodedQueryString[UserInfoConst.UserId];
        action = EnumHelper.Parse<ActionType>(DecodedQueryString[SalaryConst.QueryAction]);
        if (!IsPostBack)
        {
            this.InitializeControl();
        }
    }

    private void InitializeControl()
    {
        Tools.DropDownListDataBindByEnum(this.ddlUserType, typeof(UserType), string.Empty, true, true);
        if (action != ActionType.Add)
        {
            UserInfo userInfo = UserInfoAdapter.Instance.LoadUserInfo(userId);
            this.tbUserName.Text = userInfo.UserName;
            this.tbLogonName.Text = userInfo.LogonName;
            this.ddlUserType.SelectedValue = ((Int32)userInfo.UserType).ToString();
        }
    }

    private UserInfo GetInfoFromPageControl()
    {
        UserInfo userInfo = null;
        switch (action)
        {
            case ActionType.Add:
                userInfo = new UserInfo();
                userInfo.UserId = Guid.NewGuid().ToString();
                userInfo.Password = CryptoHelper.Encode(SalaryConst.DefaultPassword);
                break;
            case ActionType.Edit:
                userInfo = UserInfoAdapter.Instance.LoadUserInfo(userId);
                break;
        }
        userInfo.UserName = this.tbUserName.Text.Trim();
        userInfo.LogonName = this.tbLogonName.Text.Trim();
        userInfo.UserType = EnumHelper.Parse<UserType>(this.ddlUserType.SelectedValue);
        return userInfo;
    }

    protected void btnSaveClick(Object sender, EventArgs e)
    {
        UserInfo userInfo = this.GetInfoFromPageControl();
        if (UserInfoAdapter.Instance.IsLogonNameUsed(userInfo, action == ActionType.Add))
        {
            base.MessageBox("登录名冲突，请重新定义！");
            this.tbLogonName.Focus();
            return;
        }
        String rtn = "";
        if (action == ActionType.Add)
        {
            UserInfoAdapter.Instance.InsertUserInfo(userInfo);
            rtn = String.Format(@"用户添加成功！原始登录密码为{0}，请尽快登录修改。", SalaryConst.DefaultPassword);
        }
        else
        {
            UserInfoAdapter.Instance.UpdateUserInfo(userInfo);
        }
        String javascript = String.Format(@"
            window.dialogArguments.location.href = window.dialogArguments.location.href ;
            alert('{0}');
            window.close();",rtn == "" ? "用户保存成功！" : rtn);
        ExecuteJavascript(javascript);
    }
}
