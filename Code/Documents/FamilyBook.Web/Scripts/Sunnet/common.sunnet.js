
// 另存为文件
function SaveCode(obj, filename) {
    var win = window.open('', '_blank', 'top=100');
    var code = obj.innerText;
    code = code == null || code == "" ? obj.value : code;
    win.opener = null;
    win.document.write(code);
    win.document.execCommand('saveas', true, filename);
    win.close();
}
// 将光标停在对象的最后
function PutCursorAtLast(obj) {
    obj.focus();
    var range = obj.createTextRange();
    range.moveStart('character', obj.value.length);
    range.collapse(true);
    range.select();
}
// 将光标停在对象的最前
function PutCursorAtFirst(obj) {
    obj.focus();
    var range = obj.createTextRange();
    range.moveStart('character', 0);
    range.collapse(true);
    range.select();
}

String.prototype.Replace = function (oldValue, newValue) {
    var reg = new RegExp(oldValue, "g");
    return this.replace(reg, newValue);
}

// 去掉字符两端的空白字符
String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
// 去掉字符左端的的空白字符
String.prototype.LeftTrim = function () {
    return this.replace(/(^[\\s]*)/g, "");
}
// 去掉字符右端的空白字符
String.prototype.RightTrim = function () {
    return this.replace(/([\\s]*$)/g, "");
}
// 判断字符串是否以指定的字符串开始
String.prototype.StartsWith = function (str) {
    return this.substr(0, str.length) == str;
}
// 判断字符串是否以指定的字符串结束
String.prototype.EndsWith = function (str) {
    return this.substr(this.length - str.length) == str;
}

function $A(arrayLike) {
    for (var i = 0, ret = []; i < arrayLike.length; i++) ret.push(arrayLike[i])
    return ret
};
Array.prototype.any = function (f) {
    for (var i = 0; i < this.length; i++) if (f(this[i], i, this)) return true;
    return false
};

//判断 字符串 是否符合 yyyy-mm-dd hh:mm:ss的日期格式, 格式正确而且闰年闰月等也要正确
String.prototype.isDateTime = function () {
    try {
        var arr = (this.length == 19) ? this.split(/\D/) : []
        --arr[1]
        eval_r("var d=new Date(" + arr.join(",") + ")")
        return Number(arr[0]) == d.getFullYear() && Number(arr[1]) == d.getMonth()
                      && Number(arr[2]) == d.getDate() && Number(arr[3]) == d.getHours()
                     && Number(arr[4]) == d.getMinutes() && Number(arr[5]) == d.getSeconds()
    } catch (x) { return false }
}