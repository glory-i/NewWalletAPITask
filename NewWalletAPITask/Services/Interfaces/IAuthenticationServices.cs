using NewWalletAPITask.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Services.Interfaces
{
    public interface IAuthenticationServices
    {
        public Task<AuthenticationResponse> RegisterUser(RegisterModel model);
        public Task<AuthorizationToken> Login(LoginModel model);
        public Task<string> CreateRoles();
    }
}
