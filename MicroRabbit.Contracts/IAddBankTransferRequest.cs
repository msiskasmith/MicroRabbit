using MicroRabbit.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Contracts
{
    public interface IAddBankTransferRequest
    {
        AccountTransfer Transfer { get;}   
    }
}
