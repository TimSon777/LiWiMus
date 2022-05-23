namespace LiWiMus.Web.API.Shared;

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

        public static class Artists
        {
            public const string Add = $"{Prefix}/{nameof(Albums)}/{nameof(Artists)}";
            public const string Remove = $"{Prefix}/{nameof(Albums)}/{nameof(Artists)}";
            public const string List = $"{Prefix}/{nameof(Albums)}/{{albumId:int}}/{nameof(Artists)}";
        }
    }

    public static class Playlists
    {
        public const string Read = $"{Prefix}/{nameof(Playlists)}/{{id:int}}";
        public const string Update = $"{Prefix}/{nameof(Playlists)}";
        public const string RemovePhoto = $"{Prefix}/{nameof(Playlists)}/{{id:int}}/{nameof(RemovePhoto)}";
        public const string Delete = $"{Prefix}/{nameof(Playlists)}/{{id:int}}";

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public static class Tracks
        {
            public const string Add = $"{Prefix}/{nameof(Playlists)}/{nameof(Tracks)}";
            public const string Remove = $"{Prefix}/{nameof(Playlists)}/{nameof(Tracks)}";
            public const string List = $"{Prefix}/{nameof(Playlists)}/{{playlistId:int}}/{nameof(Tracks)}";
        }
    }

    public static class Tracks
    {
        public const string Create = $"{Prefix}/{nameof(Tracks)}";

        public static class Owners
        {
            public const string List = $"{Prefix}/{nameof(Tracks)}/{{id:int}}/artists";
        }
    }

    public static class Genres
    {
        public const string Create = $"{Prefix}/{nameof(Genres)}";
    }

    public static class Transactions
    {
        public const string Update = $"{Prefix}/{nameof(Transactions)}";
        public const string Create = $"{Prefix}/{nameof(Transactions)}";
    }

    public static class Users
    {
        public const string Create = $"{Prefix}/{nameof(Users)}";
    }

    public static class Plans
    {
        public const string List = $"{Prefix}/{nameof(Plans)}";
        public const string Update = $"{Prefix}/{nameof(Plans)}";
        public const string Read = $"{Prefix}/{nameof(Plans)}/{{id:int}}";
        public const string Create = $"{Prefix}/{nameof(Plans)}";
        public const string Delete = $"{Prefix}/{nameof(Plans)}/{{id:int}}";

        public static class Permissions
        {
            public const string Add = $"{Prefix}/{nameof(Plans)}/{nameof(Permissions)}";
            public const string Remove = $"{Prefix}/{nameof(Plans)}/{nameof(Permissions)}";
            public const string ReplaceAll = $"{Prefix}/{nameof(Plans)}/{nameof(Permissions)}";
        }
    }

    public static class Permissions
    {
        public const string List = $"{Prefix}/{nameof(Permissions)}";
    }

    public static class Roles
    {
        public const string List = $"{Prefix}/{nameof(Roles)}";
        public const string Update = $"{Prefix}/{nameof(Roles)}";
        public const string Read = $"{Prefix}/{nameof(Roles)}/{{id:int}}";
        public const string Create = $"{Prefix}/{nameof(Roles)}";
        public const string Delete = $"{Prefix}/{nameof(Roles)}/{{id:int}}";

        public static class SystemPermissions
        {
            public const string Add = $"{Prefix}/{nameof(Roles)}/{nameof(SystemPermissions)}";
            public const string Remove = $"{Prefix}/{nameof(Roles)}/{nameof(SystemPermissions)}";
            public const string ReplaceAll = $"{Prefix}/{nameof(Roles)}/{nameof(SystemPermissions)}";
        }
    }

    public static class SystemPermissions
    {
        public const string List = $"{Prefix}/{nameof(SystemPermissions)}";
    }
}