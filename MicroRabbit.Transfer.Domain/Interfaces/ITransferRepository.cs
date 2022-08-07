using MicroRabbit.DataModels;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Domain.Interfaces
{
    public interface ITransferRepository
    {
        Task<IEnumerable<TransferLog>> GetTransferLogsAsync();

        Task<ProcessResponse> AddAsync(TransferLog transferLog);
    }
}
