using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using UUAC.Interface.Service;

namespace UUAC.Business.ServiceImpl
{
    public class SystemService : ISystemService
    {
        private readonly IUserManageService _uService;
        public SystemService(IUserManageService uService){
            this._uService = uService;
        }

        public Task<IUserInfo> GetUserInfo(string userId)
        {
            return this._uService.GetUserInfo(userId);
        }
    }
}
