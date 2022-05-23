namespace LiWiMus.Web.API.Shared.Extensions;

public static class ResultsExtensions
{
    public static IResult NotFoundById(this IResultExtensions _, EntityType entityType, int id)
    {
        return Results.UnprocessableEntity(new { detail = $"No {entityType} with Id {id}." });
    }
}

public enum EntityType : byte
{
    Artists,
    Tracks,
    Genres,
    Albums,
    Playlists,
    Transactions,
    Plans,
    Permissions,
    Roles
}