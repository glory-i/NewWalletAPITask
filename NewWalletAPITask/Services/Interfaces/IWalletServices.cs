using NewWalletAPITask.DTOs.TransactionDTOs;
using NewWalletAPITask.DTOs.WalletDTOs;
using NewWalletAPITask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Services.Interfaces
{
    public interface IWalletServices
    {
        public Task<WalletResponse> CreateWallet(CreateWalletDTO walletDTO, string username);
        public Task<WalletResponse> ViewWallet(string username);

        public HttpClient SetClient();
        public Task<PaymentResponse> FundWallet(PaymentRequest request, string username);
        public Task<PaymentResponse> TransferToBankAccount(PaymentRequest request, string username);

        public Task<WalletHistory> GetWalletTransactions(string username);
    }
}
