namespace LiWiMus.Core.Constants;

public static class Permissions
{
    public const string ClaimType = nameof(Permissions);

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
        public const string Create  = $"{nameof(Permissions)}.{nameof(Artist)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Permissions)}.{nameof(Artist)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Permissions)}.{nameof(Artist)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Permissions)}.{nameof(Artist)}.{nameof(Delete)}";
    }

    public static class Album
    {
        public const string Create  = $"{nameof(Permissions)}.{nameof(Album)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Permissions)}.{nameof(Album)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Permissions)}.{nameof(Album)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Permissions)}.{nameof(Album)}.{nameof(Delete)}";
    }

    public static class Genre
    {
        public const string Create  = $"{nameof(Permissions)}.{nameof(Genre)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Permissions)}.{nameof(Genre)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Permissions)}.{nameof(Genre)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Permissions)}.{nameof(Genre)}.{nameof(Delete)}";
    }

    public static class Playlist
    {
        public const string Create  = $"{nameof(Permissions)}.{nameof(Playlist)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Permissions)}.{nameof(Playlist)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Permissions)}.{nameof(Playlist)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Permissions)}.{nameof(Playlist)}.{nameof(Delete)}";
    }

    public static class Role
    {
        public const string Create  = $"{nameof(Permissions)}.{nameof(Role)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Permissions)}.{nameof(Role)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Permissions)}.{nameof(Role)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Permissions)}.{nameof(Role)}.{nameof(Delete)}";
    }

    public static class Track
    {
        public const string Create  = $"{nameof(Permissions)}.{nameof(Track)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Permissions)}.{nameof(Track)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Permissions)}.{nameof(Track)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Permissions)}.{nameof(Track)}.{nameof(Delete)}";
    }

    public static class User
    {
        public const string Create  = $"{nameof(Permissions)}.{nameof(User)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Permissions)}.{nameof(User)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Permissions)}.{nameof(User)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Permissions)}.{nameof(User)}.{nameof(Delete)}";
    }

    public static class Transaction
    {
        public const string Create  = $"{nameof(Permissions)}.{nameof(Transaction)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Permissions)}.{nameof(Transaction)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Permissions)}.{nameof(Transaction)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Permissions)}.{nameof(Transaction)}.{nameof(Delete)}";
    }
}