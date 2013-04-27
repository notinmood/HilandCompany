<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserEdit.aspx.cs" Inherits="UI_User_UserEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户信息维护</title>
    <base target="_self" />
    <script type="text/javascript">
        function validateInput() {
            if ($.trim($('#tbUserName').val()) == '') {
                alert('用户姓名不能为空！');
                $('#tbUserName').focus();
                return false;
            }
            if ($.trim($('#tbLogonName').val()) == '') {
                alert('登录名不能为空！');
                $('#tbLogonName').focus();
                return false;
            }
            if ($.trim($('#ddlUserType').val()) == '') {
                alert("请选择用户类型！");
                $('#ddlUserType').focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="text-align: center; padding-top:10px">
    <form id="frmUserEdit" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title">用户信息编辑</div>
	<div style="width:310px; margin:0px auto;">
        <table style="width: 300px;">
            <tr>
                <td align="right">用户名称：</td>
                <td align="left"><asp:TextBox ID="tbUserName" runat="server" MaxLength="36" /><font color="red">*</font></td>
            </tr>
            <tr>
                <td align="right">登录名：</td>
                <td align="left">
                    <asp:TextBox ID="tbLogonName" runat="server" MaxLength="36" /><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td align="right">用户类型：</td>
                <td align="left"><asp:DropDownList ID="ddlUserType" runat="server" /><font color="red">*</font></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 40px">
                    <asp:Button runat="server" ID="btnSave" OnClientClick="return validateInput();" OnClick="btnSaveClick" Text="保存" />
                    <input type="button" value="关闭" onclick="window.close();" />
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate> 
    </asp:UpdatePanel>
    </form>
</body>
</html>
