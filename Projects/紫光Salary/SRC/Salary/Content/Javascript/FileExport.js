var xh;
var localTempDir;

function getRootPath() {
    var location = document.location;
    if ("file:" == location.protocol) {
        var str = location.toString();
        return str.replace(str.split("/").reverse()[0], "");
    }
    var pathName = location.pathname.split("/");
    return location.protocol + "//" + location.host + "/" + pathName[1] + "/";
}

//自动下载文件到本地
function getTemplateFromServer(geturl) {
    try {
        xh = new ActiveXObject("Microsoft.XMLHTTP");
        xh.onreadystatechange = getReady;
        xh.open("GET", geturl, false);
        xh.send();
        return true;
    }
    catch (e)
        { return false }
}

function getReady() {
    while (xh.readyState == 4) {
        if (xh.readyState == 4) {
            if (xh.status == 200) {
                SaveFileToLocal(localTempDir);
                return true;
            }
            else
            { return false; }
        }
        else
            return false;
    }
}


function SaveFileToLocal(tofile) {
    var objStream;
    var imgs;
    imgs = xh.responseBody;
    try {
        objStream = new ActiveXObject("ADODB.Stream");
    }
    catch (e) {
        alert("无法创建ADODB.Stream对象，解决方法如下：\n1、请将http://" + document.location.host + "加入可信站点"
	        + "\n2、将可信站点‘自定义级别’中的‘ActiveX控件和插件’中的所有子项全部修改为启用（注意：必须是启用，不能是提示）’"
	          + "\n3、windows自动更新后经常会使‘ActiveX控件和插件’中的一些子项变为‘提示’，请检查并确保其所有子项均为启用！");
        return false;
    }
    try {
        objStream.Type = 1;
        objStream.open();

        objStream.write(imgs);
        objStream.SaveToFile(tofile)
        objStream.close();
    }
    catch (e) {
        if (!fileExists(tofile)) {
            alert("此计算机上的安全设置禁止访问其它域的数据源");
            return false;
        }
    }
    return true;
}

function ExportToFile(templatename, yearMonth, reportId) {
    if (GetExtensionName(templatename) == "doc")
        ExportToWord(templatename, id);
    if (GetExtensionName(templatename) == "xls")
        ExportToExcel(templatename, yearMonth, reportId);
}


//导出方法，参数templatename要导出的模版名称，带扩展名，id为要导出的数据的唯一索引id
function ExportToWord(templatename, id) {
    var temp = getTempDirName();
    if (temp == null) return false;
    localTempDir = temp + "\\" + templatename;

    if (fileExists(localTempDir))
        deleteFile(localTempDir);
    try {
        if (getTemplateFromServer(getRootPath() + 'UI/Template/' + templatename))
            printWord(templatename, id);
    }
    catch (e) {
        alert("无法导出Word文件！");
    }

}

function ExportToExcel(templatename, yearMonth, reportId) {
    localTempDir = getTempDirName() + "\\" + templatename;
    if (fileExists(localTempDir))
        deleteFile(localTempDir);
    try {
        if (getTemplateFromServer(getRootPath() + 'UI/Template/' + templatename))
            printExcel(templatename, yearMonth, reportId);            
    }
    catch (e) {
        alert("无法导出Excel文件！");
    }

}

