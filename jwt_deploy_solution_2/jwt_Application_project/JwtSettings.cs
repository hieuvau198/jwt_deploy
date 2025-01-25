using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwt_Application_project
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = "your_secret_key_here"; // Replace with a secure key
        public int AccessTokenDuration { get; set; } = 15; // Minutes
        public int RefreshTokenDuration { get; set; } = 1440; // Minutes
    }

}
