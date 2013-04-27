<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="UI_UserList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户列表</title>
</head>
<body style="padding-top:10px">
    <form id="frmUserList" runat="server" style="width:450px;"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title" style="width: 100%">用户列表</div>
        <table>
            <tr align="left">
                <td style="padding-left:0">
                    状态：<asp:DropDownList ID="ddlUse" runat="server" /><asp:Button ID="btnQuery" runat="server" Text="查 询" onclick="btnQuery_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="false" Width="450px"
                        CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="UserId,Logout" OnDataBound="gvList_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="序号" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate><%# Container.DataItemIndex +1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="姓名" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("UserId") %>' Text='<%# Eval ("UserName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="登录名" DataField="LogonName" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="用户类型" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("UserType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.UserType>(Eval("UserType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="用户状态" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnUse" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("UserId") %>'/><asp:LinkButton runat="server" ID="btnLogout" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("UserId") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" ToolTip="添加用户"  runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="添加用户" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ToolTip="修改用户信息" ImageUrl="~/Content/Default/Images/gedit.gif" />
                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除用户" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("UserId") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" />
                                    <asp:ImageButton ID="btnResetPassword" runat="server" ToolTip="重置密码" OnClientClick="return confirm('您确认要重置该用户的密码？')"  OnClick="lbtnResetPassword_Click" CommandArgument='<%# Eval("UserId") %>' Width="16" Height="16" ImageUrl="~/Content/Default/Images/help.ico" />
                                    
                                    <%--<asp:LinkButton ID="btnEdit" Visible="false" Text="编辑" runat="server" CommandArgument='<%# Eval("UserId") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("UserId") %>' Text="删除" />--%>
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