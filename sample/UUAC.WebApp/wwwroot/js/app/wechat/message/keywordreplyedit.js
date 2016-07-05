define(function (require, exports, module) {
    exports.init = function (opt) {
    	$('#btnCancel').click(function(){
            parent.window._CloseDailog();
        });
        $('#btnClose').click(function(){
            parent.window._CloseDailog();
        });
    };//init
});