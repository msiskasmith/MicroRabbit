using MassTransit;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Contracts;
using MicroRabbit.DataModels;
using MicroRabbit.Infrastructure.ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController : ControllerBase
    {
        private readonly ILogger<BankingController> _logger;
        private readonly IAccountService _accountService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IDistributedCache _distributedCache;

        public BankingController(ILogger<BankingController> logger
            , IAccountService accountService
            , IPublishEndpoint publishEndpoint
            , IDistributedCache distributedCache)
        {
            _logger = logger;
            _accountService = accountService;
            _publishEndpoint = publishEndpoint;
            _distributedCache = distributedCache;
        }

        // GET api/banking
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            string recordKey = "Accounts_" + DateTime.Now.ToString("yyyyMMdd_hhmm");

            var accounts = await _distributedCache.GetRecordAsync<IEnumerable<Account>>(recordKey);

            if (accounts is null)
            {
                accounts = await _accountService.GetAccountsAsync();

                await _distributedCache.SetRecordAsync(recordKey, accounts);
            }

            return Ok(accounts);
        }

        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer([FromBody] AccountTransfer transfer,
            [FromServices] IRequestClient<IAddBankTransferRequest> client)
        {
            var response = await client.GetResponse<IProcessResponse>(new  { Transfer = transfer });
            
            if (response.Message.ProcessResponse.IsErrorOccurred)
            {
                return BadRequest(response.Message.ProcessResponse.Message);
            }
            
            return Created("", response.Message.ProcessResponse.Message);
        }

        [HttpPost("Publish")]
        public async Task<IActionResult> Publish([FromBody] AccountTransfer transfer)
        {
           
            await _publishEndpoint.Publish<AccountTransfer>(transfer);    

            return Created("", "Created");
        }
    }
}
