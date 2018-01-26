/*
 *@Copyright lhgcore.js (c) 2009 By LiHuiGang Reserved
 *@Author 本皮肤由网友陆奕堂提供 extjs样式
 */

// 此文件用于修正IE6下窗口尺寸
(function()
{
    var fixsize = window.doretsize = function()
	{
	    var lhgdlg = window.document.body;
		
		for( var i = 0, l = lhgdlg.childNodes.length; i < l; i++ )
		{
		    var child = lhgdlg.childNodes[i];
			switch( child.className )
			{
			    case 'contain':
					child.style.width = Math.max( 0, lhgdlg.offsetWidth - 7 - 7 );
					child.style.height = Math.max( 0, lhgdlg.clientHeight - 7 - 25 );
					break;
				case 'tr':
				    child.style.left = Math.max( 0, lhgdlg.clientWidth - 7 );
					break;
				case 'tc':
				    child.style.width = Math.max( 0, lhgdlg.clientWidth - 7 - 7 );
					break;
				case 'ml':
				    child.style.height = Math.max( 0, lhgdlg.clientHeight - 25 - 7 );
					break;
				case 'mr':
				    child.style.left = Math.max( 0, lhgdlg.clientWidth - 7 );
					child.style.height = Math.max( 0, lhgdlg.clientHeight - 25 - 7 );
					break;
				case 'bl':
				    child.style.top = Math.max( 0, lhgdlg.clientHeight - 7 );
					break;
				case 'br':
				    child.style.left = Math.max( 0, lhgdlg.clientWidth - 7 );
					child.style.top = Math.max( 0, lhgdlg.clientHeight - 7 );
					break;
				case 'bc':
				    child.style.width = Math.max( 0, lhgdlg.clientWidth - 7 - 7 );
					child.style.top = Math.max( 0, lhgdlg.clientHeight - 7 );
					break;
			}
		}
	};
	
	var load = function()
	{
		window.attachEvent( 'onresize', fixsize );
		fixsize(); window.detachEvent( 'onload', load );
	};
	
	window.attachEvent( 'onload', load );
})();