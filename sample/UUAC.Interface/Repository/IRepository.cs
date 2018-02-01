using Vulcan.DataAccess;

namespace UUAC.Interface.Repository
{
    public interface IRepository
    {
        TransScope BeginTransScope(TransScopeOption option = TransScopeOption.Required);
        ConnectionScope BeginConnectionScope();
    }
}
