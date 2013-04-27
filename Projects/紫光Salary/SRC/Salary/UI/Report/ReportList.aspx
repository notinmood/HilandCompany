<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportList.aspx.cs" Inherits="UI_Report_ReportList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报表列表</title>
</head>
<body style="padding-top:10px">
    <form id="frmReportList" runat="server" style="width:340px; text-align:center">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title" style="width: 340px">报表列表</div>
        <table>
            <tr align="left">
                <td>
                    状态：<asp:DropDownList ID="ddlUse" runat="server" /><asp:Button ID="btnQuery" runat="server" Text="查 询" onclick="btnQuery_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="false" 
                        CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="ReportID,UseFlag" OnDataBound="gvList_DataBound" Width="340px">
                        <Columns>
                            <asp:BoundField HeaderText="编码" DataField="ReportCode" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="报表名称" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("ReportID") %>' Text='<%# Eval ("ReportName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="状态" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnUse" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("ReportID") %>'/><asp:LinkButton runat="server" ID="btnLogout" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("ReportID") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" ToolTip="新增报表"  runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增报表" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ToolTip="修改报表信息" ImageUrl="~/Content/Default/Images/gedit.gif" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除报表" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("ReportID") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" />
                                    <%--<asp:LinkButton ID="btnEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("ReportID") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("ReportID") %>' Text="删除" />--%>
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