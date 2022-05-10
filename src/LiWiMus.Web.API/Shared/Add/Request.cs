using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Shared.Add;

public class Request : HasId
{
    public List<int> Ids { get; set; } = new();
}