using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.ReservationOperations.Commands.UpdateReservation
{
    public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
    {
        public UpdateReservationCommandValidator()
        {
            RuleFor(u=>u.Id).GreaterThan(0);
            RuleFor(u => u.StartTime).GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(u => u.EndTime).GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(u => u.Status).NotNull();
        }
    }
}
