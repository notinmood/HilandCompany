<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepartmentList.aspx.cs" Inherits="UI_Department_DepartmentList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门管理</title>
</head>
<body style="padding-top:10px">
    <form id="frmDepartment" runat="server" style="width:480px; text-align:center"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title" style="width: 100%">部门列表</div>
        <table width="100%">
            <tr align="left">
                <td>
                    状态：<asp:DropDownList ID="ddlUse" runat="server" /><asp:Button ID="btnQuery" runat="server" Text="查 询" onclick="btnQuery_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="false" Width="100%"
                        CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="DepartmentID,DepartmentCode,UseFlag" OnDataBound="gvList_DataBound">
                        <Columns>
                            <asp:BoundField HeaderText="编码" DataField="DepartmentCode" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="排序" DataField="OrderNo" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Right" Visible="false" />
                            <asp:TemplateField HeaderText="部门名称" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("DepartmentID") %>' Text='<%# Eval ("DepartmentName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="上级部门" DataField="ParentName" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="状态" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnUse" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("DepartmentID") %>'/><asp:LinkButton runat="server" ID="btnLogout" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("DepartmentID") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增根部门" ToolTip="新增根部门" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit"  runat="server" ToolTip="修改部门信息" ImageUrl="~/Content/Default/Images/gedit.gif" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除部门" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("DepartmentID") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" /><asp:ImageButton ID="ibtnAddChild" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增子部门" ToolTip="新增子部门" />
                                    <%--<asp:LinkButton ID="btnEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("DepartmentID") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("DepartmentID") %>' Text="删除" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate> 
    </asp:UpdatePanel>
    </form>
</body>
</html>