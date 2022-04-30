using AutoMapper;
using LiWiMus.API.Transactions.Update;

namespace LiWiMus.API.Transactions;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Core.Transactions.Transaction>();
    }
}