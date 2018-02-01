using System.Threading.Tasks;
using UUAC.Entity;
using UUAC.Interface.Service;

namespace UUAC.Business.ServiceImpl
{
    public class SystemService : ISystemService
    {
        private readonly IUserManageService _uService;
        private readonly IPrivilegeService _pService;
        private readonly IRoleService _rService;

        public SystemService(IUserManageService uService, IPrivilegeService pService, IRoleService rService)
        {
            this._uService = uService;
            this._pService = pService;
            this._rService = rService;
        }

        public Task<IUserInfo> GetUserInfo(string userId)
        {
            return this._uService.GetUserInfo(userId);
        }

        public Task<bool> HasPrivilege(string identity, string privilegeCode)
        {
            return this._pService.HasPrivilege(identity, privilegeCode);
        }

        public Task<bool> IsInRole(string identity, string roleCode)
        {
            return this._rService.IsInRole(identity, roleCode);
        }
    }
}
