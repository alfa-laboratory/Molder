namespace AlfaBank.AFT.Core.Infrastructure.DataBase
{
    /// <summary>
    /// Типы запросов SQL.
    /// </summary>
    public enum DbCommandExecuteMode
    {
        /// <summary>
        /// Тип "NonQuery"
        /// </summary>
        NonQuery,

        /// <summary>
        /// Тип "Query"
        /// </summary>
        Query,

        /// <summary>
        /// Тип "Scalar"
        /// </summary>
        Scalar,
    }
}
