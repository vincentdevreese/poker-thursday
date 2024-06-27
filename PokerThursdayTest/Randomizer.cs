using AutoFixture;
using PokerThursdayTest.AutoFixture;

namespace PokerThursdayTest;

public static class Randomizer
{
    public static T Any<T>() => RandomData.Fixture.Create<T>();

    public static bool Another(bool? value) => AnythingBut(value);
    public static int Another(int? value) => AnythingBut(value);
    public static decimal Another(decimal? value) => AnythingBut(value);
    public static string Another(string? value) => AnythingBut(value);
    public static Guid Another(Guid? value) => AnythingBut(value);
    public static DateTime Another(DateTime? value) => AnythingBut(value);
    public static DateOnly Another(DateOnly? value) => AnythingBut(value);
    public static T Another<T>(T value) where T : class => AnythingBut(value);

    private static T AnythingBut<T>(T? _) where T : struct =>
        Any<T>();

    private static T AnythingBut<T>(T? _) where T : class =>
        Any<T>();
}