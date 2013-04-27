<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectClassEdit.aspx.cs" Inherits="UI_Project_ProjectClassEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>项目分类信息维护</title>
    <base target="_self" />
    <script type="text/javascript">
        function validateInput() {
            if ($.trim($('#tbProjectClassName').val()) == '') {
                alert('项目分类名称不能为空！');
                $('#tbProjectClassName').focus();
                return false;
            }
            var pattern = new RegExp("^\\d{0,10}$");
            if (!pattern.test($.trim($('#tbOrderNo').val()))) {
                alert("排序ID必须为不超过10位的数字，请修正！");
                $('#tbOrderNo').focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="text-align: center; padding-top:10px">
    <form id="form1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title">项目分类信息编辑</div>
	<div style="width:310px; margin:0px auto;">
        <table style="width: 300px;">
            <tr>
                <td align="right">项目分类编码：</td>
                <td align="left"><asp:TextBox ID="tbProjectClassCode" runat="server" MaxLength="36" /></td>
            </tr>
            <tr>
                <td align="right">项目分类名称：</td>
                <td align="left"><asp:TextBox ID="tbProjectClassName" runat="server" MaxLength="36" /><font color="red">*</font></td>
            </tr>
            <tr>
                <td align="right">上级项目分类：
                    </td>
                <td align="left">
                    <asp:DropDownList ID="ddlProjectClass" runat="server" AutoPostBack="True" 
                        ondatabound="ddlProjectClass_DataBound" 
                        onselectedindexchanged="ddlProjectClass_SelectedIndexChanged" />
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
    </ContentTemplate> 
    </asp:UpdatePanel>
    </form>
</body>
</html>
