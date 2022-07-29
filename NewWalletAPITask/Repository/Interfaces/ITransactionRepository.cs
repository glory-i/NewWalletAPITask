using NewWalletAPITask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Transactions;

namespace NewWalletAPITask.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        //public Task<Transaction> CreateTransaction(Transaction transaction);
        public Task<Transaction> CreateTransaction(Transaction transaction);
        public Task<IEnumerable<Transaction>> ViewTransactionHistory(string username);

    }
}
