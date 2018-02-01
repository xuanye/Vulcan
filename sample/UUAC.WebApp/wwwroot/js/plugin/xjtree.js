; (function (window, undefined, $) {
    //扩展jquery的方法
    $.fn.swapClass = function(c1, c2) {
        return this.removeClass(c1).addClass(c2);
    };
    $.fn.switchClass = function(c1, c2) {
        if (this.hasClass(c1)) {
            return this.swapClass(c1, c2);
        }
        else {
            return this.swapClass(c2, c1);
        }
    };
	var defaults = {
        method: "POST",
        datatype: "json",
        url: false,
        cbiconpath: "/img/icon/",
        icons: ["checkbox_0.gif", "checkbox_1.gif", "checkbox_2.gif"],
        emptyiconpath: "/img/icon/s.gif",
        showcheck: false, //是否显示选择 
        oncheckboxclick: false, //当checkstate状态变化时所触发的事件，但是不会触发因级联选择而引起的变化
        onnodeclick: false,
        parsedata: false,
        cascadecheck: true,
        data: null,
        preloadcomplete: false,      
        theme: "xj-tree-arrows" //xj-tree-line,xj-tree-no-lines,xj-tree-arrows-newico
    };
    function xjTree(id,options){
    	var self = this;
     	this.treeid = id;
        this.el = $("#"+id);
        if(this.el.length ===0)
        {
            alert("错误的ID："+id);
            return;
        }
        this.options = $.extend({},defaults,options);
        this.treedata = this.options.data || [];
     
        var html = [];
        __BuildTreeRoot__(this.treeid,this.treedata,html,this.options); //构建HTML
      
        //console.info(html.length);
        this.el.addClass('xj-tree').html(html.join(""));
       
     	__BindEvent__(this.treeid,this.el,this.options);
       
    }
    function __BuildTreeRoot__(treeid,data,ht,options){
		ht.push("<div class='xj-tree-bwrap'>"); // Wrap ;
        ht.push("<div class='xj-tree-body'>"); // body ;
        ht.push("<ul class='xj-tree-root ", options.theme, "'>"); //root
        if (data && data.length > 0) {
            var l = data.length;
            for (var i = 0; i < l; i++) {
                __BuildNode__(treeid,data[i], ht, 0, i, i == l - 1,options);
            }
        }
        else {           
            __AsnyLoad__(null, false,options, function(data) {
                options.preloadcomplete && options.preloadcomplete();
                if (data && data.length > 0) {
                    options.parsedata && options.parsedata(data);                       
                    options.data = data;
                    var l = data.length;
                    for (var i = 0; i < l; i++) {
                        __BuildNode__(treeid,data[i], ht, 0, i, i === l - 1,options);
                    }
                }                
            });
        }
        ht.push("</ul>"); // root and;
        ht.push("</div>"); // body end;
        ht.push("</div>"); // Wrap end;
    }   
   
    function __BuildNode__(treeid,nd, ht, deep, path, isend,options){
            nd.isend = isend;
            var nid = nd.id.replace(/[^\w]/gi, "_");
            ht.push("<li class='xj-tree-node'>");
            ht.push("<div id='", treeid, "_", nid, "' tpath='", path, "' unselectable='on' title='", nd.text, "'");
            var cs = [];
            cs.push("xj-tree-node-el");
            if (nd.hasChildren) {
                cs.push(nd.isexpand ? "xj-tree-node-expanded" : "xj-tree-node-collapsed");
            }
            else {
                cs.push("xj-tree-node-leaf");
            }
            if (nd.classes) { cs.push(nd.classes); }

            ht.push(" class='", cs.join(" "), "'>");
            //span indent
            ht.push("<span class='xj-tree-node-indent'>");
            if (deep === 1) {
                ht.push("<img class='xj-tree-icon' src='", options.emptyiconpath, "'/>");
            }
            else if (deep > 1) {
                ht.push("<img class='xj-tree-icon' src='", options.emptyiconpath, "'/>");
                for (var j = 1; j < deep; j++) {
                    ht.push("<img class='xj-tree-elbow-line' src='", options.emptyiconpath, "'/>");
                }
            }
            ht.push("</span>");
            //img
            cs.length = 0;
            if (nd.hasChildren) {
                if (nd.isexpand) {
                    cs.push(isend ? "xj-tree-elbow-end-minus" : "xj-tree-elbow-minus");
                }
                else {
                    cs.push(isend ? "xj-tree-elbow-end-plus" : "xj-tree-elbow-plus");
                }
            }
            else {
                cs.push(isend ? "xj-tree-elbow-end" : "xj-tree-elbow");
            }
            ht.push("<img class='xj-tree-ec-icon ", cs.join(" "), "' src='", options.emptyiconpath, "'/>");
            ht.push("<img class='xj-tree-node-icon' src='", options.emptyiconpath, "'/>");
            //checkbox
            if (options.showcheck && nd.showcheck) {
                if (nd.checkstate == null || nd.checkstate == undefined) {
                    nd.checkstate = 0;
                }
                ht.push("<img  id='", treeid, "_", nid, "_cb' class='xj-tree-node-cb' src='", options.cbiconpath, options.icons[nd.checkstate], "'/>");
            }
            //a
            ht.push("<a hideFocus class='xj-tree-node-anchor' tabIndex=1 href='javascript:void(0);'>");
            ht.push("<span unselectable='on'>", nd.text, "</span>");
            ht.push("</a>");
            ht.push("</div>");
            //Child
            if (nd.hasChildren) {
                if (nd.isexpand) {
                    ht.push("<ul  class='xj-tree-node-ct'  style='z-index: 0; position: static; visibility: visible; top: auto; left: auto;'>");
                    if (nd.childNodes) {
                        var l = nd.childNodes.length;
                        for (var k = 0; k < l; k++) {
                            nd.childNodes[k].parent = nd;
                            __BuildNode__(treeid, nd.childNodes[k], ht, deep + 1, path + "." + k, k === l - 1, options);
                        }
                    }
                    ht.push("</ul>");
                }
                else {
                    ht.push("<ul style='display:none;'></ul>");
                }
            }
            ht.push("</li>");
            nd.render = true;
    }

    function __BindEvent__(treeid,parent,options){
    	var nodes = $("li.xj-tree-node>div", parent);
        nodes.each(function(i){
            __BuildEvent__.call(this,i,treeid,options);
        });
    }

    function __BuildEvent__(i,treeid,options){
        $(this).hover(function() {
            $(this).addClass("xj-tree-node-over");
        }, function() {
            $(this).removeClass("xj-tree-node-over");
        }).click(function(e){
            __NodeClick__.call(this,e,treeid,options);
        })
         .find("img.xj-tree-ec-icon").each(function(e) {
             if (!$(this).hasClass("xj-tree-elbow")) {
                 $(this).hover(function() {
                     $(this).parent().addClass("xj-tree-ec-over");
                 }, function() {
                     $(this).parent().removeClass("xj-tree-ec-over");
                 });
             }
         });
    }

    function __NodeClick__(e,treeid,options){
        var nodeElement = this;
        var path = $(this).attr("tpath");
        var et = e.target || e.srcElement;
        var item = __GetItem__(path,options);
        if (et.tagName == "IMG") {
            // +号需要展开
            if ($(et).hasClass("xj-tree-elbow-plus") || $(et).hasClass("xj-tree-elbow-end-plus")) {
                var ul = $(nodeElement).next(); //"xj-tree-node-ct"
                if (ul.hasClass("xj-tree-node-ct")) {
                    ul.show();
                }
                else {
                    var deep = path.split(".").length;
                    if (item.complete) {
                        item.childNodes != null && __AsnyBuild__(treeid, item.childNodes, deep, path, ul, item, options);
                    }
                    else {
                        $(nodeElement).addClass("xj-tree-node-loading");
                        __AsnyLoad__(item, true, options,function(data) {
                            options.parsedata && options.parsedata(data);
                            item.complete = true;
                            item.childNodes = data;
                            __AsnyBuild__(treeid,data, deep, path, ul, item,options);
                        });
                    }
                }
                if ($(et).hasClass("xj-tree-elbow-plus")) {
                    $(et).swapClass("xj-tree-elbow-plus", "xj-tree-elbow-minus");
                }
                else {
                    $(et).swapClass("xj-tree-elbow-end-plus", "xj-tree-elbow-end-minus");
                }
                $(this).swapClass("xj-tree-node-collapsed", "xj-tree-node-expanded");
            }
            else if ($(et).hasClass("xj-tree-elbow-minus") || $(et).hasClass("xj-tree-elbow-end-minus")) {  //- 号需要收缩                    
                $(this).next().hide();
                if ($(et).hasClass("xj-tree-elbow-minus")) {
                    $(et).swapClass("xj-tree-elbow-minus", "xj-tree-elbow-plus");
                }
                else {
                    $(et).swapClass("xj-tree-elbow-end-minus", "xj-tree-elbow-end-plus");
                }
                $(this).swapClass("xj-tree-node-expanded", "xj-tree-node-collapsed");
            }
            else if($(et).hasClass("xj-tree-node-cb")) // 点击了Checkbox
            {
                var s = item.checkstate != 1 ? 1 : 0;
                var r = true;
                if (options.oncheckboxclick) {
                    r = options.oncheckboxclick.call(et, item, s);
                }
                if (r != false) {
                    if (options.cascadecheck) {
                        //遍历
                        __Cascade__(__Check__, treeid,item, s , options);
                        //上溯
                        __Bubble__(__Check__,treeid, item, s,options);
                    }
                    else {
                        __Check__(treeid,item, s, 1 , options);
                    }
                }
            }       
        }
        else {
            if (options.citem) {
                var nid = options.citem.id.replace(/[^\w]/gi, "_");
                $("#" + treeid + "_" + nid).removeClass("xj-tree-selected");
            }
            options.citem = item;
            $(this).addClass("xj-tree-selected");
            if (options.onnodeclick) {
                if (!item.expand) {
                   item.expand = function() { __ExpandNode__.call(item,treeid); };
                }
                options.onnodeclick.call(this, item);
            }
        }   
    }

    function __ExpandNode__(treeid,onlydoplus){       
        var item = this;  
        var nid = item.id.replace(/[^\w]/gi, "_");
        var img = $("#" + treeid + "_" + nid + " img.xj-tree-ec-icon");
        if (img.length > 0) {
            if(onlydoplus)
            {
                if(img.hasClass("xj-tree-elbow-minus") || img.hasClass("xj-tree-elbow-end-minus") )  
                {
                    return false;
                } 
            }
            img.click();
        }
    }

    function __GetItem__(path,options){
		var ap = path.split(".");
        var t = options.data;
        for (var i = 0; i < ap.length; i++) {
            if (i === 0) {
                t = t[ap[i]];
            }
            else {
                t = t.childNodes[ap[i]];
            }
        }
        return t;
    }

    function __AsnyBuild__(treeid,nodes,deep,path,ul,pnode,options){
	    var l = nodes.length;
        if (l > 0) {
            var ht = [];
            for (var i = 0; i < l; i++) {
                nodes[i].parent = pnode;
                __BuildNode__(treeid,nodes[i], ht, deep, path + "." + i, i == l - 1,options);                
            }
            ul.html(ht.join(""));
            ht = null;
            __BindEvent__(treeid,ul,options);
        }           
        ul.addClass("xj-tree-node-ct").css({ "z-index": 0, position: "static", visibility: "visible", top: "auto", left: "auto", display: "" });
        ul.prev().removeClass("xj-tree-node-loading");
    }

    function __AsnyLoad__(pnode,isAsync,options,callback){
	    if (options.url) {
            var param;
            if (pnode) {
                param =__BuilParam__(pnode);
            }
            else {
                param = [];
            }
            if (options.extParam) {
                for (var pi = 0; pi < options.extParam.length; pi++) param[param.length] = options.extParam[pi];
            }
            
            $.ajax({
                type: options.method,
                url: options.url,
                data: param,
                async: isAsync,
                dataType: options.datatype,
                success: callback,
                error: function(e) {                       
                    alert("error occur:" + e.responseText);
                }
            });
        }
    }

    function __BuilParam__(node){
        var p = [ { name: "id", value: encodeURIComponent(node.id) }
                , { name: "text", value: encodeURIComponent(node.text) }
                , { name: "value", value: encodeURIComponent(node.value) }
                , { name: "checkstate", value: node.checkstate}];
        return p;
    }

    function __CheckItembyId__( teedata,treeid,id, state, type, options) {       
        if (teedata != null && teedata.length > 0) {
            for (var i = 0, j = teedata.length; i < j; i++) {
                __Cascade__(__DyCheck__, treeid ,teedata[i], [id, state, type],options);
            }
        }        
    }

    function __DyCheck__(treeid,item,idAndState,type,options) {      
        if (item.id == idAndState[0]) { //完成匹配
            if (idAndState[2] == 1) { //需要级联选中的
                //遍历
                __Cascade__(__Check__, treeid,item,idAndState[1] , options);
                //上溯
                __Bubble__(__Check__,treeid, item, idAndState[1],options);
            }
            else {              
                __Check__(treeid,item, idAndState[1], 1 , options);
            }
            return false;
        }
    }
    function __LocateNodeById__(teedata,treeid,sid, options){
        //step1:找到该节点
        var item = __SearchById__(sid,null,teedata);
        if(item){ //节点找到了。。        
            //确保所有父祖节点都是展开的，注：展开的自然是已经输出过了
            var arrPath = [];           
            var p = item;
            while(p){
                arrPath.push(p);
                p = p.parent;
            }
            //step2 依次展开节点
            for(var i=arrPath.length-1;i>=0;i--){
                //展开节点
                __ExpandNode__.call(arrPath[i],treeid,true);
            }
            //设置当前节点          
            if (options.citem) {
                var nid = options.citem.id.replace(/[^\w]/gi, "_");
                $("#" + treeid + "_" + nid).removeClass("xj-tree-selected");
            }
            options.citem = item;
            $("#"+treeid+"_"+sid.replace(/[^\w]/gi, "_")).addClass("xj-tree-selected");
            return true;
        }
        else{
            return false;//节点未找到
        }
    }    
    function __SearchById__(sid,pitem,list){
        if(list !=null && list.length>0){
            //先判断List第一层中是否存在
            for(var i =0,l=list.length ;i<l;i++)
            {
                list[i].parent = pitem; //便于输出
                if( list[i].id == sid){ //找到的匹配
                    return list[i];
                }
            }
            //先判断调用下一层
            for(var i =0,l=list.length ;i<l;i++)
            {
                var res = __SearchById__(sid, list[i], list[i].childNodes);
               if(res)
               {
                  return res;
               }
            }
        }
        return null;
    }

    function __Check__(treeid,item, state, type,options){
        var pstate = item.checkstate;
        if (type === 1) {
           item.checkstate = state;
        }
        else {// 上溯
            var cs = item.childNodes;
            var l = cs.length;
            var ch = true;
            for (var i = 0; i < l; i++) {
                if ((state == 1 && cs[i].checkstate != 1) || state == 0 && cs[i].checkstate != 0) {
                    ch = false;
                    break;
                }
            }
            if (ch) {
                item.checkstate = state;
            }
            else {
                item.checkstate = 2;
            }
        }
        //change show
        if (item.render && pstate != item.checkstate) {
            var nid = item.id.replace(/[^\w]/gi, "_");
            var et = $("#" + treeid + "_" + nid + "_cb");
            if (et.length === 1) {
                et.attr("src", options.cbiconpath + options.icons[item.checkstate]);
            }
        }
    }

    function __Cascade__(fn,treeid, item, args,options){
        if (fn(treeid,item, args, 1,options) != false) { // istrue ==break终止遍历
            if (item.childNodes != null && item.childNodes.length > 0) {
                var cs = item.childNodes;
                for (var i = 0, len = cs.length; i < len; i++) {
                    __Cascade__(fn,treeid, cs[i], args,options);
                }
            }
        }
    }

    function __Bubble__(fn, treeid,item, args,options){
        var p = item.parent;
        while (p) {
            if (fn(treeid,p, args, 0,options) === false) {
                break;
            }
            p = p.parent;
        }
    }

    function __GetCk__(items , c , halfCheck , fn){
        for (var i = 0, l = items.length; i < l; i++) {
            (items[i].showcheck == true && (items[i].checkstate == 1 || ( halfCheck && items[i].checkstate == 2)) ) && c.push(fn(items[i]));
            if (items[i].childNodes != null && items[i].childNodes.length > 0) {
                __GetCk__(items[i].childNodes, c, halfCheck, fn);
            }
        }
    }

    function __Reflash__(treeid,itemid,options){
        var nid = itemid.replace(/[^\w-]/gi, "_");
        var node = $("#" + treeid + "_" + nid);

        if (node.length > 0) {
            node.addClass("xj-tree-node-loading");
            var isend = node.hasClass("xj-tree-elbow-end") || node.hasClass("xj-tree-elbow-end-plus") || node.hasClass("xj-tree-elbow-end-minus");
            var path = node.attr("tpath");
            var deep = path.split(".").length;
            var item = __GetItem__(path,options);
            console.log(item);
            if (item) {
                __AsnyLoad__(item, true,options,function(data) {
                    options.parsedata && options.parsedata(data);
                    item.complete = true;
                    item.childNodes = data;
                    item.isexpand = true;
                    if (data && data.length > 0) {
                        item.hasChildren = true;
                    }
                    else {
                        item.hasChildren = false;
                    }
                    var ht = [];
                    __BuildNode__(treeid,item, ht, deep - 1, path, isend,options);
                    ht.shift();
                    ht.pop();
                    var li = node.parent();
                    li.html(ht.join(""));
                    ht = null;
                    __BindEvent__(treeid, li, options);

                    __BuildEvent__.call(li.find(">div"), 0, treeid, options);
                });
            }
        }
        else {
            alert("该节点还没有生成");
        }
    }

    function __ToggleById__(treeid,itemId){
        var nid = itemId.replace(/[^\w]/gi, "_");
        var img = $("#" + treeid + "_" + nid + " img.xj-tree-ec-icon");
        if (img.length > 0) {
            img.click();
        }
    }
    //xjTree公开的方法    
    xjTree.prototype ={
        GetCheckedItems:function(gethalfchecknode){ //获取选中的项 包含所有的节点数据
            var s = [];
            __GetCk__(this.options.data, s, gethalfchecknode, function (item) { return item; });            
            return s;
        },
        GetCheckedValues:function(gethalfchecknode){ //获取选中的项的Values
            var s = [];
            __GetCk__(this.options.data, s, gethalfchecknode, function (item) { return item.value; });
            return s;
        },
        GetCurrentItem:function(){ //获取当前项
            return this.options.citem;
        },
        Refresh:function(itemOrItemId){ //刷新某节点
            var id;
            if (typeof (itemOrItemId) == "string") {
                id = itemOrItemId;
            }
            else {
                id = itemOrItemId.id;
            }
            __Reflash__(this.treeid,id,this.options);
        },
        CheckAll:function(){ //选中全部
            if (this.options.data != null && this.options.data.length > 0) {
                for (var i = 0, j = this.options.data.length; i < j; i++) {
                    __Cascade__(__Check__, this.treeid, this.options.data[i] , 1 , this.options);
                }
            }
        },
        UnCheckAll:function(){ //反选全部
            if (this.options.data != null && this.options.data.length > 0) {
                for (var i = 0, j = this.options.data.length; i < j; i++) {
                    __Cascade__(__Check__, this.treeid, this.options.data[i] , 0 , this.options);
                }
            }
        },
        SetItemsCheckState:function(itemIds, ischecked, cascadecheck){ //设置某些节点的状态
            if (itemIds != null) {
                var arrIds = itemIds.split(",");
                if (arrIds.length > 0) {
                    var iscascadecheck = this.options.cascadecheck;
                    if (cascadecheck != null) {
                        iscascadecheck = cascadecheck;
                    }
                    var s = ischecked ? 1 : 0;                  
                    for (var i = 0, j = arrIds.length; i < j; i++) {
                        __CheckItembyId__(this.options.data,this.treeid,arrIds[i], s, iscascadecheck ? 1 : 0,this.options);
                    }
                }
            }
        },
        ToggleNode:function(itemId){  //展开和关闭某节点
            if (itemId) {
                __ToggleById__(this.treeid,itemId);
            }
        },
        GetTreeData:function(){ //获取所有数据
            return this.options.data;
        },
        LocateNode:function(nodeid){ //定位到某个节点
            if(nodeid){
                return __LocateNodeById__(this.options.data,this.treeid,nodeid,this.options);
            }
            else{
                return false;
            }
            
        }
    };
    window.xjTree = xjTree;
  
})(window, undefined, jQuery);
