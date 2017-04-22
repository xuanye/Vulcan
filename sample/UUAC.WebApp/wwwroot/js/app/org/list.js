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
         	{ display: "组织代码", name: "OrgCode", sortable: false, align: "center", iskey: true },
	        { display: "组织名称", name: "OrgName", sortable: false, align: "center" },
	        { display: "序号", name: "Sequence", sortable: false, align: "center" },
	        { display: "备注", name: "Remark",  sortable: false, align: "center" },
	        { display: "操作", name: "OrgCode", width: 120, sortable: false, align: "center",dvCss: "hidden-sm hidden-xs action-buttons", process: formatOp, toggle: false }
           
        ],
        sortname: "OrgCode",
        sortorder: "DESC",
        title: false,
        rp: 30,
        usepager: false,
        showcheckbox: false,
        extparams: []
    };
    var xjgrid = new xjGrid("orgList", listOption);

    var treeOption = {
        url: opt.loadTreeUrl,
        onnodeclick: navi
    }
    var tree = new xjTree("orgTree", treeOption);

    function navi(item) {
        if (xjgrid) {
            xjgrid.QueryByFields([{ "name": "pcode", "value": item.value }]);
        }
    }

    //按钮事件
    $("#btnAdd").click(function (e) {
      
        var node = tree.GetCurrentItem();
        if (!!node) {
            var url = StrFormatNoEncode("{0}?pcode={1}&pname={2}", [opt.editUrl, encodeURIComponent(node.value), encodeURIComponent(node.text)]);
            window.Choose.Open(url, {
                width: 580,
                height: 450,
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

    function formatOp(value) {
        if (value !== "UUAC") {
            var ret = [];
            ret.push('<a class="green" href="javascript:window.options.Edit(\'', value, '\')"><i class="ace-icon fa fa-pencil bigger-130"></i></a>');
            ret.push('<a class="red" href="javascript:window.options.Remove(\'', value, '\')"><i class="ace-icon fa fa-trash-o bigger-130"></i></a>');
            return ret.join("");
        }
        return "";

    }
    opt.Edit = function (orgCode) {
        var url = opt.editUrl + "?orgCode=" + orgCode;
        window.Choose.Open(url, {
            width: 580,
            height: 450,
            caption: '修改组织信息',
            onclose: function () {
                xjgrid.Reload();
            }
        });
    };
    opt.Remove = function (orgCode) {
        if (!confirm("你确定要删除该组织吗？")) {
            return false;
        }
        var url = opt.removeUrl + "?orgCode=" + orgCode;
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