using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.Core.Enities
{
    public class JsonMsg
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
