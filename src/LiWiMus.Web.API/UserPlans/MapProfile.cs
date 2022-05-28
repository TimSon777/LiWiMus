using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Web.API.UserPlans.Update;

namespace LiWiMus.Web.API.UserPlans;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<UserPlan, Dto>()
            .ForMember(dto => dto.UserName, expression => expression
                .MapFrom(plan => plan.User.UserName))
            .ForMember(dto => dto.PlanName, expression => expression
                .MapFrom(plan => plan.Plan.Name))
            .ForMember(dto => dto.PlanDescription, expression => expression
                .MapFrom(plan => plan.Plan.Description))
            .ForMember(dto => dto.Start, expression => expression
                .MapFrom(plan => DateTime.SpecifyKind(plan.Start, DateTimeKind.Utc)))
            .ForMember(dto => dto.End, expression => expression
                .MapFrom(plan => DateTime.SpecifyKind(plan.End, DateTimeKind.Utc)))
            .ForMember(dto => dto.CreatedAt, expression => expression
                .MapFrom(plan => DateTime.SpecifyKind(plan.CreatedAt, DateTimeKind.Utc)))
            .ForMember(dto => dto.ModifiedAt, expression => expression
                .MapFrom(plan => DateTime.SpecifyKind(plan.ModifiedAt, DateTimeKind.Utc)));

        CreateMap<Request, UserPlan>()
            .ForMember(up => up.End, expression => expression
                .MapFrom(request => DateTime.SpecifyKind(request.End, DateTimeKind.Utc)));

        CreateMap<Create.Request, UserPlan>()
            .ForMember(up => up.End, expression => expression
                .MapFrom(request => DateTime.SpecifyKind(request.End, DateTimeKind.Utc)))
            .ForMember(up => up.Start, expression => expression
                .MapFrom(request => DateTime.SpecifyKind(request.Start, DateTimeKind.Utc)));
    }
}