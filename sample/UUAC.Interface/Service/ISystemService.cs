using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;

namespace UUAC.Interface.Service
{
    public interface ISystemService
    {
        Task<IUserInfo> GetUserInfo(string userId);
    }
}
