using System;
using System.Collections.Generic;
using System.Text;

namespace Vulcan.DataAccess
{
    public interface IConnectionManagerFactory
    {
        ConnectionManager GetConnectionManager(IConnectionFactory factory, string constr);

        ConnectionManager GetConnectionManager(string constr);

    }

    public class ConnectionManagerFactory : IConnectionManagerFactory
    {

        private static object _lock = new object();

        private readonly IRuntimeContextStorage _ctxStorage;
        private readonly IConnectionFactory _defautFactory;
        public ConnectionManagerFactory(IRuntimeContextStorage ctxStorage, IConnectionFactory factory)
        {
            _ctxStorage = ctxStorage ?? throw new ArgumentNullException(nameof(ctxStorage));
            _defautFactory = factory ?? throw new ArgumentNullException(nameof(factory));

        }


        public ConnectionManager GetConnectionManager(IConnectionFactory factory, string constr)
        {
            ConnectionManager mgr = null;
            if (!_ctxStorage.ContainsKey(constr))
            {
                lock (_lock)
                {
                    if (!_ctxStorage.ContainsKey(constr))
                    {
                        // 创建一个新的连接 并缓存起来
                        mgr = new ConnectionManager(factory?? _defautFactory, constr, _ctxStorage);
                        _ctxStorage.Set(constr, mgr);
                    }                    
                }
                if(mgr != null)
                {
                    mgr.AddRef();
                    return mgr;
                }
            }

            mgr = (ConnectionManager)_ctxStorage.Get(constr);
            mgr.AddRef();
            return mgr;

        }

        public ConnectionManager GetConnectionManager(string constr)
        {
           return GetConnectionManager(null, constr);
        }
    }
}
