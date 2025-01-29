using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using XGym.Application.Common.Parameter;
using XGym.Application.Common.Shared;
using XGym.Application.UserDetailOperations.Commands.CreateUserDetail;
using XGym.Application.UserDetailOperations.Commands.DeleteUserDetail;
using XGym.Application.UserDetailOperations.Commands.UpdateUserDetail;
using XGym.Application.UserDetailOperations.Queries.GetUserDetailByUserId;
using XGym.Application.UserDetailOperations.Queries.GetUsersDetailsWithPagination;

namespace XGym.WebApi.Controllers
{
    [Route("api/userdetails")]
    [ApiController]
    public class UserDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<Response<IEnumerable<UsersDetailsDto>>> Get([FromQuery]RequestParameters requestParameters)
        {
            var query = new GetUsersDetailsWithPaginationQuery() { requestParameters = requestParameters };
            var result = await _mediator.Send(query);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Item2));
            return result.Item1;
        }

        [Authorize]
        [HttpGet]
        public async Task<Response<UserDetailDto>> GetByUserId()
        {
            var query = new GetUserDetailByUserIdQuery();
            return await _mediator.Send(query);
        }
       
        [Authorize]
        [HttpPost]
        public async Task<Response> Create([FromBody] CreateUserDetailCommand command)
        {
            return await _mediator.Send(command);
        }
        
        [Authorize]
        [HttpPut]
        public async Task<Response> Update([FromBody] UpdateUserDetailCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpDelete]
        public async Task<Response> Delete([FromQuery] DeleteUserDetailCommand? command)
        {
            return await _mediator.Send(command);
        }

    }
}
