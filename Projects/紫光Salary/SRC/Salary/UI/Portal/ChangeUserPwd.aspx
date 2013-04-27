<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeUserPwd.aspx.cs" Inherits="UI_Portal_ChangeUserPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户密码修改</title>
    <base target="_self" />
    <script language="javascript" type="text/javascript" for="document" event="onkeydown">
　　     if(event.keyCode==13 && event.srcElement.type!='button' && event.srcElement.type!='image'
　　         && event.srcElement.type!='submit' && event.srcElement.type!='reset' 
　　         && event.srcElement.type!='textarea' && event.srcElement.type!='')
　　     {
　　            if(event.srcElement.id == 'txtUserPsw')
　　                form1.document.getElementById("btnLogin").click();
　　            else
　　            event.keyCode=9;
　　     }
    </script>
</head>
<body style="text-align:center; padding-top:10px">
    <form id="form1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<div class="title">用户密码修改</div>

<div>原&nbsp;&nbsp;密&nbsp;&nbsp;码：</span><asp:TextBox ID="txtOldPwd" TextMode="Password" runat="server" MaxLength="8"></asp:TextBox><font color="red">*</font></div><br/>
<div>新&nbsp;&nbsp;密&nbsp;&nbsp;码：<asp:TextBox ID="txtNewPwd" TextMode="Password" runat="server" MaxLength="8"></asp:TextBox>&nbsp;&nbsp;
</div><br/>
<div>确认密码：<asp:TextBox ID="txtConfirmPwd" TextMode="Password" runat="server" MaxLength="8"></asp:TextBox>&nbsp;&nbsp;
</div><br/>
           <div><asp:Button ID="btnModify" runat="server" CssClass="PMButton" Text="确&nbsp;&nbsp;定" OnClick="btnModifyClick" />
                <asp:Button ID="btnCancel" runat="server" OnClientClick="window.close();return false;"
                    CssClass="PMButton" Text="关&nbsp;&nbsp;闭" /></div>
    </ContentTemplate> 
    </asp:UpdatePanel>
    </form>
</body>
</html>