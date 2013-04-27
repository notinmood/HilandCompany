<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="UI_Portal_Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>菜单</title>
    <base target="_self" />
    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
            border: 0;
        }
        body
        {
            font-family: arial, 宋体, serif;
            background-color: #CEF3FF;
            font-size: 12px;
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
        #nav
        {
            width: 160px;
            line-height: 29px;
            text-align: left; /*定义整个ul菜单的行高和背景色*/
        }
        /*==================一级目录===================*/#nav a
        {
            width: 160px;
            display: block;
            padding-left: 20px; /*Width(一定要)，否则下面的Li会变形*/
        }
        #nav li
        {
            border-bottom: #ccc 1px solid; /*下面的一条白边*/
            float: left;
            background-image: url(../../Content/Default/Images/left2.gif); /*float：left,本不应该设置，但由于在Firefox不能正常显示
 继承Nav的width,限制宽度，li自动向下延伸*/
        }
        #nav li a:hover
        {
        }
        #nav a:link
        {
            color: #004160;
            text-decoration: none;
            font-size: 12px;
        }
        #nav a:visited
        {
            color: #004160;
            text-decoration: none;
            font-size: 12px;
        }
        #nav a:hover
        {
            color: #0066FF;
            text-decoration: none;
            font-size: 12px;
        }
        /*==================二级目录===================*/#nav li ul
        {
            list-style: none;
            text-align: left;
        }
        #nav li ul li
        {
            background: #FFFFFF; /*二级目录的背景色*/
        }
        #nav li ul a
        {
            width: 160px; /* padding-left二级目录中文字向右移动，但Width必须重新设置=(总宽度-padding-left)*/
        }
        /*下面是二级目录的链接样式*/#nav li ul a:link
        {
            color: #666;
            text-decoration: none;
        }
        #nav li ul a:visited
        {
            color: #666;
            text-decoration: none;
        }
        #nav li ul a:hover
        {
            color: #000000;
            text-decoration: none;
            font-weight: normal;
            background: #F2FFCC;
            filter: FILTER: progid:DXImageTransform.Microsoft.Alpha(style=1,opacity=25,finishOpacity=100,startX=0,finishX=0,startY=0,finishY=100); /* 二级onmouseover的字体颜色、背景色*/
        }
        #nav li:hover ul
        {
            left: auto;
        }
        #nav li.sfhover ul
        {
            left: auto;
        }
        #content
        {
            clear: left;
        }
        #nav ul.collapsed
        {
            display: none;
        }
        #PARENT
        {
            width: 160px;
        }
    </style>

    <script type="text/javascript">
        $(function() { $('#lbtnReLogin').click(function() { return confirm('你确定要重新登录？'); }); })
        
    </script>

</head>
<body bgcolor="#214096" style="overflow-x: hidden">
    <form id="form1" runat="server">
    <table align="left" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td width="160" align="left" valign="top" bgcolor="#587DD6" class="shu">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="160" align="left" valign="top" bgcolor="#587DD6" class="shu">
                            <div id="PARENT">
                                <ul id="nav">
                                    <li class="LIITEM"><a href="#Menu=Salary" onclick="DoMenu('Salary')" runat="server" id="menuSalary">薪资管理</a>
                                        <ul id="Salary" class="collapsed">
                                            <%--<li class="LIITEM"><a href="../Salary/SalaryEdit.aspx?Action=Add" target="main" runat="server" id="menuAddSalary">新建薪资</a></li>--%>
                                            <li class="LIITEM"><a href="../Salary/SalaryList.aspx" target="main" runat="server" id="menuSalaryList">薪资列表</a></li>
                                        </ul>
                                    </li>
                                    <li class="LIITEM"><a href="#Menu=Report" onclick="DoMenu('Report')" runat="server" id="menuReport">报表管理</a>
                                        <ul id="Report" class="collapsed">
                                            <%--<li class="LIITEM"><a href="../Report/ReportEdit.aspx?Action=Add" runat="server" id="menuAddReport" target="main">新建报表</a></li>--%>
                                            <li class="LIITEM"><a href="../Report/ReportList.aspx" runat="server" id="menuReportList" target="main">报表管理</a></li>
                                        </ul>
                                    </li>
                                    <li class="LIITEM"><a href="#Menu=Person" onclick="DoMenu('Person')" runat="server" id="menuPerson">员工管理</a>
                                        <ul id="Person" class="collapsed">
                                            <li class="LIITEM" style="display:none"><a href="../Person/PersonList.aspx" runat="server" id="menuPersonList" target="main">员工管理</a></li>
                                            <li class="LIITEM"><a href="../Person/PersonList.aspx?&PersonType=QuanZhi" runat="server" id="menuQuanZhiPersonList" target="main">全职人员</a></li>
                                            <li class="LIITEM"><a href="../Person/PersonList.aspx?&PersonType=JianZhi" runat="server" id="menuJianZhiPersonList" target="main">兼职人员</a></li>
                                            <li class="LIITEM"><a href="../Person/PersonList.aspx?&PersonType=WaiPin" runat="server" id="menuWaiPinPersonList" target="main">外聘人员</a></li>
                                            <li class="LIITEM"><a href="../Person/PersonList.aspx?&PersonType=PaiChu" runat="server" id="menuPaiChuPersonList" target="main">派出人员</a></li>
                                            <li class="LIITEM"><a href="../Person/PersonList.aspx?&PersonType=Retire" runat="server" id="menuReTirePersonList" target="main">退休人员</a></li>
                                            <li class="LIITEM"><a href="../Person/PersonList.aspx?&PersonType=LaoWu" runat="server" id="menuLaoWuPersonList" target="main">劳务人员</a></li>
                                            <li class="LIITEM"><a href="../Person/PersonList.aspx?&PersonType=Soldier" runat="server" id="menuSoldierPersonList" target="main">军队人员</a></li>
                                        </ul>
                                    </li>
                                    <li class="LIITEM"><a href="#Menu=Fee" onclick="DoMenu('Fee')" runat="server" id="menuFee">工资设置</a>
                                        <ul id="Fee" class="collapsed">
                                            <li class="LIITEM"><a href="../Fee/FeeList.aspx?FeeType=Common&CommonFeeType=CommonSalary,Position,Float" runat="server" id="menuFeeCommonSalaryList" target="main">工资</a></li>
                                            <li class="LIITEM"><a href="../Fee/FeeList.aspx?FeeType=Common&CommonFeeType=Cooperate,Virtual" runat="server" id="menuFeeCooperateList" target="main">分摊工资</a></li>
                                            <li class="LIITEM"><a href="../Fee/FeeList.aspx?FeeType=Common&CommonFeeType=Welfare" runat="server" id="menuFeeWelfareList" target="main">福利</a></li>
                                            <li class="LIITEM"><a href="../Fee/FeeList.aspx?FeeType=Common&CommonFeeType=Service" runat="server" id="menuFeeServiceList" target="main">劳务费</a></li>
                                            <li class="LIITEM"><a href="../Fee/FeeList.aspx?FeeType=Common&CommonFeeType=Provide" runat="server" id="menuFeeProvideList" target="main">薪资发放</a></li>
                                            <li class="LIITEM"><a href="../Fee/FeeList.aspx?FeeType=Sum" runat="server" id="menuFeeSumList" target="main">工资计算</a></li>
                                        </ul>
                                    </li>
                                    <li class="LIITEM"><a href="#Menu=SysParameter" onclick="DoMenu('SysParameter')" runat="server" id="menuSysParameter">系统参数</a>
                                        <ul id="SysParameter" class="collapsed">
                                            <li class="LIITEM"><a href="../Fee/FeeList.aspx?FeeType=Parameter" runat="server" id="menuFeeParameterList" target="main">参数管理</a></li>
                                            <li class="LIITEM"><a href="../Fee/FeeList.aspx?FeeType=Tax" runat="server" id="menuFeeTaxList" target="main">个税管理</a></li>
                                        </ul>
                                    </li>
                                    
                                    <li class="LIITEM"><a href="#Menu=Project" onclick="DoMenu('Project')" runat="server" id="menuProject">项目管理</a>
                                        <ul id="Project" class="collapsed">
                                            <li class="LIITEM"><a href="../Project/ProjectClassList.aspx" runat="server" id="menuProjectClass" target="main">项目分类</a></li>
                                            <li class="LIITEM"><a href="../Project/ProjectList.aspx" runat="server" id="menuProjectML" target="main">项目目录</a></li>                                            
                                        </ul>
                                    </li>

                                    <li class="LIITEM"><a href="#Menu=Department" onclick="DoMenu('Department')" runat="server" id="menuDepartment">部门管理</a>
                                        <ul id="Department" class="collapsed">
                                            <li class="LIITEM"><a href="../Department/DepartmentList.aspx" runat="server" id="menuDepartmentList" target="main">部门管理</a></li>
                                        </ul>
                                    </li>
                                    <li class="LIITEM"><a href="#Menu=SysManager" onclick="DoMenu('SysManager')" runat="server" id="menuSysManager">系统管理</a>
                                        <ul id="SysManager" class="collapsed">
                                            <li class="LIITEM"><a href="../User/UserList.aspx" runat="server" id="menuUser" target="main">用户管理</a></li>
                                            <li class="LIITEM"><a href="../Portal/ChangeUserPwd.aspx" runat="server" id="menuChagePwd" target="main">修改密码</a></li>
                                        </ul>
                                    </li>
                                    <li class="LIITEM">
                                        <asp:LinkButton ID="lbtnReLogin" runat="server" OnClick="lbtnReLogin_Click">重新登录</asp:LinkButton>
                                    </li>
                                </ul>
                            </div>

                            <script type="text/javascript"><!--
                                var LastLeftID = "";

                                function menuFix() {
                                    var obj = document.getElementById("nav").getElementsByTagName("li");

                                    for (var i = 0; i < obj.length; i++) {
                                        obj[i].onmouseover = function() {
                                            this.className += (this.className.length > 0 ? " " : "") + "sfhover";
                                        }
                                        obj[i].onMouseDown = function() {
                                            this.className += (this.className.length > 0 ? " " : "") + "sfhover";
                                        }
                                        obj[i].onMouseUp = function() {
                                            this.className += (this.className.length > 0 ? " " : "") + "sfhover";
                                        }
                                        obj[i].onmouseout = function() {
                                            this.className = this.className.replace(new RegExp("( ?|^)sfhover\\b"), "");
                                        }
                                    }
                                }

                                function DoMenu(emid) {
                                    var obj = document.getElementById(emid);
                                    obj.className = (obj.className.toLowerCase() == "expanded" ? "collapsed" : "expanded");
                                    if ((LastLeftID != "") && (emid != LastLeftID)) //关闭上一个Menu
                                    {
                                        document.getElementById(LastLeftID).className = "collapsed";
                                    }
                                    LastLeftID = emid;
                                }

                                function GetMenuID() {

                                    var MenuID = "";
                                    var _paramStr = new String(window.location.href);

                                    var _sharpPos = _paramStr.indexOf("#");

                                    if (_sharpPos >= 0 && _sharpPos < _paramStr.length - 1) {
                                        _paramStr = _paramStr.substring(_sharpPos + 1, _paramStr.length);
                                    }
                                    else {
                                        _paramStr = "";
                                    }

                                    if (_paramStr.length > 0) {
                                        var _paramArr = _paramStr.split("&");
                                        if (_paramArr.length > 0) {
                                            var _paramKeyVal = _paramArr[0].split("=");
                                            if (_paramKeyVal.length > 0) {
                                                MenuID = _paramKeyVal[1];
                                            }
                                        }
                                        /*
                                        if (_paramArr.length>0)
                                        {
                                        var _arr = new Array(_paramArr.length);
                                        }
  
  //取所有#后面的，菜单只需用到Menu
  //for (var i = 0; i < _paramArr.length; i++)
                                        {
                                        var _paramKeyVal = _paramArr[i].split('=');
   
   if (_paramKeyVal.length>0)
   {
                                        _arr[_paramKeyVal[0]] = _paramKeyVal[1];
                                        } 
                                        }
                                        */
                                    }

                                    if (MenuID != "") {
                                        DoMenu(MenuID)
                                    }
                                }

                                GetMenuID(); //*这两个function的顺序要注意一下，不然在Firefox里GetMenuID()不起效果
                                menuFix();
--></script>

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
