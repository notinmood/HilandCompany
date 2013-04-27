<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalaryEdit.aspx.cs" Inherits="UI_Salary_SalaryEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>薪资表</title>
    <script src="../../Content/Javascript/FileExport.js" type="text/javascript"></script>
</head>
<body style="text-align:center;">
    <form id="frmSalaryEdit" runat="server">
    <center>
    <div class="title"><% =String.IsNullOrEmpty(yearMonth) ? "" : yearMonth.Insert(6, "月").Insert(4, "年")%> 薪资表</div>
	<div style="margin:0px auto;">
        <table>
            <tr>
                <td align="right" style="padding-left:0px">
                    <asp:DropDownList ID="ddlReport" runat="server" AutoPostBack="True" onselectedindexchanged="ddlReport_SelectedIndexChanged" />
                    <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" Text="导 出"/>
                </td>
            </tr>
            <tr>
                <td style="padding-left:3px">
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="true"
                        CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="PersonID" 
                        OnDataBound="gvList_DataBound" onrowcreated="gvList_RowCreated" 
                        onrowdatabound="gvList_RowDataBound" ShowFooter="true" >
                        <HeaderStyle Wrap="false" />
                        <RowStyle HorizontalAlign="Justify" Wrap="false" />
                        <FooterStyle HorizontalAlign="Right" ForeColor="Red" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ToolTip="修改员工薪资信息" ImageUrl="~/Content/Default/Images/gedit.gif" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除员工薪资" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("PersonId") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" />
                                    <%--<asp:LinkButton ID="btnEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("PersonId") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("PersonId") %>' Text="删除" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </center>
    </form>
</body>
</html>