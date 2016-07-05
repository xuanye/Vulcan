define(function (require, exports, module) {

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
                $('#hdpicurl').val('');
                $('#appmsg_thumb').removeClass('default').attr('src','');
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
        $('#title').keydown(function(){
            $("#appmsgItem1 h4.appmsg_title a").text(this.value);
        }).blur(function(){
            $("#appmsgItem1 h4.appmsg_title a").text(this.value);
        });
        $('#desc').keydown(function(){
            $("#appmsgItem1 p.appmsg_desc ").text(this.value);
        }).blur(function(){
            $("#appmsgItem1 p.appmsg_desc ").text(this.value);
        });
        // 保存按钮
        $('#btnSave').click(function(){
            var btn = $(this);
            btn.button('loading');
            //校验
            var data = {};
            $('#appmsg_editor input,#appmsg_editor textarea').each(function(){
                if(this.id != 'filepic')
                {
                    data[this.name] = this.value;
                }
            });
            data['mc0_content'] = editor.getContent();


            var errMsg = validate(data);
            if(errMsg){
                btn.button('reset');
                showWarn('校验错误',errMsg);
                return;
            }
            data._csrf = $('#_csrf').val();
            post(opt.saveUrl,data,function(res){
                btn.button('reset');
                if(res && res.status == 0){   
                    showSuccess('操作成功','保存成功了');
                    $('#id').val(res.data);
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
            if(!!mediaid){
                //已经
            }
            var btn = $(this);
            btn.button('loading');
            var _csrf = $('#_csrf').val();
            //console.log(opt);
            post(opt.syncUrl,{idx:id,mediaid:mediaid,_csrf:_csrf,index:0},function(res){
                btn.button('reset');
                if(res && res.status == 0){   
                    showSuccess('操作成功','保存成功了'); 
                }
                else{
                   showError('操作失败',res.message||'意外错误');
                }
            });

        });
        window.utils.uploadCallback = function(res){
            if(res && res.status == 0){
                $('#hdpicurl').val(res.data);
                $('#appmsg_thumb').addClass('default').attr('src',res.data);
            }
            else{
                showError('错误信息',res.message);
            }
        };

        function validate(data){
            var errmsg = [];
            if(!data['mc0_title']){
                errmsg.push('标题不能为空');
            }
            else if(data['mc0_title'].length >64){
                errmsg.push('标题长度不能超过64个字符');
            }

            if(data['mc0_author'] && data['mc0_author'].length >8){
                errmsg.push('作者长度不能超过8个字符');
            }

            if(!data['mc0_pic_url']){
                errmsg.push('必须选择一个图片哦');
            }
            if(data['mc0_desc'] && data['mc0_desc'].length >120){
                errmsg.push('摘要长度不能超过120_字符');
            }

            if(!data['mc0_content']){
                errmsg.push('内容不能为空');
            }
            else if(data['mc0_content'].length >20000){
                errmsg.push('内容长度不能超过20000个字符');
            }
            return errmsg.join(';');
        }
    };
});
