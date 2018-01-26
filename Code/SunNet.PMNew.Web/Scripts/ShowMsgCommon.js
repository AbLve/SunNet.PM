;
function ShowMessage(msg, level, reLoadParentWindow, closeWindow, width, height) {
    MessageBox.Alert3(null, msg, function () {
        if (reLoadParentWindow) {
            //window.dialogArguments.location.reload();
            window.returnValue = 0;
            if (ISModalPage !== undefined && ISModalPage === false) {
                //                window.location.reload();
                window.location.href = window.location.href;
            }
        }
        if (closeWindow) {
            CloseCurrent();
        }
    }, width, height);
}

function ShowMessageAndJumpToAnchor(msg, isReloadCurrentWindow, anchor) {
    MessageBox.Alert3(null, msg, function () {
        if (typeof anchor === "string" && isReloadCurrentWindow) {
            refreshToAnchor(anchor);
        }
    });
}

function ShowMessageAndCallback(msg, callback) {
    MessageBox.Alert3(null, msg, function () {
        if (callback) {
            callback();
        }
    });
}

function ShowMessageAndRedirect(msg, redirectUrl) {
    MessageBox.Alert3(null, msg, function () {
        if (redirectUrl) {
            window.location = redirectUrl;
        }
    });
}

function ShowMessage2(msg, level, reLoadNowWindow, closeWindow) {
    MessageBox.Alert3(null, msg, function () {
        if (reLoadNowWindow) {
            //window.dialogArguments.location.reload();
            window.location.reload();
            //window.location.href = window.location.href;
        }
        if (closeWindow) {
            CloseCurrent();
        }
    });
}

function refreshToAnchor(achor) {
    var urlMatch = window.location.href.match(/[\s\S]+([a-z0-9A-Z])$/);
    if (urlMatch) {
        if (/\?/.test(urlMatch[1])) {
            window.location.href.replace(/$/, "#" + achor);
        }
        else {
            window.location.href.replace(/$/, "&#" + achor);
        }
    }
    window.location.reload();
}

function ShowInfo(msg) {
    if (msg && typeof msg === "string") {
        if (arguments[1]) {
            MessageBox.Alert3(null, msg, arguments[1]);
        }
        else {
            MessageBox.Alert3(null, msg, function () { });
        }
    }
}

function CloseCurrent() {
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        if (navigator.userAgent.indexOf("MSIE 6.0") > 0) {
            window.opener = null; window.close();
        }
        else {
            window.open('', '_top'); window.top.close();
        }
    }
    else if (navigator.userAgent.indexOf("Firefox") > 0) {
        if (window.opener) {
            window.close();
        }
        else {
            window.location.href = 'about:blank ';
        }
    }
    else {
        window.opener = null;
        window.open('', '_self', '');
        window.close();
    }
}
