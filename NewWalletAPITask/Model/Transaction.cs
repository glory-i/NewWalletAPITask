using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentReference { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
