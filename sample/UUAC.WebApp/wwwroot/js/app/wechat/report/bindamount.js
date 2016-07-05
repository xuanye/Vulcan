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
                { display: '截止日期', name: 'dt',  sortable: false, hide: false, align: 'center', iskey: true,process: formatDt },
                { display: '盛大账号绑定量', name: 'snda_bind_num', sortable: false, align: 'left' },
                { display: '绑定用户数', name: 'wechat_bind_num', sortable: false, align: 'left' },
                { display: '用户数', name: 'wechat_num', sortable: false, align: 'left' },
                { display: '用户绑定率', name: 'wechat_bind_ratio', sortable: false, align: 'right',process:formatRate }
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

        function formatRate(value,cell){
           return value*100+"%";
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