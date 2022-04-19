using System.Reflection;

namespace LiWiMus.Core.Permissions;

public static class DefaultPermissions
{
    private static List<string>? _allPermissions;

    public static List<string> GetAllPermissions()
    {
        if (_allPermissions is not null)
        {
            return _allPermissions;
        }

        _allPermissions = new List<string>();
        var type = typeof(DefaultPermissions);
        var nestedTypes = type.GetNestedTypes();
        foreach (var nestedType in nestedTypes)
        {
            var constants =
                nestedType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                          .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                          .Select(x => x.GetRawConstantValue() as string)
                          .Where(x => !string.IsNullOrEmpty(x));
            _allPermissions.AddRange(constants!);
        }

        return _allPermissions;
    }

    public static class Artist
    {
        public const string Create = $"{nameof(Artist)}.{nameof(Create)}";
        public const string Read = $"{nameof(Artist)}.{nameof(Read)}";
        public const string Update = $"{nameof(Artist)}.{nameof(Update)}";
        public const string Delete = $"{nameof(Artist)}.{nameof(Delete)}";
    }

    public static class Album
    {
        public const string Create = $"{nameof(Album)}.{nameof(Create)}";
        public const string Read = $"{nameof(Album)}.{nameof(Read)}";
        public const string Update = $"{nameof(Album)}.{nameof(Update)}";
        public const string Delete = $"{nameof(Album)}.{nameof(Delete)}";
    }

    public static class Genre
    {
        public const string Create = $"{nameof(Genre)}.{nameof(Create)}";
        public const string Read = $"{nameof(Genre)}.{nameof(Read)}";
        public const string Update = $"{nameof(Genre)}.{nameof(Update)}";
        public const string Delete = $"{nameof(Genre)}.{nameof(Delete)}";
    }

    public static class Playlist
    {
        public const string Create = $"{nameof(Playlist)}.{nameof(Create)}";
        public const string Read = $"{nameof(Playlist)}.{nameof(Read)}";
        public const string Update = $"{nameof(Playlist)}.{nameof(Update)}";
        public const string Delete = $"{nameof(Playlist)}.{nameof(Delete)}";
    }

    public static class Role
    {
        public const string Create = $"{nameof(Role)}.{nameof(Create)}";
        public const string Read = $"{nameof(Role)}.{nameof(Read)}";
        public const string Update = $"{nameof(Role)}.{nameof(Update)}";
        public const string Delete = $"{nameof(Role)}.{nameof(Delete)}";
    }

    public static class Track
    {
        public const string Create = $"{nameof(Track)}.{nameof(Create)}";
        public const string Read = $"{nameof(Track)}.{nameof(Read)}";
        public const string Update = $"{nameof(Track)}.{nameof(Update)}";
        public const string Delete = $"{nameof(Track)}.{nameof(Delete)}";
    }

    public static class User
    {
        public const string Create = $"{nameof(User)}.{nameof(Create)}";
        public const string Read = $"{nameof(User)}.{nameof(Read)}";
        public const string Update = $"{nameof(User)}.{nameof(Update)}";
        public const string Delete = $"{nameof(User)}.{nameof(Delete)}";
    }

    public static class Transaction
    {
        public const string Create = $"{nameof(Transaction)}.{nameof(Create)}";
        public const string Read = $"{nameof(Transaction)}.{nameof(Read)}";
        public const string Update = $"{nameof(Transaction)}.{nameof(Update)}";
        public const string Delete = $"{nameof(Transaction)}.{nameof(Delete)}";
    }

    public static class Chat
    {
        public const string Ask = $"{nameof(Chat)}.{nameof(Ask)}";
        public const string Answer = $"{nameof(Chat)}.{nameof(Answer)}";
    }
}