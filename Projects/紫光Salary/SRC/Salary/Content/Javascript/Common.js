$(function() {
    $("form").submit(function() {
//        pageWait();
    });
    $('input[readOnly]').addClass("inputTextReadOnly");
    $('textarea').height(40);
    $('textarea').bind('propertychange', function() {
        var height = $(this).scroll()[0].scrollHeight;
        if (height > 40) {
            $(this).height(height);
        }
    });
});

var x = document.getElementsByTagName('textarea');
for (var i = 0; i < x.length; i++) {
    if (x[i].getAttribute('maxlength')) {
        x[i].onkeypress = doKeyPress;
        x[i].onkeydown = doKeyDown;
        x[i].onbeforepaste = doBeforePaste;
        x[i].onpaste = doPaste;
        x[i].ondrop = doDrop;
    }
}

function doKeyPress() {
    var obj = event.srcElement;
    if (obj.maxlength != null) {
        var len = obj.maxlength;
    }
    var oTR = obj.document.selection.createRange();
    if (oTR.text.length >= 1)
        event.returnValue = true;
    else if (obj.value.length > len - 1)
        event.returnValue = false;
}
function doKeyDown() {
    var obj = event.srcElement;
    if (obj.maxlength != null) {
        var len = obj.maxlength;
    }
    setTimeout(function() {
        if (obj.value.length > len) {
            var oTR = window.document.selection.createRange();
            oTR.moveStart("character", -1 * (obj.value.length - len));
            oTR.text = "";
        }
    }, 1)
}
function doBeforePaste() {
    event.returnValue = false;
}
function doPaste() {
    event.returnValue = false;
    var obj = event.srcElement;
    if (obj.maxlength != null) {
        var len = obj.maxlength;
    }
    var oTR = obj.document.selection.createRange();
    var iInsertLength = len - obj.value.length + oTR.text.length;
    oTR.text = window.clipboardData.getData("Text").substring(0, iInsertLength);
}
function doDrop() {
    event.returnValue = false;
}


//页面加载或者等待时调用方法
function pageWait() {
    $("form").append("<div class='divForWait'><div class='divForWaitText'>页面加载中...</div></div>");
    $('.divForWait').width($(window).width());
    $('.divForWait').height($(window).height());
    $(".divForWait").css({ "position": "absolute", "overflow": "hidden", "top": "0", "left": "0", "z-index": "300000", "background": "white", "filter": "alpha(Opacity = 50)" });
    $(".divForWait").offset({ top: 0, left: 0 });
    var pTop = $(window).scrollTop() + ($(window).height() - $('.divForWaitText').height()) / 2;
    pTop = pTop < 0 ? 0 : pTop;
    $(".divForWaitText").css("font-size", "20");
    $(".divForWaitText").css({ "position": "absolute", "overflow": "hidden", "background": "white", "border-style": "solid", "border-width": "0", "top": pTop, "z-index": "500000" });
    $(".divForWait").css("text-align", "center");
}
function checkRskCode(name) {
    //obj = document.getElementById(name);
    //obj=document.all.code;
    var patrn = /^[C,K]{1}[0-9]{14}$/;
    if (!patrn.exec(name.value)) {
        alert("合法输入为:C或K打头，后面14个数字!");
    }
    else {
        return true;
    }
}
/******************************************************************************************
检查参数em是不是有效的email地址
	
应用举例:  isEmail("feng_yue@163.com")   // return true
isEmail("@2342.com"); isEmail("feng@@.com"); isEmail("feng@ss@") // return false
	   
*******************************************************************************************/
function isEmail(em) {
    var n = em.indexOf("@");
    var lastn = em.indexOf("@", n + 1);

    //判断四种情况 1.没有@符号 2.@符号在第一位 3.@符号在最后一位 4.有多个@符号
    if (n <= 0 || (n + 1) == em.length || lastn >= 0) {
        return false;  	// 为非法email返回false.
    }
    else {
        return true;
    }
}
//**********************************判断数据项非空**************************************//
function isEmpty(inputStr) {
    inputStr = trim(inputStr)
    //inputStr = inputStr.Trim().toString();
    if (inputStr.length == 0) {
        return true
    }
    return false
}
//************去掉字符串的前后空格
function trim(inputStr) {
    inputStr = inputStr.toString()
    var iStrHead = 0
    var iStrTail = inputStr.length
    for (; iStrHead < iStrTail; iStrHead++) {
        if (inputStr.charAt(iStrHead) != " ") break
    }
    for (; iStrTail > iStrHead; iStrTail--) {
        if (inputStr.charAt(iStrTail - 1) != " ") break
    }
    return inputStr.substring(iStrHead, iStrTail)
}
//************判断数据项是integer型


