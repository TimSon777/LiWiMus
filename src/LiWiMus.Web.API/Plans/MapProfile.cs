using AutoMapper;
using LiWiMus.Core.Plans;
using LiWiMus.Web.API.Plans.Create;

namespace LiWiMus.Web.API.Plans;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Plan>();
        CreateMap<Update.Request, Plan>();
        CreateMap<Plan, Dto>()
            .ForMember(dto => dto.CreatedAt, expression => expression
                .MapFrom(plan => DateTime.SpecifyKind(plan.CreatedAt, DateTimeKind.Utc)))
            .ForMember(dto => dto.ModifiedAt, expression => expression
                .MapFrom(plan => DateTime.SpecifyKind(plan.ModifiedAt, DateTimeKind.Utc)));
    }
}