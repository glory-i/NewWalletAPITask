using NewWalletAPITask.DTOs.TransactionDTOs;
using NewWalletAPITask.DTOs.WalletDTOs;
using NewWalletAPITask.Enumerations;
using NewWalletAPITask.Model;
using NewWalletAPITask.Repository.Interfaces;
using NewWalletAPITask.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Services.Implementation
{
    public class WalletServices : IWalletServices
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        public WalletServices(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        //Method to convert a Wallet class to a Data Transfer Object which contains just the necessary properties
        public ViewWalletDTO ConvertToDTO(Wallet wallet)
        {
            ViewWalletDTO walletDTO = new ViewWalletDTO
            {
                WalletName = wallet.WalletName,
                WalletOwner = wallet.Owner.Username,
                WalletBalance = wallet.WalletBalance,
                DateCreated = wallet.DateCreated
            };
            return walletDTO;
        }


        //function to set http client to be used to integrate Monnify API
        public HttpClient SetClient()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://sandbox.monnify.com/api/v1/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add($"Authorization", $"Basic TUtfVEVTVF9DSFZRRlJBN1NHOjZRSE1aMlBMQ1VHVDJUQzlNUFBEWTRXSFhQRERDWlBK");

            //The transfer endpoint is protected by basic authorization with this as the encrypted username and password

            return client;

        }


        public async Task<WalletResponse> CreateWallet(CreateWalletDTO walletDTO, string username)
        {
            var MyWallet = await _walletRepository.CreateWallet(walletDTO.WalletName, username);

            //A User cannot create another wallet if they already have a wallet
            if (MyWallet == null)
            {
                return (new WalletResponse
                {
                    Status = StatusResponseEnum.Failure.GetEnumDescription(),
                    Error = new Error { Message = "You cannot have more than one wallet" },
                    Data = null
                });
            }

            //Wallet successfully created
            return (
                new WalletResponse
                {
                    Status = StatusResponseEnum.Success.GetEnumDescription(),
                    Error = null,
                    Data = ConvertToDTO(MyWallet)
                });


        }

        public async Task<WalletResponse> ViewWallet(string username)
        {
            var MyWallet = await _walletRepository.ViewWallet(username);

            //User cannot view wallet if they do not have one
            if (MyWallet == null)
            {
                return (new WalletResponse
                {
                    Status = StatusResponseEnum.Failure.GetEnumDescription(),
                    Error = new Error { Message = "You do not have a wallet" },
                    Data = null
                });
            }

            //User successfully views their wallet
            return (
                new WalletResponse
                {
                    Status = StatusResponseEnum.Success.GetEnumDescription(),
                    Error = null,
                    Data = ConvertToDTO(MyWallet)
                });


        }

        public async Task<PaymentResponse> FundWallet(PaymentRequest request, string username)
        {
            //get httpclient instance to be used to integrate monnify api
            var client = SetClient();
            bool IsTransferSuccessful = false;

            //get the wallet that funds will be transferred from
            var wallet = await _walletRepository.ViewWallet(username);
            if (wallet == null)
            {
                return (new PaymentResponse
                {
                    requestSuccessful = false,
                    responseBody = null,
                    responseCode = "400",
                    responseMessage = $"You cannot fund wallet because you do not have a wallet"
                });
            }

            try
            {
                //post the request to the endpoint using http client instance, and get the response 

                string path = $"merchant/transactions/init-transaction";
                HttpResponseMessage Res = await client.PostAsJsonAsync(path, request);
                var response = await Res.Content.ReadAsAsync<PaymentResponse>();

                if (Res.IsSuccessStatusCode)
                {
                    IsTransferSuccessful = true;
                }

                if (IsTransferSuccessful) //if the request was successfully posted
                {
                    //increase the balance of the wallet 

                    wallet.WalletBalance += request.amount;
                    await _walletRepository.UpdateWallet(wallet.Id);


                    //create a new transaction to represent the transfer that just happened and store in the database
                    Transaction transaction = new Transaction
                    {
                        WalletId = wallet.Id,
                        Description = request.paymentDescription,
                        Type = TransferTypeEnum.Inflow.GetEnumDescription(),
                        Amount = request.amount,
                        TransactionReference = response.responseBody.transactionReference,
                        PaymentReference = request.paymentReference,
                        DateCreated = DateTime.Now
                    };

                    await _transactionRepository.CreateTransaction(transaction);
                    return response;
                }

                //if the request was unsuccessul, it will display the reason why not
                else
                {
                    return (new PaymentResponse
                    {
                        requestSuccessful = false,
                        responseBody = null,
                        responseCode = "400",
                        responseMessage = $"{response.responseMessage}"
                    });
                }

            }

            //catch exception

            catch (Exception my_ex)
            {
                return (new PaymentResponse
                {
                    requestSuccessful = false,
                    responseBody = null,
                    responseCode = "400",
                    responseMessage = $"{my_ex.Message}"
                });

            }


        }

        public async Task<PaymentResponse> TransferToBankAccount(PaymentRequest request, string username)
        {
            //get httpclient instance to be used to integrate monnify api
            var client = SetClient();
            bool IsTransferSuccessful = false;


            //get the wallet that funds will be transferred from

            var wallet = await _walletRepository.ViewWallet(username);
            if (wallet == null)
            {
                return (new PaymentResponse
                {
                    requestSuccessful = false,
                    responseBody = null,
                    responseCode = "400",
                    responseMessage = $"You do not have a wallet"
                });
            }

            //ensure wallet has enough funds to transfer

            if ((wallet.WalletBalance - request.amount) <= 0.0)
            {
                return (new PaymentResponse
                {
                    requestSuccessful = false,
                    responseBody = null,
                    responseCode = "400",
                    responseMessage = $"Insufficient Balance for this transfer"
                });
            }


            try
            {
                //post the request to the endpoint using http client instance, and get the response 

                string path = $"merchant/transactions/init-transaction";
                HttpResponseMessage Res = await client.PostAsJsonAsync(path, request);
                var response = await Res.Content.ReadAsAsync<PaymentResponse>();

                if (Res.IsSuccessStatusCode)
                {
                    IsTransferSuccessful = true;
                }

                if (IsTransferSuccessful)  //if the request was successfully posted
                {
                    //deduct from the balance of the wallet 
                    wallet.WalletBalance -= request.amount;
                    await _walletRepository.UpdateWallet(wallet.Id);


                    //create a new transaction to represent the transfer that just happened and store in the database
                    Transaction transaction = new Transaction
                    {
                        WalletId = wallet.Id,
                        Description = request.paymentDescription,
                        Type = TransferTypeEnum.Outflow.GetEnumDescription(),
                        Amount = request.amount,
                        TransactionReference = response.responseBody.transactionReference,
                        PaymentReference = request.paymentReference,
                        DateCreated = DateTime.Now
                    };

                    await _transactionRepository.CreateTransaction(transaction);
                    return response;
                }


                //if the request was unsuccessul, it will display the reason why not
                else
                {
                    return (new PaymentResponse
                    {
                        requestSuccessful = false,
                        responseBody = null,
                        responseCode = "400",
                        responseMessage = $"{response.responseMessage}"
                    });
                }

            }

            //catch exception 

            catch (Exception my_ex)
            {
                return (new PaymentResponse
                {
                    requestSuccessful = false,
                    responseBody = null,
                    responseCode = "400",
                    responseMessage = $"{my_ex.Message}"
                });

            }

        }

        public async Task<WalletHistory> GetWalletTransactions(string username)
        {
            //default values for balance, outflow and inflow
            double TotalInflow = 0;
            double TotalOutflow = 0;
            double CurrentBalance = 0;

            //get transaction history of the wallet owned by the logged in user
            var TransactionHistory = _walletRepository.ViewTransactionHistory(username, out TotalInflow, out TotalOutflow, out CurrentBalance);


            //if null, the user has no wallet, therefore no transaction history
            if (TransactionHistory == null)
            {
                return (new WalletHistory
                {
                    Status = StatusResponseEnum.Failure.GetEnumDescription(),
                    Data = null,
                    Error = new Error { Message = "You do not have a wallet" }
                });

            }

            //if count is 0, user has a wallet, but no transactions have been made with the wallet
            if (TransactionHistory.Count() == 0)
            {
                return await Task.FromResult(new WalletHistory
                {
                    Status = StatusResponseEnum.Success.GetEnumDescription(),
                    Data = new WalletRecord
                    {
                        TotalInflow = 0,
                        TotalOutflow = 0,
                        CurrentBalance = 0,
                        ListOfTransactions = null
                    },
                    Error = null

                });
            }

            //a list of transaction DTOs which represent the transactions made by the wallet

            List<TransactionDTO> transactionDTOs = new List<TransactionDTO>();

            foreach (var transaction in TransactionHistory)
            {
                TransactionDTO transactionDTO = new TransactionDTO
                {
                    Description = transaction.Description,
                    Type = transaction.Type,
                    Amount = transaction.Amount,
                    TransactionReference = transaction.TransactionReference,
                    PaymentReference = transaction.PaymentReference,
                    DateCreated = transaction.DateCreated
                };
                transactionDTOs.Add(transactionDTO);
            }


            //return the Wallet History 
            return (new WalletHistory
            {
                Status = StatusResponseEnum.Success.GetEnumDescription(),
                Data = new WalletRecord
                {
                    TotalInflow = TotalInflow,
                    TotalOutflow = TotalOutflow,
                    CurrentBalance = CurrentBalance,
                    ListOfTransactions = transactionDTOs
                },
                Error = null
            });



        }



    }
}
