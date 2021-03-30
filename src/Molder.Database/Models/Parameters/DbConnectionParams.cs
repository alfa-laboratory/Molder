using System.ComponentModel.DataAnnotations;

namespace Molder.Database.Models.Parameters
{
    public class DbConnectionParams
    {
        [Required(ErrorMessage = "Source is required")]
        public string Source { get; set; } = null;
        [Required(ErrorMessage = "Database is required")]
        public string Database { get; set; } = null;
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; } = null;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public int? Timeout { get; set; } = 60;

        [Range(0, 255)]
        public int ConnectRetryCount { get; set; } = 1;

        [Range(1, 60)]
        public int ConnectRetryInterval { get; set; } = 10;
    }
}