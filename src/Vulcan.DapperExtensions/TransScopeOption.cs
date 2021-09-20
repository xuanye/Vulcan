namespace Vulcan.DapperExtensions
{
    /// <summary>
    ///     Transaction Option
    /// </summary>
    public enum TransScopeOption
    {
        /// <summary>
        ///     The scope requires a transaction.
        ///     If a transaction is already exists in this scope,use that transaction.
        ///     Otherwise, create a new transaction before entering the scope.
        ///     This is the default value.
        /// </summary>
        Required,

        /// <summary>
        ///     always create a new transaction
        /// </summary>
        RequiresNew
    }
}
