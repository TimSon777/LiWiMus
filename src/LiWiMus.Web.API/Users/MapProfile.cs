using AutoMapper;
using LiWiMus.Core.Users;
using LiWiMus.Web.API.Users.Create;

namespace LiWiMus.Web.API.Users;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, User>();
        CreateMap<User, Dto>()
            .ForMember(dto => dto.CreatedAt, expression => expression
                .MapFrom(user => DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc)))
            .ForMember(dto => dto.ModifiedAt, expression => expression
                .MapFrom(user => DateTime.SpecifyKind(user.ModifiedAt, DateTimeKind.Utc)));
    }
}