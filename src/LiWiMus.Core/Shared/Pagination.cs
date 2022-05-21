namespace LiWiMus.Core.Shared;

public record Pagination(int Page, int Take)
{
    public static implicit operator Pagination((int page, int take) obj)
    {
        return new Pagination(obj.page, obj.take);
    }
};
