using AutoMapper;
using LiWiMus.Core.Transactions;
using LiWiMus.Web.API.Transactions.Update;

namespace LiWiMus.Web.API.Transactions;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Transaction>();
        CreateMap<Create.Request, Transaction>();
        CreateMap<Transaction, Dto>();
    }
}