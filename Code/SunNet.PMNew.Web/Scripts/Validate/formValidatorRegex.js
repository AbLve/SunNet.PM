﻿var regexEnum = 
{
	intege:"^-?[1-9]\\d*$",					//int
	intege1:"^[1-9]\\d*$",					//int>0
	intege2:"^-[1-9]\\d*$",					//int<0
	num:"^([+-]?)\\d*\\.?\\d+$",			//number
	num1:"^[1-9]\\d*|0$",					//int>=0
	num2:"^-[1-9]\\d*|0$",					//int<=0
	decmal:"^([+-]?)\\d*\\.\\d+$",			//float
	decmal1: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*$", //float >0
	decmal2: "^-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*)$", //float<0
	decmal3: "^-?([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0)$", //float
	decmal4: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0$", //float >=0
	decmal5: "^(-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*))|0?.0+|0$", //float<=0
	email:"^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", //email
	color:"^[a-fA-F0-9]{6}$",				//color
	url:"^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$",	//url
	chinese:"^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$",					//chinese
	ascii:"^[\\x00-\\xFF]+$",				//Ascll Only
	zipcode:"^\\d{6}$",						//postcode
	mobile:"^13[0-9]{9}|15[012356789][0-9]{8}|18[0256789][0-9]{8}|147[0-9]{8}$",				//mobile
	ip4:"^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)$",	//ip
	notempty:"^\\S+$",						//notnull
	picture:"(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$",	//picture
	rar:"(.*)\\.(rar|zip|7zip|tgz)$",								//rar
	date:"^\\d{4}(\\-|\\/|\.)\\d{1,2}\\1\\d{1,2}$",					//date
	qq:"^[1-9]*[1-9][0-9]*$",				//qq
	tel:"^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$",	//phone
	username:"^\\w+$",						//username
	letter:"^[A-Za-z]+$",					//letter
	letter_u:"^[A-Z]+$",					//Bigletter
	letter_l:"^[a-z]+$",					//small letter
	idcard:"^[1-9]([0-9]{14}|[0-9]{17})$"	//
}

var aCity={11:"北京",12:"天津",13:"河北",14:"山西",15:"内蒙古",21:"辽宁",22:"吉林",23:"黑龙江",31:"上海",32:"江苏",33:"浙江",34:"安徽",35:"福建",36:"江西",37:"山东",41:"河南",42:"湖北",43:"湖南",44:"广东",45:"广西",46:"海南",50:"重庆",51:"四川",52:"贵州",53:"云南",54:"西藏",61:"陕西",62:"甘肃",63:"青海",64:"宁夏",65:"新疆",71:"台湾",81:"香港",82:"澳门",91:"国外"} 

function isCardID(sId){ 
	var iSum=0 ;
	var info="" ;
	if(!/^\d{17}(\d|x)$/i.test(sId)) return "你输入的身份证长度或格式错误"; 
	sId=sId.replace(/x$/i,"a"); 
	if(aCity[parseInt(sId.substr(0,2))]==null) return "你的身份证地区非法"; 
	sBirthday=sId.substr(6,4)+"-"+Number(sId.substr(10,2))+"-"+Number(sId.substr(12,2)); 
	var d=new Date(sBirthday.replace(/-/g,"/")) ;
	if(sBirthday!=(d.getFullYear()+"-"+ (d.getMonth()+1) + "-" + d.getDate()))return "身份证上的出生日期非法"; 
	for(var i = 17;i>=0;i --) iSum += (Math.pow(2,i) % 11) * parseInt(sId.charAt(17 - i),11) ;
	if(iSum%11!=1) return "你输入的身份证号非法"; 
	return true;//aCity[parseInt(sId.substr(0,2))]+","+sBirthday+","+(sId.substr(16,1)%2?"男":"女") 
} 

//短时间，形如 (13:04:06)
function isTime(str)
{
	var a = str.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})$/);
	if (a == null) {return false}
	if (a[1]>24 || a[3]>60 || a[4]>60)
	{
		return false;
	}
	return true;
}

//短日期，形如 (2003-12-05)
function isDate(str)
{
	var r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/); 
	if(r==null)return false; 
	var d= new Date(r[1], r[3]-1, r[4]); 
	return (d.getFullYear()==r[1]&&(d.getMonth()+1)==r[3]&&d.getDate()==r[4]);
}

//长时间，形如 (2003-12-05 13:04:06)
function isDateTime(str)
{
	var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/; 
	var r = str.match(reg); 
	if(r==null) return false; 
	var d= new Date(r[1], r[3]-1,r[4],r[5],r[6],r[7]); 
	return (d.getFullYear()==r[1]&&(d.getMonth()+1)==r[3]&&d.getDate()==r[4]&&d.getHours()==r[5]&&d.getMinutes()==r[6]&&d.getSeconds()==r[7]);
}