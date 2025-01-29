using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.ReservationOperations.Commands.DeleteReservation
{
    public class DeleteReservationCommandValidator : AbstractValidator<DeleteReservationCommand>
    {
        public DeleteReservationCommandValidator() 
        {
            RuleFor(dc => dc.Id).GreaterThan(0);
        }
    }
}
