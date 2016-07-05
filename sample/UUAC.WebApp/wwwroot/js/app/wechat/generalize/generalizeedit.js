define(function (require, exports, module) {
    exports.init = function (opt) {
        init();
    	$('#btnCancel').click(function(){
            parent.window._CloseDailog();
        });
        $('#btnClose').click(function(){
            parent.window._CloseDailog();
        });

        $('#btnOk').click(function(){
            var postData={
                            idx:            $("#idx").val(),
                        channel:            $("#channel").val(),
                       scene_id:            $("#scene_id").val(),
                    create_time:            $("#create_time").val(),
                 create_user_id:            $("#create_user_id").val(),
                  qrcode_ticket:            $("#qrcode_ticket").val(),
                     qrcode_url:            $("#qrcode_url").val(),
                   show_in_list:            $("#show_in_list").val(),
                          _csrf:            $('#_csrf').val() 
            };
            var errMsg = validate(postData);
            if(errMsg){
                //showWarn('校验错误',errMsg);
                alert(errMsg);
                return;
            }
            post(opt.saveUrl,postData,function(res){
                if(res && res.status == 0){
                    alert('操作成功');
                   //showSuccess('操作成功','保存成功了');
                   parent.window._CloseDailog(null,true,{});
                }
                else{
                    alert(res.message);
                   //showError('操作失败',res.message||'意外错误');
                }
            });
        	
        });

        function validate(data){
            var errmsg = [];
            if(!data['channel']){
                errmsg.push('渠道不能为空');
            }
            return errmsg.join(';');
        }

        function init() {
            $("#show_in_list").val(opt.show_in_list);
        }

    };//init
});