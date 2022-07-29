using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Model
{
    public class PaymentRequest
    {
        public double amount { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string paymentReference { get; set; }
        public string paymentDescription { get; set; }
        public string currencyCode { get; set; }
        public string contractCode { get; set; }
        public string redirectUrl { get; set; }
        public List<string> paymentMethods { get; set; }
    }
}
