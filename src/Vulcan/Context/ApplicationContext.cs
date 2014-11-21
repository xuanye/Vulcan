using System.Collections.Specialized;
using Vulcan.Threading;

namespace Vulcan
{
    public class ApplicationContext
    {
        #region 上下文级别的

        private const string _localContextName = "ApplicationContext.LocalContext";

        public static HybridDictionary LocalContext
        {
            get
            {
                HybridDictionary ctx = HybridContextStorage.GetData(_localContextName) as HybridDictionary;
                if (ctx == null)
                {
                    ctx = new HybridDictionary();
                    HybridContextStorage.SetData(_localContextName, ctx);
                }
                return ctx;
            }
        }

        #endregion 上下文级别的
    }
}