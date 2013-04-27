<%@ Page Language="C#" AutoEventWireup="true" CodeFile="加密测试.cs" Inherits="Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        <br />
        <br />
        <asp:Button ID="加密" runat="server" OnClick="加密_Click" Text="加密" />
    
    </div>
    </form>
</body>
</html>
