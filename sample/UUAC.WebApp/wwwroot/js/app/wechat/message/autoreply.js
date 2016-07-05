define(function (require, exports, module) {
     var SEditor = require('plugin/seditor');
    var seditor = null;
    exports.init = function (opt) {
        seditor = new SEditor('seditor',{ 
            mixedChooseUrl:opt.mixedChooseUrl,
            getMsgUrl:opt.getMsgUrl 
        });
        seditor.init();
        seditor.setContent({type:opt.reply_type == 1 ? 1 : 5,content:opt.reply_content,msgidx:opt.message_idx});

    	$("#btnSave").click(function(){
            var editorData=seditor.getContent();
            //content: "55"msgidx: -1type: 1
    		var data = {
    			idx:            $("#idx").val(),
    			event_type:     $("#event_type").val(),
    			reply_type:     editorData.type == 1 ? 1 : 5,
    			reply_content: 	editorData.content,
    			message_idx:    editorData.msgidx,
    			_csrf:          $('#_csrf').val() 
    		};
    		var errMsg = validate(data);
            if(errMsg){
                showWarn('校验错误',errMsg);
                return;
            }
            post(opt.saveUrl,data,function(res){
                if(res && res.status == 0){   
                    showSuccess('操作成功','保存成功了');
                    $("#idx").val(res.data.idx);
                }
                else{
                   showError('操作失败',res.message||'意外错误');
                }
            });
            return false;
    	});//save
    	
    	function validate(data){
            var errmsg = [];
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