function ExportToChart(templatename, startdate, enddate, reporttype) {
    localTempDir = getTempDirName() + "\\" + templatename;
    if (fileExists(localTempDir))
        deleteFile(localTempDir);
    try {
        if (getTemplateFromServer(getRootPath() + 'Template/' + templatename))
            printExcelChart(templatename, startdate, enddate, reporttype);
    }
    catch (e) {
        alert("无法导出Excel文件！");
    }

}
var xlbook;
function printExcel(templatename, yearMonth, reportId) {

    var xmldom = getXmlData(yearMonth, reportId);

    //参数为模板(与页面的相对)路径
    var excel = new ExcelApp(localTempDir);

    var NodeHead = xmldom.selectNodes("//Table");
    //var tablenum = parseInt(NodeHead.item(0).childNodes[0].text);

    var bookmark, replacecontent;
    for (var k = 0; k < NodeHead.item(0).childNodes.length; k++) {
        bookmark = "##" + NodeHead.item(0).childNodes[k].nodeName + "##";
        replacecontent = NodeHead.item(0).childNodes[k].text;
        if (xlbook.worksheets(1).Cells.Find(bookmark))
            xlbook.worksheets(1).Cells.Find(bookmark).Replace(bookmark, replacecontent);
    }
    var rowIndex = 0, colIndex = 0;
    for (var t = 1; t < 100; t++) {
        var cell = xlbook.worksheets(1).Cells.Find("##table" + t.toString() + "_record_begin##");
        if (cell) {
            var begin_row = cell.Row;
            var begin_col = cell.Column;
            var objNodeList = xmldom.selectNodes("//Table" + t.toString());
            rowIndex = objNodeList.length + begin_row;
            var content = "";
            if (objNodeList) {
                for (var i = 0; i < objNodeList.length; i++) {
                    var xmlChild = objNodeList.item(i);
                    colIndex = xmlChild.childNodes.length;
                    for (var j = 0; j < colIndex; j++) {
                        content = xmlChild.childNodes[j].text;
                        xlbook.worksheets(1).rows(i + begin_row).cells(j + begin_col).Value = content;
                        if (content == "合计" || content == "小计") {
                            var range = xlbook.worksheets(1).Range(xlbook.worksheets(1).rows(i + begin_row).cells(1), xlbook.worksheets(1).rows(i + begin_row).cells(colIndex))
                            range.Interior.ColorIndex = 19;
                            range.Font.Bold = true;
                        }
                    }
                }
                //xlbook.worksheets(1).Range(xlbook.worksheets(1).rows(begin_row).cells(1), xlbook.worksheets(1).rows(rowIndex).cells(colIndex)).Select();

                if (colIndex > 0) {
                    //去掉根据内容自适应列宽度modify by yufeng at 20091126
                    //xlbook.worksheets(1).Range(xlbook.worksheets(1).rows(begin_row-1).cells(1), xlbook.worksheets(1).rows(rowIndex).cells(colIndex)).Columns.AutoFit();

                }
                else
                    xlbook.worksheets(1).rows(begin_row).cells(begin_col).Value = "";
            }
        }
        else
            continue;
    }


    if (excel.excelObj != undefined)
        excel.excelObj.visible = true;
}

function printExcelChart(templatename, url) {

    var xmldom = getChartXmlData(url);
    //    alert(xmldom.xml);
    //参数为模板(与页面的相对)路径
    var excel = new ExcelApp(localTempDir);

    var NodeHead = xmldom.selectNodes("//Table_Head");
    var bookmark = "##TITLE_DATE##"
    var rowIndex = 0, colIndex = 0;
    var range;
    for (var t = 0; t < NodeHead.length; t++) {
        var childHead = NodeHead.item(t);
        if (childHead.childNodes.length == 4) {
            if (xlbook.worksheets(childHead.childNodes[1].text).Cells.Find(bookmark))
                xlbook.worksheets(childHead.childNodes[1].text).Cells.Find(bookmark).Replace(bookmark, childHead.childNodes[3].text);
        }

        var cell = xlbook.worksheets(childHead.childNodes[1].text).Cells.Find(childHead.childNodes[2].text);
        if (cell) {
            var begin_row = cell.Row;
            var begin_col = cell.Column;
            var objNodeList = xmldom.selectNodes(childHead.childNodes[0].text);
            rowIndex = objNodeList.length + begin_row;
            var content = "";
            if (objNodeList) {
                for (var i = 0; i < objNodeList.length; i++) {
                    var xmlChild = objNodeList.item(i);
                    colIndex = xmlChild.childNodes.length;
                    for (var j = 0; j < colIndex; j++) {
                        content = xmlChild.childNodes[j].text;
                        xlbook.worksheets(childHead.childNodes[1].text).rows(i + begin_row).cells(j + begin_col).Value = content;
                        //                        if (content == "合计" || content == "小计") {
                        //                            var range = xlbook.worksheets(childHead.childNodes[1].text).Range(xlbook.worksheets(childHead.childNodes[1].text).rows(i + begin_row).cells(1), xlbook.worksheets(childHead.childNodes[1].text).rows(i + begin_row).cells(colIndex))
                        //                            range.Interior.ColorIndex = 19;
                        //                            range.Font.Bold = true;
                        //                        }
                    }
                }

                if (colIndex > 0) {
                    //给内容区增加边框
                    //xlbook.worksheets(childHead.childNodes[1].text).Range(xlbook.worksheets(childHead.childNodes[1].text).rows(begin_row - 1).cells(1), xlbook.worksheets(childHead.childNodes[1].text).rows(rowIndex).cells(colIndex)).Columns.AutoFit();
                    range = xlbook.worksheets(childHead.childNodes[1].text).Range(xlbook.worksheets(childHead.childNodes[1].text).rows(begin_row - 1).cells(1), xlbook.worksheets(childHead.childNodes[1].text).rows(rowIndex - 1).cells(colIndex));
                    range.Borders.LineStyle = 1;
                }
                else
                    xlbook.worksheets(childHead.childNodes[1].text).rows(begin_row).cells(begin_col).Value = "";
            }
        }
        else
            continue;
    }


    if (excel.excelObj != undefined)
        excel.excelObj.visible = true;
}

