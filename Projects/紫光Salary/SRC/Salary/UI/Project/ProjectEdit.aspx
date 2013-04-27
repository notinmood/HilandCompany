<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectEdit.aspx.cs" Inherits="UI_Project_ProjectEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>项目信息维护</title>
    <base target="_self" />
    <script type="text/javascript">
        function validateInput() {
            if ($.trim($('#tbProjectCode').val()) == '') {
                alert('项目编码不能为空！');
                $('#tbProjectCode').focus();
                return false;
            }
            if ($.trim($('#tbProjectName').val()) == '') {
                alert('项目名称不能为空！');
                $('#tbProjectName').focus();
                return false;
            }
            if ($.trim($('#ddlProjectClass').val()) == '') {
                alert('请选择所属项目分类！');
                $('#ddlProjectClass').focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="text-align: center; padding-top:10px">
    <form id="form1" runat="server">  
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
        <div class="title">项目信息编辑</div>
	<div style="width:310px; margin:0px auto;">
        <table style="width: 300px;">
            <tr>
                <td align="right">项目编码：</td>
                <td align="left"><asp:TextBox ID="tbProjectCode" runat="server" MaxLength="36" /><font color="red">*</font></td>
            </tr>
            <tr>
                <td align="right">项目名称：</td>
                <td align="left"><asp:TextBox ID="tbProjectName" runat="server" MaxLength="36" /><font color="red">*</font></td>
            </tr>
            <tr>
                <td align="right">项目分类：
                    </td>
                <td align="left">
                    <asp:DropDownList ID="ddlProjectClass" runat="server" AutoPostBack="True" 
                        ondatabound="ddlProjectClass_DataBound" 
                        onselectedindexchanged="ddlProjectClass_SelectedIndexChanged" /><font color="red">*</font>
                </td>
            </tr>
            <tr><td></td><td></td></tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSave" OnClientClick="return validateInput();" OnClick="btnSaveClick" Text="保存" />
                    <input type="button" value="关闭" onclick="window.close();" />
                </td>
            </tr>
        </table>
    </div>
    <%--</ContentTemplate> 
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
