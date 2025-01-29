using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGym.Application.ReservationOperations.Commands.CreateReservation;
using XGym.Application.ReservationOperations.Commands.UpdateReservation;
using XGym.Application.ReservationOperations.Queries.GetReservationsByUserIdWithPagination;
using XGym.Application.ReservationOperations.Queries.GetReservationsWithPagination;
using XGym.Application.UserDetailOperations.Commands.CreateUserDetail;
using XGym.Application.UserDetailOperations.Commands.UpdateUserDetail;
using XGym.Application.UserDetailOperations.Queries.GetUserDetailByUserId;
using XGym.Application.UserDetailOperations.Queries.GetUsersDetailsWithPagination;
using XGym.Application.UserOperations.Commands.CreateUser;
using XGym.Application.UserOperations.Commands.UpdateUser;
using XGym.Application.UserOperations.Queries.GetUserById;
using XGym.Application.UserOperations.Queries.GetUsersWithPagination;
using XGym.Domain.Entities;

namespace XGym.Application.Common.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<CreateReservationCommand,Reservation>();
            CreateMap<CreateUserDetailCommand,UserDetail>();
            CreateMap<UserDetail, UsersDetailsDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"));
            CreateMap<UserDetail, UserDetailDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"));
            CreateMap<Reservation, ReservationsDto>().ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"));
            CreateMap<Reservation, UserReservationsDto>().ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"));
            CreateMap<CreateUserCommand,User>();
            CreateMap<UpdateUserCommand, User>();
            CreateMap<UpdateUserDetailCommand, UserDetail>();
            CreateMap<UpdateReservationCommand, Reservation>();
            CreateMap<User, UserViewModel>();
            CreateMap<User,UsersDto>(); 
        }
    }
}
