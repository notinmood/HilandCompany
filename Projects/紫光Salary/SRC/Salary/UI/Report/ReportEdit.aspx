<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportEdit.aspx.cs" Inherits="UI_Report_ReportEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报表编辑</title>
    <base target="_self" />
    <link href="../../Content/Default/Styles/base/jquery.ui.all.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Content/Default/Styles/demos.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Javascript/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Javascript/jquery.effects.core.js" type="text/javascript"></script>
    <script src="../../Content/Javascript/jquery.ui.tabs.js" type="text/javascript"></script>
</head>
<body style="margin: 0px auto; padding-top: 10px; text-align: center;">
    <form id="form1" runat="server">    <input type="hidden" runat="server" id="tabSelected" value="0" />
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs({ selected: $('#tabSelected').val(),
                'select': function (event, ui) {
                    $('#tabSelected').val(ui.index);
                }
            });
            //$("#tabs").tabs();
        });
        function validateInput() {
            if ($.trim($('#tbReportCode').val()) == '') {
                alert('报表编码不能为空！');
                $('#tbReportCode').focus();
                return false;
            }
            if ($.trim($('#tbReportName').val()) == '') {
                alert('报表名称不能为空！');
                $('#tbReportName').focus();
                return false;
            }
            return true;
        }
    </script>
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>--%>
    <div class="title">报表生成</div>
    <fieldset class="fieldset" style="width: 300px">
        <legend>报表基本信息</legend>
        <table style="width: 300px;">
            <tr>
                <td align="right">
                    报表编码：
                </td>
                <td align="left">
                    <asp:TextBox ID="tbReportCode" runat="server" MaxLength="36" /><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td align="right">
                    报表名称：
                </td>
                <td align="left">
                    <asp:TextBox ID="tbReportName" runat="server" MaxLength="36" /><font color="red">*</font>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSave" OnClientClick="return validateInput();" OnClick="btnSaveClick"
                        Text="保存" />
                    <input type="button" value="关闭" onclick="window.close();" />
                </td>
            </tr>
        </table>
    </fieldset>
    
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
    <div style="width:1200px;">
    <div style="float: left;width: 380px;">
        <fieldset class="fieldset">
            <legend>可添加项目列表</legend>
            <div class="demo" style="width: 350px;">
                <div id="tabs">
                    <ul>
                        <li><a href="#tabs-1">工资项目</a></li>
                        <li><a href="#tabs-2">福利项目</a></li>
                        <li><a href="#tabs-3">计算工资项目</a></li>
                    </ul>
                    <div id="tabs-1">
                        <asp:GridView runat="server" ID="gvFeeList" AutoGenerateColumns="false" CellPadding="2"
                            BorderStyle="Solid" EmptyDataText=" " DataKeyNames="FeeName,FeeID,FeeCode,FeeType"
                            OnDataBound="gvFeeList_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="排序" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbOrderNo" runat="server" Width="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckbSelect" runat="server" CssClass="checkBox" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="工资项目" ItemStyle-Width="240px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("FeeID") %>'
                                            Text='<%# Eval ("FeeName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="tabs-2">
                        <asp:GridView runat="server" ID="gvWelfareList" AutoGenerateColumns="false" CellPadding="2"
                            BorderStyle="Solid" EmptyDataText=" " DataKeyNames="FeeName,FeeID,FeeCode,FeeType"
                            OnDataBound="gvFeeList_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="排序" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbOrderNo" runat="server" Width="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckbSelect" runat="server" CssClass="checkBox" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="工资项目" ItemStyle-Width="240px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("FeeID") %>'
                                            Text='<%# Eval ("FeeName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="tabs-3">
                        <asp:GridView runat="server" ID="gvCalculateFeeList" AutoGenerateColumns="false"
                            CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="FeeName,FeeID,FeeCode,FeeType"
                            OnDataBound="gvFeeList_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="排序" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbOrderNo" runat="server" Width="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckbSelect" runat="server" CssClass="checkBox" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="计算工资项目" ItemStyle-Width="240px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("FeeID") %>'
                                            Text='<%# Eval ("FeeName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <!-- End demo -->
        </fieldset>
    </div>
    
    <div style="float: left; padding-left:30px; padding-top: 200px;">
        <asp:Button ID="btnAdd" runat="server" Text="→" Width="28px" OnClick="btnAdd_Click" ToolTip="添加选中" /><br /><br />
        <asp:Button ID="btnMinus" runat="server" Text="←" Width="28px" OnClick="btnMinus_Click" ToolTip="取消选中" />
    </div>

    <div style="float: left;">
        <fieldset class="fieldset" style="width: 580px; padding-bottom: 1px;">
        <legend>报表项目列表</legend>
        <br />
        <asp:GridView runat="server" ID="gvSelectedFeeList" AutoGenerateColumns="false" CellPadding="2"
            BorderStyle="Solid" EmptyDataText=" " DataKeyNames="FeeName,FeeID,FeeType" OnDataBound="gvSelectedFeeList_DataBound" Width="580px"
            onrowcancelingedit="gvSelectedFeeList_RowCancelingEdit" onrowediting="gvSelectedFeeList_RowEditing" onrowupdating="gvSelectedFeeList_RowUpdating" EditRowStyle-Height="29" RowStyle-Height="29" >
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbSelect" runat="server" CssClass="checkBox" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="排序" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Eval("OrderNo")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbOrderNo" runat="server" Text='<%# Eval("OrderNo") %>' Width="20px" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="工资项目" ItemStyle-Width="210px" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("FeeID") %>' Text='<%# Eval ("FeeName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="报表列名" ItemStyle-Width="210px" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%# Eval ("ReportFeeName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="tbReportFeeName" runat="server" Text='<%# Eval ("ReportFeeName") %>' Width="200px" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="项目类型" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("FeeType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.FeeType>(Eval("FeeType").ToString())%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnReportFeeEdit" runat="server" ToolTip="修改" ImageUrl="~/Content/Default/Images/gedit.gif" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("FeeID") %>'/>
                    </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </fieldset>
    </div>
    </div>
    <div style="padding-top: 10px; padding-bottom: 10px; margin: 0px auto; float: left;">
        <%--<asp:Button ID="btnAddAll" runat="server" Text="->>" 
                onclick="btnAddAll_Click" ToolTip="添加全部" Width="28px" />--%>
        
        <%--<asp:Button ID="btnMinusAll" runat="server" Text="<<-"  Width="28px"
                onclick="btnMinusAll_Click" ToolTip="取消全部" />--%>
    </div>
    <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </form>
</body>
</html>
