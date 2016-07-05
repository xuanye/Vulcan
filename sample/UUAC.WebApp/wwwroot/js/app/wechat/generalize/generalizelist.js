define(function (require, exports, module) {

    var choose = require('plugin/choose');
    exports.init = function (opt) {
    	var maiheight = document.documentElement.clientHeight;
        var mainWidth = document.documentElement.clientWidth - 2; // 减去边框和左边的宽度
        var otherpm = 120;
        var gh =maiheight - otherpm;
        var option = {
            height: gh,
            width: '100%',
            url: opt.listUrl,
            colModel: [
                { display: 'id', name: 'idx',  sortable: false, hide: false, align: 'left', iskey: true },
                { display: '渠道', name: 'channel', sortable: true,width:300, align: 'left' },
                { display: '场景ID', name: 'scene_id', sortable: true,width:70, align: 'left' },
                { display: '二维码', name: 'qrcode_ticket',width:80,  sortable: false, align: 'center',process: formatQrcodeTicket },
                { display: '微信关注地址', name: 'qrcode_url',width1:200,  sortable: false, align: 'left' },
                { display: '列表显示', name: 'show_in_list', sortable: true,width:80, align: 'center',process: formatShowInList },
                { display: '操作', name: 'idx', sortable: false,width:160, align: 'center',process:formatOp }
			],
            sortname: "idx",
            sortorder: "ASC",
            title: false,
            rp: 15,
            usepager: true,
            showcheckbox: false,
             onsuccess:success,
            extparams:[{name:'_csrf',value:$('#_csrf').val()}]
        };
        var xjgrid = new xjGrid("list", option);

         function formatOp(value, cell) {
            var show_in_list=cell[5];
            var ops = [];
            ops.push("<a href='javascript:void(0)' onclick='_utils.Edit(", value, ")' class='linkbtn' title='编辑'><i class='ace-icon fa fa-pencil bigger-120'></i><span>编辑</span></a>");
            ops.push("&nbsp;&nbsp;");
            if(show_in_list==1){
                ops.push("<a href='javascript:void(0)' onclick='_utils.updateShowInList(", value, ",0)' class='linkbtn purplex' title='列表不显示'><i class='ace-icon fa fa-list bigger-120'></i><span>列表不显示</span></a>");
            }
            else{
                ops.push("<a href='javascript:void(0)' onclick='_utils.updateShowInList(", value, ",1)' class='linkbtn purplex' title='列表显示'><i class='ace-icon fa fa-list bigger-120'></i><span>列表显示</span></a>");
            }
             return ops.join("");
        }

        function formatQrcodeTicket(value,cell){
            if(value){
                var qrcodeUrl=StrFormatNoEncode('https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}',[encodeURIComponent(value)]);
                var img=StrFormatNoEncode("<image src='{0}'/>",[qrcodeUrl]);
                return StrFormatNoEncode('<div class="qrcode-url" title="{0}"><a href="{1}" target="_blank">二维码</a></div>',[img,qrcodeUrl]);
            }
            return '';
        }
        function formatShowInList(value,cell){
            if(value==1){
                return "是";
            }
            else{
                return "否";
            }
        }
        function success(){
            $('.qrcode-url').tooltip({
                placement:'right',
                container:'body',
                html:true,
                'template': '<div class="tooltip tooltip-info"><div class="tooltip-arrow"></div><div style="padding:5px 435px 5px 5px;border-radius:3px;" class="tooltip-inner"></div></div>',
            });
        }

        $("#btnQuery").click(function(){
            var channel=$("#txtChannel").val();
            var p = { newp: 1, extparams: [{ name: "channel", value: channel},{name:'_csrf',value:$('#_csrf').val()}] };
            xjgrid.SetOptions(p);
            xjgrid.Reload();
            return false;
            
        });

            
        $("#btnAdd").click(function(){
            openEditPage('');
            return false;
        });

        _utils.Edit=function(id){
            openEditPage(id);
            return false;
        };

        _utils.updateShowInList=function(id,show_in_list) {
            var url=opt.updateShowInListUrl;
            $.post(url,{id:id,show_in_list:show_in_list,_csrf:$('#_csrf').val()},function(res){
                if(res.status==0){
                    xjgrid.Reload();
                }
                else{
                    showError('错误信息',res.message);
                }

            });
        }

       function openEditPage(id){
            var url=opt.editUrl+'?id='+id;
            choose.Open(url,{
                width: 800,
                height: 400,
                caption: '编辑',              
                onclose: function (ret) {
                     xjgrid.Reload();
                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
       }
        
    };//init
});