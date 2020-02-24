namespace AlfaBank.AFT.Core.Infrastructure.DataBase
{
    /// <summary>
    /// Виды запросов.
    /// </summary>
    public enum DbCommandType : ushort
    {
        /// <summary>
        /// Запросы "Select".
        /// </summary>
        Select,

        /// <summary>
        /// Запрос "Insert"
        /// </summary>
        Insert,

        /// <summary>
        /// Запрос "Update"
        /// </summary>
        Update,

        /// <summary>
        /// Запрос "Delete"
        /// </summary>
        Delete,
    }
}
