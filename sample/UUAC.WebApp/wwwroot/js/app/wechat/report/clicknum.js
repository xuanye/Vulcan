define(function (require, exports, module) {

    exports.init = function (opt) {
        init();
        var maiheight = document.documentElement.clientHeight;
        var mainWidth = document.documentElement.clientWidth - 2; // 减去边框和左边的宽度
        var otherpm = 120;
        var gh =maiheight - otherpm;
        var option = {
            height: gh,
            width: '100%',
            url: opt.listUrl,
            colModel: [
                { display: '日期', name: 'dt',  sortable: false, hide: false, align: 'center', iskey: true,process: formatDt },
                { display: '账号管理', name: 'AccountManage', sortable: false, align: 'left' },
                { display: '在线客服', name: 'os', sortable: false, align: 'left' },
                //{ display: '装备找回', name: 'equip', sortable: false, align: 'left' },
                { display: '事件追踪', name: 'event', sortable: false, align: 'left' },
                //{ display: '热点攻略', name: 'games', sortable: false, align: 'left' },
                { display: '事件自助提交量', name: 'event_submit', sortable: false, align: 'left' },
                //{ display: '举报建议', name: 'apply', sortable: false, align: 'left' },
                { display: '在线搜索', name: 'os_search', sortable: false, align: 'left' },
                { display: '其他', name: 'others', sortable: false, align: 'left' },
                { display: '全部', name: 'AllNum', sortable: false, align: 'left' }
            ],
            sortname: "dt",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false,
            extparams:getSearch()
        };

        
        var xjgrid = new xjGrid("list", option);

        function formatDt(value,cell){
           var date= date_help.convert(value);
           return date_help.format(date,"yyyy-MM-dd");
        }

        $("#btnQuery").click(function(){
            var p = { newp: 1, extparams: getSearch() };
            xjgrid.SetOptions(p);
            xjgrid.Reload();
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