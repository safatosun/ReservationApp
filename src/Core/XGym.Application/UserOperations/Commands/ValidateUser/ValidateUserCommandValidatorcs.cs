using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.UserOperations.Commands.ValidateUser
{
    public class ValidateUserCommandValidatorcs : AbstractValidator<ValidateUserCommand>
    {
        public ValidateUserCommandValidatorcs() 
        {
            RuleFor(vu=> vu.UserEmail).NotNull();
            RuleFor(vu=> vu.Password).NotNull();
        } 
    }
}
