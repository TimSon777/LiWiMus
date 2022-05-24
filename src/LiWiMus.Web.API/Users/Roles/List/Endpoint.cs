using AutoMapper;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.API.Shared.Extensions;
using LiWiMus.Web.Shared.Extensions;
using MinimalApi.Endpoint;
using RoleDto = LiWiMus.Web.API.Roles.Dto;

namespace LiWiMus.Web.API.Users.Roles.List;

public class Endpoint : IEndpoint<IResult, int>
{
    private IRepository<User> _userRepository = null!;
    private IRepository<Role> _roleRepository = null!;
    private readonly IMapper _mapper;

    public Endpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IResult> HandleAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            return Results.Extensions.NotFoundById(EntityType.Users, userId);
        }

        var roles = await _roleRepository.GetByUserAsync(user);
        var dto = _mapper.MapList<Role, RoleDto>(roles);

        return Results.Ok(dto);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(RouteConstants.Users.Roles.List,
            async (int userId, IRepository<User> userRepository, IRepository<Role> roleRepository) =>
            {
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                return await HandleAsync(userId);
            });
    }
}