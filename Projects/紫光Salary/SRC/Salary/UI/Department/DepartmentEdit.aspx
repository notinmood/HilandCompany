<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepartmentEdit.aspx.cs" Inherits="UI_Department_DepartmentEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门信息维护</title>
    <base target="_self" />
    <script type="text/javascript">
        function validateInput() {
            if ($.trim($('#tbDepartmentCode').val()) == '') {
                alert('部门编码不能为空！');
                $('#tbDepartmentCode').focus();
                return false;
            }
            if ($.trim($('#tbDepartmentName').val()) == '') {
                alert('部门名称不能为空！');
                $('#tbDepartmentName').focus();
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
        <div class="title">部门信息编辑</div>
	<div style="width:300px; margin:0px auto;">
        <table style="width: 300px;">
            <tr>
                <td align="right">部门编码：</td>
                <td align="left"><asp:TextBox ID="tbDepartmentCode" runat="server" MaxLength="36" /></td>
            </tr>
            <tr>
                <td align="right">部门名称：</td>
                <td align="left"><asp:TextBox ID="tbDepartmentName" runat="server" MaxLength="36" /><font color="red">*</font></td>
            </tr>
            <tr>
                <td align="right">上级部门：
                    </td>
                <td align="left">
                    <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="True" ondatabound="ddlDepartment_DataBound" onselectedindexchanged="ddlDepartment_SelectedIndexChanged" />
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">排序ID：</td>
                <td align="left"><asp:TextBox ID="tbOrderNo" runat="server" MaxLength="36" /></td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
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