function isInteger(inputVal) {
    inputStr = inputVal.toString()
    for (var i = 0; i < inputStr.length; i++) {
        var oneChar = inputStr.charAt(i)
        if (oneChar < "0" || oneChar > "9") {
            return false
        }
    }
    return true
}
//************判断数据项是double型


function isDouble(inputVal) {
    inputStr = inputVal.toString()
    oneDecimal = false
    for (var i = 0; i < inputStr.length; i++) {
        var oneChar = inputStr.charAt(i)
        if (oneChar == "." && !oneDecimal) {
            oneDecimal = true
            continue
        }
        if (oneChar < "0" || oneChar > "9") {
            return false
        }
    }
    return true
}

//************判断数据项为0

function isZero(inputStr) {
    var inputVal = parseFloat(inputStr)
    if (inputVal == 0) {
        return true
    } else {
        return false
    }
}
//*********判断输入项类型是否合法日期


function isCorrectDate(inputStr) {
    if (inputStr.length != 8) {
        return false
    }
    for (var i = 0; i < 8; i++) {
        var oneChar = inputStr.charAt(i)
        if (oneChar < "0" || oneChar > "9") {
            return false
        }
    }
    if (!isDate(inputStr)) {
        return false
    } else {
        return true
    }
}

//*********判断是否闰年

function isRyear(inputInt) {
    if (inputInt % 100 == 0 && inputInt % 400 == 0 || inputInt % 100 != 0 && inputInt % 4 == 0) {
        return true
    } else {
        return false
    }
}
//*********判断日期是否合法

function isDate(inputStr) {
    var year = parseFloat(inputStr.substring(0, 4))
    var month = parseFloat(inputStr.substring(4, 6))
    var day = parseFloat(inputStr.substring(6, 8))
    if (month < 1 || month > 12 || day < 1 || day > 31 || year < 1000 || year > 2050) {
        return false
    }
    else if ((month == 4 || month == 6 || month == 9 || month == 11) && (day > 30)) {
        return false
    }
    else if (isRyear(year) && month == 2 && day > 29 || !isRyear(year) && month == 2 && day > 28) {
        return false
    } else {
        return true
    }
}

//*********直接调用函数验证日期所有合法性


function CheckDate(inputStr, name) {
    var inputValue = form1[inputStr].value
    var inputForm = form1[inputStr]
    if (isEmpty(inputValue)) {
        alert(name + "不能为空！")
        inputForm.focus()
        return false
    }
    else if (!isInteger(inputValue)) {
        alert(name + "输入项应为整数！")
        inputForm.focus()
        return false
    }
    else if (!isCorrectDate(inputValue)) {
        alert(name + "输入格式不合法！\n正确输入应为YYYYMMDD格式！")
        inputForm.focus()
        return false
    }
    else {
        return true
    }
}


//*********发生日期不能大于终结日期*****(直接调用函数验证两个日期所有合法性)

function CompareDate1(beginDate, bdName, endDate, edName) {
    var bdValue = form1[beginDate].value
    var edValue = form1[endDate].value
    if (!CheckDate(beginDate, bdName)) {
        return false
    }
    else if (!CheckDate(endDate, edName)) {
        return false
    }
    else if (bdValue > edValue) {
        alert(bdName + "不能大于" + edName)
        form1[beginDate].focus()
        return false
    }
    else {
        return true
    }
}

//*********终结日期必须大于发生日期*****(直接调用函数验证两个日期所有合法性)

function CompareDate2(beginDate, bdName, endDate, edName) {
    var bdValue = form1[beginDate].value
    var edValue = form1[endDate].value
    if (!CheckDate(beginDate, bdName)) {
        return false
    }
    else if (!CheckDate(endDate, edName)) {
        return false
    }
    else if (bdValue >= edValue) {
        alert(edName + "必须大于" + bdName)
        form1[beginDate].focus()
        return false
    }
    else {
        return true
    }
}
//*************************************验证时间的合法性****************************************//

