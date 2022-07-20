using System;
using System.Collections.Generic;
using System.Text;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class DefaultUnitOfWork : IUnitOfWork
    {
        private readonly IScope _innerScope;

        public DefaultUnitOfWork(IScope innerScope)
        {
            _innerScope = innerScope;
        }

        public void Commit()
        {
            _innerScope.Commit();
        }

        public void Dispose()
        {
            _innerScope.Dispose();
        }

        public void Rollback()
        {
            _innerScope.Rollback();
        }
    }
}
