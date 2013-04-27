<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeeEdit.aspx.cs" Inherits="UI_Fee_FeeEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><% =name %>信息编辑</title>
    <base target="_self" />
    <script src="../../Content/DateYMDPicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $('#litbCalculateFee').dblclick(function() {
                addCalculateFee("litbCalculateFee");
            });
            $('#litbCommonFee').dblclick(function () {
                addCalculateFee("litbCommonFee");
            });
            $('#litbParameterFee').dblclick(function () {
                addCalculateFee("litbParameterFee");
            });
            $('#litbTaxFee').dblclick(function () {
                addCalculateFee("litbTaxFee");
            });
            $('#litbFunction').dblclick(function () {
                addCalculateFunction("litbFunction");
            });
        });
        function addCalculateFee() {
            obj = $('#tbCalculateExp');
            obj.focus();
            document.selection.createRange().text += " [" + $('#litbCalculateFee option:selected').val() + "] ";
//            $('#tbCalculateExp').val($('#tbCalculateExp').val() + "[");
//            var vSelect = $('#litbCalculateFee option:selected');
//            vSelect.clone().appendTo('#tbCalculateExp');
//            $('#tbCalculateExp').val($('#tbCalculateExp').val() + "]");
        }
        function addCalculateFee(litbName) {
            obj = $('#tbCalculateExp');
            obj.focus();
            document.selection.createRange().text += " [" + $('#' + litbName +' option:selected').val() + "] ";
        }
        function addCalculateFunction(litbName) {
            obj = $('#tbCalculateExp');
            obj.focus();
            document.selection.createRange().text += " " + $('#' + litbName + ' option:selected').text() + "(  ) ";
        }
        function validateInput() {
            if ($.trim($('#tbFeeCode').val()) == '') {
                alert('编码不能为空！');
                $('#tbFeeCode').focus();
                return false;
            }
            if ($.trim($('#tbFeeName').val()) == '') {
                alert('名称不能为空！');
                $('#tbFeeName').focus();
                return false;
            }
//            if ($.trim($('#ddlFeeType').val()) == '') {
//                alert("请选择类型！");
//                $('#ddlFeeType').focus();
//                return false;
//            }
////            if ($.trim($('#ddlFee').val()) != '' && $.trim($('#ddlCalculateSign').val()) == '') {
////                alert("请选择运算符号！");
////                $('#ddlCalculateSign').focus();
////                return false;
////            }
//            if ($.trim($('#ddlFeeType').val()) == '6' && $.trim($('#ddlTaxTargetFee').val()) == '') {
//                alert("请选择应税项目！");
//                $('#ddlTaxTargetFee').focus();
//                return false;
//            }
            if ($.trim($('#ddlFeeType').val()) == '6' && $.trim($('#tbTaxBaseValue').val()) == '') {
                alert("扣除标准不能为空！");
                $('#tbTaxBaseValue').focus();
                return false;
            }
            return true;
        }
        function storeCaret(textE) {
            if (textE.createTextRange)
                textE.caretPos = document.selection.createRange().duplicate();
        }
        function insetAtCaret(textE, text) {
            alert(textE.createTextRange);
            if (textE.createTextRange && textE.caretPos) {
                var carePos = textE.caretPos;
                carePos.text = carePos.text.charAt(caretPos.text.length - 1) == ' ' ? text + ' ' : text;
            }
            else {
                textE.value = text;
            }
        }
        function addText(txt) {
            obj = $('#tbCalculateExp');
            obj.focus();
            document.selection.createRange().text += " " + txt + " ";
//            selection = document.selection;
//            obj.focus();
//            if (selection && selection.createRange) {
//                var sel = selection.createRange();
//                alert(sel.text);
//                sel.text = txt;
//            }
//            else {
//               obj.vlaue += txt;
//            }
        }
    </script>