//************判断输入项类型是否合法时间


function isCorrectTime(inputStr) {
    if (inputStr.length != 4) {
        return false
    }
    for (var i = 0; i < inputStr.length; i++) {
        var oneChar = inputStr.charAt(i)
        if (oneChar < "0" || oneChar > "9") {
            return false
        }
    }
    if (!isTime(inputStr)) {
        return false
    } else {
        return true
    }
}

//************判断时间是否合法

function isTime(inputStr) {
    var hour = parseFloat(inputStr.substring(0, 2))
    var minute = parseFloat(inputStr.substring(2, 4))
    //      var second = parseFloat(inputStr.substring(4,6))
    //      if (hour < 0 || hour > 24 || minute < 0 || minute > 59 || second < 0 || second > 59){
    if (hour < 0 || hour > 24 || minute < 0 || minute > 59) {
        return false
    }
    else if (hour == 24 && minute > 0) {
        return false
    }
    else {
        return true
    }
}

//*********直接调用函数验证时间所有合法性


function CheckTime(inputStr, name) {
    var inputValue = form1[inputStr].value
    var inputForm = form1[inputStr]
    if (isEmpty(inputValue)) {
        alert(name + "不能为空！")
        inputForm.focus()
        return false
    }
    else if (!isInteger(inputValue)) {
        alert(name + "输入项应为整数！")
        inputForm.focus()
        return false
    }
    else if (!isCorrectTime(inputValue)) {
        alert(name + "输入类型不合法！\n正确输入应为HHMM格式！")
        inputForm.focus()
        return false
    }
    else {
        return true
    }
}

//************起始时间不能大于截止时间,State请传入1；截止时间必须大于起始时间，State请传入2

function CompareTime(beginTime, bdName, endTime, edName, State) {
    //alert("传入参数为" + State)
    var bdValue = form1[beginTime].value
    var edValue = form1[endTime].value
    if (State != "1" && State != "2") {
        alert("最后一个参数请输入1或者2")
        return false
    }
    else if (!CheckTime(beginTime, bdName)) {
        return false
    }
    else if (!CheckTime(endTime, edName)) {
        return false
    }
    else if (State == "1" && bdValue > edValue) {
        alert(bdName + "不能大于" + edName)
        form1[beginTime].focus()
        return false
    }
    else if (State == "2" && bdValue >= edValue) {
        alert(edName + "必须大于" + bdName)
        form1[beginTime].focus()
        return false
    }
    else {
        return true
    }
}

function CompareDate(beginDate, bdName, endDate, edName) {
    if (!isEmpty(beginDate) && !isEmpty(endDate)) {
        if (!isCorrectDate(beginDate.replace(/-/g, ""))) {
            alert(bdName + "输入格式不合法！\n正确输入应为YYYYMMDD格式！")
            return false;
        }
        else if (!isCorrectDate(endDate.replace(/-/g, ""))) {
            alert(edName + "输入格式不合法！\n正确输入应为YYYYMMDD格式！")
            return false;
        }
        else if (beginDate > endDate) {
            alert(bdName + "不能大于" + edName)
            return false
        }
        else {
            return true
        }
    }
    else {
        return true;
    }
}

//************打开一个新窗口

function openNewWindow(URL) {
    var page = URL;
    windowprops = "height=350,width=500,location=no,scrollbars=yes,status=no,menubars=no,toolbars=no,resizable=yes";
    window.open(page, "Popup", windowprops);
    //	self.moveTo(0,0);
}
//************判断输入项中是否有引号

function isYinhao(inputStr) {
    //inputStr = input.toString()
    for (var i = 0; i < inputStr.length; i++) {
        var oneChar = inputStr.charAt(i)
        if (oneChar == "'" || oneChar == "\"") {
            return true
            //break
        }
    }
    return false
}

function roundFloat(f, point) {
    var b = 1;
    var count = parseInt(point);

    for (i = 0; i < count; i++) {
        b = b * 10;
    }

    f = Math.round(f * b) / b;
    return f;
}
/*888 电话 888*/
function isPhone(ph) {
    if (ph.length == 0)
        return true;
    var flag = ph.search(/[^0-9,\(,\),-]/);
    if (flag < 0) /*888 格式正确 888*/
        return true;
    return false;
}

