define(function (require, exports, module) {
   
    exports.init = function (opt) {

        var badgeInfoTemplate='<div class="badge-info {0}"><span>{1}</span><span class="badge-content">{2}</span><i class="ace-icon fa {3}"></i></div>';
        
        createBadgeContainer();

        function createBadgeContainer(){
            var msgInfo=opt.msgInfo;
            var html=[];//新增
            html.push('<div class="badge-container">');
            html.push(createBadgeInfo('日',msgInfo.new_user_info.add_day));
            html.push(createBadgeInfo('周',msgInfo.new_user_info.add_week));
            html.push(createBadgeInfo('月',msgInfo.new_user_info.add_month));
            html.push('</div>');
            $('#newuser').append(html.join(''));

            html=[];//取消
            html.push('<div class="badge-container">');
            html.push(createBadgeInfo('日',msgInfo.cancel_user_info.cancel_day));
            html.push(createBadgeInfo('周',msgInfo.cancel_user_info.cancel_week));
            html.push(createBadgeInfo('月',msgInfo.cancel_user_info.cancel_month));
            html.push('</div>');
            $('#canceluser').append(html.join(''));

            html=[];//净增
            html.push('<div class="badge-container">');
            html.push(createBadgeInfo('日',msgInfo.net_user_info.net_day));
            html.push(createBadgeInfo('周',msgInfo.net_user_info.net_week));
            html.push(createBadgeInfo('月',msgInfo.net_user_info.net_month));
            html.push('</div>');
            $('#netuser').append(html.join(''));

            html=[];//总数
            html.push('<div class="badge-container">');
            html.push(createBadgeInfo('日',msgInfo.cumulate_user_info.total_day));
            html.push(createBadgeInfo('周',msgInfo.cumulate_user_info.total_week));
            html.push(createBadgeInfo('月',msgInfo.cumulate_user_info.total_month));
            html.push('</div>');
            $('#cumulateuser').append(html.join(''));
        }

        function createBadgeInfo(title,number){
            var badgeInfoClass=number >=0 ? 'badge-success' : 'badge-danger';
            var badgeInfoArrowClass=number >=0 ? 'fa-arrow-up' : 'fa-arrow-down';
            return StrFormatNoEncode(badgeInfoTemplate,[badgeInfoClass,title,formatNumber(number),badgeInfoArrowClass]);
        }
        function formatNumber(number){
            return Math.abs(Math.round(number*100))+"%";
        }
        function StrFormatNoEncode(temp, dataarry) {
            return temp.replace(/\{([\d]+)\}/g, function (s1, s2) { var s = dataarry[s2]; if (typeof (s) != "undefined") { if (s instanceof (Date)) { return s.getTimezoneOffset(); } else { return (s); } } else { return ""; } });
        }
           
    };//init
});