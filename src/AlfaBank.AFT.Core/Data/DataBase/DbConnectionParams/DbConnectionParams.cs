using AlfaBank.AFT.Core.Helpers;

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
            get => new Encryptor().Decrypt(_password);
            set => _password = value;
        }

        public string GetDataSource()
        {
            return Source;
        }
    }
}