var timerID = null
var timerRunning = false
function MakeArray(size) {
    this.length = size;
    for (var i = 1; i <= size; i++) {
        this[i] = "";
    }
    return this;
}

function stopclock() {
    if (timerRunning)
        clearTimeout(timerID);
    timerRunning = false
}

function showtime() {
    var now = new Date();
    var year = now.getYear();
    var month = now.getMonth() + 1;
    var date = now.getDate();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();
    var day = now.getDay();
    Day = new MakeArray(7);
    Day[0] = "星期天";
    Day[1] = "星期一";
    Day[2] = "星期二";
    Day[3] = "星期三";
    Day[4] = "星期四";
    Day[5] = "星期五";
    Day[6] = "星期六";
    var timeValue = "";
    timeValue += year + "年";
    timeValue += ((month < 10) ? "0" : "") + month + "月";
    timeValue += date + "日 ";
    timeValue += (Day[day]) + " ";
    timeValue += ((hours <= 12) ? hours : hours - 12);
    timeValue += ((minutes < 10) ? ":0" : ":") + minutes;
    timeValue += ((seconds < 10) ? ":0" : ":") + seconds;
    timeValue += (hours < 12) ? "上午" : "下午";
    document.jsfrm.currTime.value = timeValue;
    timerID = setTimeout("showtime()", 1000);
    timerRunning = true
}

function startclock() {
    stopclock();
    showtime()
}



/*************************************************
功能：	用于客户端获得OU
**************************************************/

function showSelectUserDialog(url, selectFlag, selectName, selectID, listObjType, rootOrg, canSelectRoot, selectSort, multiSelect, ShowMyOrg, selectObjType, maxLevel) {
    var xmlData = createDomDocument("<Params />");
    var root = xmlData.documentElement;
    if (selectFlag == "User") {
        appendNode(root, "listObjType", listObjType);       // 65535
        appendNode(root, "multiSelect", multiSelect);       // 2
        appendNode(root, "rootOrg", rootOrg);              // "中国海关"
        appendNode(root, "canSelectRoot", canSelectRoot);  // "true"
        appendNode(root, "selectSort", selectSort);         // 1
        appendNode(root, "ShowMyOrg", ShowMyOrg);          // "1"
        appendNode(root, "selectObjType", selectObjType);   // 2

    }
    else if (selectFlag == "OU") {
        appendNode(root, "listObjType", listObjType);       // 1
        appendNode(root, "rootOrg", rootOrg);              // "中国海关"
        appendNode(root, "canSelectRoot", canSelectRoot);  //"true"
        appendNode(root, "selectSort", selectSort);         // 1
        appendNode(root, "multiSelect", multiSelect);       // 2
        appendNode(root, "ShowMyOrg", ShowMyOrg);          // "1"
        appendNode(root, "selectObjType", selectObjType);   // 1
        appendNode(root, "maxLevel", maxLevel);             // 2
    }

    var strPath = "http://" + url + "/Accreditadmin/exports/selectOGU.aspx"; //不同WebServer
    var returnValue = showSelectUserDialogMulitServer(xmlData, strPath);
    if (returnValue != null && returnValue.lastIndexOf("GUID") != -1) {
        var doc = new ActiveXObject("Msxml2.DOMDocument");
        doc.loadXML(returnValue);
        document.getElementById(selectID).value = doc.childNodes[0].childNodes[0].getAttribute("GUID");
        document.getElementById(selectName).value = doc.childNodes[0].childNodes[0].getAttribute("DISPLAY_NAME");
    }
}


