using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LoadLogic.Services.Ordering.API.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    public abstract class RootController : ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator => _mediator ??= (HttpContext.RequestServices.GetService<IMediator>() ?? throw new NullReferenceException());
    }
}
