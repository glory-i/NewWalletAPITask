using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewWalletAPITask.Authentication;
using NewWalletAPITask.Enumerations;
using NewWalletAPITask.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewWalletAPITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //Use dependency injection to inject the service layer of authentication

        private readonly IAuthenticationServices _authenticationServices;
        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        //endpoint for sign up/creating an account

        [HttpPost("SignUp")]
        public async Task<ActionResult<AuthenticationResponse>> Register(RegisterModel registerModel)
        {

            var response = await _authenticationServices.RegisterUser(registerModel);
            if (response.Status == AuthenticationResponseEnum.Error.GetEnumDescription())
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        //endpoint for signing in

        [HttpPost("SignIn")]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            var token = await _authenticationServices.Login(loginModel);
            if (token == null)
            {
                return BadRequest(new
                {
                    Status = "Error",
                    Message = "Invalid Login Details"
                });
            }
            else
            {
                return Ok(token);
            }
        }

    }
}
