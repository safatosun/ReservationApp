﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.ReservationOperations.Queries.GetReservationsWithPagination;

namespace XGym.Application.ReservationOperations.Queries.GetReservationsByUserIdWithPagination
{
    public class GetReservationByUserIdWithPaginationQueryValidator : AbstractValidator<GetReservationsByUserIdWithPaginationQuery>
    {
        public GetReservationByUserIdWithPaginationQueryValidator()
        {
            RuleFor(g => g.requestParameters.PageNumber).Must(pageNum => pageNum >= 1).WithMessage("Page number must be greater than or equal to 1.");
            RuleFor(g => g.requestParameters.PageSize).Must(pageSize => pageSize >= 5).WithMessage("Page size must be greater than or equal to 5.");
        }
    }
}
