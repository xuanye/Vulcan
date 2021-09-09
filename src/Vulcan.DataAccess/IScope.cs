using System;
using System.Collections.Generic;
using System.Text;

namespace Vulcan.DataAccess
{
    public interface IScope:IDisposable
    {
        void Commit();
        void Rollback();
    }
}
