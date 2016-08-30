using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UUAC.Common
{
    public static class RuleHelper
    {
        public static string GetAppEveryoneRuleCode(string appCode)
        {
            return string.Format("{0}_{1}", appCode, Constans.EVERYONE_ROLE_POSTFIX);
        }
        
    }
}
