using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.UserOperations.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator() 
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Surname).NotEmpty();
            RuleFor(c => c.Email).NotEmpty();
            RuleFor(c => c.PhoneNumber).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }
    }
}