function showSelectUserDialogs(url, selectFlag, selectName, selectID) {
    var xmlData = createDomDocument("<Params />");
    var root = xmlData.documentElement;
    if (selectFlag == "User") {
        appendNode(root, "listObjType", 65535);
        appendNode(root, "multiSelect", 3);
        appendNode(root, "rootOrg", "中国海关\\青岛海关");
        appendNode(root, "canSelectRoot", "true");
        appendNode(root, "selectSort", 1);
        appendNode(root, "ShowMyOrg", "1");
        appendNode(root, "selectObjType", 2);

    }
    else if (selectFlag == "OU") {
        appendNode(root, "listObjType", 1);
        appendNode(root, "multiSelect", 3);
        appendNode(root, "rootOrg", "中国海关\\青岛海关");
        appendNode(root, "canSelectRoot", "true");
        appendNode(root, "selectSort", 1);
        appendNode(root, "ShowMyOrg", "1");
        appendNode(root, "selectObjType", 1);
        appendNode(root, "maxLevel", 1);

    }


    var strPath = "http://" + url + "/Accreditadmin/exports/selectOGU.aspx"; //不同WebServer
    var returnValue = showSelectUserDialogMulitServer(xmlData, strPath);
    if (returnValue != null && returnValue.lastIndexOf("GUID") != -1) {
        document.getElementById(selectID).value = "";
        document.getElementById(selectName).value = "";
        var doc = new ActiveXObject("Msxml2.DOMDocument");
        doc.loadXML(returnValue);
        for (var i = 0; i < doc.childNodes[0].childNodes.length; i++) {
            document.getElementById(selectID).value = document.getElementById(selectID).value + doc.childNodes[0].childNodes[i].getAttribute("GUID") + ",";
            document.getElementById(selectName).value = document.getElementById(selectName).value + doc.childNodes[0].childNodes[i].getAttribute("DISPLAY_NAME") + ",";
        }

    }
}

//xmlDoc:传入的Xml结构参数
//strUrl：待处理的Url地址，要求最后地址内容“/exports/selectOGU.aspx”

function showSelectUserDialogMulitServer(xmlDoc, strUrl)//跨服务器数据调用
{
    var sFeature = "dialogWidth:360px; dialogHeight:400px;center:yes;help:no;resizable:yes;scroll:no;status:no";
    var strXml = xmlDoc;
    if (typeof (xmlDoc) == "object")
        strXml = xmlDoc.xml;
    window.clipboardData.setData("Text", strXml);
    showModalDialog(strUrl, null, sFeature);

    var strResult = window.clipboardData.getData("Text");
    window.clipboardData.clearData("Text");
    return strResult; //文本返回
}


//建立Automation对象
function createObject(strName, strDescription) {
    try {
        var stm = new ActiveXObject(strName);

        return stm;
    }
    catch (e) {
        var strMsg = "您的计算机没有安装" + strName + "，或者您的浏览器为该网页没有设置本地访问权限";

        if (strDescription)
            strMsg += ", " + strDescription;
        throw strMsg;
    }
}



//建立xmlDocument对象
function createDomDocument() {
    var xmlData;

    try {
        xmlData = createObject("Msxml2.DOMDocument");
    }
    catch (e) {
        xmlData = createObject("Msxml.DOMDocument");
    }

    xmlData.async = false;

    if (arguments.length > 0) {
        var xml = arguments[0];
        if (typeof (xml) == "string")
            xmlData.loadXML(xml);
        else
            if (typeof (xml) == "object")
            xmlData.loadXML(xml.xml);
    }

    return xmlData;
}

//在指定节点上增加一个节点

function appendNode(root, strNodeName) {
    var xmlDoc = root.ownerDocument;
    var nodeText = "";

    if (arguments.length > 2)
        nodeText = arguments[2];

    var node = xmlDoc.createNode(1, strNodeName, "");

    if (nodeText.toString().length > 0)
        node.text = nodeText;

    root.appendChild(node);

    return node;
}

/*弹出窗口居中*/
function openNewWin(url, formwidth, formheight) {
    if (formwidth == -1) formwidth = window.screen.width;
    if (formheight == -1) formheight = window.screen.height * 0.92;
    var sLeft, sTop;
    sLeft = (window.screen.width - formwidth) / 2;
    sTop = (window.screen.height - formheight) / 2 - 20;

    if (formwidth == undefined) {
        formwidth = window.screen.width * 0.7;
        sLeft = (window.screen.width - formwidth) / 2;
    }
    if (formheight == undefined) {
        formheight = window.screen.height * 0.63;
        sTop = (window.screen.height - formheight) / 2;
    }
    var sFeature = "width=" + formwidth + "px,height=" + formheight + "px,top=" + sTop + "px,left=" + sLeft + "px,scrollbars=yes,resizable=yes";
    window.open(url, "_blank", sFeature);
}

