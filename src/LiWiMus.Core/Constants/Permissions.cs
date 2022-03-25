namespace LiWiMus.Core.Constants;

public static class Permissions
{
    public const string ClaimType = "Permissions";

    public static List<string> GeneratePermissionsForModule(string module)
    {
        return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.Read",
            $"Permissions.{module}.Update",
            $"Permissions.{module}.Delete",
        };
    }

    public static List<string> GetAllPermissions()
    {
        return new List<string>
        {
            Artist.Create,
            Artist.Read,
            Artist.Update,
            Artist.Delete,

            Album.Create,
            Album.Read,
            Album.Update,
            Album.Delete,

            Genre.Create,
            Genre.Read,
            Genre.Update,
            Genre.Delete,

            Playlist.Create,
            Playlist.Read,
            Playlist.Update,
            Playlist.Delete,

            Role.Create,
            Role.Read,
            Role.Update,
            Role.Delete,

            Track.Create,
            Track.Read,
            Track.Update,
            Track.Delete,

            User.Create,
            User.Read,
            User.Update,
            User.Delete,

            Transaction.Create,
            Transaction.Read,
            Transaction.Update,
            Transaction.Delete,
        };
    }

    public static class Artist
    {
        public const string Create = "Permissions.Artist.Create";
        public const string Read = "Permissions.Artist.Read";
        public const string Update = "Permissions.Artist.Update";
        public const string Delete = "Permissions.Artist.Delete";
    }

    public static class Album
    {
        public const string Create = "Permissions.Album.Create";
        public const string Read = "Permissions.Album.Read";
        public const string Update = "Permissions.Album.Update";
        public const string Delete = "Permissions.Album.Delete";
    }

    public static class Genre
    {
        public const string Create = "Permissions.Genre.Create";
        public const string Read = "Permissions.Genre.Read";
        public const string Update = "Permissions.Genre.Update";
        public const string Delete = "Permissions.Genre.Delete";
    }

    public static class Playlist
    {
        public const string Create = "Permissions.Playlist.Create";
        public const string Read = "Permissions.Playlist.Read";
        public const string Update = "Permissions.Playlist.Update";
        public const string Delete = "Permissions.Playlist.Delete";
    }

    public static class Role
    {
        public const string Create = "Permissions.Role.Create";
        public const string Read = "Permissions.Role.Read";
        public const string Update = "Permissions.Role.Update";
        public const string Delete = "Permissions.Role.Delete";
    }

    public static class Track
    {
        public const string Create = "Permissions.Track.Create";
        public const string Read = "Permissions.Track.Read";
        public const string Update = "Permissions.Track.Update";
        public const string Delete = "Permissions.Track.Delete";
    }

    public static class User
    {
        public const string Create = "Permissions.User.Create";
        public const string Read = "Permissions.User.Read";
        public const string Update = "Permissions.User.Update";
        public const string Delete = "Permissions.User.Delete";
    }

    public static class Transaction
    {
        public const string Create = "Permissions.Transaction.Create";
        public const string Read = "Permissions.Transaction.Read";
        public const string Update = "Permissions.Transaction.Update";
        public const string Delete = "Permissions.Transaction.Delete";
    }
}