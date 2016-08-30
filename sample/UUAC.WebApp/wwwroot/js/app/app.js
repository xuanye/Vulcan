'use strict';

function showError(title,text,islight){
    $.gritter.add({
        title: title || '错误信息',
        text: text,
        class_name: 'gritter-error'+(islight?'  gritter-light':'')
    });
}
function showWarn(title,text,islight){
    $.gritter.add({
        title: title || '警告信息',
        text: text,
        class_name: 'gritter-warning'+(islight?'  gritter-light':'')
    });
}

function showSuccess(title,text,islight){
    $.gritter.add({
        title: title || '操作成功',
        text: text,
        class_name: 'gritter-success'+(islight?'  gritter-light':'')
    });
}
function showTip(title,text,islight){
    $.gritter.add({
        title: title || '提示信息',
        text: text,
        class_name: 'gritter-info'+(islight?'  gritter-light':'')
    });
}

function removeGritter(){
    $.gritter.removeAll();
}

function post(url,data,callback,failcallback){
    $.ajax({
        url     : url,
        type    : 'POST',
        data    : data,
        success : callback,
        error   : failcallback || function (request, status, error) {
            showError('错误信息','异步请求发生异常，错误信息'+error,true);
        }
    });
}

function get(url,data,callback,type){
    $.get(url, data,callback, type);
}

var form_submit = function FormSubmit(form, callback) {
    $.ajax({
        url: form.action,
        type: form.method,
        data: $(form).serialize(),
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            alert("提交表单失败：" + result);
        }
    });
}
function id(){
    return s4()+s4()+s4()+s4();
}

var s4 = function () {
    return Math.floor((1 + Math.random()) * 0x10000)
        .toString(16)
        .substring(1);
};

function str_format(temp, data,encode) {
    return temp.replace(/\{([\w]+)\}/g, function (s1, s2) { var s = data[s2]; if (typeof (s) != "undefined") { if (s instanceof (Date)) { return s.getTimezoneOffset(); } else { return encode?encodeURIComponent(s):s; } } else { return ''; } });
}

function StrFormatNoEncode(temp, dataarry) {
    return temp.replace(/\{([\d]+)\}/g, function (s1, s2) { var s = dataarry[s2]; if (typeof (s) != "undefined") { if (s instanceof (Date)) { return s.getTimezoneOffset(); } else { return (s); } } else { return ""; } });
}

var ajax_goto = function ajaxGoto(url){
   var base_url = location.href.replace(/(\!ajax\#\!)(.+)/,'$1');
   location.href = base_url + url ;
};


function nice_date_str(date){
     
    var now = new Date();
    var m = new Date(date);

    var output ='';
    var diffs = date_help.diff('s', m ,now  ); // 相差的秒速  
    if(diffs<60){
        output = '就在刚刚';
    }
    else if(diffs<3600){
        var diffm = date_help.diff('n',  m ,now ); // 相差的秒速
        output = diffm+'分钟前';
    }
    else{
        if(date_help.diff('y',  m ,now  ) !=0){
            output = date_help.format(m,'yyyy-MM-dd HH:mm');
        }
        else if(date_help.diff('m',  m ,now ) !=0)
        {
            output = date_help.format(m,'MM-dd HH:mm');
        }
        else if(date_help.diff('d',  m ,now ) !=0){
            output = date_help.format(m,'MM-dd HH:mm');
        }
        else{
            output = date_help.format(m,'HH:mm');
        }
    }
    //Write the final value out to the template
    return output
}


/** 时间和日期相关的帮助函数 **/

var date_help  = {};

/**
 * 格式化日期
 * @param  {[Date]} date     [日期对象]
 * @param  {[String]} format [格式]
 * @return {[String]}        [返回对应格式的字符串表示]
 */
date_help.format =  function(date,format){
    var o = {
        'M+': date.getMonth() + 1,
        'd+': date.getDate(),
        'h+': date.getHours(),
        'H+': date.getHours(),
        'm+': date.getMinutes(),
        's+': date.getSeconds(),
        'q+': Math.floor((date.getMonth() + 3) / 3),
        'w': '0123456'.indexOf(date.getDay()),
        'S': date.getMilliseconds()
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (date.getFullYear() + '').substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp('(' + k + ')').test(format))
        {
            format = format.replace(RegExp.$1,RegExp.$1.length == 1 ? o[k] : ('00' + o[k]).substr(('' + o[k]).length));
        }
    }
    return format;
};
/**
 * 将字符串转换成日期对象
 * @param  {[String]} dateStr [日期的时间表示 yyyy-MM-dd或者yyyy/MM/dd]
 * @return {[Date]}     [返回转换后的日期对象]
 */
date_help.convert = function(dateStr){
    var reg = /^(\d{1,4})(-|\/|.)(\d{1,2})\2(\d{1,2})(\040+(\d{1,2}):(\d{1,2}):(\d{1,2}))?$/;
    var arr = dateStr.match(reg);
    if (arr == null) {
        return NaN;
    }
    else {
        return new Date(arr[1], parseInt(arr[3], 10) - 1, arr[4], arr[6] || 0, arr[7] || 0, arr[8] || 0);
    }
};
/**
 * 时间和日期操作加减操作
 * @param {[String]} interval [需要操作的单位 年月日时分秒，时间对象]
 * @param {[Integer]} number  [添加的数值，减少使用负数]
 * @param {[Date]} idate      [参照的时间对象]
 * @return {[Date]}           [操作后的结果时间对象]
 */
date_help.add = function(interval, number, idate) {
    number = parseInt(number);
    var date;
    date = idate;
    switch (interval) {
        case 'y': date.setFullYear(date.getFullYear() + number); break;
        case 'm': date.setMonth(date.getMonth() + number); break;
        case 'd': date.setDate(date.getDate() + number); break;
        case 'w': date.setDate(date.getDate() + 7 * number); break;
        case 'h': date.setHours(date.getHours() + number); break;
        case 'n': date.setMinutes(date.getMinutes() + number); break;
        case 's': date.setSeconds(date.getSeconds() + number); break;
        case 'l': date.setMilliseconds(date.getMilliseconds() + number); break;
    }
    return date;
};

date_help.diff = function(interval,date1,date2){
    var long = date2.getTime() - date1.getTime(); //相差毫秒
    switch(interval.toLowerCase()){
        case "y": return parseInt(date2.getFullYear() - date1.getFullYear());
        case "m": return parseInt((date2.getFullYear() - date1.getFullYear())*12 + (date2.getMonth()-date1.getMonth()));
        case "d": return parseInt(long/1000/60/60/24);
        case "w": return parseInt(long/1000/60/60/24/7);
        case "h": return parseInt(long/1000/60/60);
        case "n": return parseInt(long/1000/60);
        case "s": return parseInt(long/1000);
        case "l": return parseInt(long);
    }
    return NaN;
}

function OpenDailog(url,options) {
    window.Choose.Open(url, options || {});
}

function CloseDailog(callback,success,userstate) {
    if(parent && parent._CloseDailog) {
        parent._CloseDailog(callback, success, userstate);
    }
}