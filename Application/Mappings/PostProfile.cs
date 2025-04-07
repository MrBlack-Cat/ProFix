using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Handlers;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class PostProfile : Profile
{
    public PostProfile()
    {
        
        //CreateMap<Post, CreatePostDto>().ReverseMap();

        CreateMap<Post, CreatePostDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ReverseMap();

        CreateMap<Post, UpdatePostDto>().ReverseMap()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)); // Yeniləndiyi zaman əlavə edirik;
        
        CreateMap<Post, GetPostByIdDto>().ReverseMap();
        CreateMap<Post, PostListDto>().ReverseMap();


        //yeni elave command dan Post a cevirmek ucun db ye elave olunsun 
        CreateMap<CreatePostHandler.Command, Post>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)).ReverseMap();

        //yeni elave 
        CreateMap<DeletePostHandler.Command, Post>()
            .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => false)).ReverseMap();



    }
}
