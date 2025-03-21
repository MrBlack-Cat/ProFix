﻿using AutoMapper;
using Domain.Entities;
using Application.CQRS.Users.DTOs;


namespace Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RegisterUserDto>().ReverseMap();
        CreateMap<User, UpdateUserDto>().ReverseMap();
        CreateMap<User, GetUserByIdDto>().ReverseMap();
        CreateMap<User, UserListDto>().ReverseMap();
        CreateMap<User, GetAllUserDto>().ReverseMap();
    }
}
