using MicroRabbit.DataModels;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Data.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _ctx;

        public TransferRepository(TransferDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<TransferLog>> GetTransferLogsAsync()
        {
            return await _ctx.TransferLogs.ToListAsync();   
        }

        public async Task<ProcessResponse> AddAsync(TransferLog transferLog)
        {
            ProcessResponse processResponse = new();
            
            await _ctx.TransferLogs.AddAsync(transferLog);
            
            var numberOfRowsAffected = await _ctx.SaveChangesAsync();

            if(numberOfRowsAffected > 1)
            {
                processResponse.Message = "The operation was a success";
                processResponse.IsErrorOccurred = false;
            }
            else
            {
                processResponse.Message = "The operation was not a success";
                processResponse.IsErrorOccurred = true;
            }

            return processResponse;

        }
    }
}
