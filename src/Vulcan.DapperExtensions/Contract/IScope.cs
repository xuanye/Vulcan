using System;
using System.Collections.Generic;
using System.Text;

namespace Vulcan.DapperExtensions.Contract
{
    public interface IScope:IDisposable
    {
        void Commit();
        void Rollback();
    }
}
