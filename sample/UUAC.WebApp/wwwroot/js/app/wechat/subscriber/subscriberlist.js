define(function (require, exports, module) {

    exports.init = function (opt) {
    	var maiheight = document.documentElement.clientHeight;
        var mainWidth = document.documentElement.clientWidth - 2; // 减去边框和左边的宽度
        var otherpm = 120;
        var gh = maiheight - otherpm;
        var option = {
            height: gh,
            width: '100%',
            url: opt.listUrl,
            colModel: [
                { display: 'id', name: 'idx',  sortable: false, hide: false, align: 'left', iskey: true },
                { display: '头像', name: 'headimgurl', sortable: true, align: 'left',process: formatHeadImg },
                { display: '昵称', name: 'nick_name', sortable: true, align: 'left',process: formatNickName },
                { display: '性别', name: 'gender', sortable: false, align: 'center',process: formatGender },
                { display: '省', name: 'province',  sortable: false, align: 'left' },
                { display: '城市', name: 'city',  sortable: false, align: 'left' },
                { display: '语言', name: 'language', sortable: false, align: 'left',process:formatLanguage },
                { display: '订阅时间', name: 'subscribe_time', sortable: true, align: 'left' },
                { display: '状态', name: 'subscribe', sortable: false, align: 'center',process:formatStatus }
			],
            sortname: "subscribe_time",
            sortorder: "DESC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false,
            onsuccess:success,
            extparams:[{name:'_csrf',value:$('#_csrf').val()}]
        };
        var xjgrid = new xjGrid("list", option);

         function formatGender(value, cell) {
            return convertGender(value);
        }

        function formatHeadImg(value,cell){
            if(value){
                 var id=cell[0];
                return StrFormatNoEncode('<image class="headface detailContainer" src="{0}" idx="{1}" width="48px" heigth="48px"/>',[value,id])
            }
            return '';
        }

        function convertGender(value){
            var result = '';
            switch (value) {
                case 1:
                    result = "男";
                    break;
                case 2:
                    result = "女";
                    break;
                default:
                result = "未知";
                break;
            }
            return result;
        }
        function formatLanguage(value,cell){
            //zh_CN 简体，zh_TW 繁体，en 英语
            var result = '';
            switch (value) {
                case 'zh_CN':
                    result = "简体";
                    break;
                case 'zh_TW':
                    result = "繁体";
                    break;
                case 'en':
                    result = "英语";
                    break;
                default:
                result = "value";
                break;
            }
            return result;
        }

        function formatStatus(value, cell) {
            var result = '';
            switch (value) {
                case 1:
                    result = "关注";
                    break;
                default:
                result = "取消关注";
                break;
            }
            return result;
        }


        var nickNameTemplate='<span  idx="{0}">{1}</span>';
        function formatNickName(value, cell){
            var id=cell[0];
            return StrFormatNoEncode(nickNameTemplate,[id,value]);
        }

        function success(){
            remoteLoadDetail();
            $('.detailContainer').tooltip({
                placement:'right',
                container:'body',
                html:true,
                'template': '<div  class="tooltip tooltip-info"><div class="tooltip-arrow"></div><div style="padding 10px:20px 10px 10px;border-radius:3px;" class="tooltip-inner"></div></div>',
                title:function(){
                    var self=$(this);
                    var title=self.attr('data-original-title');
                    if(title){
                    return title;
                    }
                    return '正在加载...';
                }
            });
        }
        function remoteLoadDetail(){
            $('.detailContainer').each(function(){
                var self=$(this);
                post(opt.detailUrl,{id:self.attr('idx'),_csrf:$('#_csrf').val()},function(res){
                    if(res && res.status == 0){
                        self.attr('data-original-title',createTitle(res.data));
                    }
                    else{
                       showError('操作失败',res.message||'意外错误');
                    }
                });

            });
            
        }
        var detailTemplate='<div style="width:400px;"><span class="detailrowspan">{0}</span><span>{1}</span></div>';
        function createTitle(detailInfo){
            var result=[];
            result.push('<div style="text-align:left;">');
            result.push('<div>详细资料：</div>');
            result.push(StrFormatNoEncode(detailTemplate,['昵称',detailInfo.nick_name]));
            result.push(StrFormatNoEncode(detailTemplate,['性别',convertGender(detailInfo.gender)]));
            var areaInfo=StrFormatNoEncode('{0}&nbsp;{1}&nbsp;{2}',[detailInfo.country,detailInfo.province,detailInfo.city]);
            result.push(StrFormatNoEncode(detailTemplate,['区域',areaInfo]));
             result.push('</div>');
            return result.join('');
        }

        $('#btnSynchronize').click(function(){
            if(confirm("确定要同步所有数据吗？")){
                post(opt.synchronizeUrl,{_csrf:$('#_csrf').val()},function(res){
                    if(res && res.status == 0){
                        showSuccess('操作成功','后台正在同步中...，请稍后刷新');
                    }
                    else{
                       showError('操作失败',res.message||'意外错误');
                    }
                });
            }
        });
        
    };//init
});