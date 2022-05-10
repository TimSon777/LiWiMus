namespace LiWiMus.Web.API;

public static class RouteConstants
{
    private const string Prefix = "/api";
    
    public static class Albums
    {
        public const string Create = $"{Prefix}/{nameof(Albums)}";
        public const string Read = $"{Prefix}/{nameof(Albums)}/{{id:int}}";
        public const string Delete = $"{Prefix}/{nameof(Albums)}/{{id:int}}";
        public const string Update = $"{Prefix}/{nameof(Albums)}";
        //public const string ReadList = $"{Prefix}/{nameof(Albums)}";
    }
}