﻿namespace LiWiMus.Core.Permissions;

public static class DefaultPermissions
{
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

            Chat.Ask,
            Chat.Answer
        };
    }

    public static class Artist
    {
        public const string Create  = $"{nameof(Artist)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Artist)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Artist)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Artist)}.{nameof(Delete)}";
    }

    public static class Album
    {
        public const string Create  = $"{nameof(Album)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Album)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Album)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Album)}.{nameof(Delete)}";
    }

    public static class Genre
    {
        public const string Create  = $"{nameof(Genre)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Genre)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Genre)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Genre)}.{nameof(Delete)}";
    }

    public static class Playlist
    {
        public const string Create  = $"{nameof(Playlist)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Playlist)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Playlist)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Playlist)}.{nameof(Delete)}";
    }

    public static class Role
    {
        public const string Create  = $"{nameof(Role)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Role)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Role)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Role)}.{nameof(Delete)}";
    }

    public static class Track
    {
        public const string Create  = $"{nameof(Track)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Track)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Track)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Track)}.{nameof(Delete)}";
    }

    public static class User
    {
        public const string Create  = $"{nameof(User)}.{nameof(Create)}";
        public const string Read    = $"{nameof(User)}.{nameof(Read)}";
        public const string Update  = $"{nameof(User)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(User)}.{nameof(Delete)}";
    }

    public static class Transaction
    {
        public const string Create  = $"{nameof(Transaction)}.{nameof(Create)}";
        public const string Read    = $"{nameof(Transaction)}.{nameof(Read)}";
        public const string Update  = $"{nameof(Transaction)}.{nameof(Update)}";
        public const string Delete  = $"{nameof(Transaction)}.{nameof(Delete)}";
    }

    public static class Chat
    {
        public const string Ask = $"{nameof(Chat)}.{nameof(Ask)}";
        public const string Answer = $"{nameof(Chat)}.{nameof(Answer)}";
    }
}