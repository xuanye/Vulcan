(function (window, $, factory) {
    if (typeof define === 'function') {
        // 如果define已被定义，模块化代码
        define('app/role/roleusers', function (require, exports, module) {
            module.exports = factory(window, $);
        });
    } else {
        // 如果define没有被定义，正常执行
        return factory(window, $);
    }
})(window, jQuery, function (window, $) {

    var opt = window.options;
    xjgrid = null; 
    $("#btnClose").click(function () {
        window.CloseDailog();
    });

    function initGrid() {
        var maiheight = document.documentElement.clientHeight;
        //var mainWidth = document.documentElement.clientWidth - 2; // 减去边框和左边的宽度
        var otherpm = 120;
        var gh = maiheight - otherpm;
        var listOption = {
            height: gh,
            width: '100%',
            url: opt.listUrl,
            colModel: [
                { display: "用户标识", name: "UserUid", width: 100, sortable: false, align: "center", iskey: true },
                { display: "用户名称", name: "FullName", sortable: false, align: "center" },
                { display: "所属组织", name: "OrgName", sortable: false, align: "center" },
                { display: "工号", name: "UserNum", sortable: false, align: "left" },
                { display: "操作", name: "UserUid", width: 140, sortable: false, align: "center", dvCss: "action-buttons", process: formatOp, toggle: false }
            ],
            sortname: "sequence",
            sortorder: "asc",
            title: false,
            rp: 15,
            usepager: true,
            autoload: true,
            showcheckbox: false,
            extparams: [{ name: "roleCode", value: opt.roleCode }]
        };
        xjgrid = new xjGrid("glist", listOption);
    }
    initGrid();
    function formatOp(value, cell) {
        var ret = [];        
        ret.push('<a class="red" href="javascript:window.options.Remove(\'', value, '\')"><i class="ace-icon fa fa-trash-o bigger-130"></i></a>');          
        return ret.join("");

    }
    $("#btnAddRoleUser").click(function () {
        // 弹出一个新的用户选择模块窗口
        var url = opt.chooseUserUrl;
        window.Choose.Open(url, {
            width: 500,
            height: 450,
            blackBG: false,
            caption: '选择用户',
            onclose: function (item) {
            
            }
        });
    });
    $("#btnQuery").keydown(function (e) {
        if (e.keyCode === 13) { //enter
            raiseQuery(this.value);
        }
    });

    function raiseQuery(qText) {
        if (xjgrid) {
            var p = [];
            p.push({ name: "roleCode", value: opt.roleCode });
            p.push({ "name": "queryText", "value": qText });
            xjgrid.QueryByFields(p);
        }
    }
    opt.Remove = function (Id) {
        if (!confirm("你确定要该角色中移除当前用户吗？")) {
            return false;
        }
        var url = opt.removeUrl + "/" + Id;
        post(url, null, function (ret) {
            if (ret && ret.status === 0) {
                showSuccess("操作成功", "删除成功", true);
                xjgrid.Reload();
            }
            else {
                showError("操作失败", ret ? ret.message : "删除失败，请刷新后重试", true);
            }
        });
    };

});