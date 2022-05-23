namespace LiWiMus.Core.Plans;

public static class DefaultPermissions
{
    public static IEnumerable<Permission> GetPremium()
    {
        return new[]
        {
            Track.Download.Permission,
            Track.WithoutAds.Permission,
            Playlist.Cover.Permission,
            Playlist.Private.Permission,
            Avatar.Upload.Permission
        };
    }

    public static IEnumerable<Permission> GetAll()
    {
        return GetPremium();
    }

    public static class Track
    {
        public static class Download
        {
            public static readonly Permission Permission = new()
            {
                Name = Name,
                Description = "Download songs"
            };

            public const string Name = $"{nameof(Track)}.{nameof(Download)}";
        }

        public static class WithoutAds
        {
            public static readonly Permission Permission = new()
            {
                Name = Name,
                Description = "Listen without ads"
            };

            public const string Name = $"{nameof(Track)}.{nameof(WithoutAds)}";
        }
    }

    public static class Playlist
    {
        public static class Private
        {
            public static readonly Permission Permission = new()
            {
                Name = Name,
                Description = "Have private playlists"
            };

            public const string Name = $"{nameof(Playlist)}.{nameof(Private)}";
        }

        public static class Cover
        {
            public static readonly Permission Permission = new()
            {
                Name = Name,
                Description = "Upload cover for playlists"
            };

            public const string Name = $"{nameof(Playlist)}.{nameof(Cover)}";
        }
    }

    public static class Avatar
    {
        public static class Upload
        {
            public static readonly Permission Permission = new()
            {
                Name = Name,
                Description = "Upload your own avatar"
            };

            public const string Name = $"{nameof(Avatar)}.{nameof(Upload)}";
        }
    }
}