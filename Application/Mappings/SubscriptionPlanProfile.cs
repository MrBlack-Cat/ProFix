using Application.CQRS.SubscriptionPlans.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class SubscriptionPlanProfile : Profile
{
    public SubscriptionPlanProfile()
    {
        CreateMap<SubscriptionPlan, CreateSubscriptionPlanDto>().ReverseMap();
        CreateMap<SubscriptionPlan, UpdateSubscriptionPlanDto>().ReverseMap();
        CreateMap<SubscriptionPlan, GetSubscriptionPlanByIdDto>().ReverseMap();
        CreateMap<SubscriptionPlan, SubscriptionPlanListDto>().ReverseMap();

        CreateMap<SubscriptionPlan, DeleteSubscriptionPlanDto>()
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.DeletedReason))
            .ForMember(dest => dest.DeletedByUserId, opt => opt.MapFrom(src => src.DeletedBy)).ReverseMap();
    }
}
