define(function (require, exports, module) {
    exports.init = function (opt) {
        var maiheight = document.documentElement.clientHeight;
        var mainWidth = document.documentElement.clientWidth - 2; // 减去边框和左边的宽度
        var otherpm = 120;
        var gh =maiheight - otherpm;
        var option = {
            height: gh,
            width: '100%',
            url: opt.listUrl,
            colModel: [
                { display: '分组id', name: 'id',  sortable: false,width:160, hide: false, align: 'center', iskey: true },
                { display: '分组名字', name: 'name', sortable: false, align: 'left' },
                { display: '用户数量', name: 'count', sortable: false, align: 'left' }
            ],
            sortname: "id",
            sortorder: "ASC",
            title: false,
            rp: 10000,
            usepager: false,
            showcheckbox: false,
            extparams:getSearch()
        };
        
        var xjgrid = new xjGrid("list", option);

        function getSearch(){
            return [
                {name:'_csrf',value:$('#_csrf').val()}
            ];
        }
        
    };//init
});