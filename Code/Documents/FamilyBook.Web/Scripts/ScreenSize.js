

function findDimensions() //函数：获取尺寸
{
    var screenWinWidth = 0; 
    var screenWinHeight = 0;
    if (window.innerWidth)
        screenWinWidth = window.innerWidth;
    else if ((document.body) && (document.body.clientWidth))
        screenWinWidth = document.body.clientWidth;
    if (window.innerHeight)
        screenWinHeight = window.innerHeight;
    else if ((document.body) && (document.body.clientHeight))
        screenWinHeight = document.body.clientHeight;
    if (document.documentElement && document.documentElement.clientHeight && document.documentElement.clientWidth)
    {
        screenWinHeight = document.documentElement.clientHeight;
        screenWinWidth = document.documentElement.clientWidth;

    }
    if (typeof (ScreenSizeCallback) == "function")
    {
        ScreenSizeCallback(screenWinWidth, screenWinHeight);
    }
}
//$(function ()
//{
//    var newStr = "";
//    var photoTitleStr = $(".photoTitle").html();
//    if (photoTitleStr.length > 25)
//    {
//        newStr = photoTitleStr.substring(0, 25);
//        $(".photoTitle").html(newStr+"...");
//    }

//    var albumTitleStr = $(".albumTitle").html();
//    if (albumTitleStr.length > 25)
//    {
       
//        newStr = albumTitleStr.substring(0, 25);
//        $(".albumTitle").html(newStr + "...");
//    }

//    var nativeTitleStr = $(".nativeTitle").html();
//    if (nativeTitleStr.length > 25)
//    {
//        newStr = nativeTitleStr.substring(0, 25);
//        $(".nativeTitle").html(newStr + "...");
//    }
//});
findDimensions();
