using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services;
using LoadLogic.Services.Ordering.API.Models;
using LoadLogic.Services.Ordering.Application.Commands.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoadLogic.Services.Ordering.API.Controllers
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
            var message = new CreateOrder(
                100, "John Doe", (Email)"john.doe@example.com", (PhoneNumber)"1281314460",
                "Job Name", "Job Description", new Address(), DateTime.UtcNow, DateTime.UtcNow);

            var id = await Mediator.Send(message, cancellationToken);
            return Ok(id);
        }
    }
}
