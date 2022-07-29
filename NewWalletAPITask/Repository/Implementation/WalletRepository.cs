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

namespace NewWalletAPITask.Repository.Implementation
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;
        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Wallet> CreateWallet(string NameOfWallet, string username)
        {
            //ensure that particular user does not already have a wallet. One wallet per user
            var MyWallet = await ViewWallet(username);
            if (MyWallet != null)
            {
                return null;
            }

            //get the Owner with that username, that will be the owner of the wallet.
            var WalletOwner = _context.Owners.Where(o => o.Username == username).FirstOrDefault();

            Wallet wallet = new Wallet
            {

                OwnerId = WalletOwner.Id,
                WalletName = NameOfWallet,
                WalletBalance = 0.0,
                DateCreated = DateTime.Now
            };

            //add the wallet to the database and save changes

            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<Wallet> UpdateWallet(int id)
        {
            //find the wallet with that id and update the wallet

            var wallet = await _context.Wallets.FindAsync(id);
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
            return wallet;

        }

        public async Task<Wallet> ViewWallet(string username)
        {
            //find the wallet owned by that particular user.

            var wallet = _context.Wallets
                .Where(w => w.Owner.Username == username)
                .Include(o => o.Owner)
                .FirstOrDefault();


            //if the user does not have a wallet
            if (wallet == null)
            {
                return null;
            }
            return await Task.FromResult(wallet);
        }

        public IEnumerable<Transaction> ViewTransactionHistory(string username, out double TotalInflow, out double TotalOutflow, out double CurrentBalance)
        {
            //ensure the wallet exists

            var WalletExists = _context.Wallets.Where(w => w.Owner.Username == username).Any();
            if (!WalletExists)
            {
                TotalInflow = 0; TotalOutflow = 0; CurrentBalance = 0;
                return null;
            }

            //get all the transactions a wallet has performed and order by date
            var WalletTransactions = _context.Transactions
                .Where(t => t.Wallet.Owner.Username == username)
                .Include(t => t.Wallet)
                .ThenInclude(w => w.Owner)
                .OrderBy(t => t.DateCreated)
                .AsQueryable();


            //get the total inflow and total outflow into the wallet, will be returned as out parameters
            TotalInflow = WalletTransactions.Where(t => t.Type == TransferTypeEnum.Inflow.GetEnumDescription()).Sum(t => t.Amount);
            TotalOutflow = WalletTransactions.Where(t => t.Type == TransferTypeEnum.Outflow.GetEnumDescription()).Sum(t => t.Amount);



            //if the wallet has no transactions, its balance is 0
            //if the wallet has transactions, the balance of the wallet will be returned as an out parameter (CurrentBalance)

            var transaction = WalletTransactions.FirstOrDefault();
            CurrentBalance = transaction == null ? 0.0 : transaction.Wallet.WalletBalance;

            return WalletTransactions.ToList();
        }
    }
}
