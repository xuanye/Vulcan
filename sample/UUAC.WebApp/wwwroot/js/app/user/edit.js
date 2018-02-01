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
    $("#btnViewRootOrg").click(function () {

        var url = options.chooseOrgUrl;
        window.Choose.Open(url, {
            width: 320,
            height: 400,
            blackBG: false,
            caption: '选择组织',
            onclose: function (item) {
                $("#ViewRootCode").val(item.value);
                $("#ViewRootName").html(item.text);
            }
        });

    });
    $("#btnPOrg").click(function () {

        var url = options.chooseOrgUrl;
        window.Choose.Open(url, {
            width: 320,
            height: 400,
            blackBG:false,
            caption: '选择组织',
            onclose: function (item) {
                $("#OrgCode").val(item.value);
                $("#OrgName").html(item.text);
            }
        });

    });
    $('#frmEdit').validator({
        rules: {
            code: [/^[_0-9a-zA-Z\.]{3,25}$/, "3-25位数字、字母和下划线的组合"]
        },
        theme: 'yellow_right_effect',
        fields: {
            '#UserUid': 'required;code;remote[' + options.checkCodeUrl + ']',
            '#FullName': 'required;',
            '#UserNum': 'required;',
            '#Sequence': 'required;integer;',
            '#AccountType': 'required;'
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
