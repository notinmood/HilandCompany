<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Pages_Login"  Theme=""%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>系统登录</title>
    <style type="text/css">
        .STYLE1
        {
            font-size: 12px;
        }
    </style>
    <script language="javascript"  type="text/javascript"  for="document" event="onkeydown">
　　     if(event.keyCode==13 && event.srcElement.type!='button' && event.srcElement.type!='image'
　　         && event.srcElement.type!='submit' && event.srcElement.type!='reset' 
　　         && event.srcElement.type!='textarea' && event.srcElement.type!='')
　　     {
　　            if(event.srcElement.id == 'tbValidateCode')
　　                form1.document.getElementById("btnLogin").click();
　　            else
　　            event.keyCode=9;
　　     }
    </script>
</head>
<body background="../../Content/Default/Images/bg.jpg">
    <form id="form1" runat="server">
    <table width="731" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td style="height: 40px">
                &nbsp;
            </td>
        </tr>
    </table>
    <table border="0" align="center" cellpadding="0" cellspacing="0" 
        style="width: 743px">
        <tr>
            <td style="height: 163px; background-image: url(../../Content/Default/Images/top1.jpg);">
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="743" border="0" align="center" cellpadding="0" cellspacing="0" 
        background="../../Content/Default/Images/top2center.jpg" style="height: 296px">
        <tr>
            <td align="left" valign="middle" 
                style="height: 97px; width: 332px; background-image: url('../../Content/Default/Images/top3.gif');">
                &nbsp;</td>
            <td align="right" valign="middle" 
                
                style="height: 97px; background-image: url('../../Content/Default/Images/top2.gif'); text-align: center;">
                &nbsp;<table width="80%" border="0" cellspacing="0">
                    
                    <tr>
                        <td style="height: 16px; text-align: right;">
                            <span class="STYLE1">登录名：</span>
                        </td>
                        <td colspan="2" style="width: 53%; height: 16px">
                            <asp:TextBox ID="tbLogonName" runat="server" Width="138px" Text="admin"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 3px; text-align: right;">
                            <span class="STYLE1">密码：</span>
                        </td>
                        <td colspan="2" style="height: 3px">
                            <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" Width="138px" Text="88888888" MaxLength="8" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px; text-align: right;">
                            <span class="STYLE1">验证码：</span>
                        </td>
                        <td style="height: 22px">
                            <asp:TextBox ID="tbValidateCode" runat="server" Width="69px" />
                        </td>
                        <td align="left" style="height: 22px">
                            <span>
                                <img alt="" src="ValidCode.aspx" style="width: 50px" /></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">
                            <asp:CheckBox ID="cbCookie" runat="server" CssClass="lv" Text="用户信息保存7天" Font-Size="9" 
                               TextAlign="left" />
                        </td>
                        <td style="height: 22px">
                            <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="../../Content/Default/Images/go.jpg" OnClick="btnLoginClick" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
