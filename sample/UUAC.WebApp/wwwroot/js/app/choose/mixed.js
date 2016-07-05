define(function (require, exports, module) {


    exports.init = function(opt){
        var _csrf = $("#_csrf").val();


        $('#btnClear').click(function(e){
            $('#search').val('');
            loadData();
        });
        $('#btnSearch').click(loadData);

        $('#btnOk').click(function(){
            var selected = $('#content div.appmsg.selected');
            var ret = null ;
            if(selected.length==1){
                selected.removeClass('selected');
                ret = {};
                ret.msgidx = selected.attr('data-id');
                var wrap = selected.parent();
                wrap.remove('div.appmsg_mask,i.icon_card_selected');
                ret.html = wrap.html();
            }
            parent.window._CloseDailog(null,true,ret);
        });
        $('#btnCancel').click(function(){
            parent.window._CloseDailog();
        });
        $('#btnClose').click(function(){
            parent.window._CloseDailog();
        });
        
        loadData();

        function loadData(){ 
            var qtext =  $('#search').val();
            $('#loaderCircle').show();
            //console.log(_csrf);
            post(opt.queryUrl,{ page: 1, rp:20 ,"_csrf":_csrf,'qtext':qtext},onLoadData);          
        };

        function onLoadData(response){
            isLoading = false;
            $('#loaderCircle').hide();
            if(response.status !=0){
                showError('错误信息',response.message);
                return ;
            }
          
            // Create HTML for the images.
            var html = [],data = response.data.dataList;

            for (var i =0 ,length = data.length; i < length; i++) {
                var item = data[i];     
                if(item.message_type == 2){ // 单图文消息
                    buildSMessage(html,item);
                }
                else{ //多图文消息item.message_type == 1
                    buildMMessage(html,item);
                }
            }
            var str_html = html.join("");
            $("#content").html(str_html);
            checkEvent();
        };

        function checkEvent(){
            $("#content div.appmsg").click(function(){
                var selected = $("#content div.appmsg.selected");
                if(selected.length>0){
                    selected.removeClass('selected');
                }
                $(this).addClass('selected');
            })
        }

    };
   
    function buildSMessage(arr,item){
        arr.push('<li class="material-item">');
        arr.push('<div id="appmsg',item.idx,'">');
        arr.push('<div class="appmsg " data-id="',item.idx,'">');
        arr.push('<div class="appmsg_content">');
                
        arr.push('<h4 class="appmsg_title"><a href="javascript:;">',item.mc[0].title,'</a></h4>');
        arr.push('<div class="appmsg_info">');
        arr.push('<em class="appmsg_date">',nice_date_str(item.last_modi_time),'</em>');
        arr.push('</div>');
        arr.push('<div class="appmsg_thumb_wrp"><img src="',item.mc[0].pic_url,'" alt="" class="appmsg_thumb"></div>');
        arr.push('<p class="appmsg_desc">',item.mc[0].desc,'</p>');
                
        arr.push('</div>');

        arr.push('<div class="appmsg_mask"></div>');   
        arr.push('<i class="icon_card_selected fa fa-check"></i>');               
            
        arr.push('</div>');
        arr.push('</div>');
        arr.push('</li>'); 
    }

    function buildMMessage(arr,item){
        arr.push('<li class="material-item">');
            arr.push('<div id="appmsg',item.idx,'">');
                arr.push('<div class="appmsg multi" data-id="',item.idx,'">');        
                    arr.push('<div class="appmsg_content">');
                        arr.push('<div class="appmsg_info">');
                            arr.push('<em class="appmsg_date">',nice_date_str(item.last_modi_time),'</em>');
                        arr.push('</div>');

                        arr.push('<div class="cover_appmsg_item">');
                            arr.push('<h4 class="appmsg_title">');
                                arr.push('<a href="javascript:;" target="_blank">',item.mc[0].title,'</a>');
                            arr.push('</h4>');
                            arr.push('<div class="appmsg_thumb_wrp">');
                                arr.push('<img src="',item.mc[0].pic_url,'" alt="" class="appmsg_thumb"/>');
                            arr.push('</div>');
                        arr.push('</div>');
       
                    for(var i= 1,l = item.mc.length;i<l;i++){
                        arr.push('<div class="appmsg_item">');
                            arr.push('<img src="',item.mc[i].pic_url,'" alt="" class="appmsg_thumb" />');
                            arr.push('<h4 class="appmsg_title">');
                                arr.push('<a href="javascript:;" target="_blank">',item.mc[i].title,'</a>');
                            arr.push('</h4>');
                        arr.push('</div>');                      
                    }                     
                    arr.push('</div>');

                    arr.push('<div class="appmsg_mask"></div>');   
                    arr.push('<i class="icon_card_selected fa fa-check"></i>');        
                 arr.push('</div>'); 
            arr.push('</div>'); 
        arr.push('</li>'); 
    }
});