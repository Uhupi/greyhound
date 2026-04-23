using BerlinClock.Api;
using Xunit;

namespace BerlinClock.Tests;

public class BerlinClockTests
{
    /// <summary>
    /// Verifies that at midnight with an odd second all lamp rows are off:
    /// seconds lamp off (odd), no hour lamps lit, no minute lamps lit.
    /// </summary>
    [Fact]
    public void Midnight_AllLampsOff()
    {
        var state = BerlinClockService.GetState(new DateTime(2024, 1, 1, 0, 0, 1));

        Assert.False(state.Seconds);
        Assert.All(state.HoursTop, l => Assert.False(l));
        Assert.All(state.HoursBottom, l => Assert.False(l));
        Assert.All(state.MinutesTop, l => Assert.False(l));
        Assert.All(state.MinutesBottom, l => Assert.False(l));
    }

    /// <summary>
    /// Verifies that the seconds lamp is on when the current second is even (e.g. 4).
    /// </summary>
    [Fact]
    public void EvenSecond_SecondsLampOn()
    {
        var state = BerlinClockService.GetState(new DateTime(2024, 1, 1, 0, 0, 4));
        Assert.True(state.Seconds);
    }

    /// <summary>
    /// Verifies that the seconds lamp is off when the current second is odd (e.g. 3).
    /// </summary>
    [Fact]
    public void OddSecond_SecondsLampOff()
    {
        var state = BerlinClockService.GetState(new DateTime(2024, 1, 1, 0, 0, 3));
        Assert.False(state.Seconds);
    }

    /// <summary>
    /// Verifies the hour rows for noon (12:00).
    /// 12 = 2×5 + 2×1, so 2 lamps in the top row and 2 lamps in the bottom row must be lit.
    /// </summary>
    [Fact]
    public void Noon_CorrectHours()
    {
        var state = BerlinClockService.GetState(new DateTime(2024, 1, 1, 12, 0, 0));

        // 12 = 2x5 + 2x1
        Assert.Equal(2, state.HoursTop.Count(l => l));
        Assert.Equal(2, state.HoursBottom.Count(l => l));
    }

    /// <summary>
    /// Verifies the hour rows at the maximum representable hour (23:00).
    /// 23 = 4×5 + 3×1, so all 4 top lamps and 3 bottom lamps must be lit.
    /// </summary>
    [Fact]
    public void MaxHour_23_CorrectHours()
    {
        var state = BerlinClockService.GetState(new DateTime(2024, 1, 1, 23, 0, 0));

        // 23 = 4x5 + 3x1
        Assert.Equal(4, state.HoursTop.Count(l => l));
        Assert.Equal(3, state.HoursBottom.Count(l => l));
    }

    /// <summary>
    /// Verifies the minute rows at 59 minutes, the highest possible minute value.
    /// 59 = 11×5 + 4×1, so all 11 top lamps and all 4 bottom lamps must be lit.
    /// </summary>
    [Fact]
    public void FiftyNineMinutes_CorrectMinutes()
    {
        var state = BerlinClockService.GetState(new DateTime(2024, 1, 1, 0, 59, 0));

        // 59 = 11x5 + 4x1
        Assert.Equal(11, state.MinutesTop.Count(l => l));
        Assert.Equal(4, state.MinutesBottom.Count(l => l));
    }

    /// <summary>
    /// Verifies the minute rows at 14 minutes, a mid-range value.
    /// 14 = 2×5 + 4×1, so 2 top lamps and 4 bottom lamps must be lit.
    /// </summary>
    [Fact]
    public void FourteenMinutes_CorrectMinutes()
    {
        var state = BerlinClockService.GetState(new DateTime(2024, 1, 1, 0, 14, 0));

        // 14 = 2x5 + 4x1
        Assert.Equal(2, state.MinutesTop.Count(l => l));
        Assert.Equal(4, state.MinutesBottom.Count(l => l));
    }

    /// <summary>
    /// Verifies that the top hour row always contains exactly 4 lamps,
    /// matching the Berlin Clock specification.
    /// </summary>
    [Fact]
    public void HoursTopRow_HasFourLamps()
    {
        var state = BerlinClockService.GetState(DateTime.Now);
        Assert.Equal(4, state.HoursTop.Length);
    }

    /// <summary>
    /// Verifies that the top minute row always contains exactly 11 lamps,
    /// matching the Berlin Clock specification.
    /// </summary>
    [Fact]
    public void MinutesTopRow_HasElevenLamps()
    {
        var state = BerlinClockService.GetState(DateTime.Now);
        Assert.Equal(11, state.MinutesTop.Length);
    }
}
