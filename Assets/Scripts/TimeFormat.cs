/// <summary>One way of writing a time, so the win screen and the level list always agree.</summary>
public static class TimeFormat
{
    // 75.3 seconds is written 1:15.3
    public static string Seconds(float seconds)
    {
        int minutes = (int)(seconds / 60f);
        float rest = seconds - minutes * 60f;
        return minutes + ":" + rest.ToString("00.0");
    }
}
