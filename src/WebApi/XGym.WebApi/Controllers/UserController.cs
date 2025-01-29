using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using XGym.Application.Common.Parameter;
using XGym.Application.Common.Shared;
using XGym.Application.UserOperations.Commands.CreateRefreshToken;
using XGym.Application.UserOperations.Commands.CreateToken;
using XGym.Application.UserOperations.Commands.CreateUser;
using XGym.Application.UserOperations.Commands.DeleteUser;
using XGym.Application.UserOperations.Commands.UpdateUser;
using XGym.Application.UserOperations.Commands.UpdateUserPassword;
using XGym.Application.UserOperations.Commands.ValidateUser;
using XGym.Application.UserOperations.Queries.GetUserById;
using XGym.Application.UserOperations.Queries.GetUsersWithPagination;

namespace XGym.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("all")]
        public async Task<Response<IEnumerable<UsersDto>>> GetAll([FromQuery] RequestParameters requestParameters)
        {
            var query = new GetUsersWithPaginationQuery() { requestParameters = requestParameters };
            var result = await _mediator.Send(query);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Item2));
            return result.Item1;
        }

        [Authorize]
        [HttpGet()]
        public async Task<Response<UserViewModel>> GetById()
        {
            var query = new GetUserByUserIdQuery();
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<Response> Create([FromBody] CreateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("login")]
        public async Task<Response<TokenDto>> Login([FromBody] ValidateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpPut]
        public async Task<Response> Update([FromBody] UpdateUserCommand command)
        {
            return await _mediator.Send(command);
        }
        
        [Authorize]
        [HttpDelete]
        public async Task<Response> Delete([FromQuery]DeleteUserCommand? command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<Response<TokenDto>> RefreshToken([FromBody] CreateRefreshTokenCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpPut("update-password")]
        public async Task<Response> UpdatePassword([FromBody] UpdateUserPasswordCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
