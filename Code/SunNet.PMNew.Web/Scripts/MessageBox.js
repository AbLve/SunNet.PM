
var MessageBox =
{
    Options: { "Title": "Alert", "Caption": "Information" },

    Args: { "_sourceID": "", "_msg": "", "_btnObj": "", "_args": "", "_callback": null, "_images1": "", "_images2": "" },

    PopupWin: function (sourceID, msg, type, btnObj, args, callback, images1, images2, width, height) {


        var imgDir = "/Scripts/MessageBox/";
        var msgbox = document.getElementById("divMessageBox");

        var cWidth = typeof width == 'number' ? width : '452';
        var cHeight = typeof height == 'number' ? height : '148';

        if (msgbox == null) {
            msgbox = document.createElement("div");
            msgbox.id = "divMessageBox";
            msgbox.style.fontFamily = "Arial, Helvetica, sans-serif";
            msgbox.style.fontSize = "14px";
            msgbox.style.width = cWidth + 'px';
            msgbox.style.height = cHeight + 'px';
            msgbox.style.padding = "0px";
            msgbox.style.border = "0px solid red";
            msgbox.style.margin = "auto";
            msgbox.style.position = "absolute";
            msgbox.style.zIndex = 100000;
            msgbox.style.display = "block";
            document.body.appendChild(msgbox);

            var frame = document.createElement("iframe");
            frame.style.width = cWidth + 'px';
            frame.style.height = cHeight + 'px';
            frame.style.position = "absolute";
            frame.style.zIndex = -1;
            frame.style.borderStyle = "none";
            frame.style.borderWidth = "0px";
            frame.frameBorder = 0;
            msgbox.appendChild(frame);

            msgboxBody = document.createElement("div");

            msgboxBody.id = "divMessageBoxBody";
            msgboxBody.style.fontFamily = "Arial, Helvetica, sans-serif";
            msgboxBody.style.fontSize = "14px";
            msgboxBody.style.width = cWidth - 2 + 'px';
            // msgboxBody.style.height = "146px";
            msgboxBody.style.padding = "0px";
            msgboxBody.style.border = "1px solid #2D605E";
            msgboxBody.style.margin = "auto";
            msgboxBody.style.position = "absolute";
            //msgbox.style.zIndex          = 100000;
            msgboxBody.style.display = "block";
            msgboxBody.style.backgroundColor = "#ffffff";

            msgbox.appendChild(msgboxBody);

            var title = document.createElement("div");
            title.id = "divMessageBoxTitle";
            title.style.height = "24px";
            title.style.paddingTop = "4px";
            title.style.paddingLeft = "10px";
            title.style.backgroundImage = "url(" + imgDir + "left_t_bg2.gif)";
            title.style.color = "#ffffff";
            title.style.fontWeight = "bold";
            title.style.fontSize = "12px";
            title.style.cursor = "move";
            title.style.position = "relative";

            var caption = document.createElement("div");
            if (type == "alert")
                caption.innerHTML = "Information";
            else
                caption.innerHTML = "Question";
            title.appendChild(caption);

            msgboxBody.appendChild(title);

            var container = document.createElement("div");
            //container.style.height = "87px"
            msgboxBody.appendChild(container);

            var content = document.createElement("div");
            content.id = "divMessageBoxContent";
            content.style.padding = "10px 20px 30px 20px";
            container.appendChild(content);

            var buttons = document.createElement("div");
            buttons.style.height = "26px";
            buttons.style.paddingTop = "4px";
            buttons.style.backgroundImage = "url(" + imgDir + "warning_bottom_bg.gif)";
            buttons.style.textAlign = "center";
            msgboxBody.appendChild(buttons);

            var btnOK = document.createElement("img");
            btnOK.id = "btnOKOK";
            btnOK.style.cursor = "pointer";
            btnOK.style.width = "65px";
            btnOK.style.height = "22px";
            if (images1 == null || images1 == "")
                btnOK.src = imgDir + "buttom_sure.gif";
            else
                btnOK.src = imgDir + images1; // "/App_Themes/Default/Images/button_yes.gif";

            buttons.appendChild(btnOK);

            if (type != "alert") {
                var btnCancel = document.createElement("img");
                btnCancel.style.cursor = "pointer";
                btnCancel.style.width = "65px";
                btnCancel.style.height = "22px";
                btnCancel.style.marginLeft = "22px";
                if (images2 == null || images2 == "")
                    btnCancel.src = imgDir + "button_cancel.gif";
                else
                    btnCancel.src = imgDir + images2; // "/App_Themes/Default/Images/button_no.gif";
                btnCancel.onclick = function () {
                    document.getElementById("divMessageBox").style.display = "none";
                    if (document.getElementById("divMessageBox") != null)
                        document.getElementById("divMessageBox").parentNode.removeChild(document.getElementById("divMessageBox"));
                    MaskWindow.Hide();
                    if (callback != null) {
                        callback(false);
                    }
                }
                buttons.appendChild(btnCancel);
            }
        }

        document.getElementById("btnOKOK").onclick = function () {
            if (sourceID != null && document.getElementById(sourceID) != null) {
                document.getElementById(sourceID).focus();
            }
            if (document.getElementById("divMessageBox") != null)
                document.getElementById("divMessageBox").style.display = "none";
            if (document.getElementById("divMessageBox") != null)
                document.getElementById("divMessageBox").parentNode.removeChild(document.getElementById("divMessageBox"));
            MaskWindow.Hide();
            if (btnObj != null && btnObj != "")
                __doPostBack(btnObj.id, args);
            if (callback != null) {
                callback(true);
            }
        }

        var w = window.innerWidth || document.body.clientWidth;

        msgbox.style.left = ((w - parseFloat(msgbox.style.width)) / 2) + document.body.scrollLeft + "px";
        var testtop = ($(window).height() - $(msgbox).height()) / 2 + $(window).scrollTop();
        msgbox.style.top = testtop + "px";

        if (type == "alert")
            document.getElementById("divMessageBoxContent").innerHTML = "<table><tr><td style=\"vertical-align:middle;\" width=\"10%\"><img src='" + imgDir + "warning_2.gif'/></td><td align=\"center\">" + msg + "</td></table>";
        else
            document.getElementById("divMessageBoxContent").innerHTML = "<table><tr><td style=\"vertical-align:middle;\" width=\"10%\"><img src='" + imgDir + "warning_3.gif'/></td><td align=\"center\">" + msg + "</td></table>";
        MaskWindow.Show();

        document.onkeypress = function (e) {
            var e = e || window.event;
            if (document.getElementById("divMessageBox") != null) {
                if (document.getElementById("divMessageBox").style.display != "none") {
                    if (e.keyCode == 32 || e.keyCode == 13) {
                        if (document.getElementById("divMessageBox") != null)
                            document.getElementById("divMessageBox").parentNode.removeChild(document.getElementById("divMessageBox"));
                        //document.getElementById("divMessageBox").style.display = "none"; || e.keyCode == 0
                        MaskWindow.Hide();
                        return false;
                    }
                }
            }
        }

        return msgbox;
    },

    Alert: function (sourceID, msg) {
        MessageBox.Args = { "_sourceID": "", "_msg": "", "_btnObj": "", "_args": "", "_callback": null, "_images1": "", "_images2": "" };
        this.Args._sourceID = sourceID;
        this.Args._msg = msg;

        setTimeout(this.Alert2, 500);
    },
    Alert2: function () {
        var p = this.PopupWin(this.Args._sourceID, this.Args._msg, "alert", null, "", this.Args._callback, '', '', this.Args.width, this.Args.height);
        p.style.display = "block";
        p.focus();
        Drag(p);
    },
    Alert3: function (sourceID, msg, callback, width, height) {
        MessageBox.Args = { "_sourceID": "", "_msg": "", "_btnObj": "", "_args": "", "_callback": null, "_images1": "", "_images2": "", "_width": "", "_height": "" };
        this.Args._sourceID = sourceID;
        this.Args._msg = msg;
        this.Args._callback = callback;
        this.Args.width = width;
        this.Args.height = height;
        var that = this;
        setTimeout(function () { that.Alert2(); }, 500);
    },
    Confirm: function (sourceID, msg, btnObj, args) {
        MessageBox.Args = { "_sourceID": "", "_msg": "", "_btnObj": "", "_args": "", "_callback": null, "_images1": "", "_images2": "" };
        if (document.getElementById("divMessageBox") != null)
            document.getElementById("divMessageBox").parentNode.removeChild(document.getElementById("divMessageBox"));
        this.Args._sourceID = sourceID;
        this.Args._msg = msg;
        this.Args._btnObj = btnObj;
        this.Args._args = args;

        setTimeout(this.Confirm2, 500);
    },

    Confirm2: function () {
        if (document.getElementById("divMessageBox") != null)
            document.getElementById("divMessageBox").parentNode.removeChild(document.getElementById("divMessageBox"));
        var p = MessageBox.PopupWin(MessageBox.Args._sourceID, MessageBox.Args._msg, "confirm", MessageBox.Args._btnObj, MessageBox.Args._args);
        p.style.display = "block";
        p.focus();
        Drag(p);
    },

    Confirm3: function (sourceID, msg, images1, images2, callback) {
        MessageBox.Args = { "_sourceID": "", "_msg": "", "_btnObj": "", "_args": "", "_callback": null, "_images1": "", "_images2": "" };
        if (document.getElementById("divMessageBox") != null)
            document.getElementById("divMessageBox").parentNode.removeChild(document.getElementById("divMessageBox"));
        this.Args._sourceID = sourceID;
        this.Args._msg = msg;
        this.Args._images1 = images1;
        this.Args._images2 = images2;
        this.Args._callback = callback;
        var p = MessageBox.PopupWin(MessageBox.Args._sourceID, MessageBox.Args._msg, "confirm", MessageBox.Args._btnObj, MessageBox.Args._args, MessageBox.Args._callback, MessageBox.Args._images1, MessageBox.Args._images2);
        p.style.display = "block";

        p.focus();
        Drag(p);
    }
}

