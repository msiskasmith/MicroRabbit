using MicroRabbit.Infrastructure.ClassLibrary;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        private readonly ITransferService _transferService;
        private readonly IDistributedCache _distributedCache;

        public TransferController(ILogger<TransferController> logger
            , ITransferService transferService
            , IDistributedCache distributedCache)
        {
            _logger = logger;
            _transferService = transferService;
            _distributedCache = distributedCache;
        }

        // GET api/Transfer
        [HttpGet]
        public async Task<IActionResult> GetTransfer()
        {
            string recordKey = "TransferLogs_" + DateTime.Now.ToString("yyyyMMdd_hhmm");

            var transferLogs = await _distributedCache.GetRecordAsync<IEnumerable<TransferLog>>(recordKey);

            if (transferLogs is null)
            {
                transferLogs = await _transferService.GetTransferLogsAsync();

                await _distributedCache.SetRecordAsync(recordKey, transferLogs);
            }

            return Ok(transferLogs);
        }
    }
}
