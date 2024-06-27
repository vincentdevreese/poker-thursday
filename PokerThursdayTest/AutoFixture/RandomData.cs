using AutoFixture;

namespace PokerThursdayTest.AutoFixture;

public static class RandomData
{
    internal static Fixture Fixture { get; } = GetFixture();

    public static Fixture GetFixture() => new();
}