var v =  (new Date()).getTime();
seajs.config({
  // 别名配置
  alias: {
      'xjgrid': 'plugin/xjgrid',
      'validator': 'plugin/validator'
  },
  // 路径配置
  paths: {
  },
  // 变量配置
  vars: {
    'locale': 'zh-cn'
  },
  // 映射配置
  map: [
    [/^(.*\/js\/app(\/[^\/]+)?\/.*\.(?:js))(?:.*)$/i, '$1?_=' + v],
    [/^(.*\/js\/plugin(\/[^\/]+)?\/.*\.(?:js))(?:.*)$/i, '$1?_=2015042001']
  ],
  // 预加载项
  preload: [

  ],
  // 调试模式
  debug: false,
  // Sea.js 的基础路径
  // 文件编码
  charset: 'utf-8'
});
