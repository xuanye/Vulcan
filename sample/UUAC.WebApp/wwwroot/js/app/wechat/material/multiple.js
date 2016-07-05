define(function (require, exports, module) {
    var ITEM_TMP = '<div id="appmsgItem{idx}" class="appmsg_item js_appmsg_item"><img class="js_appmsg_thumb appmsg_thumb" src=""><i class="appmsg_thumb default">缩略图</i><h4 class="appmsg_title"><a onclick="return false;" href="javascript:void(0);" target="_blank">{title}</a></h4><div class="appmsg_edit_mask action-buttons"><a class="icon18_common edit_gray js_edit" data-id="{idx}" onclick="return false;" href="javascript:void(0);"><i class="ace-icon fa fa-pencil bigger-130 dark"></i></a><a class="icon18_common del_gray js_del"  data-id="{idx}" onclick="return false;" href="javascript:void(0);"><i class="ace-icon fa fa-trash-o bigger-130 dark"></i></a></div></div>';

    exports.init = function (opt) {

         //富文本编辑器
        var editor = UM.getEditor('editor', {
            //图片上传配置区
            imageUrl: opt.editorImgUrl,            //图片上传提交地址
            imagePath: "",                         //图片修正地址，引用了fixedImagePath,如有特殊需求，可自行配置
            //工具栏上的所有的功能按钮和下拉框，可以在new编辑器的实例时选择自己需要的从新定义
            toolbar: [
                'bold italic underline strikethrough | superscript subscript | forecolor backcolor | removeformat |',
                'insertorderedlist insertunorderedlist | fontsize',
                '| justifyleft justifycenter justifyright justifyjustify  ',
                '| image fullscreen'
            ]
        });

        //图片上传
        $('#filepic').ace_file_input({
            no_file:'请选择文件 ...',
            btn_choose:'选择',
            btn_change:'重新选择',
            droppable:false,
            allowExt:  ['jpg', 'jpeg', 'png', 'gif'],
            thumbnail:false, //| true | large
            whitelist:'gif|png|jpg|jpeg',
            //blacklist:'exe|php'
            before_remove:function(){
                $('#pic_url').val('');
                var index = $('#currentidx').val(); 
                var item = getItemByIndex(index,opt.data); 

                var elem = $('#appmsgItem'+item.idx).removeClass('has_thumb');
                $('img.appmsg_thumb',elem).attr('src','');
                    
                return true;
            }
            //
        }).on('change', function() {
            if(this.value !=''){
                $('#tmpform').submit();
            }
            else{ // 清理
                showWarn(null,'没有选择任何图片哦');
            }

        });

        $('#title').keydown(syncTitle).blur(syncTitle);
   
        //编辑
        $("#js_appmsg_preview div.js_appmsg_item a.js_edit").each(editEvent);
        //删除
        $("#js_appmsg_preview div.js_appmsg_item a.js_del").each(delEvent);

        //新增元素
        $("#js_add_appmsg").click(function(){
            var new_item = {
                idx : id(),
                title : '默认标题',
                content : '',
                author : '',
                pic_url: '',
                url :  '',
                desc : ''
            };
            new_item.index = opt.data.length ;
            opt.data.push(new_item);
            var item_dom = buidItemElement(new_item);
            $('#js_appmsg_preview').append(item_dom);

            //事件
            editEvent.call($('#appmsgItem'+new_item.idx+' a.js_edit')[0]);
            delEvent.call($('#appmsgItem'+new_item.idx+' a.js_del')[0]);
           
        });

        // 保存按钮
        $('#btnSave').click(function(){
            var btn = $(this);
            btn.button('loading');
            updateCForm();       
            //校验   
            var err = validate(opt.data);
            if(err){
                btn.button('reset');
                //console.log(err);
                //跳转位置
                var offset = $('#appmsgItem'+err.item.idx).position();                
                $('#appmsg_editor>.appmsg_editor').animate({'margin-top': offset.top},'fast');
                updateForm(err.item);
                showWarn('校验错误',err.message);
                return;
            }
            var postData = getPostData(opt.data);
            postData._csrf = $('#_csrf').val();

            post(opt.saveUrl,postData,function(res){
                btn.button('reset');
                if(res && res.status == 0){
                   showSuccess('操作成功','保存成功了');
                   ajax_goto('/wechat/material/list');
                }
                else{
                   showError('操作失败',res.message||'意外错误');
                }
            });
            return false;
        });

        $('#btnSync').click(function(){            
            var id = $('#id').val();
            var mediaid = $('#media_id').val();
            if(!id){
                showWarn(null,'请先保存');
                return;
            }
            var index = 0 ;
            if(!!mediaid){
                //已经
                index = parseInt($('#currentidx').val());
            }
            var btn = $(this);
            btn.button('loading');
            var _csrf = $('#_csrf').val();
            //console.log(opt);
            post(opt.syncUrl,{idx:id,mediaid:mediaid,_csrf:_csrf,index:index},function(res){
                btn.button('reset');
                if(res && res.status == 0){   
                    showSuccess('操作成功','保存成功了'); 
                }
                else{
                   showError('操作失败',res.message||'意外错误');
                }
            });

        });

        if(opt.data[0].pic_url !=''){
            $('#filepic').ace_file_input('show_file_list', ['hello.jpg']);
        }

        window.utils.uploadCallback = function(res){
            if(res && res.status == 0){
                $('#pic_url').val(res.data);
                var index = $('#currentidx').val(); 
                var item = getItemByIndex(index,opt.data); 
                item.pic_url = res.data ;
                var elem = $('#appmsgItem'+item.idx).addClass('has_thumb');

                $('img.appmsg_thumb',elem).attr('src',res.data);
               
            }
            else{
                showError('错误信息',res.message);
            }
        };
        function syncTitle(){
            var index = $('#currentidx').val(); 
            var item = getItemByIndex(index,opt.data); 
            $('#appmsgItem'+item.idx+' h4.appmsg_title a').text(this.value);
        }
        function delEvent(){
            //console.log('绑定删除事件：'+$(this).attr('data-id'));
            //console.log(this);
            $(this).click(function(){
                if(opt.data.length<=2){
                    return showWarn('警告信息','多图文消息不能少于2条');
                }
                var id = $(this).attr('data-id');
                var item = getItem(id,opt.data) ;
                if(item == null){
                    return showError('错误提示','意外错误，请刷新后重试');
                }
                //移除dom元素
                $('#appmsgItem'+id).remove(0);
                clearForm(item.index);
                removeItem(item,opt.data); 
                $('#appmsg_editor>.appmsg_editor').animate({'margin-top': 0},'fast');
            });
        }
        function editEvent(){
            $(this).click(function(){
                var id = $(this).attr('data-id');               
                //console.log(id);
                // 通过dataId 获取完整的详细数据
                var item = getItem(id,opt.data) ;
                if(item == null){
                    return showError('错误提示','意外错误，请刷新后重试');
                }
                // 设置编辑框的位置
                var offset = $('#appmsgItem'+id).position();                
                $('#appmsg_editor>.appmsg_editor').animate({'margin-top': offset.top},'fast');
                // 设置输入框的值
                updateForm(item);
            });
        }

        function updateCForm(){
            var index = $('#currentidx').val(); 

            var item = getItemByIndex(index,opt.data); 
            //console.log(item);
            $('#appmsg_editor input,#appmsg_editor textarea').each(function(){
                if(item.hasOwnProperty(this.id)){
                    item[this.id] = this.value;                    
                }               
            });
            if(item){
                item.content = editor.getContent();
            }
        }

        function updateForm(item){
            var index = $('#currentidx').val(); 
            var pitem = getItemByIndex(index,opt.data);           
           //将现有的值update到对象中
            $('#appmsg_editor input,#appmsg_editor textarea').each(function(){

                if(pitem && pitem.hasOwnProperty(this.id)){
                    pitem[this.id] = this.value;
                }

                if(item.hasOwnProperty(this.id)){
                    //console.log(this.id);
                    this.value = item[this.id] ;
                }
                else{
                    this.value = '';
                }
            });
            if(pitem){
                pitem.content = editor.getContent();
            }
            editor.setContent(item.content);
            // 重置
            $('#currentidx').val(item.index); 

            $('#filepic').ace_file_input('reset_input');

            if(item.pic_url)
            {
                $('#filepic').ace_file_input('show_file_list', ['hello.jpg']);
            }
           
        }
        function clearForm(index){
            var sindex = $('#currentidx').val(); 
            if(sindex == index){
                $('#currentidx').val('-1'); 
            }
            var item = getItemByIndex(0,opt.data);          
            updateForm(item);
        }
        function buidItemElement(item){
            return str_format(ITEM_TMP,item);
        }
        function validate(data){
            for(var i=0,l = data.length ; i <l ; i++){
                var errmsg = validateItem(data[i]);
                if(errmsg){
                    data[i].index = i;
                    return {item :data[i],message:errmsg};
                }
            }
            return null;
        }
        function validateItem(item){
            var errmsg = [];
            if(!item.title){
                errmsg.push('标题不能为空');
            }
            else if(item.title.length >64){
                errmsg.push('标题长度不能超过64个字符');
            }

            if(item.author && item.author.length >8){
                errmsg.push('作者长度不能超过8个字符');
            }

            if(!item.pic_url){
                errmsg.push('必须选择一个图片哦');
            }
            if(item.desc && item.desc.length >120){
                errmsg.push('摘要长度不能超过120_字符');
            }

            if(!item.content){
                errmsg.push('内容不能为空');
            }
            else if(item.content.length >20000){
                errmsg.push('内容长度不能超过20000个字符');
            }
            return errmsg.join(';');
        }
    };

    function getPostData(data){
        var postData = {} ;
        for(var i =0,l= data.length ; i < l ; i++){
            postData['mc'+i+'_title'] = data[i].title ;
            postData['mc'+i+'_content'] = data[i].content ;
            postData['mc'+i+'_author'] = data[i].author ;
            postData['mc'+i+'_pic_url'] = data[i].pic_url ;
            postData['mc'+i+'_url'] = data[i].url ;
            postData['mc'+i+'_desc'] = data[i].desc ;
        }
        return postData;
    }
    function getItem(idx,data){

        for(var i =0,l=data.length ; i < l ; i++){
            if(data[i].idx == idx){
                data[i].index = i ;
                return data[i];
            }
        }
        return null;

    }

    function getItemByIndex(index,data){
        var ret= data[index];
        ret.index = index;
        return data[index];
    }
    function removeItem(item,data){
        data.splice(item.index,1);
    }
});
