using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using XGym.Application.Common.Parameter;
using XGym.Application.Common.Shared;
using XGym.Application.ReservationOperations.Commands.CreateReservation;
using XGym.Application.ReservationOperations.Commands.DeleteReservation;
using XGym.Application.ReservationOperations.Commands.UpdateReservation;
using XGym.Application.ReservationOperations.Queries.GetReservationsByUserIdWithPagination;
using XGym.Application.ReservationOperations.Queries.GetReservationsWithPagination;
using XGym.Application.UserOperations.Queries.GetUsersWithPagination;

namespace XGym.WebApi.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("all")]
        public async Task<Response<IEnumerable<ReservationsDto>>> Get([FromQuery] ReservationRequestParameters requestParameters)
        {
            var query = new GetReservationsWithPaginationQuery { requestParameters = requestParameters };
            var result = await _mediator.Send(query);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Item2));
            return result.Item1;
        }

        [Authorize]
        [HttpGet]
        public async Task<Response<IEnumerable<UserReservationsDto>>> GetByUserId([FromQuery] ReservationRequestParameters requestParameters)
        {
            var query = new GetReservationsByUserIdWithPaginationQuery { requestParameters = requestParameters };
            var result = await _mediator.Send(query);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Item2));
            return result.Item1;
        }

        [Authorize]
        [HttpPost]
        public async Task<Response> Create([FromBody] CreateReservationCommand command)
        { 
            return await _mediator.Send(command);
        }
       
        [Authorize]
        [HttpPut]
        public async Task<Response> Update([FromBody] UpdateReservationCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpDelete]
        public async Task<Response> Delete([FromQuery] DeleteReservationCommand command)
        { 
            return await _mediator.Send(command);
        }

    }
}
