(function (window, $, factory) {
    if (typeof define === 'function') {
        // 如果define已被定义，模块化代码
        define('app/orgmanage/edit', function (require, exports, module) {
            module.exports = factory(window, $);
        });
    } else {
        // 如果define没有被定义，正常执行
        return factory(window, $);
    }

})(window, jQuery, function factory(window, $) {

    var options = window.options;
    $("#btnCancel,#btnClose").click(function () {
        window.CloseDailog();
    });
    var treeOption = {
        url: options.loadTreeUrl      
    }
    var tree = new xjTree("orgTree", treeOption);   
    $("#btnOk").click(function (e) {
        var item = tree.GetCurrentItem();
        if (!item) {
            alert("请选中一项");
            return;
        }
        window.CloseDailog(null, true, item);
    });
});