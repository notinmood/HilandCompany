<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalaryList.aspx.cs" Inherits="UI_Salary_SalaryList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>薪资列表</title>
    <script src="../../Content/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() { $('#btnAdd').click(function() { return validateInput(); }); });
        function validateInput() {
            if ($.trim($('#tbYearMonth').val()) == '') {
                alert('请先选择薪资月份！');
                $('#tbYearMonth').focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="padding-top:10px">
    <form id="frmSalaryList" runat="server" style="width:15%; text-align:center;">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title" style="width: 300px">薪资列表</div>
        <table>
            <tr style="display:none">
                <td style="padding-left:3px">
                    年月：<asp:TextBox runat="server" ID="tbYearMonth" onfocus="new WdatePicker({el:this})" Style="width: 60px" AutoPostBack="True" ontextchanged="tbYearMonth_TextChanged" />
                    <asp:Button runat="server" ID="btnAdd" Text="新增薪资月表" Width="80px" onclick="btnAdd_Click" />
                </td>
            </tr>
            <tr align="left">
                <td>
                    年份：<asp:DropDownList ID="ddlYears" runat="server" /><asp:Button ID="btnQuery" runat="server" Text="查 询" onclick="btnQuery_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="false" CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="YearMonth" OnDataBound="gvList_DataBound" Width="300px" 
                        onrowcancelingedit="gvList_RowCancelingEdit" onrowupdating="gvList_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="月份" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("YearMonth") %>' Text='<%# Eval ("YearMonth") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    年月：<asp:TextBox runat="server" ID="tbYearMonth" onfocus="new WdatePicker({el:this})" Style="width: 60px" AutoPostBack="True" ontextchanged="tbYearMonth_TextChanged" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" ToolTip="新增薪资月表"  runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增薪资月表" OnClick="ibtnAdd_Click" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ToolTip="修改薪资月表" ImageUrl="~/Content/Default/Images/gedit.gif" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除薪资月表" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("YearMonth") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" />
                                    <asp:ImageButton ID="ibtnReCreate" runat="server" ToolTip="重新生成薪资表" Width="16" Height="16" OnClientClick="return confirm('您确认要重新生成该月的薪资表？')" OnClick="ibtnReCreate_Click"  CommandArgument='<%# Eval("YearMonth") %>' ImageUrl="~/Content/Default/Images/reload.ico" /><asp:ImageButton ID="ibtnFeeMonth" runat="server" ToolTip="工资项目"  Width="16" Height="16" ImageUrl="~/Content/Default/Images/setting.ico" />
                                    <asp:ImageButton ID="ibtnKelowna" runat="server" ToolTip="复制生成薪资表" Width="16" Height="16" OnClientClick="return confirm('您确认要复制生成下月的薪资表？')" OnClick="ibtnKelowna_Click"  CommandArgument='<%# Eval("YearMonth") %>' ImageUrl="~/Content/Default/Images/file_copy.ico" />
                                    <%--<asp:LinkButton ID="btnEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("YearMonth") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("YearMonth") %>' Text="删除" />--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存薪资月表" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消添加薪资月表" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                                </EditItemTemplate>
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