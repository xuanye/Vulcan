using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.DataAccess
{
    public interface IRuntimeContextStorage
    {
        object Get(string key);

        void Set(string key, object item);

        void Remove(string key);


        bool ContainsKey(string key);
    }

}
