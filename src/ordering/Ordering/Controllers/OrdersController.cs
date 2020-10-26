using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services;
using LoadLogic.Services.Ordering.Models;
using LoadLogic.Services.Ordering.Application.Commands.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoadLogic.Services.Ordering.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : RootController
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateOrderAsync(CancellationToken cancellationToken)
        {
            var command = new CreateOrder(
                100, "John Doe", (Email)"john.doe@example.com", (PhoneNumber)"1281314460",
                "Job Name", "Job Description", new Address(), DateTime.UtcNow, DateTime.UtcNow);

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                nameof(CreateOrder),
                nameof(CreateOrder.CustomerId),
                command.CustomerId,
                command);


            var id = await Mediator.Send(command, cancellationToken);
            return Ok(id);
        }
    }
}
