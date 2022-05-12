namespace LiWiMus.Web.MailServer.Core.Settings;

public class MailSettings
{
    public const string ConfigName = "MailSettings";

    public string Mail { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
}