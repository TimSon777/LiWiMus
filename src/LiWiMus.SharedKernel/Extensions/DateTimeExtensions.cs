namespace LiWiMus.SharedKernel.Extensions;

public static class DateTimeExtensions
{
    public static DateTime AddOrMaximize(this DateTime dateTime, TimeSpan timeSpan)
    {
        try
        {
            dateTime += timeSpan;
        }
        catch (ArgumentOutOfRangeException)
        {
            dateTime = DateTime.MaxValue;
        }

        return dateTime;
    }
}