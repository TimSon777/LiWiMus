namespace LiWiMus.Core.Roles;

public static class DefaultSystemPermissions
{
    public static class Admin
    {
        public static class Access
        {
            public const string Name = $"{nameof(Admin)}.{nameof(Access)}";

            public static readonly SystemPermission Permission = new()
            {
                Name = Name,
                Description = "Gives access to the site for administrators"
            };
        }
    }

    public static class Chat
    {
        public static class Answer
        {
            public static readonly SystemPermission Permission = new()
            {
                Name = Name,
                Description = "Allows you to reply to customer messages"
            };

            public const string Name = $"{nameof(Chat)}.{nameof(Answer)}";
        }
    }

    public static IEnumerable<SystemPermission> GetAll()
    {
        return new[] {Admin.Access.Permission, Chat.Answer.Permission};
    }
}