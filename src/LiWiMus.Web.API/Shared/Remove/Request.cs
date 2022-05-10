using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Shared.Remove;

public class Request : HasId
{
    public int DeletedId { get; set; }
}