function openNewMaxWin(url) {
    var sLeft, sTop;
    //sLeft = (window.screen.width) / 2;
    //sTop = (window.screen.height) / 2;
    sLeft = 0;
    sTop = 0;

    var sFeature = "width=" + (window.screen.width - 12) + "px,height=" + (window.screen.height - 68) + "px,top=" + sTop + "px,left=" + sLeft + "px,scrollbars=yes,resizable=yes";
    window.open(url, "_blank", sFeature);
}
/*例：<a href="javascript:void(openWin('http://www.baidu.com/', 600, 500));">Test</a>*/

/*以showModleDialog方式弹出居中窗口*/
function PMshowModalDialog(url, objPara, formWidth, formHeight) {
    if (formWidth == undefined) {
        formWidth = window.screen.width;
    }
    if (formHeight == undefined) {
        formHeight = window.screen.height;
    }
    if (objPara == undefined) {
        objPara = "no para";
    }
    var sFeature = "dialogHeight:" + formWidth + "px; dialogWidth:" + formHeight + "px; center:yes; help:no; resizable:no; status:no; scroll:auto";
    var ret = showModalDialog(url, objPara, sFeature);
    //alert("para from son is:"+ret);
}

function PMshowMaxDialog(url, objPara) {
    var formWidth = window.screen.width;
    var formHeight = window.screen.width;
    if (objPara == undefined) {
        objPara = "no para";
    }
    var sFeature = "dialogHeight:" + formWidth + "px; dialogWidth:" + formHeight + "px; center:yes; help:no; resizable:no; status:no; scroll:auto";
    var ret = showModalDialog(url, objPara, sFeature);
    //alert("para from son is:"+ret);
}
function PreFillZero(num, length) {
    var newNum = num;
    for (var i = 0; i < (length - num.toString().length); i++) {
        newNum = "0" + newNum;
    }
    return newNum;
}


//getBytes用正则表达式来判断字符串中包含汉字的个数，包含的汉字都放到数组cArr中，这样cArr的长度就是汉字的总数。

String.prototype.getBytes = function() {
    var cArr = this.match(/[^\x00-\xff]/ig);
    return this.length + (cArr == null ? 0 : cArr.length);
}

String.prototype.ReplaceAll = function(AFindText, ARepText) {
    raRegExp = new RegExp(AFindText, "g");
    return this.replace(raRegExp, ARepText)
}

//参数一传入字符串，参数二最大长度。

//true  - 符合长度要求。

//false - 不符合长度要求。

function CheckLenB(aValue, aLen) {
    var BLen = aValue.getBytes();
    if (BLen > aLen) {
        return false;
    }
    return true;
}

//参数一控件ID，参数三最大长度。

//返回值 true-符合长度要求，false-不符合长度要求。

//例子：onblur="CheckLenBAlert(this,30);"
function CheckLenBAlert(aObj, aLen) {
    aObj.value = aObj.value.ReplaceAll('"', '\'');
    if (!CheckLenB(aObj.value, aLen)) {
        alert("长度超过限制：已输入的长度 " +
      aObj.value.getBytes() + "，允许最大长度 " + aLen + "！");
        aObj.focus();
        return false;
    }
    return true;
}

function check(v, vt) {

    var tL = { "da": { "0": "零", "1": "壹", "2": "贰", "3": "叁", "4": "肆", "5": "伍", "6": "陆", "7": "柒", "8": "捌", "9": "玖" },
        "mL": { "9": "亿", "8": "仟", "7": "佰", "6": "拾", "5": "万", "4": "仟", "3": "佰", "2": "拾", "1": "元" },
        "fL": { "0": "角", "1": "分" }
    }

    var tmp = "tL." + ((vt == 0) ? "da" : (vt == 1) ? "mL" : "fL") + "[" + v + "]";
    return eval(tmp);
}

function Money2Word(str) {
    str = str.split(".");
    var fj = new Array();
    var tmp = "";
    var na = new Array();
    for (var i = 0; i < str[0].length; i++) {
        fj[i] = str[0].substr(i, 1);
    }
    var t = 1;
    for (var i = 0; i < str[0].length; i++) {
        n = fj.pop();
        //na[i]=(n!=0)?check(n,0)+check((i+1),1):((i==4)?check(5,1):((t!=0)?check(n,0):""));
        na[i] = (n != 0) ? check(n, 0) + check((i + 1), 1) : ((i == 0) ? "元" : ((i == 4) ? check(5, 1) : ((t != 0) ? check(n, 0) : "")));
        t = n;
    }
    na.reverse()

    if (str.length > 1) {
        if (str[1] == 0) {
            na.push("整");
            return na.join("");
        }
        if (str[0].substr(str[0].length - 1) == 0) {
            na.push("零");
        }

        for (var i = 0; i < str[1].length; i++) {
            var n = str[1].substr(i, 1);
            na.push((n == 0) ? ((na[na.length - 1] == "零") ? "" : "零") : check(n, 0) + check(i, 2));
        }

    } else { na.push("整") }
    return na.join("");
}


