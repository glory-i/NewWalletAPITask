using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Model
{
    public class Wallet
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public virtual Owner Owner { get; set; }

        public string WalletName { get; set; }
        public double WalletBalance { get; set; }
        public DateTime DateCreated { get; set; }


    }
}
