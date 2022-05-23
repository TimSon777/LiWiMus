using AutoMapper;
using LiWiMus.Core.Roles;
using LiWiMus.Web.API.Roles.Create;

namespace LiWiMus.Web.API.Roles;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Role>();
        CreateMap<Update.Request, Role>();
        CreateMap<Role, Dto>()
            .ForMember(dto => dto.CreatedAt, expression => expression
                .MapFrom(plan => DateTime.SpecifyKind(plan.CreatedAt, DateTimeKind.Utc)))
            .ForMember(dto => dto.ModifiedAt, expression => expression
                .MapFrom(plan => DateTime.SpecifyKind(plan.ModifiedAt, DateTimeKind.Utc)));
    }
}