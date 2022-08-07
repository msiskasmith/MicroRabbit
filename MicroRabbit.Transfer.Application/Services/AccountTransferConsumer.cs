using MassTransit;
using MicroRabbit.Contracts;
using MicroRabbit.DataModels;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Application.Services
{
    public class AccountTransferConsumer : IConsumer<IAddBankTransferRequest>
    {
        private readonly ITransferRepository _transferRepository;

        public AccountTransferConsumer(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }
        public async Task Consume(ConsumeContext<IAddBankTransferRequest> context)
        {
            TransferLog transferLog = new()
            {
                FromAccount = context.Message.Transfer.FromAccount,
                ToAccount = context.Message.Transfer.ToAccount,
                TransferAmount = context.Message.Transfer.TransferAmount,
            };

            var processResponse = await _transferRepository.AddAsync(transferLog);

            await context.RespondAsync<IProcessResponse>(processResponse);
        }
    }


    public class AccountTransferConsume : IConsumer<AccountTransfer>
    {
        private readonly ITransferRepository _transferRepository;

        public AccountTransferConsume(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }
        public async Task Consume(ConsumeContext<AccountTransfer> context)
        {
            TransferLog transferLog = new()
            {
                FromAccount = context.Message.FromAccount,
                ToAccount = context.Message.ToAccount,
                TransferAmount = context.Message.TransferAmount,
            };

            var processResponse = await _transferRepository.AddAsync(transferLog);

            await context.RespondAsync<IProcessResponse>(processResponse);
        }
    }
}
