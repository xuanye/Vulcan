(function( window, $,factory ) { 

  if (typeof define === 'function') {
    // 如果define已被定义，模块化代码
    define('plugin/seditor',function (require, exports, module) {  
        var choose = require('plugin/choose');
        module.exports =  factory( window, $ , choose );
    });
  } else {
        // 如果define没有被定义，正常执行
        window.seditor = factory(window , $ ,window.choose);
  }

})( window,jQuery,function factory(window,$,choose){

    var emotions =[
        ['0.gif','微笑'],['1.gif','撇嘴'],['2.gif','色'],['3.gif','发呆'],['4.gif','得意'],['5.gif','流泪'],['6.gif','害羞'],       
        ['7.gif','闭嘴'],['8.gif','睡'],['9.gif','大哭'],['10.gif','尴尬'],['11.gif','发怒'],['12.gif','调皮'],['13.gif','呲牙'],
        ['14.gif','惊讶'],['15.gif','难过'],['16.gif','酷'],['17.gif','冷汗'],['18.gif','抓狂'],['19.gif','吐'],['20.gif','偷笑'],
        ['21.gif','可爱'],['22.gif','白眼'],['23.gif','傲慢'],['24.gif','饥饿'],['25.gif','困'],['26.gif','惊恐'],['27.gif','流汗'],
        ['28.gif','憨笑'],['29.gif','大兵'],['30.gif','奋斗'],['31.gif','咒骂'],['32.gif','疑问'],['33.gif','嘘'],['34.gif','晕'],
        ['35.gif','折磨'],['36.gif','衰'],['37.gif','骷髅'],['38.gif','敲打'],['39.gif','再见'],['40.gif','擦汗'],['41.gif','抠鼻'],
        ['42.gif','鼓掌'],['43.gif','糗大了'],['44.gif','坏笑'],['45.gif','左哼哼'],['46.gif','右哼哼'],['47.gif','哈欠'],['48.gif','鄙视'],
        ['49.gif','委屈'],['50.gif','快哭了'],['51.gif','阴险'],['52.gif','亲亲'],['53.gif','吓'],['54.gif','可怜'],['55.gif','菜刀'],['56.gif','西瓜'],
        ['57.gif','啤酒'],['58.gif','篮球'],['59.gif','乒乓'],['60.gif','咖啡'],['61.gif','饭'],['62.gif','猪头'],['63.gif','玫瑰'],['64.gif','凋谢'],
        ['65.gif','示爱'],['66.gif','爱心'],['67.gif','心碎'],['68.gif','蛋糕'],['69.gif','闪电'],['70.gif','炸弹'],['71.gif','刀'],['72.gif','足球'],
        ['73.gif','瓢虫'],['74.gif','便便'],['75.gif','月亮'],['76.gif','太阳'],['77.gif','礼物'],['78.gif','拥抱'],['79.gif','强'],['80.gif','弱'],
        ['81.gif','握手'],['82.gif','胜利'],['83.gif','抱拳'],['84.gif','勾引'],['85.gif','拳头'],['86.gif','差劲'],['87.gif','爱你'],['88.gif','NO'],
        ['89.gif','OK'],['90.gif','爱情'],['91.gif','飞吻'],['92.gif','跳跳'],['93.gif','发抖'],['94.gif','怄火'],['95.gif','转圈'],['96.gif','磕头'],
        ['97.gif','回头'],['98.gif','跳绳'], ['99.gif','挥手'],['100.gif','激动'],['101.gif','街舞'],['102.gif','献吻'],['103.gif','左太极'],['104.gif','右太极']
    ];
    var emotions_code = "/::)/::~/::B/::|/:8-)/::</::$/::X/::Z/::'(/::-|/::@/::P/::D/::O/::(/::+/:--b/::Q/::T/:,@P/:,@-D/::d/:,@o/::g/:|-)/::!/::L/::>/::,@/:,@f/::-S/:?/:,@x/:,@@/::8/:,@!/:!!!/:xx/:bye/:wipe/:dig/:handclap/:&-(/:B-)/:<@/:@>/::-O/:>-|/:P-(/::'|/:X-)/::*/:@x/:8*/:pd/:<W>/:beer/:basketb/:oo/:coffee/:eat/:pig/:rose/:fade/:showlove/:heart/:break/:cake/:li/:bome/:kn/:footb/:ladybug/:shit/:moon/:sun/:gift/:hug/:strong/:weak/:share/:v/:@)/:jj/:@@/:bad/:lvu/:no/:ok/:love/:<L>/:jump/:shake/:<O>/:circle/:kotow/:turn/:skip/:oY".split('/:');
    for(var i =1 ,l = emotions.length ; i<=l ; i++ ){
        emotions[i-1].push('/:'+emotions_code[i]);
    }
    //console.log(emotions);
    function seditor(id,options){
        this.id = id;
        this.options = options ;   
        this.options.tabIndex = 0;
        this.msgidx = -1;    
    }
    seditor.prototype.init = function(){
        this.element = $('#' + this.id).addClass('seditor');
        this.render();
        this.initEvent();
    } 
    seditor.prototype.onChoosed = function(userstate){
        var self = this;
        if(userstate)
        {
            this.msgidx = userstate.msgidx;
            var htmlwrap = [];
            htmlwrap.push('<div class="material-item">');
            htmlwrap.push(userstate.html);
            htmlwrap.push('</div>');
            $('#'+this.id+'_mixed_wrap').html(htmlwrap.join(''));   
            $('#'+this.id+'_mixed_editpanel').addClass('hascontent');   
        }
        else{
            this.msgidx = -1;
            $('#'+this.id+'_mixed_wrap').html('');
            $('#'+this.id+'_mixed_editpanel').removeClass('hascontent');   
        }        
    }
    seditor.prototype.getContent = function(){
        var ret     = {};
        ret.type    = this.options.tabIndex + 1 ;
        if(ret.type == 1){
            ret.content = $('#'+this.id+'_stext_content').html();

            //先把div替换成换行
            //ret.content  = ret.content.replace(/<div>([^<]*)<\/div>/ig,'$1<br/>'); 
            //ret.content  = ret.content.replace(/<span>([^<]*)<\/span>/ig,'$1<br/>'); 
            //ret.content  = ret.content.replace(/<p>([^<]*)<\/p>/ig,'$1<br/>'); 


            //ret.content = ret.content.replace(/&lt;/ig,'<');
            //ret.content = ret.content.replace(/&gt;/ig,'>'); 

            ret.content = ret.content.replace('\'','"');
          

            // 过去A标签之外的所有HTML 
            //ret.content  = ret.content.replace(/<(?!a|(\/a)|(br))[^>]*>/ig,''); 

            //console.log(ret.content);
            //ret.content = ret.content.replace(/<img\040+src=\"[^\"]+\"\040+alt=\"([^\"]+)\"\040+code=\"([^\"]+)\"\040+class=\"emotiongif\"[^>]*>/ig,'$2');
            //console.log(ret.content);
            ret.msgidx  = -1;
        }
        else{
            ret.content = '';
            ret.msgidx  = this.msgidx;
        } 
        return ret;
    }

    seditor.prototype.setContent = function(content){
        if(!content){
            content = {type:1,content:''};
        }
        var id = this.id;
        this.options.tabIndex = content.type -1 ;
        if(content.type == 1){
            $('#'+id+'_mixed_editpanel').hide();
            $('#'+id+'_stext_editpanel').show();

            this.msgidx = -1 ; 
            $('#'+id+'_stext_content').html(content.content);
            $('#'+id+'_mixed_wrap').html('');
            $('#'+id+'_mixedtab').removeClass('active');
            $('#'+id+'_texttab').addClass('active');     
        }
        else{            
            $('#'+id+'_stext_editpanel').hide();
            $('#'+id+'_mixed_editpanel').show();
            $('#'+id+'_texttab').removeClass('active');
            $('#'+id+'_mixedtab').addClass('active');     
            this.msgidx = content.msgidx;
            $('#'+id+'_stext_content').html('');

            get(this.options.getMsgUrl+this.msgidx,
                {},
                function(ret){
                    if(ret && ret.status ==0){
                        var item = ret.data;
                        var htmlwrap = [];
                        htmlwrap.push('<div class="material-item">');  
                        if(item.message_type == 2){ // 单图文消息
                            buildSMessage(htmlwrap,item);
                        }
                        else{ //多图文消息item.message_type == 1
                            buildMMessage(htmlwrap,item);
                        }
                        htmlwrap.push('</div>');
                        $('#'+id+'_mixed_wrap').html(htmlwrap.join(''));   
                        $('#'+id+'_mixed_editpanel').addClass('hascontent');   
                    }
                    else{
                        showError('错误提示','设置的消息不存在或已被删除');
                        this.msgidx = -1;
                    }                   
                },
            'json');
          
        }
    }

    seditor.prototype.initEvent = function(){
        var id = this.id;
        var self = this;

        //tab 
        $('#'+id+'_texttab').click(function() {        
            $('#'+id+'_mixedtab').removeClass('active');
            $(this).addClass('active');

            $('#'+id+'_mixed_editpanel').hide();
            $('#'+id+'_stext_editpanel').show();
            self.options.tabIndex =0;

        });
        $('#'+this.id+'_mixedtab').click(function() {
            $('#'+id+'_texttab').removeClass('active');
            $(this).addClass('active');              
            $('#'+id+'_stext_editpanel').hide();
            $('#'+id+'_mixed_editpanel').show();
            self.options.tabIndex = 1;

            //弹出选择框
            choose.Open(self.options.mixedChooseUrl,{
                width: 800,
                height: 600,
                caption: '选择图文素材',              
                onclose: function (userstate) {
                  self.onChoosed.call(self,userstate);
                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        });
        // tooltip
        $('a.seditor-tab',this.element).tooltip();

        //emotion
        $('#'+id+'_emotion').click(function(){
            var ewp = $('#'+id+'_emotion_wrp');
            if(ewp.css('display') !='none'){
                ewp.hide();
            }
            else{
                ewp.show();
            }
            return false;
        })
        $('#'+id+'_emotion_wrp ul i.js_emotion_i').click(function(){         
            var emi = $(this);
            var img_url = emi.attr('data-gifurl');
            var tip = emi.attr('data-title');
            var code =emi.attr('data-code');
            $('#'+id+'_stext_content').append('<img src="'+img_url+'" alt="'+tip+'" code="'+code+'" class="emotiongif"/>');
            $('#'+id+'_emotion_wrp').hide();
            return false;
        });
        $(document).click(function(){
            $('#'+id+'_emotion_wrp').hide();
        })

        $('#'+id+'_mixed_editpanel a.aremove').click(function(){          
            $('#'+id+'_mixed_wrap').html('');
            self.msgidx = -1;
            $('#'+id+'_mixed_editpanel').removeClass('hascontent');   
        });
    } 
    seditor.prototype.render = function() {
        // body...
       
        var html = [];
        //header
        html.push('<ul class="seditor-header">');
        html.push('<li><a href="javascript:;" id="',this.id,'_texttab" class="seditor-tab active"  title="文本"><i class="ic ic-text"></i></a></li>');
        html.push('<li><a href="javascript:;" id="',this.id,'_mixedtab" class="seditor-tab" title="图文"><i class="ic ic-mixed"></i></a></li>');
        html.push('</ul>');

        //body
        html.push('<div class="seditor-body">');
        html.push('<div id="',this.id,'_stext_editpanel" class="stext-editpanel">');
        html.push('<div id="',this.id,'_stext_content" class="stext-content" contenteditable="true"></div>');
        html.push('<div class="stext-bottom">');
        html.push('<a href="javascript:;" id="',this.id,'_emotion" class="emotion" href="表情"><i class="fa fa-smile-o"></i></a>');
        html.push('<span id="',this.id,'_stext_tip" class="stext-tip">最多600字</span>');
        html.push('</div>');
        html.push('<div id="',this.id,'_emotion_wrp" class="emotion_wrp">');
        html.push('<span class="hook">');
        html.push('<span class="hook_dec hook_top"></span>');
        html.push('<span class="hook_dec hook_btm"></span>');
        html.push('</span>');
        html.push('<ul class="emotions" onselectstart="return false;">');
        buildEmotionli(html);
        html.push('</ul>');
        html.push('</div>');
        html.push('</div>');
        html.push('<div id="',this.id,'_mixed_editpanel" class="mixed-editpanel"><div id="',this.id,'_mixed_wrap"></div><a href="javascript:;" class="aremove">删除</a></div>');
        html.push('</div>');
        this.element.html(html.join(''));
    };
    function buildEmotionli(html){
        var len = emotions.length;
        for(var i =0;i<len ;i++){
            html.push('<li class="emotions_item">');
            html.push('<i class="js_emotion_i" data-gifurl="https://res.wx.qq.com/mpres/htmledition/images/icon/emotion/',emotions[i][0],'" data-title="',emotions[i][1],'" data-code="',emotions[i][2],'" style="background-position:',0-i*24,'px 0;">');
            html.push('</i>');
        }
    }
    function buildSMessage(arr,item){       
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

           
            
        arr.push('</div>');
        arr.push('</div>');      
    }

    function buildMMessage(arr,item){
      
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
                 arr.push('</div>'); 
            arr.push('</div>'); 
       
    }
    return seditor;
});
