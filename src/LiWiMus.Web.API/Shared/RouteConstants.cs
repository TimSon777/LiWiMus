﻿namespace LiWiMus.Web.API.Shared;

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

        public static class Owners
        {
            public const string Add = $"{Prefix}/{nameof(Albums)}/{nameof(Owners)}/{nameof(Add)}";
            public const string Remove = $"{Prefix}/{nameof(Albums)}/{nameof(Owners)}/{nameof(Remove)}";
        }
    }

    public static class Playlists
    {
        public const string Read = $"{Prefix}/{nameof(Playlists)}/{{id:int}}";
        public const string Update = $"{Prefix}/{nameof(Playlists)}";
        public const string RemovePhoto = $"{Prefix}/{nameof(Playlists)}/{{id:int}}/{nameof(RemovePhoto)}";
    }
}