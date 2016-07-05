(function( window, $,factory ) { 

  if (typeof define === 'function') {
    // 如果define已被定义，模块化代码
    define('plugin/choose',function (require, exports, module) {       
        module.exports =  factory( window, $ );
    });
  } else {
        // 如果define没有被定义，正常执行
        window.Choose = factory(window , $ );
  }

})( window,jQuery,function factory(window,$){     

    function Choose(){
        this.opening = false ;
        this.themes = {       
            simple:{
                box:"<div id='dailog_${id}' class='xj-window xj-window-dailog'></div>",
                header:"<div id='dailog_head_${id}' class='xj-dailog-tl'></div>",
                body:"<div class='xj-window-bwrap'><div id='dailog_body_${id}' style='width: ${width}px; height: ${height}px' class='xj-window-body'>${iframehtml}</div></div>",
                iframe:"<iframe id='dailog_iframe_${id}' border='0' frameBorder='0' src='${url}' style='border:none;width:${width}px;height:${height}px'></iframe>",
                wp:0
            }
        }; 

    }

    Choose.prototype ={
        Open:function(url,options){ //打开一个新的窗口
            var self = this;
            if(this.opening){
                return false;
            }       
            this.opening = true;
            this.options = options = $.extend({
                            width: 600,
                            height: 400,
                            caption: '无主题窗口',
                            enabledrag: true,
                            theme:"simple",
                            onclose: null
            },options);         
            options.id = (new Date()).valueOf();
            var newid = options.id;
            options.caption = __escapeHTML__(options.caption);
            options.url = url + (url.indexOf('?') > -1 ? '&' : '?') + '_=' + (new Date()).valueOf(); 
            var theme = this.themes[options.theme];         
            var html = [];          
            options.iframehtml = __Tp__(theme.iframe, options);
            html.push(__Tp__(theme.header, options));
            html.push(__Tp__(theme.body, options));

            var box = $(__Tp__(theme.box, options));
            box.css({ width: options.width + theme.wp }).html(html.join(""));
       
            var margins = __getMargins__(document.body, true);
            var overlayer = $('<div></div>').css({
                position: 'absolute',
                left: 0,
                top: 0,
                width: Math.max(document.documentElement.clientWidth, document.body.scrollWidth),
                height: Math.max(document.documentElement.clientHeight, document.body.scrollHeight + margins.t + margins.b),
                zIndex: '998',
                background: '#555',
                opacity: '0.5'
            }).bind('contextmenu', function() { return false; }).appendTo(document.body);
            this.overlayer = overlayer;
            var isdrag = false;       
           
            box.appendTo(document.body);
            __SetDocumentCenter__(box);
            this.box = box;
            if (!$.support.boxModel) {
                $(document.body).addClass("hiddenselect");
                document.getElementById("dailog_iframe_" + newid).src = options.url;
            }
          
        },
        Close : function(callback,d,userstate){     
            //alert(1);
            if (!$.support.boxModel) {
                $(document.body).removeClass("hiddenselect");
            }
            //console.log(this);
            this.overlayer.remove();
            //this.closebtn.remove();
            this.box.remove();
            this.opening = false;
            this.closebtn = this.overlayer = this.box = null;
            callback && callback();
            if (d && this.options.onclose) {
                this.options.onclose(userstate);
                this.options.onclose = null;
            }
        }
    };

    function __escapeHTML__(htmlText){
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(htmlText));
        return div.innerHTML;
    }
    
    function __Tp__(temp, dataarry) {
        return temp.replace(/\$\{([\w]+)\}/g, function(s1, s2) { var s = dataarry[s2]; if (typeof (s) != "undefined") { return s; } else { return s1; } });
    }

    function __getMargins__(element, toInteger){
        var el = $(element);
        var t = el.css('marginTop') || '';
        var r = el.css('marginRight') || '';
        var b = el.css('marginBottom') || '';
        var l = el.css('marginLeft') || '';
        if (toInteger)
            return {
                t: parseInt(t) || 0,
                r: parseInt(r) || 0,
                b: parseInt(b) || 0,
                l: parseInt(l)
            };
        else
            return { t: t, r: r, b: b, l: l };
    }

    function __SetDocumentCenter__(el){
        el = $(el);
        el.css({
            position: 'absolute',
            left: Math.max((document.documentElement.clientWidth - el.width()) / 2 + $(document).scrollLeft(), 0) + 'px',
            top: Math.max((document.documentElement.clientHeight - el.height()) / 2 + $(document).scrollTop(), 0) + 'px'
        });
    }
    var choose =  new Choose();
    window._CloseDailog = function(){
        choose.Close.apply(choose,arguments);
    }
    return choose;

});