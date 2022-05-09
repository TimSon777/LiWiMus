namespace LiWiMus.Core.Permissions;

public static class DefaultPermissions
{
    public static List<Permission> GetPublic()
    {
        return new List<Permission>
        {
            Track.Download.Permission,
            Track.HighQuality.Permission,
            Track.WithoutAds.Permission,
            Playlist.Cover.Permission,
            Playlist.Private.Permission,
            Avatar.Upload.Permission
        };
    }

    public static List<Permission> GetPrivate()
    {
        return new List<Permission>
        {
            Chat.Answer.Permission,
            Chat.Ask.Permission,
            Admin.Access.Permission
        };
    }

    public static class Track
    {
        public static class Download
        {
            public static readonly Permission Permission = new(1, Name, Description);
            public const string Name = $"{nameof(Track)}.{nameof(Download)}";
            public const string Description = "Download songs";
        }

        public static class WithoutAds
        {
            public static readonly Permission Permission = new(2, Name, Description);
            public const string Name = $"{nameof(Track)}.{nameof(WithoutAds)}";
            public const string Description = "Listen without ads";
        }

        public static class HighQuality
        {
            public static readonly Permission Permission = new(3, Name, Description);
            public const string Name = $"{nameof(Track)}.{nameof(HighQuality)}";
            public const string Description = "Listen in high quality";
        }
    }

    public static class Playlist
    {
        public static class Private
        {
            public static readonly Permission Permission = new(4, Name, Description);
            public const string Name = $"{nameof(Playlist)}.{nameof(Private)}";
            public const string Description = "Have private playlists";
        }

        public static class Cover
        {
            public static readonly Permission Permission = new(5, Name, Description);
            public const string Name = $"{nameof(Playlist)}.{nameof(Cover)}";
            public const string Description = "Upload cover for playlists";
        }
    }

    public static class Avatar
    {
        public static class Upload
        {
            public static readonly Permission Permission = new(6, Name, Description);
            public const string Name = $"{nameof(Avatar)}.{nameof(Upload)}";
            public const string Description = "Upload your own avatar";
        }
    }

    public static class Chat
    {
        public static class Ask
        {
            public static readonly Permission Permission = new(7, Name, Description);
            public const string Name = $"{nameof(Chat)}.{nameof(Ask)}";
            public const string Description = "System permission";
        }

        public static class Answer
        {
            public static readonly Permission Permission = new(8, Name, Description);
            public const string Name = $"{nameof(Chat)}.{nameof(Answer)}";
            public const string Description = "System permission";
        }
    }

    public static class Admin
    {
        public static class Access
        {
            public static readonly Permission Permission = new(9, Name, Description);
            public const string Name = $"{nameof(Admin)}.{nameof(Access)}";
            public const string Description = "System permission";
        }
    }

    public static List<Permission> GetAll()
    {
        return GetPrivate().Union(GetPublic()).ToList();
    }
}