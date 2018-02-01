
using System;
using Microsoft.AspNetCore.Http;
using Vulcan.DataAccess;

namespace Vulcan.AspNetCoreMvc
{
    public class AspNetCoreContext:IRuntimeContextStorage
    {
        private readonly IHttpContextAccessor _context;
        public AspNetCoreContext(IHttpContextAccessor context){
            _context = context;
        }

        public object Get(string key){           
           
            return _context.HttpContext.Items[key];           
        }

        public void Set(string key, object item){
            _context.HttpContext.Items[key] = item;
        }

        public void Remove(string key){
           
           _context.HttpContext.Items.Remove(key);            
        }


        public bool ContainsKey(string key){
            return _context.HttpContext.Items.ContainsKey(key);
        }
    }
}
