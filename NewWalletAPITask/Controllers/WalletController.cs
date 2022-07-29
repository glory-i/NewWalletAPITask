using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewWalletAPITask.DTOs.WalletDTOs;
using NewWalletAPITask.Enumerations;
using NewWalletAPITask.Model;
using NewWalletAPITask.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewWalletAPITask.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        //use dependecy injection in the constructor inject interface of wallet service

        private readonly IWalletServices _walletServices;
        public WalletController(IWalletServices walletServices)
        {
            _walletServices = walletServices;
        }


        [HttpPost("create-wallet")]
        public async Task<IActionResult> CreateWallet(CreateWalletDTO walletDTO)
        {
            var result = await _walletServices.CreateWallet(walletDTO, User.Identity.Name);
            if (result.Status == StatusResponseEnum.Failure.GetEnumDescription())
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpGet("get-wallet")]
        public async Task<IActionResult> ViewWallet()
        {
            var result = await _walletServices.ViewWallet(User.Identity.Name);
            if (result.Status == StatusResponseEnum.Failure.GetEnumDescription())
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost("fund-wallet")]
        public async Task<IActionResult> FundWallet(PaymentRequest request)
        {
            var response = await _walletServices.FundWallet(request, User.Identity.Name);
            if (!response.requestSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("external-transfer")]
        public async Task<IActionResult> Transfer(PaymentRequest request)
        {
            var response = await _walletServices.TransferToBankAccount(request, User.Identity.Name);
            if (!response.requestSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpGet("get-wallet-transactions")]
        public async Task<IActionResult> GetWalletTransactions()
        {
            var response = await _walletServices.GetWalletTransactions(User.Identity.Name);
            if (response == null)
            {
                return BadRequest(new Error
                {
                    Message = "You do not have a wallet"
                });

            }

            return Ok(response);
        }
    }
}
