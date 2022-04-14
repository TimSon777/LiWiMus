using Microsoft.Extensions.Logging;

namespace LiWiMus.Infrastructure.Data;

public static class ApplicationContextClear
{
    public static async Task ClearAsync(ApplicationContext applicationContext, ILogger logger)
    {
        await DeleteAllOnlineConsultantsAsync(applicationContext, logger);
    }
    
    private static async Task DeleteAllOnlineConsultantsAsync(ApplicationContext applicationContext, ILogger logger)
    {
        var onlineConsultants = applicationContext.OnlineConsultants.ToList();
        applicationContext.OnlineConsultants.RemoveRange(onlineConsultants);
        await applicationContext.SaveChangesAsync();
        logger.LogInformation("Online consultants were removed");
    }
}