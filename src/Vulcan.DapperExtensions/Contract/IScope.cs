using System;

namespace Vulcan.DapperExtensions.Contract
{
    public interface IScope : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
