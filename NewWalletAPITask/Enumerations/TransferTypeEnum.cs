using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Enumerations
{
    public enum TransferTypeEnum
    {
        [Description("Funding Wallet")] Inflow = 1,
        [Description("External Transfer")] Outflow,



    }
}
