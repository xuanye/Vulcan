define(function (require, exports, module) {
	var choose = require('plugin/choose');
    var SEditor = require('plugin/seditor');
    var seditorHash={};
    exports.init = function (opt) {
        initEditorControl();
        initEditorData();
        initTooltip();
        
        function initEditorControl(){
            $('[name=reply_content]').each(function(i,obj){
                var id=$(this).attr('id');
                var  seditor = new SEditor(id,{ 
                    mixedChooseUrl:opt.mixedChooseUrl,
                    getMsgUrl:opt.getMsgUrl 
                });
                seditor.init();
                seditorHash[id]=seditor;
            });

        }
        
        function initEditorData(){
            for (var p in seditorHash) {
                var editor= seditorHash[p];
                var widgetBox=$('#'+p).parents('.widget-box');
                var reply_type=widgetBox.find('[name=hiddenReplyType]').val();
                var reply_content=widgetBox.find('[name=hiddenReplyContent]').val();
                var message_idx=widgetBox.find('[name=hiddenReplyMessageIdx]').val();
                editor.setContent({type:reply_type == 1 ? 1 : 5,content:reply_content,msgidx:message_idx});
            }
        }

        function initTooltip(){
            $('[name=keywords]').tooltip({
                placement:'top',
                container:'body',
                html:true,
                'template': '<div style="margin-left:-100px;" class="tooltip tooltip-info"><div  class="tooltip-arrow"></div><div style="padding:10px;border-radius:3px;min-width:300px;text-align:left;" class="tooltip-inner" ></div></div>',
                title:"<div>关键字|匹配类型,...</div><div>1：关键字内部‘|’分隔(1 全匹配, 0 未全匹配)。</div><div>2：多个关键字逗号分隔。</div><div>示例：全匹配关键字|1,未全匹配关键字|0,...</div>"
            });
        }

        function flushPage(){
            //alert(window.location.href);
            location.reload();
        }

        $('#btnAdd').click(function(){
            $('#newbox').show();
        });

    	$('.btnEdit').click(function(){
    		//$(this).parents('[container=form]').find('[action=foot]').show();
            var me=$(this);
            var widgetBox=me.parent();
            var isEdit=me.attr('isEdit');
            if(isEdit==0){
                widgetBox.find('.formView').hide();
                widgetBox.find('.formEdit').show();
                widgetBox.find('[action=collapseicon]').removeClass('fa-chevron-down');
                widgetBox.find('[action=collapseicon]').addClass('fa-chevron-up');
                me.attr('isEdit','1');
            }
            else{
                widgetBox.find('.formView').show();
                widgetBox.find('.formEdit').hide();
                widgetBox.find('[action=collapseicon]').removeClass('fa-chevron-up');
                widgetBox.find('[action=collapseicon]').addClass('fa-chevron-down');
                me.attr('isEdit','0');
            }
    	});

        $('[action=btnSave]').click(function(){
            var widgetBox=$(this).parents('.widget-box');
            var id=widgetBox.find('[name=hiddenIdx]').val();
            var rule_name=widgetBox.find('[name=rule_name]').val();
            var keywords=widgetBox.find('[name=keywords]').val();
            var editorId=widgetBox.find('[name=reply_content]').attr("id");
            var editor= seditorHash[editorId];
            var editorData=editor.getContent();
            var postData={
                idx:            id,
                rule_name:      rule_name,
                keywords:       keywords,
                reply_type:     editorData.type == 1 ? 1 : 5,
                reply_content:  editorData.content,
                message_idx:    editorData.msgidx,
                _csrf:          $('#_csrf').val() 
            };
            var errMsg = validate(postData);
            if(errMsg){
                showWarn('校验错误',errMsg);
                return;
            }
            post(opt.saveUrl,postData,function(res){
                if(res && res.status == 0){
                   showSuccess('操作成功','保存成功了');
                   setTimeout(flushPage,1000);
                }
                else{
                   showError('操作失败',res.message||'意外错误');
                }
            });
        });
        $('[action=btnDelete]').click(function(){
            var widgetBox=$(this).parents('.widget-box');
            var id=widgetBox.find('[name=hiddenIdx]').val();
            if(!id || id=="0"){
                widgetBox.hide();
                return;
            }
            post(opt.deleteUrl,{id:id,_csrf:$('#_csrf').val() },function(res){
                if(res && res.status == 0){
                   showSuccess('操作成功','保存成功了');
                   setTimeout(flushPage,1000);
                }
                else{
                   showError('操作失败',res.message||'意外错误');
                }
            });
        });//

        function validate(data){
            var errmsg = [];
            if(!data['rule_name']){
                errmsg.push('规则名不能为空');
            }
            if(!data['keywords']){
                errmsg.push('关键字不能为空');
            }
            if(data['reply_type'] ==1 && !data['reply_content']){
                errmsg.push('回复内容不能为空');
            }
            else if(data['reply_content'].length >2000){
                errmsg.push('标题长度不能超过2000个字符');
            }
            if(data['reply_type']==5 && data['message_idx'] == -1 ){
               errmsg.push('必须选择一个图片哦');
            }
            return errmsg.join(';');
        }
    };//init
});