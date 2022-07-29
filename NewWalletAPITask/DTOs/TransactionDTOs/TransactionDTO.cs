using NewWalletAPITask.DTOs.WalletDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.DTOs.TransactionDTOs
{
    public class WalletHistory
    {
        public string Status { get; set; }
        public Error Error { get; set; }
        public WalletRecord Data { get; set; }
    }

    public class WalletRecord
    {
        public double TotalOutflow { get; set; }
        public double TotalInflow { get; set; }
        public double CurrentBalance { get; set; }
        public List<TransactionDTO> ListOfTransactions { get; set; }
    }

    public class TransactionDTO
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentReference { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
