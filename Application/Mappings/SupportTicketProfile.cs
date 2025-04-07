using Application.CQRS.SupportTickets.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class SupportTicketProfile : Profile
{
    public SupportTicketProfile()
    {
        CreateMap<SupportTicket, CreateSupportTicketDto>().ReverseMap();
        CreateMap<SupportTicket, UpdateSupportTicketDto>().ReverseMap();
        CreateMap<SupportTicket, GetSupportTicketByIdDto>().ReverseMap();
        CreateMap<SupportTicket, SupportTicketListDto>().ReverseMap();


        CreateMap<SupportTicket, DeleteSupportTicketDto>()
    .ForMember(dest => dest.DeletedReason, opt => opt.MapFrom(src => src.DeletedReason))
    .ForMember(dest => dest.DeletedByUserId, opt => opt.MapFrom(src => src.DeletedBy));


    }
}
