using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class ArtistByNameSpecification : Specification<Artist>, ISingleResultSpecification
{
    public ArtistByNameSpecification(string name)
    {
        Query.Where(artist => artist.Name == name);
    }
}