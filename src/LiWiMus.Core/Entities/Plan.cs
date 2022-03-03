namespace LiWiMus.Core.Entities;

public class Plan : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PricePerMonth { get; set; }
}