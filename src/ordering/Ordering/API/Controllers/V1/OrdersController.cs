using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LoadLogic.Services.Ordering.API.Controllers.V1.Models;
using LoadLogic.Services.Ordering.Application.Commands.Orders;
using LoadLogic.Services.Ordering.Application.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadLogic.Services.Ordering.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/orders")]
    public class OrdersController : RootController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public OrdersController(
            IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// A list of Orders.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of Orders.</returns>
        /// <response code="200">Returns the list</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<OrderSummaryApiResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            var query = new GetOrders();
            var orders = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<IEnumerable<OrderSummaryApiResponse>>(orders);
            return Ok(response);
        }

        /// <summary>
        /// Creates an Order.
        /// </summary>
        /// <param name="request">The order model to create.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>the id of the created order</returns>
        /// <response code="201">Order was created.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderApiRequest request,
            CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateOrderCommand>(request);
            var id = await _mediator.Send(command, cancellationToken);
            return Created("", id);
        }
    }
}
