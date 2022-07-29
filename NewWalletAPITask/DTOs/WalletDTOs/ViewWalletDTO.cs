using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.DTOs.WalletDTOs
{
    public class ViewWalletDTO
    {
        public string WalletOwner { get; set; }
        public string WalletName { get; set; }
        public double WalletBalance { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class WalletResponse
    {
        public string Status { get; set; }
        public Error Error { get; set; }
        public ViewWalletDTO Data { get; set; }


    }

    public class Error
    {
        public string Message { get; set; }
    }
}