</head>
<body style="text-align:center; padding-top:10px;">
    <form id="form1" runat="server">
        <div class="title"><% =String.IsNullOrEmpty(yearMonth) ? "" : yearMonth.Insert(6, "月").Insert(4, "年")%> <% =name %>信息编辑</div>
        <fieldset class="fieldset" style="text-align:left; width:580px; margin-top:10px;">
        <legend><% =name %>基本信息</legend>  
        <table style="width:580px;  margin-top:5px; margin-bottom:10px; border-width:1px;">
            <tr>
                <td align="right"><% =name %>编码：</td>
                <td align="left"><asp:TextBox ID="tbFeeCode" runat="server" MaxLength="36" /><font color="red">*</font></td>
                <td></td><td></td>
            </tr>
            <tr>
                <td align="right"><% =name %>名称：</td>
                <td align="left">
                    <asp:TextBox ID="tbFeeName" runat="server" MaxLength="36" /><font color="red">*</font>
                </td>
                <td align="right"><asp:Label ID="lblParent" runat="server" Text="上级："></asp:Label></td>
                <td align="left"><%--AutoPostBack="True" onselectedindexchanged="ddlFee_SelectedIndexChanged"--%>
                    <asp:DropDownList ID="ddlFee" runat="server" Enabled="false" ondatabound="ddlFee_DataBound" />
                </td>
            </tr>
            <tr id="trDefaultValue" runat="server">
                <td align="right"><% =enumFeeType != null ? enumFeeType.Equals(Salary.Biz.Eunm.FeeType.Parameter) ? "参数" : "默认" : "默认"%>值：</td>
                <td align="left"><asp:TextBox ID="tbDefaultValue" runat="server" MaxLength="36" Width="60px" /></td>
                <td align="right"></td>
                <td align="left"></td>
            </tr>
            <tr id="trCalculateSign" runat="server">
                <td align="right">运算符号：</td>
                <td align="left"><asp:DropDownList ID="ddlCalculateSign" runat="server"/></td>
                <td align="right"></td>
                <td align="left">
                    <asp:DropDownList ID="ddlFeeType" runat="server" AutoPostBack="True" onselectedindexchanged="ddlFeeType_SelectedIndexChanged" />
                </td>
            </tr>
            <tr id="trTax" runat="server">
                <td align="right">启用日期：</td>
                <td align="left"><asp:TextBox ID="tbStartDate" runat="server" onfocus="new WdatePicker({el:this})" Width="100px" /><font color="red">*</font></td>
                <td align="right"><asp:Label ID="lblTaxBaseValue" runat="server" Text ="扣除标准："></asp:Label></td>
                <td align="left">
                    <asp:TextBox ID="tbTaxBaseValue" runat="server" MaxLength="36" Width="60px" />
                    <asp:Label ID="lblTaxBaseValueEnd" runat="server">元<font color="red">*</font></asp:Label>
                </td>
            </tr>
            <tr style="display:none"><%--id="trStartDate" runat="server"--%>
                <td align="right">应税项目：</td>
                <td align="left"><asp:DropDownList ID="ddlTaxTargetFee" runat="server" ondatabound="ddlTaxTargetFee_DataBound" /><font color="red">*</font></td>
            </tr>
            <tr id="trTaxRate" runat="server">
                <td align="right" valign="top">计算标准：</td>
                <td align="left" colspan="3" style="padding-left:3px">
                    <asp:GridView runat="server" ID="gvTaxList" AutoGenerateColumns="false" Width="450px"
                            CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="TaxID" 
                            onrowdatabound="gvTaxList_RowDataBound" onrowcancelingedit="gvTaxList_RowCancelingEdit" 
                                    onrowediting="gvTaxList_RowEditing" onrowupdating="gvTaxList_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="应税额（元）" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# String.Format("{0:c0}", Eval("QuantumStart"))%>~<%# String.Format("{0:c0}", Eval("QuantumEnd"))%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="40px" ID="tbQuantumStart" runat="server" Text='<%# String.Format("{0:##}", Eval("QuantumStart"))%>'/>~<asp:TextBox Width="40px" ID="tbQuantumEnd" runat="server" Text='<%# String.Format("{0:##}", Eval("QuantumEnd"))%>'/>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="税率(%)" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# Eval("Rate") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="30px" ID="tbRate" runat="server" Text='<%# Eval("Rate").ToString().Replace(".00","") %>'/>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="速算扣除数（元）" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# String.Format("{0:c2}", Eval("Subtract")) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="60px" ID="tbSubtract" runat="server" Text='<%# Eval("Subtract") %>'/>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="扣除(%)" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# Eval("SubtractMultiple")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="30px" ID="tbSubtractMultiple" runat="server" Text='<%# String.Format("{0:##}", Eval("SubtractMultiple")) %>'/>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="扣除(元)" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# String.Format("{0:c0}", Eval("SubtractMoney"))%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="30px" ID="tbSubtractMoney" runat="server" Text='<%# Eval("SubtractMoney").ToString().Replace(".00","") %>'/>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增税率" ToolTip="新增税率" OnClick="btnAdd_Click" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit"  runat="server" ToolTip="修改税率信息" ImageUrl="~/Content/Default/Images/gedit.gif" CommandArgument='<%# Eval("TaxID") %>'
                                      CausesValidation="false" CommandName="Edit" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除税率" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("TaxID") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" />
                                    <%--<asp:LinkButton ID="btnCommonEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("TaxID") %>'
                                      CausesValidation="false" CommandName="Edit" />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("TaxID") %>' Text="删除" />--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存税率信息" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消修改税率" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                                    <%--<asp:LinkButton ID="Linkbutton4" runat="server" CausesValidation="false" CommandName="Update">保存</asp:LinkButton>
                                    <asp:LinkButton ID="Linkbutton5" runat="server" CausesValidation="false" CommandName="Cancel">取消</asp:LinkButton>--%>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="trCalculateExp" runat="server">
                <td align="right" valign="top">计算公式：</td>
                <td align="left" colspan="3">
                    <asp:TextBox ID="tbCalculateExp" runat="server" TextMode="MultiLine" Width="98%" Height="600px" onClientClick="storeCaret($(this));" onkeyup="storeCaret($(this));" Rows="10" />
                </td>
            </tr>
            <tr id="trlitbFee" runat="server">
                <td align="center" colspan="4">
                    <div style="padding-bottom:5px; padding-left:35px; ">
                        <input type="button" style="width:20px" value="+" onclick="addText($(this).val());" />
                        <input type="button" style="width:20px" value="-" onclick="addText($(this).val());" />
                        <input type="button" style="width:20px" value="*" onclick="addText($(this).val());" />
                        <input type="button" style="width:20px" value="/" onclick="addText($(this).val());" />
                        <input type="button" style="width:20px" value="(" onclick="addText($(this).val());" />
                        <input type="button" style="width:20px" value=")" onclick="addText($(this).val());" />
                    </div>
                    <div style="float:left; padding-top:5px; padding-left:10px;">
                        <div style="float:left; padding-bottom:5px;">可参与运算的项目：</div>
                        <div style=" float:left;"><div style="float:left;">计算：</div><div style="clear:both;"></div><asp:ListBox ID="litbCalculateFee" runat="server" Height="200px" /></div>
                        <div style=" float:left; padding-top:15px;"><asp:Button runat="server" ID="btnCalculateUp" Text="↑" onclick="btnCalculateUp_Click" /></div>
                        <div style=" float:left;"><div style="float:left;">组成：</div><div style="clear:both;"></div><asp:ListBox ID="litbCommonFee" runat="server" Height="200px" /></div>
                        <div style=" float:left; padding-top:15px;"><asp:Button runat="server" ID="btnCommonUp" Text="↑" onclick="btnCommonUp_Click" /></div>
                        <div style=" float:left;"><div style="float:left;">函数：</div><div style="clear:both;"></div><asp:ListBox ID="litbFunction" runat="server" Height="200px" /></div>
                        <div style=" float:left; padding-top:15px;"><asp:Button runat="server" ID="btnFunctionUp" Text="↑" onclick="btnCommonUp_Click" /></div>
                    </div>
                        <div style="clear:both;"></div>
                        <div style=" float:left; display:none;"><div style="float:left;">参数：</div><div style="clear:both;"></div><asp:ListBox ID="litbParameterFee" runat="server" Height="200px" /></div>
                        <div style=" float:left; display:none; padding-top:15px;"><asp:Button runat="server" ID="btnParameterUp" Text="↑" onclick="btnParameterUp_Click" /></div>
                        <div style=" float:left; display:none;"><div style="float:left;">个税：</div><div style="clear:both;"></div><asp:ListBox ID="litbTaxFee" runat="server" Height="200px" /></div>
                        <div style=" float:left; display:none; padding-top:15px;"><asp:Button runat="server" ID="btnTaxUp" Text="↑" onclick="btnTaxUp_Click" /></div>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">排序ID：</td>
                <td align="left"><%--<asp:TextBox ID="tbOrderNo" runat="server" MaxLength="36" />--%></td>
                <td></td><td></td>
            </tr>
            <tr>
                <td colspan="4" align="center" style=" padding-top:10px;">
                    <asp:Button runat="server" ID="btnSave" OnClientClick="return validateInput();" OnClick="btnSaveClick" Text="保存" />
                    <input type="button" value="关闭" onclick="window.close();" />
                </td>
            </tr>
        </table>
        </fieldset>
    </form>
</body>
</html>
