using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.ReservationOperations.Commands.CreateReservation
{
    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(c => c.StartTime).GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(c => c.EndTime).GreaterThanOrEqualTo(DateTime.Now);
        }
    }
}
