using System;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class ConnectionManagerFactory:IConnectionManagerFactory
    {
        private static readonly object LockObject = new object();

        private readonly IRuntimeContextStorage _ctxStorage;
        private readonly IConnectionFactory _defaultFactory;
        public ConnectionManagerFactory(IRuntimeContextStorage ctxStorage, IConnectionFactory factory)
        {
            _ctxStorage = ctxStorage ?? throw new ArgumentNullException(nameof(ctxStorage));
            _defaultFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public ConnectionManager GetConnectionManager(string constr, IConnectionFactory factory = null)
        {
            ConnectionManager mgr = null;
            // if not exists
            if (!_ctxStorage.ContainsKey(constr))
            {
                lock (LockObject)
                {
                    if (!_ctxStorage.ContainsKey(constr))
                    {
                        // create a new db connection and cache it;
                        mgr = new ConnectionManager(factory ?? _defaultFactory, constr, _ctxStorage);
                        _ctxStorage.Set(constr, mgr);
                    }
                }
                if (mgr != null)
                {
                    mgr.AddRef();
                    return mgr;
                }
            }
            //else
            mgr = (ConnectionManager)_ctxStorage.Get(constr);
            mgr.AddRef();
            return mgr;

        }
    }
}
