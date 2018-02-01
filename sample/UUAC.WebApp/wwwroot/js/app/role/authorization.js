(function (window, $, factory) {
    if (typeof define === 'function') {
        // 如果define已被定义，模块化代码
        define('app/role/authorization', function (require, exports, module) {
            module.exports = factory(window, $);
        });
    } else {
        // 如果define没有被定义，正常执行
        return factory(window, $);
    }
})(window, jQuery, function (window, $) {

        var roleCode = options.roleCode;
        var appCode= options.roleCode.split("_")[0]
   
        $("#btnCancel,#btnClose").click(function () {
            window.CloseDailog();
        });

        

        var treeOption = {
            url: options.loadTreeUrl,
            showcheck: true, //是否显示选择         
            cascadecheck: true,
            extParam: [{ name: "appCode", value: appCode }, { name: "roleCode", value: roleCode }],
            onnodeclick: onnodeclick        
        }
        var tree = new xjTree("userTree", treeOption);

        
        //保存
        $("#btnOk").click(function () {
            var pCodes = tree.GetCheckedValues(true);
            post(options.saveUrl,
                { "roleCode": roleCode, "privilegeCodes": pCodes.join(",") },
                function (ret) {
                    if (ret.status == 0) {
                        alert("操作成功！");
                        window.CloseDailog();
                    }
                    else {
                        alert(ret.message || "操作失败");
                    }
                    
                }
            )
        });
        function onnodeclick(item) {
            item.expand();
        }

});
