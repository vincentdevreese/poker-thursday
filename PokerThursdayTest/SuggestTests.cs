using FluentAssertions;

using PokerThursday;

namespace PokerThursdayTest;

public class SuggestTests
{
    private readonly Suggest sut = new();

    [Fact]
    public void Should_test_name()
    {
        Debt[] a = [];
        this.Verify(a);
    }

    [Fact]
    public void Should_test_name_2()
    {
        Debt[] a = [ADebtFrom(1)];
        this.Verify(a);
    }

    private static Debt ADebtFrom(int amount) =>
        Randomizer.Any<Debt>() with { Amount = amount };


    [Fact]
    public void Should_test_name_3()
    {
        Debt debt1 = new("a", "b", 10);
        Debt debt3 = new("b", "a", 20);

        Debt debt2 = new("c", "d", 20);

        this.Verify([debt1, debt2]);

    }


    private void Verify(Debt[] a)
    {
        Debt[] actual = this.sut.Do(a);
        actual.Should().Equal(a);
    }
}

public class Suggest
{
    public Debt[] Do(Debt[] ints)
    {
        return ints;
    }
}