namespace LiWiMus.Web.API.Plans.Create;

public class Request
{
    public string Name { get; set; } = null!;
    public decimal PricePerMonth { get; set; }
    public string Description { get; set; } = null!;
}