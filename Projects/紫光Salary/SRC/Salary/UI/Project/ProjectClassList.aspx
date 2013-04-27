<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectClassList.aspx.cs" Inherits="UI_Project_ProjectClassList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>项目分类列表</title>
</head>
<body style="padding-top:10px">
    <form id="frmProjectClassList" runat="server" style="width:500px; text-align:center;"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title" style="width: 96%">项目分类列表</div>
        <table>
            <tr align="left">
                <td>
                     状态：<asp:DropDownList ID="ddlUse" runat="server" /><asp:Button ID="btnQuery" runat="server" Text="查 询" onclick="btnQuery_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="false" Width="480px"
                        CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="ProjectClassID,UseFlag" OnDataBound="gvList_DataBound">
                        <Columns>
                            <asp:BoundField HeaderText="编码" DataField="ProjectClassID" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="项目分类名称" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("ProjectClassID") %>' Text='<%# Eval ("ProjectClassName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="上级项目分类" DataField="ParentClassName" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="状态" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnUse" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("ProjectClassID") %>'/><asp:LinkButton runat="server" ID="btnLogout" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("ProjectClassID") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="75px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增分类" ToolTip="新增分类" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit"  runat="server" ToolTip="修改分类信息" ImageUrl="~/Content/Default/Images/gedit.gif" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除分类" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("ProjectClassID") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" /><asp:ImageButton ID="ibtnAddChild" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增子分类" ToolTip="新增子分类" />
                                    <%--<asp:LinkButton ID="btnEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("ProjectClassID") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("ProjectClassID") %>' Text="删除" />--%>
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