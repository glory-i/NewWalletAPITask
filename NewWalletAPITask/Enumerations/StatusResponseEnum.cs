using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Enumerations
{
    public enum StatusResponseEnum
    {
        [Description("Success")] Success = 1,
        [Description("Failure")] Failure,

    }
}
