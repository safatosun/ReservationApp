using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGym.Application.ReservationOperations.Queries.GetReservationsWithPagination
{
    public class GetReservationsWithPaginationQueryValidator : AbstractValidator<GetReservationsWithPaginationQuery>
    {
        public GetReservationsWithPaginationQueryValidator()
        {
            RuleFor(g => g.requestParameters.PageNumber).Must(pageNum => pageNum >= 1).WithMessage("Page number must be greater than or equal to 1.");
            RuleFor(g => g.requestParameters.PageSize).Must(pageSize => pageSize >= 5).WithMessage("Page size must be greater than or equal to 5.");
        }
    }
}
