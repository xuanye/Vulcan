(function (window, $, factory) {

    if (typeof define === 'function') {
        // 如果define已被定义，模块化代码
        define('app/appmanage/list', function (require, exports, module) {
            module.exports = factory(window, $);
        });
    } else {
        // 如果define没有被定义，正常执行
        return factory(window, $);
    }

})(window, jQuery, function factory(window, $) {
    var opt = window.options;
    var maiheight = document.documentElement.clientHeight;
    var mainWidth = document.documentElement.clientWidth - 2; // 减去边框和左边的宽度
    var otherpm = 120;
    var gh = maiheight - otherpm;
    var option = {
        height: gh,
        width: '100%',
        url: opt.listUrl,
        colModel: [
            { display: '系统标识', name: 'AppCode', sortable: false, hide: false, align: 'left', iskey: true },
            { display: '系统名称', name: 'AppName', sortable: true, align: 'left' },
            { display: '备注', name: 'Description', sortable: true, align: 'left' },
            { display: '操作', name: 'AppCode', width:200, sortable: false, align: 'center', dvCss:"hidden-sm hidden-xs action-buttons", process: formatOp }
        ],
        sortname: "AppCode",
        sortorder: "DESC",
        title: false,
        rp: 15,
        usepager: false,
        showcheckbox: true,
        extparams: []
    };
    var xjgrid = new xjGrid("list", option);

    function formatOp(value) {
        if (value !== "UUAC") {
            var ret = [];
            ret.push('<a class="green" href="javascript:window.options.Edit(\'', value, '\')"><i class="ace-icon fa fa-pencil bigger-130"></i></a>');
            ret.push('<a class="red" href="javascript:window.options.Remove(\'', value, '\')"><i class="ace-icon fa fa-trash-o bigger-130"></i></a>');
            return ret.join("");
        }
        return "";
     
    }

    // 初始化按钮事件
    $("#btnAdd").click(function (e) {
        window.Choose.Open(opt.editUrl, {
            width: 550,
            height: 400,
            caption: '新建系统',
            onclose:function() {
                xjgrid.Reload();
            }
        });
    });

    opt.Edit = function (appCode) {
        var url = opt.editUrl + "?appCode=" + appCode;
        window.Choose.Open(url, {
            width: 550,
            height: 400,
            caption: '修改系统信息',
            onclose: function () {
                xjgrid.Reload();
            }
        });
    };
    opt.Remove = function (appCode) {
        if(!confirm("你确定要删除该系统吗？")) {
            return false;
        }
        var url = opt.removeUrl + "?appCode=" + appCode;
        post(url,null, function (ret) {
            if(ret && ret.status ===0) {
                showSuccess("操作成功", "删除成功", true);
                xjgrid.Reload();
            }
            else {
                showError("操作失败",ret ?ret.message:"删除失败，请刷新后重试",true)
            }
        });
    };
});