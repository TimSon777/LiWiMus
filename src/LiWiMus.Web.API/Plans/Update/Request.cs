using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Plans.Update;

public class Request : HasId
{
    public decimal? PricePerMonth { get; set; }
    public string? Description { get; set; }
}