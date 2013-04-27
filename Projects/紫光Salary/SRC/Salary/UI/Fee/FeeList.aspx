<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeeList.aspx.cs" Inherits="UI_Fee_FeeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="Salary.Core.Utility" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工资项目列表</title>
</head>
<body style="padding-top:10px;">
    <form id="frmFeeList" runat="server" style="width:500px;"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title" style="width: 100%">
            <% =String.IsNullOrEmpty(yearMonth) ? "" : yearMonth.Insert(6, "月").Insert(4, "年")%> 工资<% =EnumHelper.GetDescription(this.enumFeeType)%>项目列表
        </div>
        <table>
            <tr align="left">
                <td>
                    <asp:DropDownList ID="ddlFee" runat="server" AutoPostBack="True" onselectedindexchanged="ddlFee_SelectedIndexChanged" />
                    <%--状态：<asp:DropDownList ID="ddlUse" runat="server" /><asp:Button ID="btnQuery" runat="server" Text="查 询" onclick="btnQuery_Click" />--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="false" Width="500px"
                        CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="FeeCode,FeeType,UseFlag,FeeID" OnDataBound="gvList_DataBound">
                        <Columns>
                            <asp:BoundField HeaderText="编码" DataField="FeeCode" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="项目" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("FeeID") %>' Text='<%# Eval("FeeName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参数值" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# String.Format("{0:#,##0.00}", Eval("DefaultValue")) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="运算符号" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("CalculateSign") == null || String.IsNullOrEmpty(Eval("CalculateSign").ToString().Trim()) || Eval("CalculateSign").ToString() == "0" ? "" : EnumHelper.GetDescription<Salary.Biz.Eunm.CalculateSign>(Eval("CalculateSign").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目类型" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("FeeType").ToString() == "0" ? "" : EnumHelper.GetDescription<Salary.Biz.Eunm.FeeType>(Eval("FeeType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="状态" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnUse" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("FeeID") %>'/><asp:LinkButton runat="server" ID="btnLogout" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("FeeID") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="启用日期" DataField="StartDate" ItemStyle-Width="85px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增工资项目" ToolTip="新增工资项目" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ToolTip="修改工资项目信息" ImageUrl="~/Content/Default/Images/gedit.gif" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除工资项目" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("FeeID") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" />
                                    <asp:ImageButton ID="ibtnAddChild" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增子项目" ToolTip="新增子项目" />
                                    <%--<asp:LinkButton ID="btnEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("FeeCode") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("FeeCode") %>' Text="删除" />--%>
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