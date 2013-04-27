<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="UI_Portal_Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head2" runat="server">
    <title>薪资系统</title>
</head>
<FRAMESET id="topFrame" border="0" framespacing="0" rows="86,90%,20" frameborder="NO"
    cols="*" noresize>
    <FRAME name="top" src="Header.aspx" noResize scrolling=no>
    <FRAMESET id="mainFrame" border="0" frameSpacing=0 rows=* frameBorder=NO cols=150,5,* noresize scrolling="NO">
        <frame name="left" src="Menu.aspx" noresize target="main">
        <FRAME border=0 name="bar" frameSpacing=0 src="bar.htm" cols=3,* frameBorder=NO noResize scrolling=no >
        <FRAME name="main" src="#" scrolling="auto" frameborder="no">
    </FRAMESET>
    <frame src="Footer.htm" name="botFrame" frameborder="no" scrolling="NO" noresize>    
</FRAMESET>
<noframes>
<body><form id="form1" runat="server"><div>您的浏览器不支持框架。</div></form></body>
</noframes>
</html>

