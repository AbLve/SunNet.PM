﻿/*
 *@lhgcore - Mini JavaScript Library v1.2.5 - Date : 2009-11-24
 *@Copyright lhgcore.js (c) 2009 By LiHuiGang Reserved
 */
(function(){var q=this,o,c=q.lhgcore=q.J=function(a,b){return new c.fn.init(a,b)};c.fn=c.prototype={init:function(a,b){a=a||document;if(a.nodeType){this[0]=a;this.length=1;return this}if(typeof a==="string")if(a.indexOf("<")===0)a=c.clean([a],b);else if(a.indexOf("#")===0){a=a.substr(1);b=(b||document).getElementById(a);return b=c(b||[])}else return c().find(a,b);else if(c.isFunction(a))return c(document).ready(a);this.context=b||document;return this.setArray(c.isArray(a)?a:c.makeArray(a))},context:"",setArray:function(a){this.length=0;Array.prototype.push.apply(this,a);return this},each:function(a,b){return c.each(this,a,b)},find:function(a,b){return c(x.find(a,b))},html:function(a){return a===o?this[0]?this[0].innerHTML:null:this.empty().append(a)},text:function(a){return a===o?this[0]?this[0].innerText?this[0].innerText:this[0].textContent:null:this.each(function(){this.innerText?(this.innerText=a):(this.textContent=a)})},val:function(a){return a===o?this[0]?this[0].value:null:this.each(function(){if(this.nodeType==1)this.value=a})},css:function(a,b){if((a=="width"||a=="height")&&parseFloat(b)<0)b=o;return this.attr(a,b,"curCSS")},attr:function(a,b,d){var e=a;if(typeof a==="string")if(b===o)return this[0]&&c[d||"attr"](this[0],a);else{e={};e[a]=b}return this.each(function(f){for(a in e)c.attr(d?this.style:this,a,c.prop(this,e[a],d,f,a))})},bind:function(a,b){return this.each(function(){c.event.add(this,a,b)})},unbind:function(a,b){return this.each(function(){c.event.remove(this,a,b)})},append:function(a){return this.domManip(a,function(b){this.nodeType==1&&this.appendChild(b)})},prepend:function(a){return this.domManip(a,function(b){this.nodeType==1&&this.insertBefore(b,this.firstChild)})},before:function(a){return this.domManip(a,function(b){this.parentNode.insertBefore(b,this)})},after:function(a){return this.domManip(a,function(b){this.parentNode.insertBefore(b,this.nextSibling)})},hover:function(a,b){return this.mouseover(a).mouseout(b)},domManip:function(a,b){if(this[0]){var d=typeof a==="string"?c.clean([a],this[0].ownerDocument):a.length===o||a.nodeType==3?[a]:a;return this.each(function(e){for(var f=0,g=d.length;f<g;f++){var k=d[f].cloneNode(true);b.call(this,e>0?k:d[f])}})}return this},ready:function(a){var b=function(){if(!arguments.callee.done){arguments.callee.done=true;a.apply(document,arguments)}};if(document.addEventListener)document.addEventListener("DOMContentLoaded",function(){document.removeEventListener("DOMContentLoaded",arguments.callee,false);b()},false);else if(document.attachEvent)document.documentElement.doScroll&&q==q.top?function(){try{document.documentElement.doScroll("left")}catch(d){setTimeout(arguments.callee,0);return}b()}():document.attachEvent("onreadystatechange",function(){if(document.readyState==="complete"){document.detachEvent("onreadystatechange",arguments.callee);b()}});return this}};c.fn.init.prototype=c.fn;c.exend=c.fn.exend=function(){var a=arguments[0]||{},b=1,d=arguments.length,e=false,f;if(a.constructor==Boolean){e=a;a=arguments[1]||{};b=2}if(typeof a!="object"&&typeof a!="function")a={};if(d==b){a=this;--b}for(;b<d;b++)if((f=arguments[b])!=null)for(var g in f){var k=a[g],i=f[g];if(a!==i)if(e&&i&&typeof i=="object"&&!i.nodeType)a[g]=c.extend(e,k||(i.length!=null?[]:{}),i);else if(i!==o)a[g]=i}return a};var E=/z-?index|font-?weight|opacity|zoom|line-?height/i,A=Object.prototype.toString,B=document.defaultView||{};c.exend({isFunction:function(a){return A.call(a)==="[object Function]"},isArray:function(a){return A.call(a)==="[object Array]"},nodeName:function(a,b){return a.nodeName&&a.nodeName.toUpperCase()==b.toUpperCase()},each:function(a,b,d){var e,f=0,g=a.length;if(d)if(g===o)for(e in a){if(b.apply(a[e],d)===false)break}else for(;f<g;){if(b.apply(a[f++],d)===false)break}else if(g===o)for(e in a){if(b.call(a[e],e,a[e])===false)break}else for(d=a[0];f<g&&b.call(d,f,d)!==false;d=a[++f]);return a},clean:function(a,b){b=b||document;if(typeof b.createElement==="undefined")b=b.ownerDocument||b[0]&&b[0].ownerDocument||document;if(a.length===1&&typeof a[0]==="string"){var d=/^<(\w+)\s*\/?>$/.exec(a[0]);if(d)return[b.createElement(d[1])]}var e=[],f=b.createElement("div");c.each(a,function(g,k){if(typeof k==="number")k+="";if(k){if(typeof k==="string"){k=k.replace(/(<(\w+)[^>]*?)\/>/g,function(m,j,n){return n.match(/^(abbr|br|col|img|input|link|meta|param|hr|area|embed)$/i)?m:j+"></"+n+">"});g=k.replace(/^\s+/,"").substring(0,10).toLowerCase();var i=!g.indexOf("<opt")&&[1,'<select multiple="multiple">',"</select>"]||!g.indexOf("<leg")&&[1,"<fieldset>","</fieldset>"]||g.match(/^<(thead|tbody|tfoot|colg|cap)/)&&[1,"<table>","</table>"]||!g.indexOf("<tr")&&[2,"<table><tbody>","</tbody></table>"]||(!g.indexOf("<td")||!g.indexOf("<th"))&&[3,"<table><tbody><tr>","</tr></tbody></table>"]||!g.indexOf("<col")&&[2,"<table><tbody></tbody><colgroup>","</colgroup></table>"]||c.browser.ie&&[1,"div<div>","</div>"]||[0,"",""];for(f.innerHTML=i[1]+k+i[2];i[0]--;)f=f.lastChild;if(c.browser.ie){var w=/<tbody/i.test(k);g=!g.indexOf("<table")&&!w?f.firstChild&&f.firstChild.childNodes:i[1]=="<table>"&&!w?f.childNodes:[];for(i=g.length-1;i>=0;--i)c.nodeName(g[i],"tbody")&&!g[i].childNodes.length&&g[i].parentNode.removeChild(g[i])}/^\s/.test(k)&&f.insertBefore(b.createTextNode(k.match(/^\s*/)[0]),f.firstChild);k=c.makeArray(f.childNodes)}if(k.nodeType)e.push(k);else e=c.merge(e,k)}});return e},trim:function(a){return(a||"").replace(/^\s\s*/,"").replace(/\s\s*$/,"")},makeArray:function(a){var b=[];if(a!=null){var d=a.length;if(d==null||typeof a==="string"||c.isFunction(a)||a.setInterval)b[0]=a;else for(;d;)b[--d]=a[d]}return b},inArray:function(a,b){for(var d=0,e=b.length;d<e;d++)if(b[d]===a)return d;return-1},grep:function(a,b,d){for(var e=[],f=0,g=a.length;f<g;f++)!d!=!b(a[f],f)&&e.push(a[f]);return e},merge:function(a,b){var d=0,e,f=a.length;if(c.browser.ie)for(;(e=b[d++])!=null;){if(e.nodeType!=8)a[f++]=e}else for(;(e=b[d++])!=null;)a[f++]=e;return a},map:function(a,b){for(var d=[],e=0,f=a.length;e<f;e++){var g=b(a[e],e);if(g!=null)d[d.length]=g}return d.concat.apply([],d)},prop:function(a,b,d,e,f){if(c.isFunction(b))b=b.call(a,e);return typeof b==="number"&&d=="curCSS"&&!E.test(f)?b+"px":b},attr:function(a,b,d){if(!(!a||a.nodeType==3||a.nodeType==8)){var e=d!==o;b=c.props[b]||b;if(a.tagName){var f=/href|src|style/.test(b);if(b in a&&!f){if(e){if(b=="type"&&c.nodeName(a,"input")&&a.parentNode)throw"type property can't be changed";a[b]=d}if(c.nodeName(a,"form")&&a.getAttributeNode(b))return a.getAttributeNode(b).nodeValue;return a[b]}if(c.browser.ie&&b=="style")return c.attr(a.style,"cssText",d);e&&a.setAttribute(b,""+d);a=c.browser.ie&&f?a.getAttribute(b,2):a.getAttribute(b);return a===null?o:a}if(c.browser.ie&&b=="opacity"){if(e){a.zoom=1;a.filter=(a.filter||"").replace(/alpha\([^)]*\)/,"")+(parseInt(d)+""=="NaN"?"":"alpha(opacity="+d*100+")")}return a.filter&&a.filter.indexOf("opacity=")>=0?parseFloat(a.filter.match(/opacity=([^)]*)/)[1])/100+"":""}b=b.replace(/-([a-z])/ig,function(g,k){return k.toUpperCase()});if(e)a[b]=d;return a[b]}},curCSS:function(a,b,d){var e,f=a.style;if(b=="opacity"&&c.browser.ie){e=c.attr(f,"opacity");return e==""?"1":e}if(b.match(/float/i))b=c.props["float"];if(!d&&f&&f[b])e=f[b];else if(B.getComputedStyle){if(b.match(/float/i))b="float";b=b.replace(/([A-Z])/g,"-$1").toLowerCase();if(a=B.getComputedStyle(a,null))e=a.getPropertyValue(b);if(b=="opacity"&&e=="")e="1"}else if(a.currentStyle){e=b.replace(/\-(\w)/g,function(g,k){return k.toUpperCase()});e=a.currentStyle[b]||a.currentStyle[e];if(!/^\d+(px)?$/i.test(e)&&/^\d/.test(e)){b=f.left;d=a.runtimeStyle.left;a.runtimeStyle.left=a.currentStyle.left;f.left=e||0;e=f.pixelLeft+"px";f.left=b;a.runtimeStyle.left=d}}return e},className:{add:function(a,b){c.each((b||"").split(/\s+/),function(d,e){if(a.nodeType==1&&!c.className.has(a.className,e))a.className+=(a.className?" ":"")+e})},remove:function(a,b){if(a.nodeType==1)a.className=b!==o?c.grep(a.className.split(/\s+/),function(d){return!c.className.has(b,d)}).join(" "):""},has:function(a,b){return a&&c.inArray(b,(a.className||a).toString().split(/\s+/))>-1}},cache:{guid:1,fuid:1,createListener:function(a){return function(){c.event.handle.apply(c.cache[a].elem,arguments)}}},event:{add:function(a,b,d){var e=a.guid||(a.guid=c.cache.guid++);c.cache[e]||(c.cache[e]={elem:a,listener:c.cache.createListener(e),events:{}});if(b&&!c.cache[e].events[b]){c.cache[e].events[b]={};c.bind(a,b,c.cache[e].listener)}if(d){if(!d.fuid)d.fuid=c.cache.fuid++;c.cache[e].events[b][d.fuid]=d}},remove:function(a,b,d){try{a.guid}catch(e){return}var e=c.cache[a.guid],f,g;if(e){f=e.events;if(b===o)for(b in f)this.remove(a,b);else if(f[b]){if(d)delete f[b][d.fuid];else for(var k in f[b])delete f[b][k];for(g in f[b])break;if(!g){c.unbind(a,b,e.listener);g=null;delete f[b]}}for(g in f)break;g||delete c.cache[a.guid]}},handle:function(a){a=a=c.event.fix(a||q.event);var b=c.cache[this.guid].events[a.type];for(var d in b){this.func=b[d];if(this.func(a)===false){a.preventDefault();a.stopPropagation()}}},fix:function(a){if(!a.preventDefault)a.preventDefault=function(){this.returnValue=false};if(!a.stopPropagation)a.stopPropagation=function(){this.cancelBubble=true};if(!a.target)a.target=a.srcElement||document;if(a.target.nodeType==3)a.target=a.target.parentNode;if(!a.relatedTarget&&a.fromElement)a.relatedTarget=a.fromElement==a.target?a.toElement:a.fromElement;return a}},bind:function(a,b,d){if(a.addEventListener)a.addEventListener(b,d,false);else a.attachEvent&&a.attachEvent("on"+b,d)},unbind:function(a,b,d){if(a.removeEventListener)a.removeEventListener(b,d,false);else a.detachEvent&&a.detachEvent("on"+b,d)}});var x=function(){var a=/(?:[\w\-\\=!~+^%$:.#[\]\*]+)|>/g,b=/^(?:[\w\-_]+)?\.([\w\-_]+)/,d=/^(?:[\w\-_]+)?#([\w\-_]+)/,e=/^([\w\*\-_]+)/,f=[null,null],g=function(m,j,n){for(var l,h=[],p=-1,s=-1;l=m[++p];)if(n===o){if(l[j]||l.getAttribute(j))h[++s]=l}else if(n.test(l[j]||l.getAttribute(j)))h[++s]=l;return h},k=function(m,j,n){var l=m.pop();if(l===">")return k(m,j,true);var h,p=[],s=-1,t=(l.match(d)||f)[1],C=!t&&(l.match(b)||f)[1],v=!t&&(l.match(e)||f)[1],F=-1,u,y,D;v=v&&v.toLowerCase();if(l.indexOf("[")>-1){y=i(l,x.context);D=true}for(;l=j[++F];){u=l.parentNode;do{if(D)for(var z=0,G=y.length;z<G;z++)if(u===y[z]){h=true;break}else h=false;else h=(h=(h=!v||v==="*"||v===u.nodeName.toLowerCase())&&(!t||u.id===t))&&(!C||RegExp("(^|\\s)"+C+"(\\s|$)").test(u.className));if(n||h)break}while(u=u.parentNode);if(h)p[++s]=l}return m[0]&&p[0]?k(m,p):p},i=function(m){var j=/([a-z]*)(.*|$)/.exec(m);m=j[2].replace(/\[(.*)\]/g,"$1").split("][");j=x.context.getElementsByTagName(j[1]||"*");for(var n=0,l=m.length,h=null;n<l;n++)if(h=/([\w]+)([=^%!$]+)(.*)$/.exec(m[n])){c.props[h[1]]&&(h[1]=c.props[h[1]]);switch(h[2]){case"%=":j=g(j,h[1],RegExp(h[3]));break;case"=":j=g(j,h[1],RegExp("^"+h[3]+"$"));break;case"^=":j=g(j,h[1],RegExp("^"+h[3]));break;case"$=":j=g(j,h[1],RegExp(h[3]+"$"));break}}else j=g(j,m[n]);return j},w=function(){var m=+new Date,j=function(){var n=1;return function(l){var h=l[m],p=n++;if(!h){l[m]=p;return true}return false}}();return function(n){for(var l=n.length,h=[],p=-1,s=0,t;s<l;++s){t=n[s];if(j(t))h[++p]=t}m+=1;return h}}();return{find:function(m,j){this.context=j=j||document;if(m.indexOf(",")>-1){m=m.split(/,/g);for(var n=[],l=0,h=m.length;l<h;++l)n=n.concat(this.find(m[l],j));return w(n)}m=m.match(a);h=m.pop();n=(h.match(d)||f)[1];l=!n&&(h.match(b)||f)[1];var p=!n&&(h.match(e)||f)[1];h=h.indexOf("[")>-1?i(h,j):!n&&c.makeArray(j.getElementsByTagName(p||"*"));if(l)h=g(h,"className",RegExp("(^|\\s)"+l+"(\\s|$)"));if(n){j=j.getElementById(n);return c(j||[])}return m[0]&&h[0]?k(m,h):h}}}();c.each("blur,focus,load,resize,scroll,unload,click,dblclick,contextmenu,mousedown,mouseup,mousemove,mouseover,mouseout,mouseenter,mouseleave,change,select,submit,keydown,keypress,keyup,error".split(","),function(a,b){c.fn[b]=function(d){return this.each(function(){this["on"+b]=null;c.event.remove(this,b);c.event.add(this,b,d)})}});c(q).bind("unload",function(){delete c.cache.guid;delete c.cache.fuid;delete c.cache.createListener;for(var a in c.cache)a!=1&&c.cache[a].listener&&c.event.remove(c.cache[a].elem);delete c.cache});c.each({appendTo:"append",prependTo:"prepend",insertBefore:"before",insertAfter:"after"},function(a,b){c.fn[a]=function(d){d=c(d,this.context);c(d)[b](this);return this}});c.each({removeAttr:function(a){c.attr(this,a,"");this.nodeType==1&&this.removeAttribute(a)},addClass:function(a){c.className.add(this,a)},removeClass:function(a){c.className.remove(this,a)},toggleClass:function(a,b){if(typeof b!=="boolean")b=!c.className.has(this,a);c.className[b?"add":"remove"](this,a)},remove:function(){var a=this.getElementsByTagName("*"),b=[];if(a.length>0)b=c.makeArray(a);b=b.push(this);c.each(b,function(){c.event.remove(this)});this.parentNode&&this.parentNode.removeChild(this)},empty:function(){var a=this.getElementsByTagName("*");for(c(a).remove();this.firstChild;)this.removeChild(this.firstChild)}},function(a,b){c.fn[a]=function(){return this.each(b,arguments)}});var r=navigator.userAgent.toLowerCase();c.browser={ver:(r.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/)||[0,"0"])[1],ie:/msie/.test(r)&&!/opera/.test(r),ch:/chrome/.test(r),op:/opera/.test(r),sa:/webkit/.test(r)&&!/chrome/.test(r),mz:/mozilla/.test(r)&&!/(compatible|webkit)/.test(r)};c.browser.i7=c.browser.ie&&c.browser.ver>=7;c.browser.i8=c.browser.ie&&c.browser.ver==8;c.props={"for":"htmlFor","class":"className","float":c.browser.ie?"styleFloat":"cssFloat"};c.exend({ajax:function(a){if(a===o||!a.url)return false;var b=function(){return q.ActiveXObject?new ActiveXObject("Microsoft.XMLHTTP"):new XMLHttpRequest},d=a.type?a.type.toLocaleUpperCase():"GET",e=a.ret||"text",f=a.data?a.data+"&uuid="+(new Date).getTime():null,g=c.isFunction(a.fn),k,i=b();i.open(d,a.url,g);if(g)i.onreadystatechange=function(){if(i.readyState==4){k=e==="xml"?i.responseXML:e==="json"?q.eval("("+i.responseText+")"):i.responseText;a.fn(k);delete i}else return false};if(d==="GET")i.send(f);else{i.setRequestHeader("content-type","application/x-www-form-urlencoded");if(f)i.send(f);else return false}if(!g)if(i.readyState==4&&i.status==200){k=e==="xml"?i.responseXML:e==="json"?q.eval("("+i.responseText+")"):i.responseText;delete i;return k}else return false}})})();