using Ardalis.Specification;

namespace LiWiMus.Core.Tracks.Specifications;

public sealed class TracksByTitleSpec : Specification<Track>
{
    public TracksByTitleSpec(string name, int skip, int take)
    {
        Query
            .Where(t => t.Name.ToLower().Contains(name.ToLower()))
            .Skip(skip)
            .Take(take)
            .Include(t => t.Album)
            .ThenInclude(a => a.Subscribers)
            .Include(t => t.Owners)
            .OrderByDescending(a => a.Subscribers.Count);
    }
}