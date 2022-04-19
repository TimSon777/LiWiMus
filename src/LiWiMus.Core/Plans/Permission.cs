namespace LiWiMus.Core.Plans;

public class Permission
{
    public int Id { get; set; }
    public string Subject { get; set; } = null!;
    public string Operation { get; set; } = null!;

    public List<Plan> Plans { get; set; } = new();
}