function Drag(obj) {
    var ie = document.all;
    var nn6 = document.getElementById && !document.all;
    var isdrag = false;
    var y, x;
    obj.onmousedown = function (e) {
        var divx = obj.getElementsByTagName("div");
        for (var i = 0; i < divx.length; i++) {
            if (divx[i].id == "divMessageBoxTitle") {
                isdrag = true;

                nTY = parseInt(obj.style.top + 0);

                y = nn6 ? e.clientY : event.clientY;

                nTX = parseInt(obj.style.left + 0);

                x = nn6 ? e.clientX : event.clientX;

                divx[i].onmousemove = function (e) {
                    if (isdrag) {
                        obj.style.top = (nn6 ? nTY + e.clientY - y : nTY + event.clientY - y) + "px";
                        obj.style.left = (nn6 ? nTX + e.clientX - x : nTX + event.clientX - x) + "px";

                        return false;
                    }
                };
                return false;
            }
        }
    }
    obj.onmouseup = function () { isdrag = false; };
}

var MaskWindow =
{
    Show: function () {
        var displayer = document.getElementById("divMaskWinDisplayer");

        if (displayer == null) {
            displayer = document.createElement("div");
            document.body.appendChild(displayer);

            displayer.style.position = "absolute";
            displayer.id = "divMaskWinDisplayer";
            displayer.style.zIndex = 9999;
            displayer.style.left = "0px";
            displayer.style.top = "0px";

            displayer.style.width = Math.max(document.body.scrollWidth, document.body.offsetWidth) + "px";
            displayer.style.height = Math.max(document.body.scrollHeight, document.body.offsetHeight) + "px";

            displayer.child = null;
            displayer.style.opacity = 0.1;
            displayer.style.filter = "alpha(opacity=20)";
            displayer.style.backgroundColor = "#FFFFFF";
        }

        displayer.style.display = "block";
    },

    Hide: function () {
        var displayer = document.getElementById("divMaskWinDisplayer");

        if (displayer != null) {
            displayer.style.display = "none";
        }
    }
}