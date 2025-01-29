using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.UserOperations.Commands.CreateRefreshToken
{
    public class CreateRefreshTokenCommandValidator : AbstractValidator<CreateRefreshTokenCommand>
    {
        public CreateRefreshTokenCommandValidator() 
        {
            RuleFor(c => c.AccessToken).NotEmpty();
            RuleFor(c => c.RefreshToken).NotEmpty();
        }
    }

}
