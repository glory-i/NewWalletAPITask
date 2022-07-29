using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Enumerations
{
    public enum AuthenticationResponseEnum
    {
        [Description("Error")] Error = 1,
        [Description("Success")] Success,

    }
}
