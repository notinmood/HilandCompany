<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectList.aspx.cs" Inherits="UI_Project_ProjectList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>项目列表</title>
</head>
<body>
    <form id="frmProjectList" runat="server" style="width:791px; text-align:center"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="title" style="width: 100%">项目列表</div>
        <div style="float:left; width: 280px; text-align:left; margin-left:3px;">
            <fieldset class="fieldset">
            <legend>项目分类</legend> 
            <div>
                <asp:TreeView ID="trvProjectClass" runat="server" ShowLines="true" ExpandDepth="3" 
                    BackColor="#E6FFE6" BorderColor="#D0FFD0" BorderStyle="Solid" 
                    BorderWidth="1px" Font-Size="12px" ForeColor="#336600"
                        onselectednodechanged="trvProjectClass_SelectedNodeChanged">
                    <ParentNodeStyle Font-Size="12pt" />
                    <HoverNodeStyle Font-Size="12pt" />
                    <SelectedNodeStyle Font-Size="12pt" BorderColor="#FF3300" BorderWidth="1px" />
                    <RootNodeStyle Font-Size="12pt" />
                    <NodeStyle Font-Size="12pt" />
                    <LeafNodeStyle Font-Size="12px" />
                </asp:TreeView>   
            </div>
            <div style=" padding-top:5px; display:none">
                <asp:Button runat="server" ID="btnAddProjectClass" Text="新增" />
                <asp:Button runat="server" ID="btnEditProjectClass" Text="修改" />
            </div>
            </fieldset>
        </div>
        <div style="float:right;">    
            <fieldset class="fieldset">
            <legend>项目</legend> 
            <div style="padding-top:5px; text-align:left; display:none">
                <asp:Button runat="server" ID="btnAdd" Text="新增" />
            </div>
            <div style=" padding-top:12px;">
                    <asp:GridView runat="server" ID="gvList" AutoGenerateColumns="false" Width="500px"
                        CellPadding="2" BorderStyle="Solid" EmptyDataText=" " DataKeyNames="ProjectID,UseFlag" OnDataBound="gvList_DataBound">
                        <Columns>
                            <asp:BoundField HeaderText="编码" DataField="ProjectCode" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="排序" DataField="OrderNo" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Right" Visible="false" />
                            <asp:TemplateField HeaderText="项目名称" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnView" CommandArgument='<%# Eval("ProjectID") %>' Text='<%# Eval ("ProjectName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="项目分类" DataField="ProjectClassName" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="状态" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnUse" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("ProjectID") %>'/><asp:LinkButton runat="server" ID="btnLogout" OnClick="lbtnUseLogout_Click" CommandArgument='<%# Eval("ProjectID") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                <HeaderTemplate>
                                    <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="../../Content/Default/Images/add.gif" AlternateText="新增项目" ToolTip="新增项目" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ToolTip="修改项目信息" ImageUrl="~/Content/Default/Images/gedit.gif" /><asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="删除项目" OnClientClick="return confirm('您确认要删除该项？')" OnClick="lbtnDelete_Click"  CommandArgument='<%# Eval("ProjectID") %>' ImageUrl="~/Content/Default/Images/gdelete.gif" />
                                    <%--<asp:LinkButton ID="btnEdit" Text="编辑" runat="server" CommandArgument='<%# Eval("ProjectID") %>' />
                                    <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('您确认要删除该项？')"
                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("ProjectID") %>' Text="删除" />--%>
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