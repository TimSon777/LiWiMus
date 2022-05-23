using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Roles.Update;

public class Request : HasId
{
    public decimal? PricePerMonth { get; set; }
    public string? Description { get; set; }
}