function convertCurrency(currencyDigits) {
    // Constants:
    var MAXIMUM_NUMBER = 99999999999.99;
    // Predefine the radix characters and currency symbols for output:
    var CN_ZERO = "零";
    var CN_ONE = "壹";
    var CN_TWO = "贰";
    var CN_THREE = "叁";
    var CN_FOUR = "肆";
    var CN_FIVE = "伍";
    var CN_SIX = "陆";
    var CN_SEVEN = "柒";
    var CN_EIGHT = "捌";
    var CN_NINE = "玖";
    var CN_TEN = "拾";
    var CN_HUNDRED = "佰";
    var CN_THOUSAND = "仟";
    var CN_TEN_THOUSAND = "万";
    var CN_HUNDRED_MILLION = "亿";
    //var CN_SYMBOL = "￥:";
    var CN_SYMBOL = "";
    var CN_DOLLAR = "元";
    var CN_TEN_CENT = "角";
    var CN_CENT = "分";
    var CN_LI = "厘";
    var CN_INTEGER = "整";

    // Variables:
    var integral; // Represent integral part of digit number.
    var decimal; // Represent decimal part of digit number.
    var outputCharacters; // The output result.
    var parts;
    var digits, radices, bigRadices, decimals;
    var zeroCount;
    var i, p, d;
    var quotient, modulus;

    // Validate input string:
    currencyDigits = currencyDigits.toString();
    if (currencyDigits == "") {
        //alert("请输入要转换的数字!");
        currencyDigits = "0";
        //return "";
    }
    if (currencyDigits.match(/[^,.\d]/) != null) {
        alert("数字中含有非法字符!");
        return "";
    }
    //if ((currencyDigits).match(/^((\d{1,3}(,\d{3})*(.((\d{3},)*\d{1,3}))?)|(\d+(.\d+)?))$/) == null) {
    if ((currencyDigits).match(/^((\d{1,3}(,\d{3})*(.((\d{3},)*\d{1,3}))?)|(\d+(.\d{0,3}0*)?))$/) == null) {
        alert("错误的数字格式!");
        return "";
    }

    // Normalize the format of input digits:
    currencyDigits = currencyDigits.replace(/,/g, ""); // Remove comma delimiters.
    currencyDigits = currencyDigits.replace(/^0+/, ""); // Trim zeros at the beginning.
    // Assert the number is not greater than the maximum number.
    if (Number(currencyDigits) > MAXIMUM_NUMBER) {
        alert("超出转换最大范围!");
        return "";
    }

    // Process the coversion from currency digits to characters:
    // Separate integral and decimal parts before processing coversion:
    parts = currencyDigits.split(".");
    if (parts.length > 1) {
        integral = parts[0];
        decimal = parts[1];
        // Cut down redundant decimal digits that are after the second.
        decimal = decimal.substr(0, 3);
    }
    else {
        integral = parts[0];
        decimal = "";
    }
    // Prepare the characters corresponding to the digits:
    digits = new Array(CN_ZERO, CN_ONE, CN_TWO, CN_THREE, CN_FOUR, CN_FIVE, CN_SIX, CN_SEVEN, CN_EIGHT, CN_NINE);
    radices = new Array("", CN_TEN, CN_HUNDRED, CN_THOUSAND);
    bigRadices = new Array("", CN_TEN_THOUSAND, CN_HUNDRED_MILLION);
    decimals = new Array(CN_TEN_CENT, CN_CENT, CN_LI);
    // Start processing:
    outputCharacters = "";
    // Process integral part if it is larger than 0:
    if (Number(integral) > 0) {
        zeroCount = 0;
        for (i = 0; i < integral.length; i++) {
            p = integral.length - i - 1;
            d = integral.substr(i, 1);
            quotient = p / 4;
            modulus = p % 4;
            if (d == "0") {
                zeroCount++;
            }
            else {
                if (zeroCount > 0) {
                    outputCharacters += digits[0];
                }
                zeroCount = 0;
                outputCharacters += digits[Number(d)] + radices[modulus];
            }
            if (modulus == 0 && zeroCount < 4) {
                outputCharacters += bigRadices[quotient];
            }
        }
        outputCharacters += CN_DOLLAR;
    }
    // Process decimal part if there is:
    if (decimal != "") {
        for (i = 0; i < decimal.length; i++) {
            d = decimal.substr(i, 1);
            if (d != "0") {
                outputCharacters += digits[Number(d)] + decimals[i];
            }
        }
    }
    // Confirm and return the final output string:
    if (outputCharacters == "") {
        outputCharacters = CN_ZERO + CN_DOLLAR;
    }
    if (decimal == "") {
        outputCharacters += CN_INTEGER;
    }
    outputCharacters = CN_SYMBOL + outputCharacters;
    return outputCharacters;
}

