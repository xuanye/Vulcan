using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmartCRM.Entity.Jsons
{
    [DataContract]
    public class JsonSMsg
    {
        [DataMember(Name = "status")]
        public int Status
        {
            get;
            set;
        }
        [DataMember(Name = "message")]
        public string Message
        {
            get;
            set;
        }
        [DataMember(Name = "data")]
        public object Data
        {
            get;
            set;
        }

    }
}