function getXmlData(id) {
    var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", encodeURI(getRootPath() + "UI/Salary/ExportXmlData.aspx?id=" + id), false, "", "");
    //xmlhttp.open("GET", encodeURI(getRootPath() + "UI/Salary/ExportXmlData.aspx"), false, "", "");
    xmlhttp.send();
    var xmldom = new ActiveXObject("Microsoft.XMLDOM");
    xmlhttp.responseXML.createProcessingInstruction("xml", " version=\"1.0\" encoding=\"gb2312\"");
    xmldom = xmlhttp.responseXML.documentElement;
    return xmldom;
}


function getXmlData(yearMonth, reportId) {
    var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", encodeURI(getRootPath() + "UI/Salary/ExportXmlData.aspx?yearMonth=" + yearMonth + "&reportId=" + reportId), false, "", "");
    //xmlhttp.open("GET", encodeURI(getRootPath() + "UI/Salary/ExportXmlData.aspx"), false, "", "");
    xmlhttp.send();
    var xmldom = new ActiveXObject("Microsoft.XMLDOM");
    xmlhttp.responseXML.createProcessingInstruction("xml", " version=\"1.0\" encoding=\"gb2312\"");
    xmldom = xmlhttp.responseXML.documentElement;
    return xmldom;
}

function getChartXmlData(url) {
    var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", encodeURI(getRootPath() + url), false, "", "");
    xmlhttp.send();
    var xmldom = new ActiveXObject("Microsoft.XMLDOM");
    xmlhttp.responseXML.createProcessingInstruction("xml", " version=\"1.0\" encoding=\"gb18030\"");
    xmldom = xmlhttp.responseXML.documentElement;
    return xmldom;
}

function printWord(templatename, id) {
    var xmldom = getXmlData(id);

    //参数为模板(与页面的相对)路径
    var word = new WordApp(localTempDir);

    var bm = word.wordObj.ActiveDocument.BookMarks.count;
    while (bm > 0) {
        var bookMark = "";
        var bookContent = "";
        //当用户模版书签定义错误，比如书签内套书签时，跳过错误书签。
        try {
            bookMark = word.wordObj.ActiveDocument.BookMarks(bm).name;
            var bposition = bookMark.indexOf("____");
            var node;
            if (new Number(bposition) > 0) {
                node = xmldom.selectSingleNode("//" + bookMark.substring(0, bposition));
            }
            else {
                node = xmldom.selectSingleNode("//" + bookMark);
            }
            if (node)
                bookContent = node.text;
            else
                bookContent = "";

            word.wordObj.ActiveDocument.BookMarks(bm).range.text = bookContent;
            bm = bm - 1;
        }
        catch (e) {

            bm = bm - 1;
            continue;
        }

    }

    if (word.wordObj != undefined)
        word.wordObj.visible = true;
}


