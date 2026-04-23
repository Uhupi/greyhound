namespace BerlinClock.Api;

public static class BerlinClockService
{
    public static BerlinClockState GetState(DateTime time) => new(
        Seconds: time.Second % 2 == 0,
        HoursTop: BuildRow(time.Hour / 5, 4),
        HoursBottom: BuildRow(time.Hour % 5, 4),
        MinutesTop: BuildRow(time.Minute / 5, 11),
        MinutesBottom: BuildRow(time.Minute % 5, 4)
    );

    private static bool[] BuildRow(int lit, int total) =>
        Enumerable.Range(0, total).Select(i => i < lit).ToArray();
}

public record BerlinClockState(
    bool Seconds,
    bool[] HoursTop,
    bool[] HoursBottom,
    bool[] MinutesTop,
    bool[] MinutesBottom
);