function PrintDiv(Id) {
    var mStr;
    mStr = window.document.body.innerHTML;
    var mWindow = window;
    window.document.body.innerHTML = Id.innerHTML;
    mWindow.print();
    window.document.body.innerHTML = mStr;
}




function ShowDialog(bgCtlId, ctlId) {
    var wnd = $(window);
    var doc = $(document);

    $("#" + bgCtlId).height(doc.height());
    $("#" + bgCtlId).width(doc.width());
    $("#" + bgCtlId).css({ "position": "absolute", "overflow": "hidden", "top": "0", "left": "0", "z-index": "300000", "background": "gray", "filter": "alpha(Opacity = 50)" });
    $("#" + bgCtlId).show();

    var pLeft = (wnd.width() - $("#" + ctlId).width()) / 2;
    var pTop = doc.scrollTop() + (wnd.height() - $("#" + ctlId).height()) / 2;
    pTop = pTop < 0 ? 0 : pTop;
    pLeft = pLeft < 0 ? 0 : pLeft;

    $("#" + ctlId).css({ "position": "absolute", "overflow": "hidden", "background": "white", "border-style": "solid", "border-width": "2", "top": pTop, "left": pLeft, "z-index": "500000" });
    $("#" + ctlId).show();
}

function CloseDialog(bgCtlId, ctlId) {
    $("#" + bgCtlId).hide();
    $("#" + ctlId).hide();
}


function openPageWithDimension(width, height, url) {
    sw = window.screen.width;
    sh = window.screen.height;
    w = width;
    h = height;
    if (h > sh) {
        h = sh * 0.7;
    }
    if (w > sw) {
        w = sw * 0.7;
    }
    l = (sw - w) / 2;
    t = (sh - h) / 2 - 20;
    target = '_blank';
    sFeature = 'width=' + w + 'px,height=' + h + 'px,top=' + t + 'px,left=' + l + 'px,scrollbars=yes,resizable=yes';
    window.open(url, target, sFeature);
}

function openPage(url) {
    w = window.screen.width * 0.9;
    h = window.screen.height * 0.9;
    openPageWithDimension(w, h, url);
}

function showPage(url) {
    w = window.screen.width * 0.9;
    h = window.screen.height * 0.9;

    showPageWithDimension(w, h, url);
}

function showPageWithDimension(width, height, url) {
    var feature = 'dialogWidth:' + width + 'px;dialogHeight:' + height + 'px;'
    window.showModalDialog(url, self, feature);
}

$(function() {
    $("textarea[maxlength]").keydown(function() {
        var area = $(this);
        var max = parseInt(area.attr("maxlength"), 10); //获取maxlength的值
        if (max > 0) {
            if (area.val().length > max) { //textarea的文本长度大于maxlength
                area.val(area.val().substr(0, max)); //截断textarea的文本重新赋值
                alert("最大长度为" + max + "个字母或汉字！");
            }
        }
    });
});

$("textarea[maxlength]").blur(function() {
    var area = $(this);
    var max = parseInt(area.attr("maxlength"), 10); //获取maxlength的值
    if (max > 0) {
        if (area.val().length > max) { //textarea的文本长度大于maxlength
            area.val(area.val().substr(0, max)); //截断textarea的文本重新赋值
            alert("最大长度为" + max + "个字母或汉字！");
        }
    }
}); 