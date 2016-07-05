define(function (require, exports, module) {

    exports.init = function (opt) {
        init();
        
        var maiheight = document.documentElement.clientHeight;
        var mainWidth = document.documentElement.clientWidth - 2; // 减去边框和左边的宽度
        var otherpm = 120;
        var gh =maiheight - otherpm;
        var optionDay = {
            height: gh,
            width: '100%',
            url: opt.listUrl,
            colModel: [
                { display: '日期', name: 'dt',  sortable: false,width:160, hide: false, align: 'center', iskey: true,process: formatDt },
                { display: '当日使用人数', name: 'UseNum', sortable: false, align: 'left' },
                { display: '当日关注人数', name: 'SubscriberNum', sortable: false, align: 'left' },
                { display: '新用户使用率', name: 'DayUseRate', sortable: false, align: 'left' }
            ],
            sortname: "dt",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false,
            extparams:getSearch()
        };
        var optionWeek = {
            height: gh,
            width: '100%',
            url: opt.listUrl,
            colModel: [
                { display: '日期', name: 'dt',  sortable: false,width:160, hide: false, align: 'center', iskey: true,process: formatDt },
                { display: '周二次使用数', name: 'OneWeekNum', sortable: false, align: 'left' },
                { display: '当日关注(7日前)', name: 'UseNum', sortable: false, align: 'left' },
                { display: '周二次使用率', name: 'WeekUseRate', sortable: false, align: 'left' }
            ],
            sortname: "dt",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false,
            extparams:getSearch()
        };

        var optionMonth = {
            height: gh,
            width: '100%',
            url: opt.listUrl,
            colModel: [
                { display: '日期', name: 'dt',  sortable: false,width:160, hide: false, align: 'center', iskey: true,process: formatDt },
                { display: '月二次使用数', name: 'OneMonthNum', sortable: false, align: 'left' },
                { display: '当日关注(30日前)', name: 'UseNum', sortable: false, align: 'left' },
                { display: '月二次使用率', name: 'MonthUseRate', sortable: false, align: 'left' }
            ],
            sortname: "dt",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false,
            extparams:getSearch()
        };

        
        var xjgridDay = new xjGrid("daylist", optionDay);
        var xjgridWeek = new xjGrid("weeklist", optionWeek);
        var xjgridMonth = new xjGrid("monthlist", optionMonth);
        var showType={
            getCurrentGrid:function(){
                var currentType=$("#typelist").val();
                if(currentType==1) return xjgridDay;
                if(currentType==2) return xjgridWeek;
                if(currentType==3) return xjgridMonth;
            },
            showCurrentGrid:function(){
                var currentType=$("#typelist").val();
                if(currentType==1) {$("#weeklist").hide();$("#monthlist").hide();$("#daylist").show();};
                if(currentType==2) {$("#daylist").hide();$("#monthlist").hide();$("#weeklist").show();};
                if(currentType==3) {$("#daylist").hide();$("#weeklist").hide();$("#monthlist").show();};
            }
        };

        function formatDt(value,cell){
           var date= date_help.convert(value);
           return date_help.format(date,"yyyy-MM-dd");
        }

        $("#btnQuery").click(function(){
            var p = { newp: 1, extparams: getSearch() };
            showType.showCurrentGrid();
            var xgrid=showType.getCurrentGrid();
            xgrid.SetOptions(p);
            xgrid.Reload();
            return false;
            
        });

        function getSearch(){
            return [
                {name:'_csrf',value:$('#_csrf').val()},
                {name:'startDate',value:$('#startDate').val()},
                {name:'endDate',value:$('#endDate').val()}
            ];
        }

        function init() {
            $('#startDate,#endDate').datetimepicker({
                language: 'zh-CN',
                format: "yyyy-mm-dd",
                minView: 2
            });
            $("#startDate_ICON").click(function (e) {
                $('#startDate').datetimepicker('show');
            });
            $("#endDate_ICON").click(function (e) {
                $('#endDate').datetimepicker('show');
            });

            var startDate=date_help.add('d',-7,new Date());
            var endDate=date_help.add('d',-1,new Date());
            $('#startDate').val(date_help.format(startDate,"yyyy-MM-dd"));
            $('#endDate').val(date_help.format(endDate,"yyyy-MM-dd"));
        }
        
    };//init
});