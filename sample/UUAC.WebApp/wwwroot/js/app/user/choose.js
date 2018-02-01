(function (window, $, factory) {
    if (typeof define === 'function') {
        // 如果define已被定义，模块化代码
        define('app/user/choose', function (require, exports, module) {
            require("")
            module.exports = factory(window, $);
        });
    } else {
        // 如果define没有被定义，正常执行
        return factory(window, $);
    }

})(window, jQuery, function factory(window, $) {

    var userHash = new Hash();

    var options = window.options;

    var selectNodeIds = "";
    if (parent && parent.getSelectNodeId) {
        selectNodeIds = parent.getSelectNodeId();
    }

    $("#btnCancel,#btnClose").click(function () {
        window.CloseDailog();
    });
    var treeOption = {
        url: options.loadTreeUrl,
        showcheck: true, //是否显示选择
        parsedata: parseNodeState,
        onnodeclick: onnodeclick,     
        oncheckboxclick: onchecked //当checkstate状态变化时所触发的事件，但是不会触发因级联选择而引起的变化
    }
    function onchecked(node, checkstate) {
        if (node.value == "2") //选中了用户
        {
            if (checkstate == 1) //选中
            {
                additem(node.id, node.text);
            }
            else if (checkstate == 0)//取消选中
            {
                var id = node.id.replace(/\./g, '_'); //将 . 替换成 _
                remove(node.id, id);
            }
        }
    }
    function parseNodeState(data){
        if (!selectNodeIds) {
            return;
        }
        for (var i = 0, j = data.length; i < j; i++) {
            var index = selectNodeIds.indexOf("," + data[i].value + ",");
            if (index > -1) {
                data[i].checkstate = 1;
            }
        }
    }
    function onnodeclick(item) {
        item.expand();
    }

    function additem(usercode, usertext) {
        var usul = $("#uslist>ul");
        if (usul.length == 0) {
            usul = $("<ul></ul>").appendTo($("#uslist"));
        }
        if (!userHash.hasItem(usercode)) {
            userHash.setItem(usercode, usertext);
            //操作dom;
            var id = usercode.replace(/\./g, '_');//将 . 替换成 _
            usul.append(StrFormatNoEncode("<li id='li_{2}'><div class=\"usercontainer\" style=\"margin:1px 3px 1px 6px;\"><i class='green fa fa-user' aria-hidden='true'></i><span class=\"user\">{1}({0})</span><a href=\"javascript:void(0);\"  class='red' onclick=\"javascript:_utils.removeUser(this,'{0}','{2}');\" style=\"margin-left:5px;float:right;\"><i class=\"ace-icon fa fa-times\" aria-hidden=\"true\"></i></a></div></li>", [usercode, usertext, id]));
            resettitle();
        }
    }
    function remove(usercode, id) {
        if (userHash.hasItem(usercode)) {
            userHash.removeItem(usercode)
            //操作dom;
            $("#li_" + id).remove();
           
            resettitle();
        }
    }
    function resettitle() {
        $("#uscount").text(userHash.length);
    }

    var tree = new xjTree("userTree", treeOption);

    $("#btnClear").click(function (e) {
        var usul = $("#uslist>ul");
        if (usul.length > 0) {
            usul.remove();
        }
        tree.UnCheckAll();
        var checkli = $("#userlist li.checked");
        if (checkli.length > 0) {
            checkli.each(function (e) {
                $(this).removeClass("checked");
            });
        }
        //清空
        userHash.clear();
        resettitle();
    });

    $("#btnOk").click(function (e) {
        var users = [];
        for (var a in userHash.items) {
            users.push(a);
        }
        window.CloseDailog(null, true, users);      
    });

    _utils.removeUser = function (obj, usercode, id) {
        if (userHash.hasItem(usercode)) {
            userHash.removeItem(usercode);
            //操作dom;
            $("#li_" + id).remove();
            var qu = $("#qli_" + usercode);
            if (qu.length > 0) {
                qu.removeClass("checked");
            }          
            //取消树的选中状态
            tree.SetItemsCheckState(usercode, false, false);
            resettitle();
        }
    }

    function Hash() {
        this.length = 0;
        this.items = new Array();
        for (var i = 0; i < arguments.length; i += 2) {
            if (typeof (arguments[i + 1]) != 'undefined') {
                this.items[arguments[i]] = arguments[i + 1];
                this.length++;
            }
        }

        this.removeItem = function (in_key) {
            var tmp_value;
            if (typeof (this.items[in_key]) != 'undefined') {
                this.length--;
                var tmp_value = this.items[in_key];
                delete this.items[in_key];
            }

            return tmp_value;
        }

        this.getItem = function (in_key) {
            return this.items[in_key];
        }

        this.setItem = function (in_key, in_value) {
            if (typeof (in_value) != 'undefined') {
                if (typeof (this.items[in_key]) == 'undefined') {
                    this.length++;
                }

                this.items[in_key] = in_value;
            }

            return in_value;
        }

        this.hasItem = function (in_key) {
            return typeof (this.items[in_key]) != 'undefined';
        }
        this.clear = function () {
            for (var i in this.items) {
                delete this.items[i];
            }

            this.length = 0;
        }
    }
});
