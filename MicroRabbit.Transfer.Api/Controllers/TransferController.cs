using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        private readonly ITransferService _transferService;

        public TransferController(ILogger<TransferController> logger, ITransferService transferService)
        {
            _logger = logger;
            _transferService = transferService;
        }

        // GET api/Transfer
        [HttpGet]
        public ActionResult<IEnumerable<TransferLog>> GetTransfer()
        {
            return Ok(_transferService.GetTransferLogs());
        }

        //[HttpPost]
        //public IActionResult Post([FromBody] TransferTransfer transferTransfer)
        //{
        //    _transferService.Transfer(transferTransfer);
        //    return Ok(transferTransfer);
        //}
    }
}
