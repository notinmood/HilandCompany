<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonEdit.aspx.cs" Inherits="UI_Person_PersonEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>员工信息编辑</title>
    <link href="../../Content/Default/Styles/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Default/Styles/demos.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Javascript/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Javascript/jquery.effects.core.js" type="text/javascript"></script>
    <script src="../../Content/Javascript/jquery.ui.tabs.js" type="text/javascript"></script>
    <script src="../../Content/DateYMDPicker/WdatePicker.js" type="text/javascript"></script>
  
</head>
<body style="text-align: center">
    <form id="form1" runat="server" style="padding-top:10px">    <input type="hidden" runat="server" id="tabSelected" value="0" />
    <script type="text/javascript">
        $(function () {
            //alert($('#tabSelected').val());
            $("#tabs").tabs({ selected: $('#tabSelected').val(),
                'select': function (event, ui) {
                    $('#tabSelected').val(ui.index);
                    //alert($('#tabSelected').val());
                }
            });
        }); 
        function validateInput() {
            if ($.trim($('#tbPersonCode').val()) == '') {
                alert('人员编码不能为空！');
                $('#tbPersonCode').focus();
                return false;
            }
            if ($.trim($('#tbPersonName').val()) == '') {
                alert('姓名不能为空！');
                $('#tbPersonName').focus();
                return false;
            }
            if ($.trim($('#ddlDepartment').val()) == '') {
                alert("请选择部门！");
                $('#ddlDepartment').focus();
                return false;
            }
            if ($.trim($('#ddlPersonType').val()) == '') {
                alert("请人员类型！");
                $('#ddlPersonType').focus();
                return false;
            }
            if ($.trim($('#tbEntryDate').val()) == '') {
                alert("请选择入职日期！");
                $('#tbEntryDate').focus();
                return false;
            }
            if ($.trim($('#tbEntryDate').val()) != '' && $.trim($('#tbLeftDate').val()) != '' && $.trim($('#tbEntryDate').val()) == $.trim($('#tbLeftDate').val())) {
                alert("离职日期应大于入职日期！");
                $('#tbLeftDate').focus();
                return false;
            }
            return true;
        }

        function ControlInputDecimal(obj) {
            var reg1 = /^[1-9]{1}[0-9]*\.?[0-9]{0,2}$/;
            var length = obj.value.length;
            if (!reg1.exec(obj.value)) {
                if (length == 1)
                    obj.value = '';
                if (length > 1)
                    obj.value = obj.value.substring(0, length - 1);
            }
        }
    </script>
        <div class="title"><% =String.IsNullOrEmpty(yearMonth) ? "" : yearMonth.Insert(6, "月").Insert(4, "年")%> 员工信息编辑</div>
        <div class="demo" style="width:720px;">
            <div id="tabs">
	            <ul>
		            <li><a href="#tabs-1">员工基本信息</a></li>
		            <li id="liCommon" runat="server"><a href="#tabs2">基本工资管理</a></li>
                    <li id="liPosition" runat="server"><a href="#tabs3">岗位工资管理</a></li>
                    <li id="liFloat" runat="server"><a href="#tabs4">浮动工资管理</a></li>
		            <li id="liCooperate" runat="server"><a href="#tabs5">合作工资管理</a></li>
		            <li id="liVirtual" runat="server"><a href="#tabs6">虚拟工资管理</a></li>
		            <li id="liService" runat="server"><a href="#tabs7">劳务费管理</a></li>

	            </ul>
	            <div id="tabs-1">
                    <fieldset class="fieldset" style="width:550px;">
                    <legend>员工基本信息</legend>
                        <table style="width: 90%;">
                            <tr>
                                <td align="right">人员编码：</td>
                                <td align="left"><asp:TextBox ID="tbPersonCode" runat="server" MaxLength="36" Width="85px" /><font color="red">*</font></td>
                                <td></td><td></td>
                            </tr>
                            <tr>
                                <td align="right">姓名：</td>
                                <td align="left"><asp:TextBox ID="tbPersonName" runat="server" MaxLength="36" Width="85px" /><font color="red">*</font></td>
                                <td align="right">所属部门：</td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" ondatabound="ddlDepartment_DataBound" /><!-- AutoPostBack="True" onselectedindexchanged="ddlDepartment_SelectedIndexChanged"-->
                                    <font color="red">*</font>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">人员类型：</td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlPersonType" runat="server" /><font color="red">*</font>
                                </td>
                                <td align="right">所属项目：</td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlProject" runat="server" /><!-- AutoPostBack="True" ondatabound="ddlProject_DataBound" onselectedindexchanged="ddlProject_SelectedIndexChanged"-->
                                </td>
                            </tr>
                            <tr>
                                <td align="right">入职日期：</td>
                                <td align="left"><asp:TextBox ID="tbEntryDate" onfocus="new WdatePicker({el:this})" runat="server" Width="85px" /><font color="red">*</font></td>
                                <td align="right">离职日期：</td>
                                <td align="left"><asp:TextBox ID="tbLeftDate" onfocus="new WdatePicker({el:this})" Width="85px" runat="server" ForeColor="Red" /></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return validateInput();" OnClick="btnSaveClick" Text="保存" />
                                    <input type="button" value="关闭" onclick="window.close();" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
	            </div>


	            <div id="tabs2" runat="server">
		            <fieldset class="fieldset" style="width:303px">
                    <legend>基本工资管理</legend>
                    <asp:GridView runat="server" ID="gvCommonList" AutoGenerateColumns="false" CellPadding="2" BorderStyle="Solid" EmptyDataText=" " 
                        DataKeyNames="FeeID,FeeCode,FeeValue,FeeName" onrowcancelingedit="gvBaseList_RowCancelingEdit" OnDataBound="gvBaseList_DataBound" 
                        onrowediting="gvBaseList_RowEditing" onrowupdating="gvBaseList_RowUpdating" EditRowStyle-Height="29" RowStyle-Height="29" Width="300px" ShowFooter="True" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="Silver" FooterStyle-VerticalAlign="Bottom">
                        <Columns>
                            <asp:TemplateField HeaderText="项目名称" ItemStyle-HorizontalAlign="Left" FooterText="合计：">
                                <ItemTemplate>
                                    <asp:Label ID="lblFeeName" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="项目类型" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("FeeType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.FeeType>(Eval("FeeType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="部门" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("DepartmentName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" Visible="false" FooterText="合计：">
                                <ItemTemplate>
                                    <%# Eval("ProjectName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目金额" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# String.Format("{0:#,##0.00}", Eval("FeeValue")) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="60px" ID="tbFeeValue" runat="server" Text='<%# String.Format("{0:#.00}", Eval("FeeValue")) %>' MaxLength="20" onpropertychange="ControlInputDecimal(this)" />
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCommonEdit" runat="server" ToolTip="修改项目金额" ImageUrl="~/Content/Default/Images/gedit.gif" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("FeeID") %>'/>
                                </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存员工工资项目信息" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消修改员工工资项目" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                                <%--<asp:LinkButton ID="Linkbutton4" runat="server" CausesValidation="false" CommandName="Update">保存</asp:LinkButton>
                                <asp:LinkButton ID="Linkbutton5" runat="server" CausesValidation="false" CommandName="Cancel">取消</asp:LinkButton>--%>
                            </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
	            </div>
                
	            <div id="tabs3" runat="server">
		            <fieldset class="fieldset" style="width:303px">
                    <legend>岗位工资管理</legend>
                    <asp:GridView runat="server" ID="gvPositionList" AutoGenerateColumns="false" CellPadding="2" BorderStyle="Solid" EmptyDataText=" " 
                        DataKeyNames="FeeID,FeeCode,FeeValue,FeeName" onrowcancelingedit="gvBaseList_RowCancelingEdit" OnDataBound="gvBaseList_DataBound" 
                        onrowediting="gvBaseList_RowEditing" onrowupdating="gvBaseList_RowUpdating" EditRowStyle-Height="29" RowStyle-Height="29" Width="300px" ShowFooter="True" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="Silver" FooterStyle-VerticalAlign="Bottom">
                        <Columns>
                            <asp:TemplateField HeaderText="项目名称" ItemStyle-HorizontalAlign="Left" FooterText="合计：">
                                <ItemTemplate>
                                    <asp:Label ID="lblFeeName" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="项目类型" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("FeeType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.FeeType>(Eval("FeeType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="部门" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("DepartmentName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" Visible="false" FooterText="合计：">
                                <ItemTemplate>
                                    <%# Eval("ProjectName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目金额" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# String.Format("{0:#,##0.00}", Eval("FeeValue")) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="60px" ID="tbFeeValue" runat="server" Text='<%# String.Format("{0:#.00}", Eval("FeeValue")) %>' MaxLength="20" onpropertychange="ControlInputDecimal(this)" />
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCommonEdit" runat="server" ToolTip="修改项目金额" ImageUrl="~/Content/Default/Images/gedit.gif" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("FeeID") %>'/>
                                </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存员工工资项目信息" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消修改员工工资项目" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                                <%--<asp:LinkButton ID="Linkbutton4" runat="server" CausesValidation="false" CommandName="Update">保存</asp:LinkButton>
                                <asp:LinkButton ID="Linkbutton5" runat="server" CausesValidation="false" CommandName="Cancel">取消</asp:LinkButton>--%>
                            </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
	            </div>
                
	            <div id="tabs4" runat="server">
		            <fieldset class="fieldset" style="width:303px">
                    <legend>浮动工资管理</legend>
                    <asp:GridView runat="server" ID="gvFloatList" AutoGenerateColumns="false" CellPadding="2" BorderStyle="Solid" EmptyDataText=" " 
                        DataKeyNames="FeeID,FeeCode,FeeValue,FeeName" onrowcancelingedit="gvBaseList_RowCancelingEdit" OnDataBound="gvBaseList_DataBound" 
                        onrowediting="gvBaseList_RowEditing" onrowupdating="gvBaseList_RowUpdating" EditRowStyle-Height="29" RowStyle-Height="29" Width="300px" ShowFooter="True" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="Silver" FooterStyle-VerticalAlign="Bottom">
                        <Columns>
                            <asp:TemplateField HeaderText="项目名称" ItemStyle-HorizontalAlign="Left" FooterText="合计：">
                                <ItemTemplate>
                                    <asp:Label ID="lblFeeName" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="项目类型" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("FeeType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.FeeType>(Eval("FeeType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="部门" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("DepartmentName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" Visible="false" FooterText="合计：">
                                <ItemTemplate>
                                    <%# Eval("ProjectName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目金额" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# String.Format("{0:#,##0.00}", Eval("FeeValue")) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="60px" ID="tbFeeValue" runat="server" Text='<%# String.Format("{0:#.00}", Eval("FeeValue")) %>' MaxLength="20" onpropertychange="ControlInputDecimal(this)" />
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCommonEdit" runat="server" ToolTip="修改项目金额" ImageUrl="~/Content/Default/Images/gedit.gif" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("FeeID") %>'/>
                                </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存员工工资项目信息" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消修改员工工资项目" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                                <%--<asp:LinkButton ID="Linkbutton4" runat="server" CausesValidation="false" CommandName="Update">保存</asp:LinkButton>
                                <asp:LinkButton ID="Linkbutton5" runat="server" CausesValidation="false" CommandName="Cancel">取消</asp:LinkButton>--%>
                            </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
	            </div>

                
	            <div id="tabs5" runat="server">
		            <fieldset class="fieldset" style="width:653px">
                    <legend>分摊管理</legend>
                    <asp:GridView runat="server" ID="gvCooperateList" AutoGenerateColumns="false" CellPadding="2" BorderStyle="Solid" EmptyDataText=" " 
                        DataKeyNames="FeeID,FeeCode,FeeValue,FeeName,DepartmentID,ProjectID" onrowcancelingedit="gvBaseList_RowCancelingEdit" OnDataBound="gvBaseList_DataBound" 
                        onrowediting="gvBaseList_RowEditing" onrowupdating="gvBaseList_RowUpdating" EditRowStyle-Height="29" RowStyle-Height="29" Width="650px" ShowFooter="True" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="Silver" FooterStyle-VerticalAlign="Bottom">
                        <Columns>
                            <asp:TemplateField HeaderText="项目名称" ItemStyle-HorizontalAlign="Left" FooterText="合计：">
                                <ItemTemplate>
                                    <asp:Label ID="lblFeeName" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="项目类型" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("FeeType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.FeeType>(Eval("FeeType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="部门" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%# Eval("DepartmentName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" FooterText="合计：">
                                <ItemTemplate>
                                    <%# Eval("ProjectName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目金额" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# String.Format("{0:#,##0.00}", Eval("FeeValue")) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="60px" ID="tbFeeValue" runat="server" Text='<%# String.Format("{0:#.00}", Eval("FeeValue")) %>' MaxLength="20" onpropertychange="ControlInputDecimal(this)" />
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCommonEdit" runat="server" ToolTip="修改项目金额" ImageUrl="~/Content/Default/Images/gedit.gif" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("FeeID") %>'/>
                                </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存员工工资项目信息" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消修改员工工资项目" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                                <%--<asp:LinkButton ID="Linkbutton4" runat="server" CausesValidation="false" CommandName="Update">保存</asp:LinkButton>
                                <asp:LinkButton ID="Linkbutton5" runat="server" CausesValidation="false" CommandName="Cancel">取消</asp:LinkButton>--%>
                            </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
	            </div>
                
	            <div id="tabs6" runat="server">
		            <fieldset class="fieldset" style="width:653px">
                    <legend>分摊管理</legend>
                    <asp:GridView runat="server" ID="gvVirtualList" AutoGenerateColumns="false" CellPadding="2" BorderStyle="Solid" EmptyDataText=" " 
                        DataKeyNames="FeeID,FeeCode,FeeValue,FeeName,DepartmentID,ProjectID" onrowcancelingedit="gvBaseList_RowCancelingEdit" OnDataBound="gvBaseList_DataBound" 
                        onrowediting="gvBaseList_RowEditing" onrowupdating="gvBaseList_RowUpdating" EditRowStyle-Height="29" RowStyle-Height="29" Width="650px" ShowFooter="True" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="Silver" FooterStyle-VerticalAlign="Bottom">
                        <Columns>
                            <asp:TemplateField HeaderText="项目名称" ItemStyle-HorizontalAlign="Left" FooterText="合计：">
                                <ItemTemplate>
                                    <asp:Label ID="lblFeeName" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="项目类型" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("FeeType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.FeeType>(Eval("FeeType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="部门" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%# Eval("DepartmentName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" FooterText="合计：">
                                <ItemTemplate>
                                    <%# Eval("ProjectName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目金额" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# String.Format("{0:#,##0.00}", Eval("FeeValue")) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="60px" ID="tbFeeValue" runat="server" Text='<%# String.Format("{0:#.00}", Eval("FeeValue")) %>' MaxLength="20" onpropertychange="ControlInputDecimal(this)" />
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCommonEdit" runat="server" ToolTip="修改项目金额" ImageUrl="~/Content/Default/Images/gedit.gif" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("FeeID") %>'/>
                                </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存员工工资项目信息" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消修改员工工资项目" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                                <%--<asp:LinkButton ID="Linkbutton4" runat="server" CausesValidation="false" CommandName="Update">保存</asp:LinkButton>
                                <asp:LinkButton ID="Linkbutton5" runat="server" CausesValidation="false" CommandName="Cancel">取消</asp:LinkButton>--%>
                            </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
	            </div>

                
	            <div id="tabs7" runat="server">
		            <fieldset class="fieldset" style="width:303px">
                    <legend>劳务费管理</legend>
                    <asp:GridView runat="server" ID="gvServiceList" AutoGenerateColumns="false" CellPadding="2" BorderStyle="Solid" EmptyDataText=" " 
                        DataKeyNames="FeeID,FeeCode,FeeValue,FeeName,DepartmentName,ProjectName,DepartmentID,ProjectID" onrowcancelingedit="gvBaseList_RowCancelingEdit" OnDataBound="gvBaseList_DataBound" 
                        onrowediting="gvBaseList_RowEditing" onrowupdating="gvBaseList_RowUpdating" EditRowStyle-Height="29" RowStyle-Height="29" Width="300px" ShowFooter="True" FooterStyle-HorizontalAlign="Right" FooterStyle-BackColor="Silver" FooterStyle-VerticalAlign="Bottom">
                        <Columns>
                            <asp:TemplateField HeaderText="项目名称" ItemStyle-HorizontalAlign="Left" FooterText="合计：">
                                <ItemTemplate>
                                    <asp:Label ID="lblFeeName" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="项目类型" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("FeeType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.FeeType>(Eval("FeeType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="部门" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" Visible="false">
                                <ItemTemplate>
                                    <%# Eval("DepartmentName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" Visible="false" FooterText="合计：">
                                <ItemTemplate>
                                    <%# Eval("ProjectName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterStyle Font-Bold="true" BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目金额" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# String.Format("{0:#,##0.00}", Eval("FeeValue")) %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox Width="60px" ID="tbFeeValue" runat="server" Text='<%# String.Format("{0:#.00}", Eval("FeeValue")) %>' MaxLength="20" onpropertychange="ControlInputDecimal(this)" />
                                </EditItemTemplate>
                                <FooterStyle BorderColor="Silver" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCommonEdit" runat="server" ToolTip="修改项目金额" ImageUrl="~/Content/Default/Images/gedit.gif" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("FeeID") %>'/>
                                </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnSave" runat="server" ToolTip="保存员工工资项目信息" ImageUrl="~/Content/Default/Images/save.gif" CausesValidation="false" CommandName="Update" /><asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="取消修改员工工资项目" ImageUrl="~/Content/Default/Images/return.gif" CausesValidation="false" CommandName="Cancel" />
                                <%--<asp:LinkButton ID="Linkbutton4" runat="server" CausesValidation="false" CommandName="Update">保存</asp:LinkButton>
                                <asp:LinkButton ID="Linkbutton5" runat="server" CausesValidation="false" CommandName="Cancel">取消</asp:LinkButton>--%>
                            </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
	            </div>
            </div>
        </div><!-- End demo -->
        <div runat="server" id="divBaseFee"></div>
        <div runat="server" id="divBaseFeeDepartmentProject"></div>  
    </form>
</body>
</html>
