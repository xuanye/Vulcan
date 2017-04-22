using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UUAC.Entity
{
    public interface IUser
    {
        string UserId { get; set; }
        string FullName { get; set; }
        string CompanyId { get; set; }
        string CompanyName { get; set; }
        string UnitId { get; set; }
        string UnitName { get; set; }
        string DeptId { get; set; }
        string DeptName { get; set; }
      
    }
}
