;(function(window,undefined,$){
	function xjGrid(id,options){
		options = $.extend({
		    url: false,
            width:"100%",
			striped: true, //是否显示斑纹效果，默认是奇偶交互的形式
			mhoverclass:'mhover',
			method: 'POST', // data sending method,数据发送方式		
			usepager: false, //是否分页		
			page: 0, //current page,默认当前页 索引从0 开始
			total: 1, //total pages,总页面数		
			rp: 25, // results per page,每页默认的结果数			
			autoload: true, //自动加载     
			submitcoldef:true, //是否在请求时提交字段定义信息    
			showcheckbox: false, //是否显示checkbox,
			localpage: false,//本地分页
            cbhandler:false, // checkbox 点击事件
		    rowhanlder:false,//行处理事件 ，用于绑定一些特殊的事件
			extparams: [],
			gridClass: "xjgrid"//Style
		}, options);
		this.options = options;
		options.elid= id;
		options.el = $("#"+id);
		__Init__(options);
		if(options.autoload)
		{
			__LoadData__(options);
		}
		__InitEvent__(options);
	}
	function __Init__(options){
	    options.el.addClass(options.gridClass);
	    var style_width ="width:"+ options.width+";";
	    if (options.width != "") {
	        options.el.width("width", options.width);
	    }
		if( options.colModel == null || options.colModel.length==0 ){
			alert("请设置列定义[colModel]");
			return ;
		}
		//创建grid的主体
		var html = [];	
		html.push("<div class='grid-body'>");
		html.push("<div class='grid-body-inner'>");
		html.push("<table id='", options.elid, "_detail", "' style='",style_width, "'><thead>");

		html.push("<tr class='thead'>");
		if (options.showcheckbox) {
		    html.push("<td  class='checktd'><div style='width:25px;text-align:center'><input type='checkbox' id='", options.elid, "_checkall'/></div></td>");
		}
		var k = options.colModel.length;
		for (var t = 0, l = k; t < l; t++) {
		    var col = options.colModel[t];
		    if (col.hide) {
		        continue;
		    }
		    var style = [];
		    if (col.align) {
		        style.push("text-align:", col.align, ";");
		    }
		    if (style.length > 0) {
		        style.splice(0, 0, " style=\"");
		        style.push("\"");
		    }
		    html.push("<td style='width:" , col.width?col.width+"px":"auto" , "'", col.sortable ? " class='sortable'" : "", "><div", style.join(""), ">", col.display, "</div></td>");
		}
		html.push("</tr>");

		html.push("</thead><tbody>");
		html.push("</tbody></table>");
		html.push("</div><div class='xjgrid-load'><i class='fa fa-spinner fa-spin'></i>  &nbsp; 正在加载...</div>");
		html.push("</div>");
		//
		if(options.usepager){
			//分页的代码
			html.push("<div id='",options.elid,"_pagep' class='grid-pagination'>&nbsp;</div>")
		}
		options.el.html(html.join(""));
		__InitEvent__(options);
	}
	function __InitEvent__(options){
		if(options.showcheckbox){
			$("#"+options.elid+"_checkall").click(function(){
				var check = this.checked;
				options.checkall = check;
				$("#" + options.elid + "_detail td.checktd_cl input[type='checkbox']").each(function (i) {
					this.checked = check;
				});
				for (var i = 0, l = options.data.length; i < l ; i++) {
				    options.data[i].check_state = check ? 1 : 0;
				}
			})
		}
		var obj = $("#" + options.elid + "_detail thead tr td:not(.checktd)");
		$(obj).each(function (i) {
		    if (!$(this).hasClass("sortable")) {
		        return;
		    }
		    $(this).click(function (objec) {
		        if ($(objec).hasClass("sortasc")) {
		            options.sortorder = "DESC";
		        }
		        else {
		            options.sortorder = "ASC";
		        }	
		        var obj = options.colModel[i];
		        options.sortname = obj.name;
		        __LoadData__(options);
		        return false;

		        //objec = options;
		        ////排序
		        //count++;
		        //var obj = options.colModel[i];
		        //options.sortname = obj.name;

		        //if (objec.sortorder == "ASC") {
		        //    options.sortorder = "DESC";
		        //}
		        //else {
		        //    options.sortorder == "ASC"
		        //}
		        //objec = options;
		        //__LoadData__(options);
		        //return false;
		    })
		});
	}

	function __LoadData__(options) {
		if (options.loading) 
		{	
			return true;
		}
        if (options.onSubmit) {
            var gh = p.onSubmit();
            if (!gh) 
            {	
            	return false;
            }
        }
        options.loading = true;
        if (!options.url) 
        {
        	return false;
        }
        
        //显示正在加载的信息 
        $("#" + options.elid + " .xjgrid-load").show();
        //计算分页
        if (!options.newp) 
        {
        	options.newp = 1;
        }
        if (options.page > options.pages) 
        {
        	options.page = options.pages;
        }

        if (options.localpage && options.loaded) //客户端分页， 并且数据已经在本地
        {          
            options.page = options.newp;
            __BuildRowHtml__(__GetPagedData__(options), options);
        }
        else
        { //服务端分页
            var param = [
                 { name: 'page', value: options.newp }
               , { name: 'rp', value: options.rp }
               , { name: 'sortname', value: options.sortname }
               , { name: 'sortorder', value: options.sortorder }
            ];
            if (options.submitcoldef)//如果需要向服务提交列信息
            {
                if (!options.colkey) {
                    var cols = [];
                    for (var cindex = 0, clength = options.colModel.length; cindex < clength; cindex++) {
                        if (options.colModel[cindex].iskey) {
                            options.colkey = options.colModel[cindex].name;
                        }
                        cols.push(options.colModel[cindex].name);
                    }
                    options.cols = cols.join(",");
                }
                if (!options.colkey) {
                    alert("请设置主键 { display: '说明', name: '字段名',iskey:true },");
                    return;
                }
                param.push({ name: "colkey", value: options.colkey });
                param.push({ name: "colsinfo", value: options.cols });
                param.push({ name: "checkall", value: options.checkall });
            }
            //param = jQuery.extend(param, p.extParam);             
            if (options.extparams) {
                for (var pi = 0; pi < options.extparams.length; pi++) param[param.length] = options.extparams[pi];
            }
            var purl = options.url + (options.url.indexOf('?') > -1 ? '&' : '?') + '_=' + (new Date()).valueOf();
            $.ajax({
                type: options.method,
                url: purl,
                data: param,
                dataType: "json",
                success: function (data) {
                    options.loaded = true;
                    if (data != null && data.error != null) {
                        if (options.onerror) {
                            options.onerror(data);
                        }
                    }
                    else {
                        __ProcessData__(data, options);
                    }
                },
                error: function (data) {
                    try {
                        if (options.onerror) {
                            options.onerror(data);
                        }
                        else {
                        	alert(JSON.stringify(data));
                            alert("获取数据发生异常;")
                        }
                    }
                    catch (e) {
                    }
                }
            });
        }   
	}
	function __BuildRowHtml__(data, options)
	{
	    options.loading = false;
	  
	    if (options.total == 0) { //没有数据          
	        $("#" + options.elid + "_detail tbody").html("");
	        options.pages = 1;
	        options.page = 1;
	        __BuildPageNavition__(options);
	        $("#" + options.elid + " .xjgrid-load").hide();
	        return false;
	    }	   
	    var html = [];
	    var k = options.colModel.length;
	    for (var i = 0, l = data.length; i < l && i < options.rp; i++) {
	        var rowid = data[i].rowid;
	        if (options.striped) {
	            html.push("<tr id='" , options.elid , "_tr_" , rowid , "' class='", (i % 2) == 0 ? "" : "strip", "'>");
	        }
	        else {
	            html.push("<tr>");
	        }	      
	        if (options.showcheckbox) {
	            html.push("<td class='checktd checktd_cl'><div style='width:25px;text-align:center'><input type='checkbox' id='", options.elid, "_cb_" + rowid + "' value='", data[i].id, "' ", data[i].check_state == 1 ? "checked='checked'" : "", "/></div></td>");
	        }
	        for (var j = 0 ; j < k ; j++) {
	            var col = options.colModel[j];

	            if (col.hide) {
	                continue;
	            }

	            var style = [];
	            //if(col.width){
	            //	style.push("width:",col.width,"px;");
	            //}
	            if (col.align) {
	                style.push("text-align:", col.align, ";");
	            }
	            if (style.length > 0) {
	                style.splice(0, 0, " style=\"");
	                style.push("\"");
	            }
	            var text = col.process ? col.process(data[i].cell[j], data[i].cell, rowid) : data[i].cell[j];
	            html.push("<td style='width:", col.width ? col.width : "auto", "'><div", style.join(""), ">", text, "</div></td>");
	        }
	        html.push("</tr>")
	    };	   
	    $("#" + options.elid + "_detail tbody").html(html.join(""));
	    if (options.mhoverclass || options.rowhanlder) { //行事件
	        $("#" + options.elid + "_detail tbody tr").each(function (i) {
	            if (options.mhoverclass) {
	                $(this).hover(function (e) { $(this).addClass(options.mhoverclass); }, function (e) { $(this).removeClass(options.mhoverclass); });
	            }
	            if (options.rowhanlder) {
	                var id = this.id;
	                var tar = this.id.split("_");
	                var rowid = parseInt(tar[tar.length - 1]);
	                options.rowhanlder.call(this, rowid,options.data[rowid]);
	            }
	        });
	    }
	    if (options.showcheckbox) {
	        $("#" + options.elid + "_detail td.checktd_cl input[type='checkbox']").each(function (i) {                
	            $(this).click(function (e) {
	                var cbid = this.id;
	                var tar = cbid.split("_");
	                var rowid = tar[tar.length - 1];	                
	                options.data[rowid].check_state = this.checked ? 1 : 0;
	                var staticIndex = options.dataindex[rowid];
	                options.staticdata[staticIndex].check_state = this.checked ? 1 : 0;
	                if (options.cbhanlder) {
	                    options.cbhanlder.call(this,e);
	                }
	            });
	        });
	    }
	    
	    __BuildPageNavition__(options);
	    $("#" + options.elid + " .xjgrid-load").hide();
	 
	    return true;
	}
	function __ProcessData__(data, options)
	{
	    options.data = {};	   

		if (options.preprocess)
		{ 
			data = options.preprocess(data);
		}

		var temp = options.total;
	
		options.total = data.total;
		if (options.total < 0) {
		    options.total = temp;
		}

		options.page = data.page;
		options.pages = Math.ceil(options.total / options.rp);
		
		options.data = [];
		options.staticdata = [];
		options.dataindex = [];
		for (var i = 0, l = data.rows.length ; i < l ; i++) {
		    data.rows[i].rowid = i;
		    options.dataindex[i] = i;
		    options.data.push(data.rows[i]);
		    options.staticdata.push(data.rows[i]);
        }		  
	    // 取出对应的页面数据      
		__BuildRowHtml__(__GetPagedData__(options), options);

		if (options.onsuccess) {
		    options.onsuccess();
		}
        return true;
	}
	function __GetPagedData__(options) {	   
	    var length = options.data.length;
	    var page = options.page;
	    var rp = options.rp;
	    var sp = 0;
	    if (options.localpage) {
	        sp = (page - 1) * rp;
	    }  
	    var data = [];
	    for (var j = 0; j < rp ; j++) {	      
	        if (options.data[sp + j]) {
	            data.push(options.data[sp + j]);
	        }
	        else {
	            break;
	        }
	    }
	    return data;
	}
	function __BuildPageNavition__(options){
		if(!options.usepager)
		{
			return ;
		}
		if(options.pages <=10){

		}
		var html = [];
		html.push("<ul>");
		html.push("<li><span>",options.total," 条记录 ",options.page," / ",options.pages," 页 </span></li>");
		if (options.page > 1) {
		    html.push("<li action='first'><a href='javascript:void(0)'><i class='fa fa-angle-double-left'></i></a></li>");
		    html.push("<li action='prev'><a href='javascript:void(0)'><i class='fa fa-angle-left'></i></a></li>");
		}
		else {
		    html.push("<li class='disabled'><span style='color:#ddd;' ><i class='fa fa-angle-double-left'></i></span></li>");

		    html.push("<li class='disabled'><span style='color:#ddd;'><i class='fa fa-angle-left'></i></<span></li>");
		}
		var index = options.page - 2;
		if(options.page + 2 >  options.pages){
			index = index - 2;
		}
		if(index<1){
			index = 1;
		}
		for(var i= 0;i< 5 && (i+index) <=options.pages ;i++){
			var className = "";
			if(i+index == options.page){
				className = "active";
			}
			html.push("<li action='navi'",className!=""?" class='"+className+"'":"","><a href='javascript:void(0)'>",i+index,"</a></li>");
		}
		if (options.page < options.pages) {
		    html.push("<li action='next'><a href='javascript:void(0)'><i class='fa fa-angle-right'></i></a></li>");
		    html.push("<li action='last'><a href='javascript:void(0)'><i class='fa fa-angle-double-right'></i></a></li>");
		}
		else {
		    html.push("<li class='disabled'><span style='color:#ddd;'><i class='fa fa-angle-right'></i></span></li>");
		    html.push("<li class='disabled'><span style='color:#ddd;'><i class='fa fa-angle-double-right'></i></span></li>");
		}
		html.push("</ul>");          
		$("#"+options.elid+"_pagep")[0].innerHTML = html.join("");

		//事件处理
		$("#" + options.elid + "_pagep li").each(function (i) {
		    $(this).click(function () {
		        var action = $(this).attr("action");
		        __PageNavi__(action, $(this).text(), options);
		    })
		});
	}
	function __PageNavi__(action, page, options) {
	    switch(action)
	    {
			case "first"://第一页
				options.newp=1;
				break;
			case "prev"://上一页
				options.newp= options.page -1 ;
				break;
			case "navi":
				options.newp= parseInt(page);
				if(options.newp<=0 || options.newp==options.page){
					return;
				}
				break;
			case "next":
				options.newp= options.page + 1 ;
				break;
			case "last":
				options.newp= options.pages;
				break;
			default:
				return;				
		}
		__LoadData__(options);
	}
	function __Query__(form, options) {     
	    var datas = $(form).serialize();//查询条件
	    var ardata = datas.split("&");
	    var p = {page: 0,extparams: [],checkall:false,loaded:false};

	    for (var i = 0, l = ardata.length; i < l; i++) {
	        var arkv = ardata[i].split("=");
	        p.extparams.push({ name: arkv[0], value: decodeURIComponent(arkv[1]) });
	    }
	    $.extend(options, p);	 
	    __LoadData__(options)
	    return false;
	}

    //根据字段排序
	function __QueryByFields__(search, options) {
	    var p = { page: 0, extparams: search, checkall: false,loaded:false };
	    $.extend(options, p);
	    __LoadData__(options)
	    return false;
	}
	xjGrid.prototype = {
	    Reload: function () {
	        // body...
	        this.options.page = 0;
	        this.options.loaded = false;
	        if (this.options.showcheckbox) {
	            this.options.checkall = false;
	            $("#" + this.options.elid + "_checkall")[0].checked = false; //刷新时取消全选
	        }
	        __LoadData__(this.options)
	    },
	    SetOptions: function (p) {
	        // body...
	        $.extend(this.options, p);
	    },
	    GetCheckedRowIds: function () {
	        var ids = [];
	        for (var i = 0, l = this.options.staticdata.length; i < l ; i++) {
	            if (this.options.staticdata[i].check_state == 1) {
	                ids.push(this.options.staticdata[i].id);
	            }
	        }
	        return ids;
	    },
	    GetCheckedRowDatas: function (format) {
	        var data = [];
	        for (var i = 0, l = this.options.staticdata.length; i < l ; i++) {
	            if (this.options.staticdata[i].check_state == 1) {
	                if (format) {
	                    data.push(format(this.options.staticdata[i].cell));
	                }
	                else {
	                    data.push(this.options.staticdata[i].cell);
	                }
	                
	            }
	        }
	        return data;
	    },
	    Query: function (form) {
	        __Query__(form, this.options);
	    },
	    QueryByFields: function (search) {
	        __QueryByFields__(search, this.options);
	    },
	    //QueryByFielsaSort: function () {
        //   //根据字段排序
	    //    __QueryByFielsaSort__(search, this.options);
	    //},
	    UpdateBind: function (rowid, cellId, value) { //从外部更新内部数据字段	       
	        this.options.data[rowid].cell[cellId] = value; //同步到临时数据
	        var staticIndex = this.options.dataindex[rowid];
	        this.options.staticdata[staticIndex].cell[cellId] = value;//同步到完整数据
	    },
	    Fitler: function (cell, mode) { //客户端过滤功能。。很牛叉 ，但是场景 真的，尽量避免吧！	       
	        this.options.data = [];
	        this.options.dataindex = [];
	        var index = 0;
	        for (var i = 0, l = this.options.staticdata.length; i < l; i++) {
	            var pass = true;
	            for (var j = 0, k = mode.length; j < k; j++) {
	                if (cell[j] == null) {
	                    pass = true;
	                    continue;
	                }
	                switch (mode[j]) {
	                    case 0:	                    
	                        pass = this.options.staticdata[i].cell[j] == cell[j];
	                        break;
	                    case 1:	                        
	                        if (typeof (this.options.staticdata[i].cell[j]) == "string") {	                          
	                            pass = this.options.staticdata[i].cell[j].indexOf(cell[j]) >= 0;
	                        }
	                        else {
	                            pass = false;
	                        }
	                        break;
	                    default:
	                        pass = true;
	                        break;
	                }
	                if(!pass)
	                {
	                    break;
	                }
	            }
	            if (pass) {
	                this.options.dataindex[index] = i;
	                this.options.staticdata[i].rowid = index; //重新注册rowid;
	                this.options.data.push(this.options.staticdata[i]);
	                index++;
	            }
	        }

	        var temp = this.options.total;
	        this.options.total = this.options.data.length;
	        if (this.options.total < 0) {
	            this.options.total = temp;
	        }
	        this.options.page = 1;
	        this.options.pages = Math.ceil(this.options.total / this.options.rp);

	        __BuildRowHtml__(__GetPagedData__(this.options), this.options);
	    }
	}
	window.xjGrid = xjGrid ;
})(window,undefined,jQuery);