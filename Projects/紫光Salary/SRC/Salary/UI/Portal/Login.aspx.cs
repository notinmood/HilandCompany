using System;
using System.Linq;
using Salary.Biz;
using Salary.Core.Utility;
using Salary.Web.BasePage;
using Salary.Web.Utility;
using System.Collections.Generic;
using Salary.Core.Data;
using System.Web;

public partial class Pages_Login : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RemoveCss(SysConst.BaseCss);
        if (Request.Cookies["qdunisSalarySettings"] != null)
        {
            String userSettings = string.Empty;
            if (Request.Cookies["qdunisSalarySettings"]["Info"] != null)
            {
                userSettings = Request.Cookies["qdunisSalarySettings"]["Info"].ToString();
            }
            String[] strSplit = new String[] { "/@#" };
            String[] strUserInfo = CodeHelper.DecodeQueryString(userSettings).Split(strSplit, StringSplitOptions.None);
            if ((Convert.ToDateTime(strUserInfo[2]).AddDays(-8) < DateTime.Now) && (Convert.ToDateTime(strUserInfo[2]) > DateTime.Now))
            {
                UserInfo userInfo = UserInfoAdapter.Instance.LoadUserInfoByLogon(strUserInfo[0], strUserInfo[1]);
                if (userInfo != null)
                {
                    CurrentUserInfo = userInfo;
                    this.InsertUserLog(userInfo);
                    Response.Redirect(SalaryConst.IndexPageUrl);
                }
            }
        }
    }

    protected void btnLoginClick(object sender, EventArgs e)
    {
        String validateCode = GetSessionString(SalaryConst.ValidateCode);
        if (!validateCode.Equals(tbValidateCode.Text.Trim(), StringComparison.CurrentCultureIgnoreCase))
        {
            MessageBox("验证码输入有误！");
            return;
        }
        UserInfo userInfo = UserInfoAdapter.Instance.LoadUserInfoByLogon(tbLogonName.Text.Trim(), tbPassword.Text.Trim());
        if (userInfo == null)
        {
            MessageBox("登录失败，请重新输入账号密码！");
            return;
        }
        //Session[UserInfoConst.UserType] = EnumHelper.GetDescription<UserInfo>(userInfo.UserType.ToString()).ToString();
        CurrentUserInfo = userInfo;
        this.InsertUserLog(userInfo);
        if (this.cbCookie.Checked)
        {
            HttpCookie myCookie = new HttpCookie("qdunisSalarySettings");
            myCookie["Info"] = CodeHelper.EncodeQueryString(userInfo.LogonName + "/@#" + CryptoHelper.Decode(userInfo.Password) + "/@#" + DateTime.Now.AddDays(7).ToString());
            myCookie.Expires = DateTime.Now.AddDays(100);
            Response.Cookies.Add(myCookie);
        }
        Response.Redirect(SalaryConst.IndexPageUrl);
        //MessageHelper.Redirect(ResolveUrl(AccreditConst.IndexPageUrl));
    }

    private void InsertUserLog(UserInfo userInfo)
    {
        UserLogInfo userLogInfo = new UserLogInfo();
        userLogInfo.UserId = userInfo.UserId;
        userLogInfo.LogonName = userInfo.LogonName;
        userLogInfo.InputDate = DateTime.Now;
        UserInfoAdapter.Instance.InsertUserLogInfo(userLogInfo);
    }
}