//创建Word对象
var WordApp = function(wordTplPath) {
    if (!fileExists(localTempDir)) {
        alert("没有Word模板文件！");
        return;
    }
    var wordObj = new ActiveXObject("Word.Application");
    if (wordObj == null) {
        alert("不能创建Word对象！");
    }
    wordObj.visible = false;
    this.wordObj = wordObj;
    this.docObj = this.wordObj.Documents.Open(wordTplPath);
}


//创建Excel对象   
var ExcelApp = function(excelTplPath) {
    if (!fileExists(localTempDir)) {
        alert("没有Excel模板文件！");
        return;
    }
    var excelObj = new ActiveXObject("Excel.Application");
    if (excelObj == null) {
        alert("不能创建Excel对象！");
    }
    excelObj.visible = false;
    this.excelObj = excelObj;
    xlbook = excelObj.Workbooks.Add(excelTplPath);
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

function fileExists(strFileName) {
    var bExists = false;

    try {
        var fso = createObject("Scripting.FileSystemObject");

        bExists = fso.fileExists(strFileName);
    }
    catch (e) {
    }

    return bExists;
}

//删除文件
function deleteFile(filespec) {
    try {
        var fso = createObject("Scripting.FileSystemObject");
        fso.DeleteFile(filespec);
    }
    catch (e) {
    }
}

//得到一个系统临时目录
function getTempDirName() {
    try {
        var fso = createObject("Scripting.FileSystemObject");
        return fso.GetSpecialFolder(2);
    }
    catch (e) {
        return null;
    }
}
//取得文件的扩展名
function GetExtensionName(fileName) {
    var fso, s = "";
    fso = new ActiveXObject("Scripting.FileSystemObject");
    s += fso.GetExtensionName(fileName);
    return (s);
}





//BMB新增，根据不同URL地址返回的Xml数据导出到Word-------------------------------------------------------------------------

function ExportToWordByUrl(templatename, pathUrl) {
    var temp = getTempDirName();
    if (temp == null) return false;
    localTempDir = temp + "\\" + templatename;

    if (fileExists(localTempDir))
        deleteFile(localTempDir);
    try {
        if (getTemplateFromServer(getRootPath() + 'Template/' + templatename))
            printWordByUrlNew(templatename, pathUrl);
    }
    catch (e) {
        alert("无法导出Word文件！");
    }

}

function getXmlDataByUrl(pathUrl) {
    var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", encodeURI(getRootPath() + pathUrl), false, "", "");
    xmlhttp.send();

    //pathUrl="Pages/AnalysisReport/ExportTest.aspx";
    var xmldom = new ActiveXObject("Microsoft.XMLDOM");
    xmlhttp.responseXML.createProcessingInstruction("xml", " version=\"1.0\" encoding=\"gb2312\"");
    xmldom = xmlhttp.responseXML.documentElement;
    return xmldom;
}

function printWordByUrl(templatename, pathUrl) {
    var xmldom = getXmlDataByUrl(pathUrl);
    //参数为模板(与页面的相对)路径
    var word = new WordApp(localTempDir);

    var bm = word.wordObj.ActiveDocument.BookMarks.count;
    while (bm > 0) {
        var bookMark = "";
        var bookContent = "";
        //当用户模版书签定义错误，比如书签内套书签时，跳过错误书签。
        try {
            bookMark = word.wordObj.ActiveDocument.BookMarks(bm).name.toString();
            var bposition = bookMark.lastIndexOf("____");

            if (new Number(bposition) > 0) {
                node = xmldom.selectSingleNode("//" + bookMark.substring(0, bposition).toString());
            }
            else {
                node = xmldom.selectSingleNode("//" + bookMark);
            }

            if (node)
                bookContent = node.text;
            else
                bookContent = "";

            word.wordObj.ActiveDocument.BookMarks(bm).range.text = bookContent;
            bm = bm - 1;
        }
        catch (e) {
            bm = bm - 1;
            continue;
        }
    }

    if (word.wordObj != undefined)
        word.wordObj.visible = true;
}


//将书签替换为特殊串，注：对于书签的rangeText中含有回车键的，书签删除不掉，单替换成功！
function printWordReplaceBookMarks(templatename, pathUrl) {
    //参数为模板(与页面的相对)路径
    var word = new WordApp(localTempDir);

    var bm = word.wordObj.ActiveDocument.BookMarks.count;
    while (bm > 0) {
        try {
            word.wordObj.ActiveDocument.BookMarks(bm).range.text = "[#" + word.wordObj.ActiveDocument.BookMarks(bm).name.toString() + "#]";
            bm = bm - 1;
        }
        catch (e) {
            bm = bm - 1;
            continue;
        }
    }

    if (word.wordObj != undefined)
        word.wordObj.visible = true;
}


function printWordByUrlNew(templatename, pathUrl) {

    var xmldom = getXmlDataByUrl(pathUrl);
    var hstable = LoadXmlTextToHsTable(xmldom.xml);
    //参数为模板(与页面的相对)路径
    var word = new WordApp(localTempDir);

    var myFind = word.wordObj.Selection.Find;

    while (myFindFunc(myFind)) {
        var findText = word.wordObj.Selection.Text;
        findText = findText.substring(2, findText.length - 2);
        try {
            //var node = xmldom.selectSingleNode("//" + findText.toString());
            var node = hstable.get(findText);
            if (node) {
                word.wordObj.Selection.Text = node;
            }
            else {
                word.wordObj.Selection.Text = "##" + findText + "##";
            }
        }
        catch (e) {
            alert(findText);
            word.wordObj.Selection.Text = "##" + findText + "##";
            continue;
        }
    }

    if (word.wordObj != undefined)
        word.wordObj.visible = true;
}

function myFindFunc(myFind) {
    myFind.ClearFormatting();
    myFind.Text = "\\[#*#\\]";
    myFind.Replacement.Text = "";
    myFind.Forward = true;
    myFind.Wrap = 1;
    myFind.Format = false;
    myFind.MatchCase = false;
    myFind.MatchWholeWord = false;
    myFind.MatchByte = false;
    myFind.CorrectHangulEndings = false;
    myFind.MatchAllWordForms = false;
    myFind.MatchSoundsLike = false;
    myFind.MatchFuzzy = false;
    myFind.MatchWildcards = true;
    myFind.Execute().Replace = 0;
    return myFind.Found;

}

function LoadXmlTextToHsTable(xmlText) {

    xmlDoc = loadXMLString(xmlText);
    // documentElement always represents the root node
    x = xmlDoc.documentElement.childNodes[0].childNodes;
    var hstable = new Hashtable();

    for (i = 0; i < x.length; i++) {
        try {
            hstable.put(x[i].nodeName, x[i].childNodes[0].nodeValue);
        }
        catch (e) {
            hstable.put(x[i].nodeName, " ");
            continue;
        }
    }

    return hstable
    //alert(hstable.get("year"));
}

function loadXMLString(xmlText) {
    try { //Internet Explorer
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = false;
        xmlDoc.loadXML(xmlText);
        return (xmlDoc);
    }
    catch (e) {
        try {//Firefox,Mozilla,Opera,etc
            parser = new DomParser();
            xmlDoc = parser.parseFormString(xmlText, "text/xml");
            return (xmlDoc);
        }
        catch (e) {
            alert(e.message())
            return (null);
        }
    }
}

function Hashtable() {
    this.container = new Object();

    /**//** put element */
    this.put = function(key, value) {
        if (typeof (key) == "undefined") {
            return false;
        }
        if (this.contains(key)) {
            return false;
        }
        this.container[key] = typeof (value) == "undefined" ? null : value;
        return true;
    };

    /**//** remove element */
    this.remove = function(key) {
        delete this.container[key];
    };

    /**//** get size */
    this.size = function() {
        var size = 0;
        for (var attr in this.container) {
            size++;
        }
        return size;
    };

    /**//** get value by key */
    this.get = function(key) {
        return this.container[key];
    };

    /**//** containts a key */
    this.contains = function(key) {
        return typeof (this.container[key]) != "undefined";
    };

    /**//** clear all entrys */
    this.clear = function() {
        for (var attr in this.container) {
            delete this.container[attr];
        }
    };

    /**//** hashTable 2 string */
    this.toString = function() {
        var str = "";
        for (var attr in this.container) {
            str += "," + attr + "=" + this.container[attr];
        }
        if (str.length > 0) {
            str = str.substr(1, str.length);
        }
        return "{" + str + "}";
    };
}