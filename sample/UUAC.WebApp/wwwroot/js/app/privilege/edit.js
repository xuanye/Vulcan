(function (window, $, factory) {
    if (typeof define === 'function') {
        // 如果define已被定义，模块化代码
        define('app/privilege/edit', function (require, exports, module) {
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
   
    $('#frmEdit').validator({
        rules: {
            code: [/^[_0-9a-zA-Z]{3,25}$/, "3-25位数字、字母和下划线的组合"]
        },
        theme: 'yellow_right_effect',
        fields: {
            '#PrivilegeCode': 'required;code;remote[' + options.checkCodeUrl + ']',
            '#PrivilegeName': 'required;',
            '#UserNum': 'required;',
            '#Sequence': 'required;integer;',
            '#PrivilegeType': 'required;'
        },
        valid: function (form) {
            form_submit(form, function (res) {
                if (res.status === 0) {
                    CloseDailog(null, true); //关闭窗口 ，激活回调
                }
                else {
                    alert(res.message);
                }
            });
        }
    });
    $("#btnSave").click(function (e) {
        $('#frmEdit').submit();
    });
});