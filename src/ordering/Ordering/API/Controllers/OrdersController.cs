using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services;
using LoadLogic.Services.Ordering.Application.Commands.Orders;
using LoadLogic.Services.Ordering.Application.Models.Orders;
using LoadLogic.Services.Ordering.Application.Queries.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoadLogic.Services.Ordering.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class OrdersController : RootController
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            var query = new GetOrders();

            _logger.LogInformation(
                "Sending query: {CommandName}: {@query}",
                nameof(GetOrders), query);

            var orders = await Mediator.Send(query, cancellationToken);
            return Ok(orders);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand(
                dto.Type, dto.CustomerId,
                dto.CustomerFirstName, dto.CustomerLastName,
                (Email)dto.CustomerEmail, (PhoneNumber)dto.CustomerPhone,
                dto.JobName, dto.JobDescription,
                dto.JobAddress ?? new Address(), dto.JobStartDate);

            _logger.LogInformation(
                "Sending command: {CommandName}: {@Command}",
                nameof(CreateOrderCommand), command);

            var id = await Mediator.Send(command, cancellationToken);
            return Ok(id);
        }
    }
}
