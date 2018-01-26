/*
 *@lhgdialog - Dialog Plugin v2.2.2 - Date : 2009-11-23
 *@Copyright lhgcore (c) 2009 By LiHuiGang Reserved
 */

var lhgdialog = (function()
{
    var topWin = window, cover, topDoc;
	while( topWin.parent && topWin.parent != topWin )
	{
	    try{
		    if( topWin.parent.document.domain != document.domain ) break;
			//假如你不想跨frameset只跨iframe就把下面这句的注释去掉
			//if( J('frameset',topWin.parent.document).length > 0 ) break;
		}
		catch(e){ break; }
		topWin = topWin.parent;
	}
	topDoc = topWin.document;
	
	var getZIndex = function()
	{
	    if( !J.dialog.zIndex )
		    J.dialog.zIndex = 1999;
		return ++J.dialog.zIndex;
	};
	
	// 取指定的js文件的绝对路径，返回形式为：http://www.xxx.com/xxx/xxx/
	var getMap = function( file )
	{
	    file = file || 'lhgcore.min.js';
		var len, bp, fp, src = J('script'), i = 0, l = src.length;
		
		for( ; i < l; i++ )
		{
		    bp = src[i].src.substr( 0, src[i].src.toLowerCase().indexOf(file) );
			len = bp.lastIndexOf('/');
			
			if( len > 0 )
			    bp = bp.substr( 0, len + 1 );
			
			if( bp ) break;
		}
			
		if( !J.browser.ie || J.browser.i8 || bp.indexOf('http:') !== -1 )
			return bp;
		else
		{
			fp = window.location.href;
			fp = fp.substr( 0, fp.lastIndexOf('/') );
			
			if( bp == '' ) return fp + '/';
			
			if( bp.indexOf('../') !== -1 )
			{
			    while( bp.indexOf('../') >= 0 )
				{
				    bp = bp.substr(3); fp = fp.substr( 0, fp.lastIndexOf('/') );
				}
				return fp + '/' + bp;
			}
			else if( bp.indexOf('/') == 0 )
			{
			    bp = document.location.protocol + '//' + document.location.host + bp;
				return bp;
			}
			else
			    return fp + '/' + bp;
		}
	};
	
	var isDTD = function( doc )
	{
	    return ( 'CSS1Compat' == ( doc.compatMode || 'CSS1Compat' ) );
	};
	
	var reSizeHdl = function()
	{
		if( !cover ) return;
		var rel = isDTD( topDoc ) ? topDoc.documentElement : topDoc.body;
		
		J(cover).css({
		    width: Math.max( rel.scrollWidth, rel.clientWidth, topDoc.scrollWidth || 0 ) - 1,
			height: Math.max( rel.scrollHeight, rel.clientHeight, topDoc.scrollHeight || 0 ) - 1
		});
	};
	
	return {
	    zIndex : null, indoc : {}, inwin : {}, infrm : {}, inndoc : {}, innwin : {},
		get : function( d )
		{
		    if( typeof d !== 'object' || !d.id || J( '#lhg_' + d.id, topDoc )[0] ) return;
			
			if( d.cover )
			    this.dispCover();
			else{ if(cover) cover = null; }
			
			var w = d.width || 400, h = d.height || 300, t = d.title || 'lhgdialog';
			
			//强制使用link参数时前面必须加http://
			if( d.link && !/\http:/.test(d.link) ) d.link = null;
			if( d.page && d.noch )
			    d.page = ( /\?/.test(d.page) ? d.page + '&' : d.page + '?' ) + 'uuid=' + new Date().getTime();
			
			//强制使用page参数时前面必须不能加http://	
			if( d.page && /\http:/.test(d.page) ) d.page = this.getvoid();
			
			//用于传递到lhgdialog.html页的参数
			var dialogInfo =
			{
			    tit: t, page: d.page, link: d.link, html: d.html, win: window, top: topWin, rng: d.rang,
				cus: d.custom, drg: d.nodrag, style: d.skin||'default', fot: d.nofoot
			},
			
			viewS = this.client( topWin ), scroS = this.scroll( topWin ),
			//插件路径，注意lhgdialog.js和lhgdialog.html一定要在同一目录下
			dialogPath = getMap( 'lhgdialog.js' ),
			
			iTop = d.top ? scroS.y + d.top : Math.max( scroS.y + ( viewS.h - h - 20 ) / 2, 0 ),
			iLeft = d.left ? scroS.x + d.left : Math.max( scroS.x + ( viewS.w - w - 20 ) / 2, 0 );
			
			J('<iframe frameborder="0" scrolling="no"></iframe>',topDoc).attr({
			    id: 'lhg_' + d.id, allowTransparency: true, src: dialogPath + 'lhgdialog.html'
			}).css({
			    top: iTop, left: iLeft, position: 'absolute', width: w, height: h, zIndex: getZIndex()
			}).appendTo( topDoc.body )[0]._dlgargs = dialogInfo;
		},
		
		getvoid : function()
		{
			if( J.browser.ie )
				return ( J.browser.i7 ? '' : "javascript:''" );
			else
				return 'javascript:void(0)';
		},
		
		client : function( win )
		{
			win = win || window;
			
			if( J.browser.ie )
			{
				var oSize, doc = win.document.documentElement;
				if( doc && doc.clientWidth ) oSize = doc; else oSize = win.document.body;
				
				if( oSize )
					return { w : oSize.clientWidth, h : oSize.clientHeight };
				else
					return { w : 0, h : 0 };
			}
			else
				return { w : win.innerWidth, h : win.innerHeight };
		},
		
		scroll : function( win )
		{
		    win = win || window;
			
			if( J.browser.ie )
			{
				var doc = win.document;
				oPos = { x : doc.documentElement.scrollLeft, y : doc.documentElement.scrollTop };
				if( oPos.x > 0 || oPos.y > 0 ) return oPos;
				
				return { x : doc.body.scrollLeft, y : doc.body.scrollTop };
			}
			else
				return { x : win.pageXOffset, y : win.pageYOffset };
		},
		
		close : function( dlgWin, co )
		{
		    var dlg = dlgWin.frameElement;
			
			if( dlg )
			{
			    dlg.src = this.getvoid();
				J(dlg).remove(); dlg = null;
			}
			
			if( co )
			    this.hideCover( co );
		},
		
		//显示遮罩层
		dispCover : function()
		{
			cover = J('<div/>',topDoc).css({
			    position: 'absolute', zIndex: getZIndex(), top: '0px',
				left: '0px', backgroundColor: '#000', opacity: 0.6
			}).appendTo( topDoc.body )[0];
			
			if( J.browser.ie && !J.browser.i7 )
			{
			    J('<iframe/>',topDoc).attr({
				    hideFocus: true, frameBorder: 0, src: this.getvoid()
				}).css({
				    width: '100%', height: '100%', position: 'absolute', left: '0px',
					top: '0px', filter: 'progid:DXImageTransform.Microsoft.Alpha(opacity=0)'
				}).appendTo( cover );
			}
			
			//窗口大小发生变化时重新计算遮罩层的尺寸
			J( topWin ).bind( 'resize', reSizeHdl ); reSizeHdl();
		},
		
		getCover : function(){ return cover; },
		hideCover : function( co ){ J(co).remove(); cover = null; co = null; }
	};
})();

J.dialog = lhgdialog;