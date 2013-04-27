<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestManage.aspx.cs" Inherits="Pages_TestManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Images/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                密串：
            </td>
            <td>
                <asp:TextBox ID="StrText" runat="server"  Width="800"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Sql：
            </td>
            <td>
                <asp:TextBox ID="SqlText" runat="server" TextMode="MultiLine" Width="800" Rows="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <td></td>
            <td align=center>
                <asp:Button ID="btnNoQuery" CssClass="PMButton" runat="server" Text="执行无返回值操作"
                    onclick="btnNoQuery_Click"></asp:Button>
                <asp:Button ID="btnQuery"  CssClass="PMButton" runat="server" Text="执行查询" 
                    onclick="btnQuery_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:DataGrid ID="DGResult" runat="server" AllowPaging="true" PageSize="30" BorderStyle="Solid" 
                    onpageindexchanged="DGResult_PageIndexChanged">
                    <AlternatingItemStyle Wrap="false" HorizontalAlign="Center" CssClass="gridRow" />
                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="gridRow" />
                    <HeaderStyle CssClass="gridHead" Wrap="false" HorizontalAlign="Center" />
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
