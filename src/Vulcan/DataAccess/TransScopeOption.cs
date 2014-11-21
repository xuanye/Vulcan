namespace Vulcan.DataAccess
{
    /// <summary>
    /// 该范围需要一个事务。如果已经存在环境事务，则使用该环境事务。否则，在进入范围之前创建新的事务。这是默认值。
    /// </summary>
    public enum TransScopeOption
    {
        /// <summary>
        /// 该范围需要一个事务。如果已经存在环境事务，则使用该环境事务。否则，在进入范围之前创建新的事务。这是默认值。
        /// </summary>
        Required,

        /// <summary>
        /// 始终创建新事务
        /// </summary>
        RequiresNew
    }
}