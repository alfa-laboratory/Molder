using AlfaBank.AFT.Core.Helpers;
using FluentAssertions;
using System;

namespace AlfaBank.AFT.Core.Data.DataBase.DbConnectionParams
{
    public class DbConnectionParams : IDbConnectionParams
    {
        private string _password = string.Empty;

        public string Source { get; set; } = null;
        public string Database { get; set; } = null;
        public string Login { get; set; } = null;
        public string Password
        {
            get
            {
                var validPassword = string.Empty;
                var act = new Action(() => validPassword = new Encryptor().Decrypt(_password));
                act.Should().NotThrow<FormatException>("Неверный пароль.");
                return validPassword;
            }
            set => _password = value;
        }

        public string GetDataSource()
        {
            return Source;
        }
    }
}
