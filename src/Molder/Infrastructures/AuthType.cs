namespace Molder.Infrastructure
{
    /// <summary>
    /// Тиы аутентификации.
    /// </summary>
    public enum AuthType
    {
        /// <summary>
        /// Соединение без передачи учетных данных.
        /// </summary>
        Anonymous,

        /// <summary>
        /// Соединение с базовой аутентификацией.
        /// </summary>
        Basic,

        /// <summary>
        /// Соединение с дайджест-аутентификацией.
        /// </summary>
        Digest,

        /// <summary>
        /// Соединение с распределенной проверкой пароля.
        /// </summary>
        Dpa,

        /// <summary>
        /// Соединение с внешней аутентификацией.
        /// </summary>
        External,

        /// <summary>
        /// Соединение с kerberos аутентификацией.
        /// </summary>
        Kerberos,

        /// <summary>
        /// Соединение с сетевой аутентификацией Microsoft.
        /// </summary>
        Msn,

        /// <summary>
        /// Соединение с проверкой подлинности Microsoft Negotiate.
        /// </summary>
        Negotiate,

        /// <summary>
        /// Соединение с проверкой подлинности Windows NT Challenge (NTLM)
        /// </summary>
        NTLM,

        /// <summary>
        /// Механизм согласования будет использоваться MSN, DPA или NTLM
        /// </summary>
        Sicily
    }
}
