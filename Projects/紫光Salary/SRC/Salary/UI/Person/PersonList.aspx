<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonList.aspx.cs" Inherits="UI_Person_PersonList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>员工列表</title>
</head>
<body>
    <form id="frmProjectList" runat="server" style="width:810px; text-align:center;">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title" style="width: 100%"><%=String.IsNullOrEmpty(this.PersonType) ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.PersonType>(this.PersonType)%>人员列表</div>
        <div style="float:left; width: 233px; text-align:left; margin-left:3px;">      
            <fieldset class="fieldset">
            <legend>部门</legend> 
                <div style="display:none">
                    <asp:Button runat="server" ID="btnAddDepartment" Text="新增" />
                    <asp:Button runat="server" ID="btnModifyDepartment" Text="修改" />
                </div>
                <div>
                    <asp:TreeView ID="trvDeparment" runat="server" ShowLines="true" ExpandDepth="3" 
                        BackColor="#E6FFE6" BorderColor="#D0FFD0" BorderStyle="Solid" 
                        BorderWidth="1px" Font-Size="12px" ForeColor="#336600"
                            onselectednodechanged="trvDeparment_SelectedNodeChanged">
                        <ParentNodeStyle Font-Size="12pt" />
                        <HoverNodeStyle Font-Size="12pt" />
                        <SelectedNodeStyle Font-Size="12pt" BorderColor="#FF3300" BorderWidth="1px" />
                        <RootNodeStyle Font-Size="12pt" />
                        <NodeStyle Font-Size="12pt" />
                        <LeafNodeStyle Font-Size="12px" />
                    </asp:TreeView>
                </div>
            </fieldset>
        </div>
        <div style="float:right;">    
            <fieldset class="fieldset">
            <legend>员工</legend> 
                <div style="padding-top:5px; text-align:left; display:none">
                    状态：<asp:DropDownList ID="ddlUse" runat="server" /><asp:Button ID="btnQuery" runat="server" Text="查 询" />
                </div>
                <div style="padding-top:12px; padding-bottom:1px">
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="false" Width="565px"
                        CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="PersonID,ParentID,Dimission" OnDataBound="gvList_DataBound">
                        <Columns>
                            <asp:BoundField HeaderText="编码" DataField="PersonCode" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="姓名" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("PersonID") %>' Text='<%# Eval("PersonName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="入职日期" DataField="EntryDate" ItemStyle-Width="85px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:TemplateField HeaderText="人员类型" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("PersonType").ToString() == "0" ? "" : Salary.Core.Utility.EnumHelper.GetDescription<Salary.Biz.Eunm.PersonType>(Eval("PersonType").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="所属部门" DataField="DepartmentName" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="状态" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="labDimission"></asp:Label>
                                    <%--<asp:LinkButton runat="server" ID="btnDisdimission" Text="在职" OnClick="lbtnDimission_Click" CommandArgument='<%# Eval("PersonID") %>'/><asp:LinkButton runat="server" ID="btnDimission" Text="离职" OnClick="lbtnDimission_Click" CommandArgument='<%# Eval("PersonID") %>'/>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增员工" ToolTip="新增员工" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ToolTip="修改员工信息" ImageUrl="~/Content/Default/Images/gedit.gif" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除员工" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("PersonID") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" />
                                    <asp:ImageButton ID="ibtnAddChild" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增员工变动信息" ToolTip="新增员工变动信息" />
                                    <%--<asp:LinkButton ID="btnEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("PersonID") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("PersonID") %>' Text="删除" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
               </div>
           </fieldset>
        </div>
    </ContentTemplate> 
    </asp:UpdatePanel>
    </form>
</body>
</html>