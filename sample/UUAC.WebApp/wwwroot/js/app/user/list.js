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
})(window, jQuery, function (window, $) {
    var opt = window.options;

    var maiheight = document.documentElement.clientHeight;
    //var mainWidth = document.documentElement.clientWidth - 2; // 减去边框和左边的宽度
    var otherpm = 120;
    var gh = maiheight - otherpm;
    var listOption = {
        height: gh,
        width: '100%',
        url: opt.listUrl,
        colModel: [
         	{ display: "用户标识", name: "UserUid", sortable: false, align: "center", iskey: true },
	        { display: "用户名称", name: "FullName", sortable: false, align: "center" },
	        { display: "工号", name: "UserNum", sortable: false, align: "center" },
            { display: "账号类型", name: "AccountType", sortable: false, align: "center" },
            { display: "排序号", name: "Sequence", sortable: false, align: "center" },
            { display: "是否管理员", name: "IsAdmin", sortable: false, align: "center" },
	        { display: "操作", name: "UserUid", width: 120, sortable: false, align: "center", dvCss: "hidden-sm hidden-xs action-buttons", process: formatOp, toggle: false }
        ],
        sortname: "sequence",
        sortorder: "asc",
        title: false,
        rp: 30,
        usepager: true,
        showcheckbox: true,
        extparams: []
    };
    var xjgrid = new xjGrid("userList", listOption);

    var treeOption = {
        url: opt.loadTreeUrl,
        onnodeclick: navi
    }
    var tree = new xjTree("orgTree", treeOption);

    function navi(item) {
        if (xjgrid) {
            xjgrid.QueryByFields([{ "name": "orgCode", "value": item.value }]);
        }
    }

    //按钮事件
    $("#btnAdd").click(function (e) {
        var node = tree.GetCurrentItem();
        console.log(node);
        if (!!node) {
            if(node.value==="000000") {
                showError("错误信息", "不能在根组织下创建用户");
                return;
            }
            var url = StrFormatNoEncode("{0}?pcode={1}&pname={2}", [opt.editUrl, encodeURIComponent(node.value), encodeURIComponent(node.text)]);
            window.Choose.Open(url, {
                width: 580,
                height:650,
                caption: '新建组织',
                theme: "simple", //默认
                onclose: function (state) {
                    showSuccess("操作成功","操作成功",true);
                    tree.Refresh(node.value);
                    xjgrid.Reload();
                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        }
        else { //根节点

            showError("错误信息","请先选中一个父组织");
        }
       
    });
    $("#btnQuery").keydown(function (e) {
        if(e.keyCode ===13) { //enter
            raiseQuery(this.value);
        }
    });

    function raiseQuery(qText) {
        if (xjgrid) {
            var p = [];
            var item = tree.GetCurrentItem();
            if(item !=null) {
                p.push({ "name": "orgCode", "value": item.value });
            }
            p.push({ "name": "qText", "value": qText });

            xjgrid.QueryByFields(p);
        }
    }
    function formatOp(value) {
        if (value !== "UUAC") {
            var ret = [];
            ret.push('<a class="green" href="javascript:window.options.Edit(\'', value, '\')"><i class="ace-icon fa fa-pencil bigger-130"></i></a>');
            ret.push('<a class="red" href="javascript:window.options.Remove(\'', value, '\')"><i class="ace-icon fa fa-trash-o bigger-130"></i></a>');
            return ret.join("");
        }
        return "";

    }
    opt.Edit = function (userId) {
        
        var url = opt.editUrl + "?userId=" + userId;
        window.Choose.Open(url, {
            width: 580,
            height: 650,
            caption: '修改用户信息',
            onclose: function () {
                xjgrid.Reload();
            }
        });
    };
    opt.Remove = function (userId) {
        if (!confirm("你确定要删除该用户吗？")) {
            return false;
        }
        var url = opt.removeUrl + "?userId=" + userId;
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