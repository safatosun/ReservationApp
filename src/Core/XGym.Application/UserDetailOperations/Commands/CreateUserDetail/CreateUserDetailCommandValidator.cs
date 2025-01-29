using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.UserDetailOperations.Commands.CreateUserDetail
{
    public class CreateUserDetailCommandValidator : AbstractValidator<CreateUserDetailCommand>
    {
        public CreateUserDetailCommandValidator() 
        {
            RuleFor(c => c.Shoulder).GreaterThan(0);
            RuleFor(c => c.Chest).GreaterThan(0);
            RuleFor(c => c.Arm).GreaterThan(0);
            RuleFor(c => c.Waist).GreaterThan(0);
            RuleFor(c => c.Hip).GreaterThan(0);
            RuleFor(c => c.Thigh).GreaterThan(0);
        }
    }
}
