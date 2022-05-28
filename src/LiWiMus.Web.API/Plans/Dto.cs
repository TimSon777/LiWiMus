using LiWiMus.Core.Plans;
using LiWiMus.SharedKernel;
using PermissionDto = LiWiMus.Web.API.Permissions.Dto;

namespace LiWiMus.Web.API.Plans;

public class Dto : BaseDto
{
    public string Name { get; set; } = null!;
    public decimal PricePerMonth { get; set; }
    public string Description { get; set; } = null!;

    public List<PermissionDto> Permissions { get; set; } = null!;

    public bool Deletable => DefaultPlans.GetAll().All(p => p.Name != Name);
}