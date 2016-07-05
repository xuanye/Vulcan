define(function (require, exports, module) {
    var SEditor = require('plugin/seditor');
    var menus = null ;
    var seditor = null;
    exports.init = function (options) {
        var _csrf = $('#_csrf').val();

        post(
            options.getMenuUrl,
            {'_csrf':_csrf},
            function(res){
                if(res && res.status ==0){
                   menus = process(res.data);
                }
                else{
                   return showError('错误信息',res.message);
                }
            }
        );

        // 顶部按钮的事件
        $('#btnAddTop').click(function(){
            if(menus == null){
                return showError('错误信息','自定义菜单信息不存在,请刷新后重试');
            }
            var l = menus.button.length;
            if(l >=3){
                return showWarn('警告信息','顶级菜单最多只能添加 <em class="light-red">3</em> 个,不能加了哦');
            }

            var new_btn = {name:'新菜单',sub_button:[]} ;
            var html  = [];
            processTop(html,[new_btn]);


            menus.button.push(new_btn);

            var newid = new_btn.id;

            if(l == 0){ //没有菜单
                $('#menuList').append(html.join(''));
            }
            else{
                var lastId = menus.button[l-1].id;
                //处理HTML
                var pItem = $('#clist_'+lastId);
                if(pItem.length !=1){
                    return showError('错误信息','意外错误，请刷新后重试');
                }
                pItem.after(html.join(''));
            }

            bindEvent($("#item_"+newid));

            showEditDailog(new_btn);

            return false;
        });
        $('#edit_Save').click(saveItem);

        $('#edit_Type').change(setEditTypeDailog);

        $('#btnPublish').click(function(){
            var btn = $(this);
            btn.button('loading');
            var menuJson = JSON.stringify(menus);
            post(
                options.pubMenuUrl,
                {'_csrf':_csrf,'menus':menuJson},
                function(res){
                    if(res && res.status ==0){
                       showSuccess('操作成功','发布成功，赶紧登录微信看看吧');
                    }
                    else{
                       showError('操作失败',res.message||'意外错误');
                    }
                    btn.button('reset');
                }
            );


        });

        seditor = new SEditor('seditor',{ 
            mixedChooseUrl:options.mixedChooseUrl,
            getMsgUrl:options.getMsgUrl 
        });
        seditor.init();


        function saveItem(){
            var name = $('#edit_Name').val();
            var id= $('#edit_Id').val();
            if(name==''){
                showWarn('警告信息','名称不能为空');
                return false;
            }
            if(name.length>=10){
                showWarn('警告信息','文字是不是多了点，不要超过10个字符');
                return false;
            }

            var typeVal  = $('#edit_Type').val();
            //console.log(typeVal);
            var type = '';
            var key ='';
            if(typeVal=='click'){
                type = typeVal ;
                //TODO:获取KEY
                //key = typeVal.substr(6);
            }
            else{
                type = typeVal ;
            }
            var url = $('#edit_Url').val();
            var item = getTopItem(id);
            if(item ==null){
                item = getChildItem(id);
            }
            //console.log(id);
            if(item == null){
                showError('错误信息','意外错误，请刷新后重试');
                return false;
            }

            item.name = name;
            item.type = type;

            if(item.type =='view'){
                item.url = url;
            }
            else if( item.type=='click' ){
                item.content = seditor.getContent();
                item.key = key;
                //console.log(item);
            }
            $('#text_'+id).html(name);

            $('#editDailog').removeClass('active');
            showSuccess('操作成功','操作成功，很完美');
            return false;

        }

      

    };

    function process(data){
        var html = [];
        var buttons = data.menu.button;
        if(!!buttons){
           processTop(html,buttons);
        }
        var mlist =  $('#menuList');
        mlist.html(html.join(''));
        bindEvent(mlist,true);
        return data.menu;
    }
    function processTop(html,buttons){

        for( var i=0,l=buttons.length ; i<l ;i++){
            buttons[i].id = id();
            if(!buttons[i].type){
                buttons[i].type = 'parent';
            }
            html.push('<li id="item_',buttons[i].id,'" class="list-group-item">');
            html.push('<div class="list-group-item-heading">');
            html.push('<span id="text_',buttons[i].id,'">',buttons[i].name,'</span>');
            html.push('<div class="toolbar action-buttons">');
            html.push('<a href="javascript:;" class="btn-top-item-add" id="add_',buttons[i].id,'"><i class="ace-icon fa fa-plus green bigger-110"></i></a>');
            html.push('<a href="javascript:;" class="btn-top-item-edit" id="edit_',buttons[i].id,'"><i class="ace-icon fa fa-pencil blue bigger-110"></i></a>');
            html.push('<a href="javascript:;" class="btn-top-item-del" id="del_',buttons[i].id,'"><i class="ace-icon fa fa-trash-o red bigger-110"></i></a>');
            html.push('</div>');
            html.push('</div>');
            html.push('</li>');
            //子菜单
            html.push('<ul id="clist_',buttons[i].id ,'" class="list-group sub-list">');
            processChild( html , buttons[i].sub_button,  buttons[i].id ) ;
            html.push('</ul>');
        }

    }
    function processChild (html,subdata,pid){
        if(subdata){
            for(var i =0 ,l= subdata.length ; i<l ;i++){
                subdata[i].id = id();
                subdata[i].pid = pid ;
                html.push('<li id="sitem_',subdata[i].id,'"  class="list-group-item">');
                html.push('<div class="list-group-item-heading">');
                html.push('<span id="text_',subdata[i].id,'">',subdata[i].name,'</span>');
                html.push('<div class="toolbar action-buttons">');
                html.push('<a href="javascript:;" class="btn-item-edit" id="cedit_',subdata[i].id,'"><i class="ace-icon fa fa-pencil blue bigger-110"></i></a>');
                html.push('<a href="javascript:;" class="btn-item-del" id="cdel_',subdata[i].id,'"><i class="ace-icon fa fa-trash-o red bigger-110"></i></a>');
                html.push('</div>');
                html.push('</div>');
                html.push('</li>');
            }
        }


    }

    function bindEvent(parent,bindChild){
        $('a.btn-top-item-add',parent).click(topAdd);
        $('a.btn-top-item-edit',parent).click(topEdit);
        $('a.btn-top-item-del',parent).click(topDel);

        bindChild && bindItemEvent(parent)
    }
    function bindItemEvent(parent){
        $('a.btn-item-edit',parent).click(itemEdit);
        $('a.btn-item-del',parent).click(itemDel);
    }
    function topAdd(){
        var id = this.id.split('_')[1] ;
        var clist = $('#clist_'+id);
        var item = getTopItem(id);
        if(!item.sub_button){
            item.sub_button = [];
        }
        if(item.sub_button.length>=5){
            return showWarn('警告信息','二级菜单最多只能添加 <em class="light-red">5</em> 个,不能加了哦');
        }
        var new_child = {name:'新菜单',type:'view',url:''};
        var html = [];
        processChild(html,[new_child],id);

        //console.log(clist);

        clist.append(html.join(''));
        item.sub_button.push(new_child);
        var newid = new_child.id;
        bindItemEvent($('#sitem_'+newid));

        showEditDailog(new_child);

        //阻止冒泡
        return false;
    }
    function topEdit(){
        var id = this.id.split('_')[1] ;
        var item = getTopItem(id);

        //弹出修改名称的
        showEditDailog(item);
        //阻止冒泡
        return false;
    }
    function topDel(){
        var id = this.id.split('_')[1] ;
        var clist = $('#clist_'+id +" li.list-group-item");
        if(clist.length>0){
            return showError('错误信息','子菜单不为空不能删除，请先删除子菜单');
        }

        //删除数据
        delTopItem(id);
        //删除节点
        $('#item_'+id).remove();
        $('#clist_'+id).remove();

        //阻止冒泡
        return false;
    }
    function itemEdit(){
        var id = this.id.split('_')[1] ;
        var item = getChildItem(id);
        //弹出修改名称的
        showEditDailog(item);
        //阻止冒泡
        return false;
    }
    function itemDel(){
        var id = this.id.split('_')[1] ;
        var item = getChildItem(id);
        delChildItem(item);
        //删除DOM元素
        $('#sitem_'+id).remove(0);
        //阻止冒泡
        return false;
    }
    function getTopItem(id){
        var l = menus.button.length;
        for(var i=0 ; i<l ;i++){
            if(menus.button[i].id ==id ){
                return menus.button[i];
            }
        }
        return null;
    }

    function delTopItem(id){
        return delItem(menus.button,id);
    }
    function delChildItem(item){
        var pitem = getTopItem(item.pid);
        return delItem(pitem.sub_button,item.id);
    }
    function delItem(parent,id){
        var l = parent.length;
        var index = -1 ;
        for(var i=0 ; i<l ;i++){
            if(parent[i].id ==id ){
               index = i;
               break;
            }
        }
        if(index>=0){
            parent.splice(index,1);
            return true;
        }
        return false;
    }
    function getChildItem(id){
        var l = menus.button.length;
        for(var i=0 ; i<l ;i++){
            if(!menus.button[i].sub_button)
            {
                continue;
            }
            for(var j=menus.button[i].sub_button.length-1;j>=0;j--){
                if( menus.button[i].sub_button[j].id ==id ){
                    return menus.button[i].sub_button[j];
                }
            }
        }
        return null;
    }
    function showEditDailog(item){
        $('div.editPanel.active').removeClass('active');

        $('#edit_Name').val(item.name);
        $('#edit_Id').val(item.id);

        if(item.url){
            $('#edit_Url').val(item.url);
        }
        else{
            $('#edit_Url').val('');
        }

        if(item.type == 'click'){
            $('#edit_Type').val('click');
            //item.key  TODO:修改当前菜单的KEY
            seditor.setContent(item.content);
        }
        else{
            $('#edit_Type').val(item.type);
            seditor.setContent();
        }

        setEditTypeDailog();
        $('#editDailog').addClass('active');
    }
    
    function setEditTypeDailog(){
        var typeArr =  $('#edit_Type').val().split('_') ; //以后会用到
        var type = typeArr[0];

        var id= $('#edit_Id').val();
        var item = getTopItem(id);
        if(item ==null){
            item = getChildItem(id);
        }
        //console.log(id);
        if(item == null){
            showError('错误信息','意外错误，请刷新后重试');
            return false;
        }
        switch(type){
            case 'parent':// 父菜单隐藏所有编辑项
                $('#edit_Url_Panel').hide();
                $('#edit_Msg_Panel').hide();
                break;
            case 'view':// 链接跳转，显示编辑链接的区域
                $('#edit_Url_Panel').show();
                $('#edit_Msg_Panel').hide();
                break;
            case 'click':// 点击推送,可能还要根据KEY的区别显示图文还是单消息
                $('#edit_Url_Panel').hide();
                $('#edit_Msg_Panel').show();
                break;
        }

    }
   
});
