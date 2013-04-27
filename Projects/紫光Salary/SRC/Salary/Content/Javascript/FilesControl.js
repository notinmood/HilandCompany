function addSelectFileControl(controlsDiv, fileDiv) {
    var control = $("#" + fileDiv).clone();
    control.show();
    $("#" + controlsDiv).append(control);
}
function addDeleteFileControl(ctl) {
    $(ctl).parent().append("<input type='button' value='删除' onclick='deleteSelectFileControls(this);' />");
}
//fileMaxCount文件最大数量
function inputFileChange(ctl, controlsDiv, fileDiv, fileMaxSize) {
    if (!valiDateFileMaxSize($(ctl).val(), fileMaxSize)) {
        $(ctl).parent().after($(ctl).parent().clone());
        deleteSelectFileControls(ctl);
        return false;
    }
    if ($(ctl).next().length == 0) {
        addDeleteFileControl(ctl);
        addSelectFileControl(controlsDiv, fileDiv);
    }
}
function deleteSelectFileControls(ctl) {
    $(ctl).parent().remove();
}
function valiDateFileMaxSize(filePath, fileMaxSize) {
    try {
        var objStream = new ActiveXObject("ADODB.Stream");
        objStream.Type = 1;
        objStream.Open();
        objStream.LoadFromFile(filePath);
        if ((objStream.Size) > fileMaxSize) {
            alert("文件的大小（字节）不能超过" + Math.ceil(fileMaxSize / 1024 / 1024) + "M！");
            return false;
        }
        return true;
    }
    catch (N) {
        alert("请将该网站添加到信任站点！");
        return false;
    }

}