using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Model
{
    public class ResponseBody
    {
        public string transactionReference { get; set; }
        public string paymentReference { get; set; }
        public string merchantName { get; set; }
        public string apiKey { get; set; }
        public string redirectUrl { get; set; }
        public List<string> enabledPaymentMethod { get; set; }
        public string checkoutUrl { get; set; }
    }

    public class PaymentResponse
    {
        public bool requestSuccessful { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public ResponseBody responseBody { get; set; }
    }
}
