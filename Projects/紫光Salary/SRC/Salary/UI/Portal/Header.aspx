<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Header.aspx.cs" Inherits="UI_Portal_Header" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Content/Default/Styles/Base.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
    <div id="headerleft"></div>
    <div id="headerright">
    <div id="dhleft"></div>
    <div id="dhright"><p class="welcome" style="color: #3366CC">欢迎您：<asp:Label runat="server" ID="lbLoginName"></asp:Label>
    <img src="../../Content/Default/Images/GIF2.gif" /><a href="MainPage.aspx?GoodsTarget=General" target="main" style="color: #3366CC">工作台</a><img 
            src="../../Content/Default/Images/GIF2.gif" /><a onclick="showPageWithDimension(420,240,'./ChangeUserPwd.aspx');" style="cursor: hand; color: #3366CC">修改密码</a><img 
            src="../../Content/Default/Images/GIF2.gif" /><a target="main" href="#" onclick="javascript:window.parent.close();return false;" style="color: #3366CC">退出</a></p></div>
    </div>    
    </div>
    </form>
</body>
</html>
