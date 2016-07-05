define(function (require, exports, module) {

    exports.init = function (opt) {

        //$('.js_tooltip').tooltip();
       
        var $tiles = $('#material_list'),
        $handler = $('li.material-item', $tiles),
        page = 1,rp=10,
        isLoading = false,
        lastRequestTimestamp = 0,
        fadeInDelay = 2000,
        $window = $(window),
        $document = $(document);

        var _csrf = $("#_csrf").val()
        // Prepare layout options.
        var options = {
            autoResize: true, // This will auto-update the layout when the browser window is resized.
            container: $tiles, // Optional, used for some extra CSS styling
            offset: 25, // Optional, the distance between grid items
            itemWidth: 300 // Optional, the width of a grid item
        };


        /**
        * When scrolled all the way to the bottom, add more tiles.
        */
        function onScroll(event){
            if($('#material_list').length==0){
                return ;
            }
            // Only check when we're not still waiting for data.
            if (!isLoading) {
                // Check if we're within 100 pixels of the bottom edge of the broser window.
                var closeToBottom = ($window.scrollTop() + $window.height() > $document.height() - 100);
                if (closeToBottom) {
                    // Only allow requests every second
                    var currentTime = new Date().getTime();
                    if (lastRequestTimestamp < currentTime - 1000) {
                        lastRequestTimestamp = currentTime;
                        loadData();
                    }
                }
            }
        }


          /**
         * Refreshes the layout.
         */
        function applyLayout($newImages){
            //console.log($.fn.imagesLoaded);
            options.container.imagesLoaded(function(){
                // Destroy the old handler
                if ($handler.wookmarkInstance) {
                    $handler.wookmarkInstance.clear();
                }

                // Create a new layout handler.
                $tiles.append($newImages);
                $handler = $('li.material-item', $tiles);
                $handler.wookmark(options);

                // Set opacity for each new image at a random time
                $newImages.each(function(){
                    var $self = $(this);
                    window.setTimeout(function(){
                        $self.css('opacity', 1);
                    }, Math.random() * fadeInDelay);
                });
            });
        };

        /**
         * Loads data from the API.
         */
        function loadData(){     
           
            if($('#material_list').length>0){
                isLoading = true;
                $('#loaderCircle').show();
                post(opt.loadUrl,{ page: page, rp:rp ,"_csrf":_csrf},onLoadData);
            }
          
        };

        /**
         * Receives data from the API, creates HTML for images and updates the layout
         */
        function onLoadData(response){
            isLoading = false;
            $('#loaderCircle').hide();
            if(response.status !=0){
                showError('错误信息',response.message);
                return ;
            }
            if(page ==1){
               total = parseInt(response.data.total / 15) ;
               var mod  = response.data.total % 15 ;
               if(mod>0){
                    total++;
               }
            }
            // Increment page index for future calls.
            page++;
            if (total<=page -1 ) {
                $document.off('scroll', onScroll);
            }
            // Create HTML for the images.
            var html = [],data = response.data.dataList, opacity,$newImages;

            for (var i =0 ,length = data.length; i < length; i++) {
                var item = data[i];     
                if(item.message_type == 2){ // 单图文消息
                    buildSMessage(html,item);
                }
                else{ //多图文消息item.message_type == 1
                    buildMMessage(html,item);
                }
            }
            var imgHtml = html.join("");
            if(imgHtml !=""){
                $newImages = $(imgHtml);
                // Apply layout.
                applyLayout($newImages);
            }


        };

        // Capture scroll event.
        $document.on('scroll', onScroll);

        // Load first data from the API.
        loadData();
    }

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
            
        arr.push('<div class="appmsg_opr action-buttons">'); 
            arr.push('<ul>'); 
                arr.push('<li class="appmsg_opr_item grid_item size1of2">'); 
                    arr.push('<a class="js_edit js_tooltip" href="#!/wechat/material/edit/',item.idx,'" data-toggle="tooltip" title="编辑">'); 
                        arr.push('<i class="ace-icon fa fa-pencil bigger-130"></i>');                                
                    arr.push('</a>'); 
                arr.push('</li>'); 
                arr.push('<li class="appmsg_opr_item grid_item size1of2 no_extra">'); 
                    arr.push('<a class="js_del no_extra js_tooltip"  href="javascript:utils.delmaterial(',item.idx,');" data-id="',item.idx,'"  data-toggle="tooltip" title="删除">'); 
                        arr.push('<i class="ace-icon fa fa-trash-o bigger-130"></i>'); 
                    arr.push('</a>'); 
                arr.push('</li>'); 
            arr.push('</ul>'); 
        arr.push('</div>');
            
            
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

                    arr.push('<div class="appmsg_opr action-buttons">'); 
                        arr.push('<ul>'); 
                            arr.push('<li class="appmsg_opr_item grid_item size1of2">'); 
                                arr.push('<a class="js_edit js_tooltip" href="#!/wechat/material/edit/',item.idx,'" data-toggle="tooltip" title="编辑">'); 
                                    arr.push('<i class="ace-icon fa fa-pencil bigger-130"></i>');                                
                                arr.push('</a>'); 
                            arr.push('</li>'); 
                            arr.push('<li class="appmsg_opr_item grid_item size1of2 no_extra">'); 
                                arr.push('<a class="js_del no_extra js_tooltip"  href="javascript:utils.delmaterial(',item.idx,');" data-id="',item.idx,'" data-toggle="tooltip" title="删除">'); 
                                    arr.push('<i class="ace-icon fa fa-trash-o bigger-130"></i>'); 
                                arr.push('</a>'); 
                            arr.push('</li>'); 
                        arr.push('</ul>'); 
                    arr.push('</div>');

                 arr.push('</div>'); 
            arr.push('</div>'); 
        arr.push('</li>'); 
    }

    window.utils.delmaterial = function(id){
        if(confirm('你确定要删除该条消息吗？')){
            var _csrf = $("#_csrf").val();
            //alert(_csrf);
            post('/wechat/material/del/'+id,{"_csrf":_csrf},function(res){
                 if(res && res.status == 0){   
                    showSuccess('操作成功','删除成功了');
                    $('#appmsg'+id).parent().remove();
                    ajax_goto('/wechat/material/list');
                }
                else{
                   showError('操作失败',res.message||'意外错误');
                }
            });
        }
    }
});
