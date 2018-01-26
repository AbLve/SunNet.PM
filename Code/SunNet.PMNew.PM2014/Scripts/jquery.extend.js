///    <summary>
///    jQuery原型扩展，重新封装Ajax请求WebServeice
///    </summary>
///    <param name="url" type="String">
///     处理请求的地址
///</param>
///    <param name="dataMap" type="String">
///     参数，json格式的字符串
///</param>
///    <param name="fnSuccess" type="function">
///     请求成功后的回调函数
///</param>
    $.ajaxWebService = function(url, dataMap, fnSuccess) {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: url,
            data: dataMap,
            dataType: "json",
            success: fnSuccess
        });
    }




///    <summary>
///    jQuery实例扩展，Ajax加载封装用户控件(*.ascx)，输出Html，仅适用于Asp.Net。
///     依赖 $.ajaxWebService(url, dataMap, fnSuccess)
///    </summary>
///    <param name="control" type="String">
///     需要加载的用户控件的相对路径
///     </param>
///    <param name="page" type="String">
///     输出控件Html片段的页面，不一定是当前页面。可选，缺省值为当前页面。
///     </param>
$.fn.loadUserControl = function(control) {
    var $dom = this;
    page = "Dashboard.aspx/RenderUserControl"; //RenderUserControl是PageProxy中的方法，不要轻易修改
    $.ajaxWebService(page, "{ticketId:" + control + "}", function (result) {
        $dom.html(result.d);
    });

}