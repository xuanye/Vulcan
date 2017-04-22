
using System;
using Microsoft.AspNetCore.Http;
using Vulcan.Core;

namespace Vulcan.AspNetCoreMvc
{
    public class AspNetMvcContext:IRuntimeContextStorage
    {
        private readonly IHttpContextAccessor _context;
        public AspNetMvcContext(IHttpContextAccessor context){
            _context = context;
        }

        public object Get(string key){
            object item = null;
            if(ContainsKey(key)){
                return _context.HttpContext.Items[key];
            }
            return item;
        }

        public void Set(string key, object item){
            _context.HttpContext.Items[key] = item;
        }

        public void Remove(string key){
            if(ContainsKey(key)){
                _context.HttpContext.Items.Remove(key);
            }
        }


        public bool ContainsKey(string key){
            return _context.HttpContext.Items.ContainsKey(key);
        }
    }
}