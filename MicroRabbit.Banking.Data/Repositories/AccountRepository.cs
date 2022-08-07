using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDbContext _ctx;

        public AccountRepository(BankingDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            return await _ctx.Accounts.ToListAsync();   
        }
    }
}
