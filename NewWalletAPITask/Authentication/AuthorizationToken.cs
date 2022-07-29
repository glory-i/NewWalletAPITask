using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Authentication
{
    public class AuthorizationToken
    {
        public string Status { get; set; }
        public string Token { get; set; }
        public string TokenUser { get; set; }
        public DateTime Expiration { get; set; }
    }
}
