public class TimeDisplay : AmountDisplay
{
    private const int MinutesPerHour = 60;
    private const int SecondsPerMinute = 60;
    private const int MillisecondsPerSeconds = 100;

    protected override string ConvertAmount(float amount)
    {
        int totalSeconds = (int)amount;
        int hours = totalSeconds / (SecondsPerMinute * MinutesPerHour);
        int minutes = (totalSeconds % (SecondsPerMinute * MinutesPerHour)) / SecondsPerMinute;
        int seconds = totalSeconds % SecondsPerMinute;
        int milliseconds = (int)((amount - totalSeconds) * MillisecondsPerSeconds);

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}.{milliseconds:D2}";
    }
}