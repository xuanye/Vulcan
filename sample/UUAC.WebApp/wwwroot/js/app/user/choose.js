(function (window, $, factory) {
    if (typeof define === 'function') {
        // 如果define已被定义，模块化代码
        define('app/user/choose', function (require, exports, module) {
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
        url: options.loadTreeUrl,
        showcheck: true, //是否显示选择 
        oncheckboxclick: onchecked //当checkstate状态变化时所触发的事件，但是不会触发因级联选择而引起的变化
    }
    function onchecked(item,state) {

    }

    var tree = new xjTree("userTree", treeOption);   
    $("#btnOk").click(function (e) {
        var item = tree.GetCurrentItem();
        if (!item) {
            alert("请选中一项");
            return;
        }
        window.CloseDailog(null, true, item);
    });
});