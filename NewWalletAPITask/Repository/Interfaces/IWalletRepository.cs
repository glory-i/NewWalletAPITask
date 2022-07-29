using NewWalletAPITask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Repository.Interfaces
{
    public interface IWalletRepository
    {
        public Task<Wallet> CreateWallet(string NameOfWallet, string username);
        public Task<Wallet> ViewWallet(string username);
        public Task<Wallet> UpdateWallet(int id);

        public IEnumerable<Transaction> ViewTransactionHistory(string username, out double TotalInflow, out double TotalOutflow, out double CurrentBalance);

    }
}
