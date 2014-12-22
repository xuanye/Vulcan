using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vulcan.AspNetMvc.Sample.Core
{
    public class HelloService:IHelloService
    {

        public string Hello(string name)
        {
            return string.Format("Hello,{0} !", name);
        }
    }
}