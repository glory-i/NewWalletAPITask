using Microsoft.EntityFrameworkCore;
using NewWalletAPITask.Authentication;
using NewWalletAPITask.Enumerations;
using NewWalletAPITask.Model;
using NewWalletAPITask.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Transactions;

namespace NewWalletAPITask.Repository.Implementation
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> ViewTransactionHistory(string username)
        {
            var WalletTransactions = _context.Transactions
                .Where(t => t.Wallet.Owner.Username == username)
                .Include(t => t.Wallet)
                .ThenInclude(w => w.Owner)
                .ToList();

            return await Task.FromResult(WalletTransactions);
        }
    }
}
